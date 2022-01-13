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
        public Setting(string lps) : base(lps)
        {

        }
        public Setting()
        {
            AddLine(new Line("LineputScriptEditor", "", "", new Sub("ver", "100")));
        }
        public Color KeyWordColor
        {
            get
            {
                var nc = FindLine("keywordcolor");
                if (nc == null)
                    return Color.FromRgb(32, 127, 127);// return Color.FromRgb(143,8,196);
                else
                    return Color.FromArgb((byte)FindSub("a").InfoToInt, (byte)FindSub("r").InfoToInt, (byte)FindSub("g").InfoToInt, (byte)FindSub("b").InfoToInt);
            }
            set
            {
                AddorReplaceLine(new Line("keywordcolor", "","",new Sub("a",value.A.ToString()), new Sub("r", value.R.ToString()), new Sub("g", value.G.ToString()), new Sub("b", value.B.ToString())));
            }
        }
        public Color NameColor
        {
            get
            {
                var nc = FindLine("namecolor");
                if (nc == null)
                    return Color.FromRgb(0, 0, 255);
                else
                    return Color.FromArgb((byte)FindSub("a").InfoToInt, (byte)FindSub("r").InfoToInt, (byte)FindSub("g").InfoToInt, (byte)FindSub("b").InfoToInt);
            }
            set
            {
                AddorReplaceLine(new Line("namecolor", "", "", new Sub("a", value.A.ToString()), new Sub("r", value.R.ToString()), new Sub("g", value.G.ToString()), new Sub("b", value.B.ToString())));
            }
        }
        public Color InfoColor
        {
            get
            {
                var nc = FindLine("infocolor");
                if (nc == null)
                    return Color.FromRgb(163, 21,21); //return Color.FromRgb(32, 127, 127);// return Color.FromRgb(43, 145, 175);
                else
                    return Color.FromArgb((byte)FindSub("a").InfoToInt, (byte)FindSub("r").InfoToInt, (byte)FindSub("g").InfoToInt, (byte)FindSub("b").InfoToInt);
            }
            set
            {
                AddorReplaceLine(new Line("infocolor", "", "", new Sub("a", value.A.ToString()), new Sub("r", value.R.ToString()), new Sub("g", value.G.ToString()), new Sub("b", value.B.ToString())));
            }
        }
        public SolidColorBrush KeyWordColorBrush => new SolidColorBrush(KeyWordColor);
        public SolidColorBrush NameColorBrush => new SolidColorBrush(NameColor);
        public SolidColorBrush InfoColorBrush => new SolidColorBrush(InfoColor);
    }
}
