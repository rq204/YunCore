using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace YunCore
{
    /// <summary>
    /// 扩展定义,实现它就实现了个采集程序
    /// </summary>
    public interface IApp
    {
        /// <summary>
        /// 应用程序名称
        /// </summary>
        string AppName { get; }

        /// <summary>
        /// 项目标识
        /// </summary>
        string AppEnglish { get; }

        /// <summary>
        /// 扩展软件图标
        /// </summary>
        System.Drawing.Icon AppIco { get; }

        /// <summary>
        /// 创建新任务
        /// </summary>
        /// <param name="job"></param>
        /// <returns>false为添加任务时取消</returns>
        bool NewJob(IJob job);

        /// <summary>
        /// 编辑任务
        /// </summary>
        /// <param name="job"></param>
        /// <returns>false为编辑任务时取消</returns>
        bool EditJob(IJob job);

        /// <summary>
        /// 删除分组时，通知扩展删除任务
        /// </summary>
        /// <param name="job"></param>
        /// <returns>null为成功，否则为失败原因</returns>
        void DeleteJob(IJob job);

        ///<summary>
        /// 执行采集任务的采集器
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        YunCore.ISpider GetSpider(IJob job);

        /// <summary>
        /// http的api服务器设置
        /// </summary>
        /// <param name="pageurl">网址</param>
        /// <param name="RequestWebForms">网页提交的get或post表单值</param>
        /// <param name="ResponseHeaders">可以修改返回的头信息</param>
        /// <returns></returns>
        byte[] ApiHttpReuest(string pageurl, Dictionary<string, string> RequestWebForms, Dictionary<string, string> ResponseHeaders);

        /// <summary>
        /// 平台实例化后没有show出来前的操作。
        /// </summary>
        void Platform_Load();

        /// <summary>
        /// 退出整个程序时所做的操作
        /// </summary>
        void Platform_Exit();
    }
}
