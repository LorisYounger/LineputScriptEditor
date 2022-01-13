using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using LinePutScript;
namespace LineputScriptEditor
{
    /// <summary>
    /// Editor.xaml 的交互逻辑
    /// </summary>
    public partial class Editor : UserControl
    {
        public string FilePath;

        public Editor(string path, LpsDocument lps)
        {
            InitializeComponent();
            FilePath = path;
            loadfromlps(lps);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ColumTreeView.Width.Value == 0)
            {
                ColumTreeView.Width = new GridLength(200);
                ((Button)sender).Content = ">";
            }
            else
            {
                ColumTreeView.Width = new GridLength(0);
                ((Button)sender).Content = "<";
            }
        }

        private void loadfromlps(LpsDocument lps)
        {
            //LinePutScriptBox.Document.Blocks.Clear();
            //foreach (Line line in lps)
            //{
            //    Paragraph nl = new Paragraph();
            //    nl.Inlines.Add(new Run(line.Name) { Foreground = MainWindow.setting.NameColorBrush });
            //    if (line.info != "")
            //    {
            //        nl.Inlines.Add(new Run("#") { Foreground = MainWindow.setting.KeyWordColorBrush });
            //        nl.Inlines.Add(new Run(line.info) { Foreground = MainWindow.setting.InfoColorBrush });
            //    }
            //    nl.Inlines.Add(new Run(":|") { Foreground = MainWindow.setting.KeyWordColorBrush });
            //    foreach(Sub sub in line)
            //    {
            //        nl.Inlines.Add(new Run(sub.Name) { Foreground = MainWindow.setting.NameColorBrush });
            //        if (line.info != "")
            //        {
            //            nl.Inlines.Add(new Run("#") { Foreground = MainWindow.setting.KeyWordColorBrush });
            //            nl.Inlines.Add(new Run(sub.info) { Foreground = MainWindow.setting.InfoColorBrush });                        
            //        }
            //        nl.Inlines.Add(new Run(":|") { Foreground = MainWindow.setting.KeyWordColorBrush });
            //    }
            //    nl.Inlines.Add(new Run(line.Text));
            //    LinePutScriptBox.Document.Blocks.Add(nl);
            //}
        }
    }
}
