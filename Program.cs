using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Yun
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Config.InitializeDefault();
            AsynLoader.LoadAction += new Action(JobManager.ReloadJob);//加载任务
            AsynLoader.LoadAction += new Action(GroupManager.ReloadSite);//加载站点
            //加载计划任务
            AsynLoader.StartLoader();

            YunForm yun = new YunForm();
            Application.Run(yun);
        }
    }
}
