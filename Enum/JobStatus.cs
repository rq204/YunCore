using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore
{
    /// <summary>
    /// 任务的状态空闲，运行，暂停，队列等待
    /// </summary>
    public enum JobStatus
    {
        /// <summary>
        /// 空闲中
        /// </summary>
        Idle,
        /// <summary>
        /// 运行中
        /// </summary>
        Running,
        /// <summary>
        /// 暂停
        /// </summary>
        Paused
        ///// <summary>
        ///// 队列等待中
        ///// </summary>
        //Queue
    }
}