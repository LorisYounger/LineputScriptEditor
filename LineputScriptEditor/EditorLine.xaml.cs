using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LinePutScript;

namespace LineputScriptEditor
{
    /// <summary>
    /// EditorLine.xaml 的交互逻辑
    /// </summary>
    public partial class EditorLine : UserControl, IEditorLines
    {
        public Action SetisEdit;
        public Line ToLine()
        {
            Line lin = ((EditorSub)SubsWrap.Children[0]).ToLine();
            for (int i = 1; i < SubsWrap.Children.Count; i++)
            {
                lin.AddSub(((EditorSub)SubsWrap.Children[i]).ESub);
            }
            return lin;
        }
        public string Text;
        public string Comment;
        public EditorLine(Action setisedit)
        {
            SetisEdit = setisedit;
            InitializeComponent();
            SubsWrap.Children.Add(new EditorSub(SetisEdit, true));
            Text = "Text";
            Comment = "Comment";
            DisplayReadText();
            DisplayReadComment();
        }
        public EditorLine(Action setisedit, Line line)
        {
            SetisEdit = setisedit;
            InitializeComponent();
            SubsWrap.Children.Add(new EditorSub(SetisEdit, new Sub(line.Name, line.Info), true));
            foreach (Sub sub in line.Subs)
            {
                SubsWrap.Children.Add(new EditorSub(SetisEdit, sub));
            }
            Text = line.text;
            Comment = line.Comments;
            DisplayReadText();
            DisplayReadComment();
        }
        public void DisplayEditorText()
        {
            TText.Visibility = Visibility.Collapsed;
            BText.Visibility = Visibility.Visible;
            BText.Text = Text;
            TextBoxSelect(BText);
        }
        public void DisplayReadText()
        {
            TText.Visibility = Visibility.Visible;
            BText.Visibility = Visibility.Collapsed;
            TText.Text = Text;
            ButtonText.Visibility = (Text == "" ? Visibility.Visible : Visibility.Collapsed);
        }
        public void DisplayEditorComment()
        {
            TComment.Visibility = Visibility.Collapsed;
            BComment.Visibility = Visibility.Visible;
            BComment.Text = Comment;
            TextBoxSelect(BComment);
        }
        public void TextBoxSelect(TextBox tb)
        {
            new Thread(() =>
            {
                Thread.Sleep(10);
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    tb.SelectAll();
                    tb.Focus();
                }));
            }).Start();
        }
        public void DisplayReadComment()
        {
            TComment.Visibility = Visibility.Visible;
            BComment.Visibility = Visibility.Collapsed;
            TComment.Text = Comment;
        }
        private void TText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DisplayEditorText();
        }

        private void BText_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Text = BText.Text;
            SetisEdit?.Invoke();
            DisplayReadText();
        }

        private void BText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //开始判断是否可以添加新的sub
                if (BText.Text.Contains(":|"))
                {
                    Line nl = new Line(BText.Text);
                    SubsWrap.Children.Add(new EditorSub(SetisEdit, new Sub(nl.Name, nl.Info)));
                    foreach (Sub sub in nl.Subs)
                    {
                        SubsWrap.Children.Add(new EditorSub(SetisEdit, sub));
                    }
                    Text = nl.Text;
                }
                else
                    Text = BText.Text;
                SetisEdit?.Invoke();
                DisplayReadText();
            }
            else if (e.Key == Key.Escape)
            {
                DisplayReadText();
            }
        }

        private void TComment_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DisplayEditorComment();
        }

        private void BComment_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Comment = BComment.Text;
            SetisEdit?.Invoke();
            DisplayReadComment();
        }

        private void BComment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Comment = BComment.Text;
                SetisEdit?.Invoke();
                DisplayReadComment();
            }
            else if (e.Key == Key.Escape)
            {
                DisplayReadComment();
            }
        }

        public Line[] ToLines()
        {
            return new Line[] { ToLine() };
        }
    }
}
