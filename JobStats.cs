using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore
{
    /// <summary>
    /// 当前任务统计状态
    /// </summary>
    public class JobStats
    {
        public int JobId = 0;

        public string JobName = "";

        /// <summary>
        /// 总运行数
        /// </summary>
        public int RunCountAll = 0;

        /// <summary>
        /// 任务状态
        /// </summary>
        public JobStatus JobStatus = JobStatus.Idle;
    }
}
