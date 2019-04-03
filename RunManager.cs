using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore
{
    /// <summary>
    /// 任务运行管理,有一个任务运行队列，有一个
    /// 关于manager,全是内存中进行维护一个调用的实例
    /// 几个在运行，状态是什么,计划任务是什么
    /// </summary>
    public class RunManager
    {
        static object lkSpider = new object();
        static Dictionary<int, ISpider> spiderDic = new Dictionary<int, ISpider>();
        static IApp ia = Config.GetApp();

        public static void StartJob(int jobid)
        {
            ISpider spider = getSpider(jobid);
            switch (spider.GetJobStatus())
            {
                case JobStatus.Running:
                    return;
                case JobStatus.Idle:
                    lastRunTime[jobid] = DateTime.Now;
                    spider.StartJob();
                    break;
                case JobStatus.Paused:
                    spider.ContinueJob();
                    break;
            }
        }

        public static void StopJob(int jobid)
        {
            ISpider spider = getSpider(jobid);
            switch (spider.GetJobStatus())
            {
                case JobStatus.Running:
                    spider.StopJob();
                    break;
                case JobStatus.Idle:
                    break;
                case JobStatus.Paused:
                    spider.StopJob();
                    break;
            }
            if (changeJobs.Contains(jobid))
            {
                lock (lkSpider)
                {
                    changeJobs.Remove(jobid);
                    spiderDic.Remove(jobid);
                }
            }
        }

        private static List<int> changeJobs = new List<int>();
        public static void PauseJob(int jobid)
        {
            ISpider spider = getSpider(jobid);
            switch (spider.GetJobStatus())
            {
                case JobStatus.Running:
                    spider.PauseJob();
                    break;
                case JobStatus.Idle:
                    break;
                case JobStatus.Paused:
                    break;
            }
        }

        /// <summary>
        /// 当任务有修改以后，在停止状态下，更换jobid
        /// </summary>
        /// <param name="jobid"></param>
        public static void ReSetJob(int jobid)
        {
            changeJobs.Add(jobid);
        }

        private static ISpider getSpider(int jobid)
        {
            if (spiderDic.ContainsKey(jobid))
            {
                if(!changeJobs.Contains(jobid)) return spiderDic[jobid];
                changeJobs.Remove(jobid);
                spiderDic.Remove(jobid);
            }
            if (JobManager.HasJob(jobid))
            {
                ISpider spider = ia.GetSpider(JobManager.JobList[jobid]);
                spider.InfoChange = InfoChange;
                spider.ProgressChange = ProgressChange;
                spider.JobStatusChange = JobStatusChange;
                spider.ResultChange = ResultChange;
                spider.StatsChange = StatsChange;
                lock (lkSpider)
                {
                    spiderDic.Add(jobid, spider);
                }
                return spider;
            }
            throw new Exception("不存在任务:" + jobid.ToString());
        }

        /// <summary>
        /// 得到任务运行状态
        /// </summary>
        /// <param name="jobid"></param>
        /// <returns></returns>
        public static JobStatus GetJobStatus(int jobid)
        {
            if (!spiderDic.ContainsKey(jobid)) return JobStatus.Idle;
            else
            {
                return spiderDic[jobid].GetJobStatus(); 
            }
        }

        /// <summary>
        /// 删除任务的同时，要删除计划任务，要删除字典中的记录
        /// </summary>
        /// <param name="jobid"></param>
        public static void DeleteJob(int jobid)
        {
            if (spiderDic.ContainsKey(jobid))
            {
                spiderDic[jobid].StopJob();
                spiderDic.Remove(jobid);
            }
        }

        /// <summary>
        /// 当前任务的进度信息
        /// </summary>
        public static Action<int, int, int> ProgressChange;

        /// <summary>
        /// 操作任务当前状态
        /// </summary>
        public static Action<int, YunCore.JobStatus> JobStatusChange;
       
        /// <summary>
        /// 当结果有变化时显示
        /// </summary>
        public static Action<int, YunCore.LogType, string, string> ResultChange;

        /// <summary>
        /// 统计有变量时显示
        /// </summary>
        public static Action<int, string> StatsChange;
       
        /// <summary>
        /// 显示使用信息出去
        /// </summary>
        public static Action<int, string> InfoChange;

        /// <summary>
        /// 最后运行时间
        /// </summary>
        private static Dictionary<int, DateTime> lastRunTime = new Dictionary<int, DateTime>();

        /// <summary>
        /// 最后运行时间
        /// </summary>
        /// <param name="jobid"></param>
        /// <returns></returns>
        public static DateTime GetLastRunTime(int jobid)
        {
            if (lastRunTime.ContainsKey(jobid)) return lastRunTime[jobid];
            return DateTime.Parse("1970-1-1");
        }
    }
}