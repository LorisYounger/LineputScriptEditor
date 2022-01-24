using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.IO;
using LinePutScript;
namespace LineputScriptEditor
{
    /// <summary>
    /// Editor.xaml 的交互逻辑
    /// </summary>
    public partial class LPSEditor : Editor
    {


        public LPSEditor(string path, LpsDocument lps) : base(path, lps)
        {
            SaveFile = saveFile;

            InitializeComponent();
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
                    ListLPSText.Children.Add(new EditorLine(new Action(() => IsEdit = true), lps[i]));
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

        private LpsDocument toLPS()
        {
            LpsDocument lps = new LpsDocument();
            foreach (IEditorLines iel in ListLPSText.Children)
            {
                lps.AddRange(iel.ToLines());
            }
            return lps;
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <returns>True:保存成功</returns>
        private bool saveFile()
        {
            if (!IsEdit)
            {
                return true;
            }
            if (string.IsNullOrEmpty(FilePath))
            {
                var d = new SaveFileDialog
                {
                    Filter = "LinePutScript文件(*.lps)|*.lps|Lineput文件(*.lpt)|*.lpt|所有支持的文件(*.lps; *.lpt)| *.lps; *.lpt"
                };
                if (d.ShowDialog() == true)
                {
                    FilePath = d.FileName;
                }
                else
                {
                    return false;
                }
            }
            File.WriteAllText(FilePath, toLPS().ToString());
            IsEdit = false;
            return true;
        }

        private void Grid_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == System.Windows.Input.ModifierKeys.Control)
                switch (e.Key)
                {
                    case System.Windows.Input.Key.S:
                        SaveFile();
                        break;
                }
        }
    }
}
