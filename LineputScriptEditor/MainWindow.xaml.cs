using LinePutScript;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using TextToDocument;
namespace LineputScriptEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly string CurrentPath = Path.GetDirectoryName(typeof(MainWindow).Assembly.Location);
        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists(CurrentPath + @"\setting.lps"))
                setting = new Setting(File.ReadAllText(CurrentPath + @"\setting.lps"));
            else
                setting = new Setting();
            setting.HistoryChange = RelsHistory;
            RelsHistory();
            TtD.DefaultFormart.H1.FontSize = 18;
            TtD.DefaultFormart.H2.FontSize = 16;
            TtD.DefaultFormart.H3.FontSize = 14;
            TtD.DefaultFormart.P.Foreground = (Brush)Application.Current.Resources.MergedDictionaries.Last()["Text"];
            if (App.E.Args.Length > 0)
            {
                OpenFile(App.E.Args[0]);
            }
        }
        public static Setting setting;
        public List<Editor> Editors = new List<Editor>();
        public void RelsHistory()
        {
            TextHistory.Inlines.Clear();
            for (int i = 0; i < setting["openhistory"].Count() && i < 5; i++)
            {
                string ns = setting["openhistory"][i].Name;
                if (ns.Length >= 15)
                {
                    ns = ns.Substring(0, 5) + ".." + ns.Substring(ns.Length - 5, 5);
                }
                Run r = new Run(ns);
                r.Foreground = (Brush)Application.Current.Resources.MergedDictionaries.Last()["HyperLink"];
                r.TextDecorations = TextDecorations.Underline;
                r.Cursor = Cursors.Hand;
                r.MouseEnter += HyperLink_MouseEnter;
                r.MouseLeave += HyperLink_MouseLeave;
                r.MouseLeftButtonDown += Run_MouseLeftButtonDown;
                ns = setting["openhistory"][i].Info;
                r.Tag = ns;
                TextHistory.Inlines.Add(r);
                if (ns.Length >= 40)
                {
                    ns = "..." + ns.Substring(ns.Length - 37, 37);
                }
                r = new Run(ns);
                r.Foreground = (Brush)Application.Current.Resources.MergedDictionaries.Last()["PageForeground"];
                r.FontSize = 9;
                TextHistory.Inlines.Add(r);
                TextHistory.Inlines.Add(new LineBreak());
            }
        }
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
                TabControlMain.Dispatcher.BeginInvoke(new Action(() => { TabControlMain.SelectedIndex = Editors.IndexOf(ed) + 1; }));
            }
        }
        public void OpenNewLPS()
        {
            Editor ed = new LPSEditor("新建LPS文档", new LpsDocument(":|"));
            Editors.Add(ed);
            int idx = TabControlMain.Items.Add(ed.Father);
            TabControlMain.Dispatcher.BeginInvoke(new Action(() => { TabControlMain.SelectedIndex = idx; }));
            ed.FileNameChage = new Action(() => Dispatcher.BeginInvoke(new Action(() => HeaderText.Header = ((TabItem)TabControlMain.SelectedItem).Header)));
        }

        private void HyperLink_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Run)sender).Foreground = (Brush)Application.Current.Resources.MergedDictionaries.Last()["HyperLinkDark"];
        }

        private void HyperLink_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Run)sender).Foreground = (Brush)Application.Current.Resources.MergedDictionaries.Last()["HyperLink"];
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
                if (ed.ExitSafe() == true)
                {
                    Editors.Remove(ed);
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void Run_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string filepath = (string)((Run)sender).Tag;
            if (File.Exists(filepath))
                OpenFile(filepath);
            else
                MessageBox.Show("文件已被移动或删除","无法打开");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (Editor ed in Editors)
            {
                if (!ed.ExitSafe())
                {
                    e.Cancel = true;
                    return;
                }
            }
            File.WriteAllText(CurrentPath + @"\setting.lps", setting.ToString());
        }

        private void Run_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            OpenNewLPS();
        }
        private void MenuSave(object sender, RoutedEventArgs e)
        {
            if (TabControlMain.SelectedItem != null && TabControlMain.SelectedIndex != 0)
                ((Editor)((TabItem)TabControlMain.SelectedItem).Content).SaveFile();
        }

        private void SaveAs(object sender, RoutedEventArgs e)
        {

        }

        private void SaveAll(object sender, RoutedEventArgs e)
        {
            foreach (Editor ed in Editors)
                ed.SaveFile();
        }

        private void MenuNewLPS(object sender, RoutedEventArgs e)
        {
            OpenNewLPS();
        }
    }
}
