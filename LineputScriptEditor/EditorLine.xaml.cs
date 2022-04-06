using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LinePutScript;
using TextToDocument;

namespace LineputScriptEditor
{
    /// <summary>
    /// EditorLine.xaml 的交互逻辑
    /// </summary>
    public partial class EditorLine : UserControl, IEditorLines, ISizeChange
    {
        public LPSEditor LPSED;
        public Line ToLine()
        {
            Line lin = ((EditorSub)SubsWrap.Children[0]).ToLine();
            for (int i = 1; i < SubsWrap.Children.Count; i++)
            {
                lin.AddSub(((EditorSub)SubsWrap.Children[i]).ESub);
            }
            lin.Text = Text.Replace("\r", "");
            lin.Comments = Comment;
            return lin;
        }
        public string Text;
        public string Comment;
        public EditorLine(LPSEditor lpsed)
        {
            LPSED = lpsed;
            InitializeComponent();
            SubsWrap.Children.Add(new EditorSub(this, true));
            Text = "Text";
            Comment = "Comment";
            DisplayReadText();
            DisplayReadComment();
        }
        public EditorLine(LPSEditor lpsed, Line line)
        {
            LPSED = lpsed;
            InitializeComponent();
            SubsWrap.Children.Add(new EditorSub(this, new Sub(line.Name, line.Info), true));
            foreach (Sub sub in line.Subs)
            {
                SubsWrap.Children.Add(new EditorSub(this, sub));
            }
            Text = line.Text;
            Comment = line.Comments;
            DisplayReadText();
            DisplayReadComment();
        }
        public void DisplayEditorText()
        {
            if (Text.Contains("\n"))
            {
                TText.Visibility = Visibility.Collapsed;
                BText.Visibility = Visibility.Collapsed;

                TCRLFText.Visibility = Visibility.Collapsed;
                BCRLFText.Visibility = Visibility.Visible;
                BCRLFText.Text = Text;
                BCRLFText.Margin = new Thickness(0, MainWrap.ActualHeight, 0, 0);
                TextBoxSelect(BCRLFText);
            }
            else
            {
                TText.Visibility = Visibility.Collapsed;
                BText.Visibility = Visibility.Visible;

                TCRLFText.Visibility = Visibility.Collapsed;
                BCRLFText.Visibility = Visibility.Collapsed;

                BText.Text = Text;
                TextBoxSelect(BText);
            }

        }
        public void DisplayReadText()
        {
            if (Text.Contains("\n"))
            {
                TText.Visibility = Visibility.Collapsed;
                BText.Visibility = Visibility.Collapsed;

                TCRLFText.Visibility = Visibility.Visible;
                BCRLFText.Visibility = Visibility.Collapsed;

                TCRLFText.Document = TtD.TextToDocument(Text);
                TCRLFText.Margin = new Thickness(0, MainWrap.ActualHeight, 0, 0);
                ButtonText.Visibility = Visibility.Collapsed;
            }
            else
            {
                TText.Visibility = Visibility.Visible;
                BText.Visibility = Visibility.Collapsed;

                TCRLFText.Visibility = Visibility.Collapsed;
                BCRLFText.Visibility = Visibility.Collapsed;

                TText.Text = Text;
                ButtonText.Visibility = (Text == "" ? Visibility.Visible : Visibility.Collapsed);
            }
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
            //开始判断是否可以添加新的sub
            if (BText.Text.Contains(":|"))
            {
                Line nl = new Line(BText.Text);
                SubsWrap.Children.Add(new EditorSub(this, new Sub(nl.Name, nl.Info)));
                foreach (Sub sub in nl.Subs)
                {
                    SubsWrap.Children.Add(new EditorSub(this, sub));
                }
                Text = nl.Text;
                LPSED.IsEdit = true;
            }
            else if (Text != BText.Text)
            {
                LPSED.IsEdit = true;
                Text = BText.Text;
            }
            DisplayReadText();
        }

        private void BText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                {//在下方插入
                    LPSED.InsertLine(this, new EditorLine(LPSED), 1);
                }
                //开始判断是否可以添加新的sub
                if (BText.Text.Contains(":|"))
                {
                    Line nl = new Line(BText.Text);
                    SubsWrap.Children.Add(new EditorSub(this, new Sub(nl.Name, nl.Info)));
                    foreach (Sub sub in nl.Subs)
                    {
                        SubsWrap.Children.Add(new EditorSub(this, sub));
                    }
                    Text = nl.Text;
                    LPSED.IsEdit = true;
                }
                else if (Text != BText.Text)
                {
                    LPSED.IsEdit = true;
                    Text = BText.Text;
                }
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
                {
                    TText.Visibility = Visibility.Collapsed;
                    BText.Visibility = Visibility.Collapsed;

                    TCRLFText.Visibility = Visibility.Collapsed;
                    BCRLFText.Visibility = Visibility.Visible;

                    BCRLFText.Text = Text + '\n';
                    new Thread(() =>
                    {
                        Thread.Sleep(100);
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            BCRLFText.Select(Text.Length + 1, 0);
                            BCRLFText.Focus();
                        }));
                    }).Start();
                }
                else
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
            if (Comment != BComment.Text)
            {
                Comment = BComment.Text;
                LPSED.IsEdit = true;
            }
            DisplayReadComment();
        }

        private void BComment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Comment != BComment.Text)
                {
                    Comment = BComment.Text;
                    LPSED.IsEdit = true;
                }
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
        private void BCRLFText_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //开始判断是否可以添加新的sub
            if (BCRLFText.Text.Contains(":|"))
            {
                Line nl = new Line(BCRLFText.Text);
                SubsWrap.Children.Add(new EditorSub(this, new Sub(nl.Name, nl.Info)));
                foreach (Sub sub in nl.Subs)
                {
                    SubsWrap.Children.Add(new EditorSub(this, sub));
                }
                Text = nl.Text;
                LPSED.IsEdit = true;
            }
            else if (Text != BCRLFText.Text)
            {
                LPSED.IsEdit = true;
                Text = BCRLFText.Text;
            }
            DisplayReadText();
        }
        /// <summary>
        /// 插入Sub
        /// </summary>
        /// <param name="iel">要插入的Sub定位</param>
        /// <param name="insert">要插入的新Sub</param>
        /// <param name="position">位置偏移标</param>
        public void InsertSub(EditorSub iel, EditorSub insert, int position = 0)
        {
            int p = SubsWrap.Children.IndexOf(iel) + position;
            if (p <= 0)
            {
                p = 0;
                iel.SetIsSub();
                insert.SetIsLine();
            }
            else if (p > SubsWrap.Children.Count)
                p = SubsWrap.Children.Count;
            SubsWrap.Children.Insert(p, insert);
        }

        private void SubsWrap_SizeChanged(object sender, SizeChangedEventArgs e) => SizeChange();
        public void SizeChange()
        {
            if (SubsWrap.ActualWidth >= LPSED.MainScroll.ActualWidth + 200 && MainWrap.Height == 20)
            {
                MainWrap.Height = double.NaN;
                MainWrap.Width = LPSED.MainScroll.ActualWidth - 2;
            }
            else if (SubsWrap.ActualHeight <= 30 && MainWrap.Height != 20)
            {
                MainWrap.Height = 20;
                MainWrap.Width = double.NaN;
            }
            else if (double.IsNaN(MainWrap.Height))
            {
                MainWrap.Width = LPSED.MainScroll.ActualWidth - 2;
            }
            BCRLFText.Margin = new Thickness(0, MainWrap.ActualHeight, 0, 0);
            TCRLFText.Width = LPSED.MainScroll.ActualWidth - 2;
            TCRLFText.Margin = new Thickness(5, MainWrap.ActualHeight + 2, 1, 2);
        }
    }
}
