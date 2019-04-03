using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore.Table
{
    [Serializable]
    [PetaPoco.TableName("ILog")]
    [PetaPoco.PrimaryKey("LogId")]
    public class Log : Ilog
    {
        public int LogId { get; set; }

        /// <summary>
        /// 新增记录数
        /// </summary>
        public int Append { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 任务id
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        /// 重复记录数
        /// </summary>
        public int Repeat { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        private string error = "";
        public bool InsertLog()
        {
            error = "";
            PetaPoco.Database odb = new PetaPoco.Database(DbConfig.DBConnectStr, DbConfig.DBProvider);
            try
            {
                odb.Save(this);
                return true;
            }
            catch (Exception f)
            {
                error = f.Message + f.StackTrace;
            }
            finally
            {
                try
                {
                    odb.Dispose();
                }
                catch { }
            }
            return false;
        }

        public List<Ilog> SelectLog(int jobid)
        {
            error = "";
            List<Ilog> logs = null;
            PetaPoco.Database odb = new PetaPoco.Database(DbConfig.DBConnectStr, DbConfig.DBProvider);
            try
            {
                logs = new List<Ilog>();
                List<Log> ll = odb.Fetch<Log>("seletc * from ILog where JobId= " + jobid.ToString());
                foreach (Log l in ll) logs.Add(l);
            }
            catch (Exception f)
            {
                error = f.Message + f.StackTrace;
            }
            finally
            {
                try
                {
                    odb.Dispose();
                }
                catch { }
            }
            return logs;
        }
    }
}
