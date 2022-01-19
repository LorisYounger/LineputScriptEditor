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
    public partial class EditorLine : UserControl
    {
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
        public EditorLine()
        {
            InitializeComponent();
            SubsWrap.Children.Add(new EditorSub(this, true));
            Text = "Text";
            Comment = "Comment";
            DisplayReadText();
            DisplayReadComment();
        }
        public EditorLine(Line line)
        {
            InitializeComponent();
            SubsWrap.Children.Add(new EditorSub(this, new Sub(line.Name, line.Info), true));
            foreach (Sub sub in line.Subs)
            {
                SubsWrap.Children.Add(new EditorSub(this, sub));
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
            BText.Text = Function.TextReplace(Text);
            TextBoxSelect(BText);
        }
        public void DisplayReadText()
        {
            TText.Visibility = Visibility.Visible;
            BText.Visibility = Visibility.Collapsed;
            TText.Text = Function.TextDeReplace(Text);
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
            DisplayReadText();
        }

        private void BText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Text = BText.Text;
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
            DisplayReadComment();
        }

        private void BComment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Comment = BComment.Text;
                DisplayReadComment();
            }
            else if (e.Key == Key.Escape)
            {
                DisplayReadComment();
            }
        }
    }
}
