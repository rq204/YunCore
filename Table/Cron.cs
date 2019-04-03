using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore.Table
{
    /// <summary>
    /// 计划任务，只有简单的间隔多久，同时一个任务可以有多个设置
    /// </summary>
    [PetaPoco.TableName("ICron")]
    [PetaPoco.PrimaryKey("CronId")]
    public class Cron : YunCore.ICron
    {
        private PetaPoco.Database odb = DbConfig.ConfigDb;

        public int CronId { set; get; }

        public int JobId { set; get; }

        public CronType CronType { set; get; }

        public int Intervals { set; get; }

        public DateTime FirstStart { set; get; }

        public DateTime LastEnd { set; get; }

        public DateTime DayStart { set; get; }

        public DateTime DayEnd { set; get; }

        public bool CronEnable { set; get; }

        public string CronGroup { set; get; }

        public DateTime ModifyOn { get; set; }

        public DateTime CreateOn { get; set; }

        private string error = "";
        [PetaPoco.Ignore]
        public string Error
        {
            get { return this.error; }
        }

        public bool SelectCron()
        {
            error = "";
            try
            {
                Cron p = this.odb.Single<Cron>("select * from ICron where CronId=" + this.CronId + "");
                if (p != null)
                {
                    this.CreateOn = p.CreateOn;
                    this.ModifyOn = p.ModifyOn;
                    this.CronId = p.CronId;
                    this.Intervals = p.Intervals;
                    this.CronType = p.CronType;
                    this.FirstStart = p.FirstStart;
                    this.LastEnd = p.LastEnd;
                    this.DayStart = p.DayStart;
                    this.DayEnd = p.DayEnd;
                    this.CronEnable = p.CronEnable;
                    this.ModifyOn = p.ModifyOn;
                    this.CreateOn = p.CreateOn;
                    return true;
                }
                else
                {
                    this.error = "没有查找到记录";
                }
            }
            catch (Exception f)
            {
                error = f.Message + f.StackTrace;
            }
            return false;
        }

        public bool InsertCron()
        {
            error = "";
            try
            {
                this.CreateOn = this.ModifyOn = DateTime.Now;//.ToString("yyyy-MM-dd HH:mm:ss");
                this.odb.Save(this);
                return true;
            }
            catch (Exception f)
            {
                error = f.Message + f.StackTrace;
            }
            return false;
        }

        public bool UpdateCron()
        {
            error = "";
            try
            {
                this.ModifyOn = DateTime.Now;//.ToString("yyyy-MM-dd HH:mm:ss");
                this.odb.Update(this);
                return true;
            }
            catch (Exception f)
            {
                error = f.Message + f.StackTrace;
            }
            return false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public bool DeleteCron()
        {
            error = "";
            try
            {
                this.odb.Delete(this);
                return true;
            }
            catch (Exception f)
            {
                error = f.Message + f.StackTrace;
            }
            return false;
        }

        public bool DeleteCron(string Cronids)
        {
            error = "";
            try
            {
                this.odb.Execute("delete from ICron where CronId in (@0)", new string[] { Cronids });
                return true;
            }
            catch (Exception f)
            {
                error = f.Message + f.StackTrace;
            }
            return false;
        }


        public List<ICron> GetCronList()
        {
            try
            {
                List<ICron> ips = new List<ICron>();
                foreach (ICron ip in this.odb.Fetch<Cron>("select * from ICron"))
                {
                    ips.Add(ip);
                }
                return ips;
            }
            catch (Exception f)
            {
                error = f.Message + f.StackTrace;
            }
            return null;
        }

        /// <summary>
        /// 删除指定分组的计划任务
        /// </summary>
        /// <param name="crongroup"></param>
        /// <returns></returns>
        public bool DeleteCronBySite(string crongroup)
        {
            this.error = "";
            try
            {
                this.odb.Execute("delete from ICron where CronGroup ='@0'", new string[] { crongroup });
                return true;
            }
            catch (Exception f)
            {
                error = f.Message + f.StackTrace;
            }
            return false;
        }
    }
}