using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LineputScriptEditor
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static StartupEventArgs E;
        protected override void OnStartup(StartupEventArgs e)
        {
            //e.Args为命令行参数
            //Do something
            E = e;
        }
    }
}
