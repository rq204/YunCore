using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore
{
    /// <summary>
    /// 计划任务接口类
    /// </summary>
    public interface ICron
    {
        /// <summary>
        /// 计划任务id
        /// </summary>
        int CronId { set; get; }

        /// <summary>
        /// 任务id
        /// </summary>
        int JobId { set; get; }

        /// <summary>
        /// 计划任务类型
        /// </summary>
        CronType CronType { set; get; }

        /// <summary>
        /// 每间隔几(秒，分，时，天，月)运行一次
        /// </summary>
        int Intervals { set; get; }

        /// <summary>
        /// 首次运行时间
        /// </summary>
        DateTime FirstStart { set; get; }

        /// <summary>
        /// 最后结束时间
        /// </summary>
        DateTime LastEnd { set; get; }

        /// <summary>
        /// 每天运行开始时间
        /// </summary>
        DateTime DayStart { set; get; }

        /// <summary>
        /// 每天运行结束时间
        /// </summary>
        DateTime DayEnd { set; get; }

        /// <summary>
        /// 计划任务的分组
        /// 不单独再去做分组名，直接用这里获取并组合显示
        /// </summary>
        string CronGroup { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateOn { set; get; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        DateTime ModifyOn { set; get; }

        /// <summary>
        /// 是否启用计划任务
        /// </summary>
        bool CronEnable { set; get; }

        /// <summary>
        /// 出错信息
        /// </summary>
        string Error { get; }

        /// <summary>
        /// 选择当前任务
        /// </summary>
        /// <returns></returns>
        bool SelectCron();

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <returns></returns>
        bool InsertCron();

        /// <summary>
        /// 更新Cron表
        /// </summary>
        /// <returns>是否更新</returns>
        bool UpdateCron();

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <returns>是否成功</returns>
        bool DeleteCron();

        /// <summary>
        /// 删除任务,返回null为成功
        /// </summary>
        /// <returns>是否成功</returns>
        bool DeleteCron(string Cronids);

        /// <summary>
        /// 按分组id删除任务,返回null为成功
        /// </summary>
        /// <returns>是否成功</returns>
        bool DeleteCronBySite(string siteid);

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <returns></returns>
        List<ICron> GetCronList();
    }
}