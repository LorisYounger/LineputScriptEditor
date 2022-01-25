using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LinePutScript;
namespace LineputScriptEditor
{
    /// <summary>
    /// EditorSub.xaml 的交互逻辑
    /// </summary>
    public partial class EditorSub : UserControl, IEditorLines
    {
        public Sub ESub;
        public EditorLine EL;
        public Line ToLine() => new Line(ESub.ToString());
        public EditorSub(EditorLine el, Sub sub, bool isLine = false)
        {
            EL = el;
            ESub = sub;
            InitializeComponent();
            if (isLine)
            {
                SetIsLine();
            }
            DisplayReadName();
            DisplayReadInfo();
        }
        public EditorSub(EditorLine el, bool isLine = false)
        {
            EL = el;
            InitializeComponent();
            if (isLine)
            {
                SetIsLine();
                ESub = new Sub("Name", "Info");
            }
            else
            {
                ESub = new Sub("LineName", "Info");
            }
            DisplayEditorName();
            DisplayEditorInfo();
        }
        public void SetIsLine()
        {
            TName.Foreground = (Brush)Application.Current.Resources.MergedDictionaries.Last()["LineName"];
            BName.Foreground = (Brush)Application.Current.Resources.MergedDictionaries.Last()["LineName"];
        }
        public void SetIsSub()
        {
            TName.Foreground = (Brush)Application.Current.Resources.MergedDictionaries.Last()["SubName"];
            BName.Foreground = (Brush)Application.Current.Resources.MergedDictionaries.Last()["SubName"];
        }
        public void DisplayEditorName()
        {
            TName.Visibility = Visibility.Collapsed;
            BName.Visibility = Visibility.Visible;
            BName.Text = ESub.Name;
            TextBoxSelect(BName);
        }
        public void DisplayEditorInfo()
        {
            TInfo.Visibility = Visibility.Collapsed;
            BInfo.Visibility = Visibility.Visible;
            BInfo.Text = ESub.info;
            TextBoxSelect(BInfo);
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
        public void DisplayReadName()
        {
            TName.Visibility = Visibility.Visible;
            BName.Visibility = Visibility.Collapsed;
            TName.Text = ESub.Name;
        }
        public void DisplayReadInfo()
        {
            TInfo.Visibility = Visibility.Visible;
            BInfo.Visibility = Visibility.Collapsed;
            TInfo.Text = Function.TextDeReplace(ESub.info);
        }

        private void TName_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DisplayEditorName();
        }
        private void BName_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (ESub.Name != BName.Text)
            {
                ESub.Name = BName.Text;
                EL.LPSED.IsEdit = true;
            }
            DisplayReadName();
        }
        private void BName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (ESub.Name != BName.Text)
                {
                    ESub.Name = BName.Text;
                    EL.LPSED.IsEdit = true;
                }
                DisplayReadName();
            }
            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                DisplayReadName();
            }
        }

        private void TInfo_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DisplayEditorInfo();
        }
        private void BInfo_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (ESub.info != BInfo.Text)
            {
                ESub.info = BInfo.Text;
                EL.LPSED.IsEdit = true;
            }
            DisplayReadInfo();
        }
        private void BInfo_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (ESub.info != BInfo.Text)
                {
                    ESub.info = BInfo.Text;
                    EL.LPSED.IsEdit = true;
                }
                DisplayReadInfo();
            }
            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                DisplayReadInfo();
            }
        }

        public Line[] ToLines()
        {
            return new Line[] { ToLine() };
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            EL.InsertSub(this, new EditorSub(EL));
            EL.LPSED.IsEdit = true;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            EL.InsertSub(this, new EditorSub(EL), 1);
            EL.LPSED.IsEdit = true;
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            int p = EL.SubsWrap.Children.IndexOf(this) - 1;
            if (p <= -1)
            {
                return;
            }
            else if (p == 0)
            {
                SetIsLine();
                ((EditorSub)EL.SubsWrap.Children[0]).SetIsSub();
            }
            EL.SubsWrap.Children.Remove(this);
            EL.SubsWrap.Children.Insert(p, this);
            EL.LPSED.IsEdit = true;
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            int p = EL.SubsWrap.Children.IndexOf(this) + 1;
            if (p >= EL.SubsWrap.Children.Count)
            {
                return;
            }
            else if (p == 1)
            {
                SetIsSub();
                ((EditorSub)EL.SubsWrap.Children[1]).SetIsLine();
            }

            EL.SubsWrap.Children.Remove(this);
            EL.SubsWrap.Children.Insert(p, this);
            EL.LPSED.IsEdit = true;
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            EL.SubsWrap.Children.Remove(this);
            SetIsLine();
            ((EditorSub)EL.SubsWrap.Children[0]).SetIsSub();
            EL.SubsWrap.Children.Insert(0, this);
            EL.LPSED.IsEdit = true;
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            if (EL.SubsWrap.Children.Count == 1)
                return;
            if (EL.SubsWrap.Children.IndexOf(this) == 0)
            {
                SetIsSub();
                ((EditorSub)EL.SubsWrap.Children[1]).SetIsLine();
            }
            EL.SubsWrap.Children.Remove(this);
            EL.SubsWrap.Children.Add(this);
            EL.LPSED.IsEdit = true;
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            if (EL.SubsWrap.Children.Count == 1)
            {
                EL.LPSED.ListLPSText.Children.Remove(EL);
            }
            else
            {
                if (EL.SubsWrap.Children.IndexOf(this) == 0)
                {
                    ((EditorSub)EL.SubsWrap.Children[1]).SetIsLine();
                }
                EL.SubsWrap.Children.Remove(this);
            }
            EL.LPSED.IsEdit = true;
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            EL.InsertSub(this, new EditorSub(EL,new Sub((Sub)ESub.Clone())),1);
            EL.LPSED.IsEdit = true;
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            EL.SubsWrap.Children.Add(new EditorSub(EL, new Sub((Sub)ESub.Clone())));
            EL.LPSED.IsEdit = true;
        }
    }
}
