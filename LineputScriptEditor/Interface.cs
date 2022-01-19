using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using LinePutScript;
namespace LineputScriptEditor
{
    public class Editor : UserControl
    {
        public string FileName;
        public string FilePath;
        public LpsDocument LPS;
    }

    public interface IEditorLines
    {
        Line[] ToLines();
    }
}
