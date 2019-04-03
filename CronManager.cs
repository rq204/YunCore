using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore
{
    /// <summary>
    /// 对计划计划任务进行统一管理处，为什么用manager,因为它和数据库和cron实例无关
    /// 关于manager,全是内存中进行维护一个调用的实例
    /// </summary>
    public class CronManager
    {
        /// <summary>
        /// 可用计划计划任务操作的锁
        /// </summary>
        public static readonly object cronLock = new object();

        /// <summary>
        /// 添加计划任务
        /// </summary>
        /// <param name="cj"></param>
        /// <returns></returns>
        public static void Add(YunCore.ICron cj)
        {
            lock (cronLock)
            {
                if (!cronlist.ContainsKey(cj.CronId))
                {
                    cronlist.Add(cj.CronId, cj);
                    ReSetCron(cj);
                }
            }
        }

        /// <summary>
        /// 编辑计划任务
        /// </summary>
        /// <param name="cj"></param>
        /// <returns></returns>
        public static void Edit(YunCore.ICron cj)
        {
            lock (cronLock)
            {
                if (cronlist.ContainsKey(cj.CronId))
                {
                    cronlist[cj.CronId] = cj;
                    ReSetCron(cj);
                }
            }
        }

        /// <summary>
        /// 删除计划任务
        /// </summary>
        /// <param name="CronId"></param>
        /// <returns></returns>
        public static void Remove(int CronId)
        {
            lock (cronLock)
            {
                cronlist.Remove(CronId);
            }

            List<CronStruct> removes = new List<CronStruct>();
            foreach (CronStruct kv in timeList.ToArray())
            {
                if (kv.CronId == CronId) removes.Add(kv);
            }
            if (removes.Count > 0)
            {
                lock (timeLock)
                {
                    foreach (CronStruct key in removes) timeList.Remove(key);
                }
                removes.Clear();
            }
        }

        /// <summary>
        /// 是否有某个计划任务
        /// </summary>
        /// <param name="CronId"></param>
        /// <returns></returns>
        public static bool HasCron(int CronId)
        {
            lock (cronLock)
            {
                return cronlist.ContainsKey(CronId);
            }
        }

        /// <summary>
        /// 是否有任务id
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public static bool HasJob(int jobId)
        {
            return cronlist.Values.ToList().FindAll(a => a.JobId == jobId).Count > 0;
        }

        /// <summary>
        /// 获取任务下次运行时间
        /// </summary>
        /// <param name="jobid"></param>
        /// <returns></returns>
        public static List<DateTime> GetJobNextRunTime(int jobid)
        {
            List<DateTime> times = new List<DateTime>();
            foreach (YunCore.ICron ic in cronlist.Values.ToArray())
            {
                if (ic.JobId == jobid) times.AddRange(GetNextRunTime(ic, 1));
            }
            times.Sort();
            return times;
        }


        private static Dictionary<int, YunCore.ICron> cronlist = new Dictionary<int, YunCore.ICron>();
        /// <summary>
        /// 正常的计划任务
        /// </summary>
        public static Dictionary<int, YunCore.ICron> CronList
        {
            get
            {
                lock (cronLock)
                {
                    return cronlist;
                }
            }
        }


        /// <summary>
        /// 重新加载计划任务数据
        /// </summary>
        public static void ReloadCron()
        {
            lock (cronLock)
            {
                List<ICron> lst = Config.GetICron().GetCronList();
                Dictionary<int, ICron> dic = new Dictionary<int, ICron>();
                foreach (ICron ij in lst) dic.Add(ij.CronId, ij);
                cronlist = dic;
            }
        }

        /// <summary>
        /// 得到计划任务的分组,并使用默认分组排序方式
        /// </summary>
        /// <returns></returns>
        public static List<string> GetGronGroups()
        {
            lock (cronLock)
            {
                List<string> names = new List<string>();
                foreach (YunCore.ICron ic in cronlist.Values)
                {
                    if (!names.Contains(ic.CronGroup)) names.Add(ic.CronGroup);
                }
                names.Sort();
                return names;
            }
        }

        /// <summary>
        /// 按分组名得到计划任务
        /// </summary>
        /// <param name="groupname"></param>
        /// <returns></returns>
        public static List<ICron> GetCronByGroup(string groupname)
        {
            List<ICron> names = new List<ICron>();
            foreach (YunCore.ICron ic in cronlist.Values.ToArray())
            {
                if (ic.CronGroup == groupname) names.Add(ic);
            }
            return names;
        }

        /// <summary>
        /// 删除所有
        /// </summary>
        /// <param name="groupname"></param>
        public static void DeleteCronByGroup(string groupname)
        {
            List<int> cids = new List<int>();
            lock (cronLock)
            {
                foreach (int id in cronlist.Keys.ToArray())
                {
                    if (cronlist[id].CronGroup == groupname)
                    {
                        cronlist.Remove(id);
                        cids.Add(id);
                    }
                }
            }
            lock (timeLock)
            {
                List<CronStruct> delete = new List<CronStruct>();
                foreach (CronStruct cs in timeList.ToArray())
                {
                    if (cids.Contains(cs.CronId)) timeList.Remove(cs);
                }
            }
        }

        /// <summary>
        /// 删除所有
        /// </summary>
        /// <param name="jobid"></param>
        public static void DeleteCronByJobId(int jobid)
        {
            List<int> cids = new List<int>();
            lock (cronLock)
            {
                foreach (ICron ic in cronlist.Values)
                {
                    if (ic.JobId == jobid) cids.Add(ic.CronId);
                }
                foreach (int j in cids) cronlist.Remove(j);
            }

            lock (timeLock)
            {
                List<CronStruct> delete = timeList.FindAll(a => a.JobId == jobid);
                foreach (CronStruct cs in delete)
                {
                    timeList.Remove(cs);
                }
            }
        }

        public static List<ICron> GetCronByJobId(int jobid)
        {
            return cronlist.Values.ToArray().ToList().FindAll(a => a.JobId == jobid);
        }

        /// <summary>
        /// 全局计划任务开启还是关闭
        /// </summary>
        public static bool CronEnable = true;
        private static bool cronRunning = false;

        /// <summary>
        /// 启用计划任务功能,一天计算一次
        /// </summary>
        public static void StartCron()
        {
            cronRunning = true;
            if (timer == null)
            {
                timer = new System.Threading.Timer(new System.Threading.TimerCallback(MakeCron), null, 3000, 3600 * 1000 * 24);
                new System.Threading.Thread(new System.Threading.ThreadStart(RunCron)).Start();
            }
        }

        /// <summary>
        /// 关闭所有计划任务功能
        /// </summary>
        public static void StopCron()
        {
            cronRunning = false;
            CronEnable = false;
        }

        /// <summary>
        /// 修改计划任务后，重设定时间
        /// </summary>
        private static void ReSetCron(ICron ic)
        {
            //获取新的，删除旧的
            List<CronStruct> adds = GetRunTime(ic, LastMakeTime, LastMakeTime.AddHours(24));
            List<CronStruct> removes = new List<CronStruct>();
            foreach (CronStruct kv in timeList.ToArray())
            {
                if (kv.CronId == ic.CronId) removes.Add(kv);
            }
            if (removes.Count > 0 || adds.Count > 0)
            {
                lock (timeLock)
                {
                    foreach (CronStruct key in removes) timeList.Remove(key);
                    timeList.AddRange(adds);
                }
            }
        }


        /// <summary>
        /// 获取最新运行时间
        /// </summary>
        /// <param name="ic"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public static List<DateTime> GetNextRunTime(ICron ic, int top = 100)
        {
            List<DateTime> lst = new List<DateTime>();

            DateTime final = ic.LastEnd.Date.Add(ic.DayEnd.TimeOfDay);
            DateTime start = ic.FirstStart.Date.Add(ic.DayStart.TimeOfDay);
            DateTime dayend = start.Date.Add(ic.DayEnd.TimeOfDay);
            DateTime daystart = start;

            while (true)
            {
                if (start >= DateTime.Now && start <= dayend && start >= daystart)
                {
                    lst.Add(start);
                }
                switch (ic.CronType)
                {
                    case CronType.Days:
                        start = start.AddDays(ic.Intervals);
                        break;
                    case CronType.Hours:
                        start = start.AddHours(ic.Intervals);
                        break;
                    case CronType.Miniutes:
                        start = start.AddMinutes(ic.Intervals);
                        break;
                    case CronType.Seconds:
                        start = start.AddSeconds(ic.Intervals);
                        break;
                }
                dayend = start.Date.Add(ic.DayEnd.TimeOfDay);
                daystart = start.Date.Add(ic.DayStart.TimeOfDay);

                if (start > final) break;
                if (lst.Count >= top) break;
            }
            return lst;
        }

        /// <summary>
        /// 以start时间为准+24小时,时间大于当前时间，等于下24小时
        /// </summary>
        /// <param name="ic"></param>
        /// <returns></returns>
        private static List<CronStruct> GetRunTime(YunCore.ICron ic, DateTime startTime, DateTime endTime)
        {
            List<CronStruct> times = new List<CronStruct>();

            DateTime final = ic.LastEnd.Date.Add(ic.DayEnd.TimeOfDay);
            DateTime start = ic.FirstStart.Date.Add(ic.DayStart.TimeOfDay);
            DateTime dayend = start.Date.Add(ic.DayEnd.TimeOfDay);
            DateTime daystart = start;

            if (final < startTime) return times;

            while (true)
            {
                if (start > endTime) break;
                if (start > startTime && start >= daystart && start <= dayend && start <= endTime)
                {
                    times.Add(new CronStruct(ic.JobId, ic.CronId, start));
                }
                switch (ic.CronType)
                {
                    case CronType.Days:
                        start = start.AddDays(ic.Intervals);
                        break;
                    case CronType.Hours:
                        start = start.AddHours(ic.Intervals);
                        break;
                    case CronType.Miniutes:
                        start = start.AddMinutes(ic.Intervals);
                        break;
                    case CronType.Seconds:
                        start = start.AddSeconds(ic.Intervals);
                        break;
                }
                dayend = start.Date.Add(ic.DayEnd.TimeOfDay);
                daystart = start.Date.Add(ic.DayStart.TimeOfDay);
            }

            return times;
        }


        private static System.Threading.Timer timer = null;

        private static DateTime LastMakeTime = DateTime.Now;
        private static void MakeCron(object obj)
        {
            //以当前时间为基准，先算出当至下一个小时内的任务并加入timeList
            LastMakeTime = DateTime.Now;
            List<CronStruct> times = new List<CronStruct>();
            foreach (YunCore.ICron ic in cronlist.Values.ToArray())
            {
                if (ic.CronEnable)
                {
                    times.AddRange(GetRunTime(ic, LastMakeTime, LastMakeTime.AddHours(24)));
                }
            }

            lock (timeLock)
            {
                timeList.AddRange(times);
            }
        }


        /// <summary>
        /// 所有计划任务时间信息，是任务id和时间
        /// </summary>
        private static List<CronStruct> timeList = new List<CronStruct>();

        /// <summary>
        /// 当任务运行时忽略或是等待
        /// </summary>
        public static bool IgnoreWhenJobRun = true;
        private static object timeLock = new object();
        private static void RunCron()
        {
            while (true)
            {
                List<CronStruct> removes = new List<CronStruct>();
                foreach (CronStruct kv in timeList.ToArray())
                {
                    if (kv.Time <= DateTime.Now)
                    {
                        if (JobManager.HasJob(kv.JobId))
                        {
                            if (cronRunning)
                            {
                                if (IgnoreWhenJobRun)
                                {
                                    RunManager.StartJob(kv.JobId);
                                    removes.Add(kv);
                                }
                                else
                                {
                                    YunCore.JobStatus js = RunManager.GetJobStatus(kv.JobId);
                                    if (js == JobStatus.Idle)
                                    {
                                        RunManager.StartJob(kv.JobId);
                                        removes.Add(kv);
                                    }
                                }
                            }
                            else//如果关闭的话，所的有直接跳过
                            {
                                removes.Add(kv);
                            }
                        }
                        else removes.Add(kv);
                    }
                }

                if (removes.Count > 0)
                {
                    lock (timeLock)
                    {
                        foreach (CronStruct k in removes) timeList.Remove(k);
                    }
                }

                System.Threading.Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 更新分组名
        /// </summary>
        /// <param name="old"></param>
        /// <param name="now"></param>
        public static void UpdateCronGroup(string old, string now)
        {
            lock (cronLock)
            {
                foreach (YunCore.ICron ic in cronlist.Values.ToList())
                {
                    if (ic.CronGroup == old) ic.CronGroup = now;
                }
            }
        }
    }
}