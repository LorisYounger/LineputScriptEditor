using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using LinePutScript;
using System.IO;
using System.Threading;
namespace LineputScriptEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists(Environment.CurrentDirectory + @"\setting.lps"))
                setting = new Setting(File.ReadAllText(Environment.CurrentDirectory + @"\setting.lps"));
            else
                setting = new Setting();
        }
        public static Setting setting;
        public List<Editor> Editors = new List<Editor>();

        private void LPTHead_Click(object sender, RoutedEventArgs e)
        {
            if (TabItemMain.Visibility != Visibility.Visible)
            {
                TabItemMain.Visibility = Visibility.Visible;
            }
            TabControlMain.SelectedIndex = 0;
        }

        private void TabControlMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (e.AddedItems.Count != 0)
            //    HeaderText.Header = ((TabItem)e.AddedItems[0]).Header;
            if (TabControlMain.SelectedItem != null)
                HeaderText.Header = ((TabItem)TabControlMain.SelectedItem).Header;
        }

        private void MenuOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }
        public void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "所有支持的文件(*.lps;*.lpt)|*.lps;*.lpt|LinePutScript文件(*.lps)|*.lps|Lineput文件(*.lpt)|*.lpt"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                OpenFile(openFileDialog.FileName);
            }
        }
        public void OpenFile(string Path)
        {
            //如果是已经打开过的文件,跳转到那个文件
            Editor ed = Editors.Find(x => x.FilePath.ToLower() == Path.ToLower());
            if (ed == null)
            {
                LpsDocument lps = new LpsDocument(File.ReadAllText(Path));
                ed = new LPSEditor(Path, lps);
                Editors.Add(ed);
                int idx = TabControlMain.Items.Add(ed.Father);
                TabControlMain.Dispatcher.BeginInvoke(new Action(() => { TabControlMain.SelectedIndex = idx; }));
                ed.FileNameChage = new Action(() => Dispatcher.BeginInvoke(new Action(() => HeaderText.Header = ((TabItem)TabControlMain.SelectedItem).Header)));
            }
            else
            {
                TabControlMain.SelectedIndex = Editors.IndexOf(ed) + 1;
            }
        }


        private void HyperLink_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Run)sender).Foreground = new SolidColorBrush(Color.FromRgb(30, 100, 200));
        }

        private void HyperLink_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Run)sender).Foreground = new SolidColorBrush(Color.FromRgb(51, 136, 255));
        }

        private void RunOpenFile_Click(object sender, MouseButtonEventArgs e)
        {
            OpenFile();
        }

        private void TabControlMain_ItemRemoving(object sender, Panuon.UI.Core.ItemRemovingEventArgs e)
        {
            TabItem send = (TabItem)e.Item;
            if (send.Name == "TabItemMain")
            {
                e.Cancel = true;
                if (TabControlMain.Items.Count != 1)
                {
                    TabItemMain.Visibility = Visibility.Collapsed;
                    if (TabControlMain.SelectedItem == e.Item)
                    {
                        TabControlMain.SelectedIndex = 1;
                    }
                }
            }
            else if (((Editor)send.Content).IsEdit)
            {
                e.Cancel = ((Editor)send.Content).ExitSafe() == false;
            }
            else if (TabControlMain.Items.Count == 2)
            {
                TabItemMain.Visibility = Visibility.Visible;
            }
            else
            {//开始清除
                Editor ed = ((Editor)send.Content);
                if(ed.ExitSafe() == true)
                {
                    Editors.Remove(ed);
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
