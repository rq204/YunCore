using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore.Table
{
    /// <summary>
    /// 任务运行统计专用,统计开始时间，结束时间，采多少，重复多少，发布多少
    /// </summary>
    [Serializable]
    [PetaPoco.TableName("IStats")]
    [PetaPoco.PrimaryKey("StatsId")]
    public class Stats : IStats
    {
        PetaPoco.Database odb = YunCore.Table.DbConfig.ConfigDb;

        public int StatsId { get; set; }

        public int JobId { get; set; }

        public int Appends { get; set; }

        public int Repeats { get; set; }

        public int Posts { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        private string error = "";
        [PetaPoco.Ignore]
        public string Error
        {
            get { return this.error; }
        }
        
        public bool InsertStats()
        {
            error = "";
            try
            {
                this.odb.Save(this);
                return true;
            }
            catch (Exception f)
            {
                error = f.Message + f.StackTrace;
            }
            return false;
        }

        /// <summary>
        /// 得到列表
        /// </summary>
        /// <returns></returns>
        public List<IStats> GetStatsList()
        {
            try
            {
                List<IStats> ips = new List<IStats>();
                foreach (IStats ip in this.odb.Fetch<Stats>("select * from IStats"))
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
    }
}