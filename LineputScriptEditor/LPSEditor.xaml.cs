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
using System.Threading;

namespace LineputScriptEditor
{
    /// <summary>
    /// Editor.xaml 的交互逻辑
    /// </summary>
    public partial class LPSEditor : Editor
    {


        public LPSEditor(string path, LpsDocument lps) : base(path)
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
                    ListLPSText.Children.Add(new EditorTable(this, line));
                    i += len - 1;
                }
                else
                    ListLPSText.Children.Add(new EditorLine(this, lps[i]));
            }
            GridToolBox.Children.Add(new ToolBox(this));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ColumTreeView.Width.Value == 0)
            {
                ColumTreeView.Width = new GridLength(160);
                ((Button)sender).Content = "<";
            }
            else
            {
                ColumTreeView.Width = new GridLength(0);
                ((Button)sender).Content = ">";
            }
            new Thread(() =>
            {
                Thread.Sleep(10);
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    foreach (ISizeChange line in ListLPSText.Children)
                        line.SizeChange();
                }));
            }).Start();
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

        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="iel">要插入的行定位</param>
        /// <param name="insert">要插入的新行</param>
        /// <param name="position">位置偏移标</param>
        public void InsertLine(UIElement iel, UIElement insert, int position = 0)
        {
            int p = ListLPSText.Children.IndexOf(iel) + position;
            if (p < 0)
                p = 0;
            else if (p > ListLPSText.Children.Count)
                p = ListLPSText.Children.Count;
            ListLPSText.Children.Insert(p, insert);
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (ISizeChange line in ListLPSText.Children)
                line.SizeChange();
        }
    }
}
