using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore
{
    /// <summary>
    /// 计划任务对象
    /// </summary>
    public struct CronStruct
    {
        public CronStruct(int jobid, int cronid, DateTime time)
        {
            this.JobId = jobid;
            this.CronId = cronid;
            this.Time = time;
        }
        public int JobId;
        public int CronId;
        public DateTime Time;
    }
}
