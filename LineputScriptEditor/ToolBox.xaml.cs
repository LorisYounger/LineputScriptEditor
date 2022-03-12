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
using LinePutScript;

namespace LineputScriptEditor
{
    /// <summary>
    /// ToolBox.xaml 的交互逻辑
    /// </summary>
    public partial class ToolBox : UserControl
    {
        public string InputType;
        LPSEditor lpsed;
        public ToolBox(LPSEditor lpse)
        {
            InitializeComponent();
            HideInput();
            lpsed = lpse;
        }
        public void HideInput()
        {
            GridInput.Visibility = Visibility.Collapsed;
            ListToolBox.Margin = new Thickness(0, 26, 0, 0);
        }
        public void ShowInput()
        {
            GridInput.Visibility = Visibility.Visible;
            ListToolBox.Margin = new Thickness(0, 128, 0, 0);
        }
        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            switch (InputType)
            {
                case "line":
                    if(TextBoxInput.Text == "")
                    {
                        lpsed.ListLPSText.Children.Add(new EditorLine(lpsed, new Line()));
                        break;
                    }
                    var lps = new LpsDocument(TextBoxInput.Text);
                    lpsed.IsEdit = true;
                    for (int i = 0; i < lps.Count; i++)
                        lpsed.ListLPSText.Children.Add(new EditorLine(lpsed, lps[i]));
                    break;
                case "table":
                    lpsed.ListLPSText.Children.Add(new EditorTable(lpsed, new LpsDocument(TextBoxInput.Text).Assemblage));
                    break;
                case "function":
                    //TODO
                    break;
                default:
                    string[] li = TextBoxInput.Text.Split('\n');
                    for (int i = 0; i < li.Length; i++)
                        InputType.Replace($"|{i}|", li[i]);
                    lps = new LpsDocument(InputType);
                    for (int i = 0; i < lps.Count; i++)
                    {
                        int len = Function.IsTableLine(lps, i);
                        if (len > 2)
                        {
                            List<Line> line = new List<Line>();
                            for (int j = i; j < i + len; j++)
                                line.Add(lps[j]);
                            lpsed.ListLPSText.Children.Add(new EditorTable(lpsed, line));
                            i += len - 1;
                        }
                        else
                            lpsed.ListLPSText.Children.Add(new EditorLine(lpsed, lps[i]));
                    }
                    break;
            }
            HideInput();
        }

        private void ButtonNewLine_Click(object sender, RoutedEventArgs e)
        {
            ShowInput();
            LableDesc.Content = "请输入新行内容:";
            TextBoxInput.Text = "";
            InputType = "line";
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            HideInput();
        }

        private void ButtonNewTable_Click(object sender, RoutedEventArgs e)
        {
            ShowInput();
            LableDesc.Content = "请输入表格内容:";
            TextBoxInput.Text = "";
            InputType = "table";
        }
        private void ButtonDIYFunction_Click(object sender, RoutedEventArgs e)
        {
            InputType = (string)((Button)sender).Tag;
            if (InputType.Contains("|1|"))
            {
                ShowInput();
                LableDesc.Content = "请输入预定|i|内容,每行一个 :";
                TextBoxInput.Text = "";
            }
            else
            {
                var lps = new LpsDocument(InputType);
                for (int i = 0; i < lps.Length; i++)
                {
                    int len = Function.IsTableLine(lps, i);
                    if (len > 2)
                    {
                        List<Line> line = new List<Line>();
                        for (int j = i; j < i + len; j++)
                            line.Add(lps[j]);
                        lpsed.ListLPSText.Children.Add(new EditorTable(lpsed, line));
                        i += len - 1;
                    }
                    else
                        lpsed.ListLPSText.Children.Add(new EditorLine(lpsed, lps[i]));
                }
            }
        }
        private void ButtonNewFunction_Click(object sender, RoutedEventArgs e)
        {
            ShowInput();
            LableDesc.Content = "请输入自定义的新内容:";
            TextBoxInput.Text = "首行为按钮名字\n|1|为需要输入的预定义内容\n|i|为行数";
            InputType = "function";
        }
    }
}
