using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using LinePutScript;

namespace LineputScriptEditor
{
    /// <summary>
    /// EditorTable.xaml 的交互逻辑
    /// </summary>
    public partial class EditorTable : UserControl, IEditorLines
    {
        private List<string> Names = new List<string>();
        public LPSEditor LPSED;
        private ObservableCollection<ObservableCollection<string>> SubList = new ObservableCollection<ObservableCollection<string>>();
        public EditorTable(LPSEditor lpsed, List<Line> Lines)
        {
            LPSED = lpsed;
            InitializeComponent();
            Names.Add(Lines[0].Name);

            foreach (Sub sb in Lines[0])
            {
                Names.Add(sb.Name);
            }
            foreach (Line li in Lines)
            {
                ObservableCollection<string> strs = new ObservableCollection<string>();
                strs.Add(li.Info);
                foreach (Sub sb in li)
                {
                    strs.Add(sb.Info);
                }
                SubList.Add(strs);
            }

            for (int i = 0; i < Names.Count; i++)
            {
                dataTable.Columns.Add(new DataGridTextColumn() { Header = Names[i], Binding = new Binding($"[{i}]"), Foreground = (Brush)Application.Current.Resources.MergedDictionaries.Last()["LineName"] });
            }
            dataTable.ItemsSource = SubList;
        }

        public Line[] ToLines()
        {
            List<Line> lines = new List<Line>();
            foreach (var strs in SubList)
            {
                Line line = new Line(Names[0], strs[0]);
                for (int i = 1; i < strs.Count; i++)
                    line.AddSub(new Sub(Names[i], strs[i]));
                lines.Add(line);
            }
            return lines.ToArray();
        }

        private void dataTable_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = MouseWheelEvent;
            eventArg.Source = sender;
            ((DataGrid)sender).RaiseEvent(eventArg);
        }

        private void ItemInstall_Click(object sender, RoutedEventArgs e)
        {
            if (dataTable.SelectedIndex == -1)
            {
                ItemAdd_Click(sender, e);
            }
            else
            {
                ObservableCollection<string> ss = new ObservableCollection<string>();
                for (int i = 0; i < Names.Count; i++)
                {
                    ss.Add(Names[i]);
                }
                SubList.Insert(dataTable.SelectedIndex, ss);
            }

        }

        private void ItemAdd_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<string> ss = new ObservableCollection<string>();
            for (int i = 0; i < Names.Count; i++)
            {
                ss.Add(Names[i]);
            }
            SubList.Add(ss);
        }

        private void ItemCopy_Click(object sender, RoutedEventArgs e)
        {
            if (dataTable.SelectedIndex != -1)
            {
                ObservableCollection<string> ss = new ObservableCollection<string>();
                ObservableCollection<string> ssr = (ObservableCollection<string>)dataTable.SelectedItem;
                for (int i = 0; i < Names.Count; i++)
                {
                    ss.Add(ssr[i]);
                }
                SubList.Insert(dataTable.SelectedIndex, ss);
            }
        }

        private void ItemCopyAdd_Click(object sender, RoutedEventArgs e)
        {
            if (dataTable.SelectedIndex != -1)
            {
                ObservableCollection<string> ss = new ObservableCollection<string>();
                ObservableCollection<string> ssr = (ObservableCollection<string>)dataTable.SelectedItem;
                for (int i = 0; i < Names.Count; i++)
                {
                    ss.Add(ssr[i]);
                }
                SubList.Add(ss);
            }
        }

        private void ItemRemove_Click(object sender, RoutedEventArgs e)
        {
            if (dataTable.SelectedItems.Count != 0)
            {
                object[] l = new object[dataTable.SelectedItems.Count];
                dataTable.SelectedItems.CopyTo(l, 0);
                foreach (object obj in l)
                {
                    SubList.Remove((ObservableCollection<string>)obj);
                }
            }
        }

        private void dataTable_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
