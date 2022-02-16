using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using LinePutScript;
namespace LineputScriptEditor
{
    public class Setting : LpsDocument
    {
        public Action HistoryChange;
        public Setting(string lps) : base(lps)
        {

        }
        public Setting()
        {
            AddLine(new Line("lpsed", "setting"));
            AddLine(new Line("stylecolor", "", "", new Sub()));
            AddLine(new Line("openhistory"));
        }
        public void AddHistory(string filename, string filepath)
        {
            Sub sub = this["openhistory"].FindInfo(filepath);
            if (sub != null)
                this["openhistory"].Remove(sub);
            this["openhistory"].Insert(0, new Sub(filename, filepath));
            HistoryChange();
        }
    }
}
