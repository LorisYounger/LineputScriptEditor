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
    public partial class LPSEditor : Editor
    {
        public LPSEditor(string path, LpsDocument lps)
        {
            InitializeComponent();
            FilePath = path;
            for (int i = 0; i < lps.Count; i++)
            {
                int len = Function.IsTableLine(lps, i);
                if (len > 2)
                {
                    List<Line> line = new List<Line>();
                    for (int j = i; j < i + len; j++)
                        line.Add(lps[j]);
                    ListLPSText.Children.Add(new EditorTable(line));
                    i += len - 1;
                }
                else
                    ListLPSText.Children.Add(new EditorLine(lps[i]));
            }
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
    }
}
