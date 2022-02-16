using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LinePutScript;
namespace LineputScriptEditor
{
    public class Editor : UserControl
    {

        public TabItem Father;
        public Editor(string path)
        {
            FilePath = path;
            Father = new TabItem()
            {
                Header = fileName,
                Content = this,
            };
            if (filePath != null)
            {
                MainWindow.setting.AddHistory(FileName, FilePath);
            }
        }
        public string FileName
        {
            get => fileName;
        }
        private string fileName;
        private string filePath;
        public string FilePath
        {
            get => filePath;
            set
            {
                if (value.Contains('\\'))
                {
                    filePath = value;
                    fileName = value.Split('\\').Last();
                }
                else
                {
                    filePath = null;
                    fileName = value;
                }
                FileNameChage?.Invoke();
            }
        }
        public LpsDocument LPS;
        public bool IsEdit
        {
            get => isedit;
            set
            {
                isedit = value;
                if (value)
                {
                    Father.Header = '*' + fileName;
                }
                else
                {
                    Father.Header = fileName;
                }
                IsEditChange?.Invoke(value);
            }
        }

        private bool isedit = false;

        public Action<bool> IsEditChange;
        public Action FileNameChage;
        public Func<bool> SaveFile;
        public Func<bool> SaveAsFile;
        /// <summary>
        /// 在退出前寻问是否需要保存
        /// </summary>
        /// <returns>如果为True,则允许关闭. 如果为false,则不允许关闭</returns>
        public bool ExitSafe()
        {
            switch (MessageBox.Show($"是否要保存对 {FileName} 的更改?", "如果不保存,你的更改将会丢失", MessageBoxButton.YesNoCancel))
            {
                case MessageBoxResult.Yes:
                    if (SaveFile?.Invoke() == true)
                    {
                        return true;
                    }
                    return false;
                case MessageBoxResult.No:
                    return true;
                default:
                    return false;
            }

        }
    }

    public interface IEditorLines
    {
        Line[] ToLines();
    }
}
