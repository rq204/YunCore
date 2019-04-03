using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore.Utility
{
    /// <summary>
    /// 得到具体对应的汉字意思
    /// </summary>
    public class EnumName
    {
         public static string GetJobStatus(YunCore.JobStatus status)
        {
            string stats = "空闲";
            switch (status)
            {
                case YunCore.JobStatus.Paused:
                    stats = "暂停";
                    break;
                case YunCore.JobStatus.Running:
                    stats = "运行";
                    break;
            }
            return stats;
        }

         public static string GetCronType(YunCore.CronType cronType)
         {
             string crontype = "";
             switch (cronType)
             {
                 case YunCore.CronType.Days:
                     crontype = "每天";
                     break;
                 case YunCore.CronType.Hours:
                     crontype = "每时";
                     break;
                 case YunCore.CronType.Miniutes:
                     crontype = "每分";
                     break;
                 case YunCore.CronType.Seconds:
                     crontype = "每秒";
                     break;
             }
             return crontype;
         }
    }
}
