using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinePutScript;
namespace LineputScriptEditor
{
    public static class Function
    {
        /// <summary>
        /// 将文本进行反转义处理(成为正常显示的文本)(无无法显示的文本)
        /// </summary>
        /// <param name="Reptex">要反转义的文本</param>
        /// <returns>反转义后的文本 正常显示的文本</returns>
        public static string TextDeReplace(string Reptex)
        {
            if (Reptex == null)
                return "";
            Reptex = Reptex.Replace("/stop", ":|");
            Reptex = Reptex.Replace("/id", "#");
            Reptex = Reptex.Replace("/com", ",");
            Reptex = Reptex.Replace("/!", "/");
            return Reptex;
        }
        /// <summary>
        /// 将文本进行转义处理(成为去除关键字的文本)(无无法显示的文本)
        /// </summary>
        /// <param name="Reptex">要转义的文本</param>
        /// <returns>转义后的文本 (去除关键字的文本)</returns>
        public static string TextReplace(string Reptex)
        {
            if (Reptex == null)
                return "";
            Reptex = Reptex.Replace("/", "/!");
            Reptex = Reptex.Replace(":|", "/stop");
            Reptex = Reptex.Replace("#", "/id");
            Reptex = Reptex.Replace(",", "/com");
            return Reptex;
        }
        public static int IsTableLine(LpsDocument lps, int nodeid)
        {
            if (lps[nodeid].text != "" || lps[nodeid].Comments != "")
                return 0;
            bool istable = true;
            int i = nodeid + 1;
            while (istable && ++i < lps.Count)
            {
                if (lps[nodeid].Name == lps[i].Name && lps[nodeid].Count == lps[i].Count && lps[i].text == "")
                {
                    for (int j = 0; j < lps[nodeid].Count; j++)
                    {
                        if (lps[nodeid][j].Name != lps[i][j].Name)
                        {
                            i--;
                            istable = false;
                            break;
                        }
                    }
                }
                else
                {
                    i--;
                    istable = false;
                }
            }
            return i - nodeid;
        }
    }
}
