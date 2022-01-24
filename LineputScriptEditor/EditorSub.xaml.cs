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
        Action SetisEdit;
        public Line ToLine() => new Line(ESub.ToString());
        public EditorSub(Action setisedit, Sub sub, bool isLine = false)
        {
            SetisEdit = setisedit;
            ESub = sub;
            InitializeComponent();
            if (isLine)
            {
                SetIsLine();
            }
            DisplayReadName();
            DisplayReadInfo();
        }
        public EditorSub(Action setisedit, bool isLine = false)
        {
            SetisEdit = setisedit;
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
            ESub.Name = BName.Text;
            SetisEdit?.Invoke();
            DisplayReadName();
        }
        private void BName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ESub.Name = BName.Text;
                SetisEdit?.Invoke();
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
            ESub.info = BInfo.Text;
            SetisEdit?.Invoke();
            DisplayReadInfo();
        }
        private void BInfo_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ESub.info = BInfo.Text;
                SetisEdit?.Invoke();
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
    }
}
