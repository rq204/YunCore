using System;
using System.Collections.Generic;
using System.Text;

namespace YunCore
{
    /// <summary>
    /// 任务管理,关于manager,全是内存中进行维护一个调用的实例
    /// </summary>
    public class JobManager
    {
        /// <summary>
        /// 可用任务操作的锁
        /// </summary>
        public static readonly object jobLock = new object();

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="cj"></param>
        /// <returns></returns>
        public static void Add(YunCore.IJob cj)
        {
            lock (jobLock)
            {
                if (!joblist.ContainsKey(cj.JobId)) joblist.Add(cj.JobId, cj);
            }
        }

        /// <summary>
        /// 编辑任务
        /// </summary>
        /// <param name="cj"></param>
        /// <returns></returns>
        public static void Edit(YunCore.IJob cj)
        {
            lock (jobLock)
            {
                if (joblist.ContainsKey(cj.JobId)) joblist[cj.JobId] = cj;
            }
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="siteid"></param>
        /// <returns></returns>
        public static void Remove(int jobid)
        {
            lock (jobLock)
            {
                joblist.Remove(jobid);
            }
        }

        /// <summary>
        /// 是否有某个任务
        /// </summary>
        /// <param name="jobid"></param>
        /// <returns></returns>
        public static bool HasJob(int jobid)
        {
            lock (jobLock)
            {
                return joblist.ContainsKey(jobid);
            }
        }

        private static Dictionary<int, YunCore.IJob> joblist = new Dictionary<int, YunCore.IJob>();
        /// <summary>
        /// 正常的任务
        /// </summary>
        public static Dictionary<int, YunCore.IJob> JobList
        {
            get
            {
                lock (jobLock)
                {
                    return joblist;
                }
            }
        }


        /// <summary>
        /// 重新加载任务数据
        /// </summary>
        public static void ReloadJob()
        {
            lock (jobLock)
            {
                List<IJob> lst = Config.GetIJob().GetJobList();
                Dictionary<int, IJob> dic = new Dictionary<int, IJob>();
                foreach (IJob ij in lst) dic.Add(ij.JobId, ij);
                joblist = dic;
            }
        }
    }
}