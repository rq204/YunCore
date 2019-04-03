using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YunCore
{
    /// <summary>
    /// 所有的全局的变量管理
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 获取IJob的操作方法
        /// </summary>
        public static Func<IJob> GetIJob;

        /// <summary>
        /// 获取Site的操作方法
        /// </summary>
        public static Func<IGroup> GetIGroup;

        /// <summary>
        /// 获取IPost的操作方法
        /// </summary>
        public static Func<IPost> GetIPost;

        /// <summary>
        /// 获取Icron的操作方法
        /// </summary>
        public static Func<ICron> GetICron;

        /// <summary>
        /// 获取Stats的操作方法
        /// </summary>
        public static Func<IStats> GetStats;

        /// <summary>
        /// 主窗体上关于我们的方法,可以显示在form的中间
        /// </summary>
        public static Func<Form> GetAbout;

        /// <summary>
        /// 主界面上选项的设置方法，,可以显示在form的中间
        /// </summary>
        public static Func<Form> GetOption;

        /// <summary>
        /// 获取app
        /// </summary>
        public static Func<YunCore.IApp> GetApp;

        /// <summary>
        /// 全局的数据处理，有它就忽略每任务的入库配置了
        /// </summary>
        public static Action<YunCore.IJob, Dictionary<string, string>> GlobalPost;

        ///// <summary>
        ///// 设置默认的Ijob和ISite,这里复制用
        ///// </summary>
        //public static void InitializeDefault()
        //{
            //YunCore.Config.GetIJob = new Func<IJob>(() => { return new YunCore.Table.Job(); });
            //YunCore.Config.GetIGroup = new Func<IGroup>(() => { return new YunCore.Table.Group(); });
            //YunCore.Config.GetIPost = new Func<IPost>(() => { return new YunCore.Table.Post(); });
            //YunCore.Config.GetICron = new Func<ICron>(() => { return new YunCore.Table.Cron(); });
        //}
    }
}