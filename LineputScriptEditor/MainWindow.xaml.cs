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
            if(File.Exists(Environment.CurrentDirectory + @"\setting.lps"))
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
            if (e.AddedItems.Count != 0)
                HeaderText.Header = ((TabItem)e.AddedItems[0]).Header;
        }

        private void MenuOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }
        public void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "LineputScript文件|*.lps";
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
                ed = new Editor(Path, lps);
                Editors.Add(ed);

                int idx = TabControlMain.Items.Add(new TabItem()
                {
                    Header = Path.Split('\\').Last(),
                    Content = ed
                });
                TabControlMain.Dispatcher.BeginInvoke(new Action(() => { TabControlMain.SelectedIndex = idx; }));
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
            else if (TabControlMain.Items.Count == 2)
            {
                TabItemMain.Visibility = Visibility.Visible;
            }
        }
    }
}
