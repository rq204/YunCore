using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore
{
    /// <summary>
    /// 运行日志
    /// </summary>
    public interface Ilog
    {
        /// <summary>
        /// 日志id
        /// </summary>
        int LogId { get; set; }

        /// <summary>
        /// 任务id
        /// </summary>
        int JobId { get; set; }

        DateTime StartTime { get; set; }

        DateTime EndTime { get; set; }

        /// <summary>
        /// 添加记录
        /// </summary>
        int Append { get; set; }

        /// <summary>
        /// 重复数
        /// </summary>
        int Repeat { get; set; }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <returns></returns>
        bool InsertLog();

        /// <summary>
        /// 获取所有日志
        /// </summary>
        /// <param name="jobid"></param>
        /// <returns></returns>
        List<Ilog> SelectLog(int jobid);
    }
}
