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
        public List<string> Names = new List<string>();
        private ObservableCollection<SimLine> Subss = new ObservableCollection<SimLine>();
        private class SimLine
        {
            public List<string> Line { get; set; }
            public SimLine(List<string> line)
            {
                Line = line;
                Name = "ABC";
            }
            public string Name { get; set; }
        }
        public class TMPTest
        {
            public TMPTest()
            {
                Name = "bcd";
            }
            public string Name { get; set; }
        }
        public EditorTable(List<Line> Lines)
        {
            InitializeComponent();
            Names.Add(Lines[0].Name);

            foreach (Sub sb in Lines[0])
            {
                Names.Add(sb.Name);
            }
            foreach (Line li in Lines)
            {
                List<string> strs = new List<string>();
                strs.Add(li.Info);
                foreach (Sub sb in li)
                {
                    strs.Add(sb.Info);
                }
                Subss.Add(new SimLine(strs));
            }

            for (int i = 0; i < Names.Count; i++)
            {
                dataTable.Columns.Add(new DataGridTextColumn() { Header = Names[i], Binding = new Binding("Name"), Foreground = (Brush)Application.Current.Resources.MergedDictionaries.Last()["LineName"] });
            }
            dataTable.DataContext = new ObservableCollection<TMPTest>() { new TMPTest(), new TMPTest() };
        }

        public Line[] ToLines()
        {
            throw new NotImplementedException();
        }
    }
}
