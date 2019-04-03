using System;
using System.Collections.Generic;
using System.Text;

namespace YunCore
{
    /// <summary>
    /// 采集程序运行逻辑
    /// </summary>
    public interface ISpider
    {
        /// <summary>
        /// 开始执行任务
        /// </summary>
        void StartJob();

        /// <summary>
        /// 暂停任务
        /// </summary>
        void PauseJob();

        /// <summary>
        /// 继续任务
        /// </summary>
        void ContinueJob();

        /// <summary>
        /// 停止任务
        /// </summary>
        void StopJob();

        /// <summary>
        /// 得到任务当前状态
        /// </summary>
        /// <returns></returns>
        JobStatus GetJobStatus();

        /// <summary>
        /// 当前任务的进度信息
        /// </summary>
        /// <param name="jobid"></param>
        /// <param name="current"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        Action<int, int, int> ProgressChange { set; }//(int jobid, int current, int max);

        /// <summary>
        /// 操作任务当前状态
        /// </summary>
        Action<int, JobStatus> JobStatusChange { set; }//(int jobid, JobStatus jobStatus);

        /// <summary>
        /// 显示使用信息出去
        /// </summary>
        Action<int, string> InfoChange { set; }

        /// <summary>
        /// 当结果有变化时显示
        /// </summary>
        /// <param name="jobid"></param>
        /// <param name="logType"></param>
        /// <param name="pageurl"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        Action<int, LogType, string, string> ResultChange { set; }//(int jobid, LogType logType, string pageurl, string text);

        /// <summary>
        /// 统计信息有变化时显示
        /// </summary>
        Action<int, string> StatsChange { set; }
    }
}
 