using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Data;
using System.IO;
using YunCore;

namespace YunCore.Table
{
    /* SortedDictionary 需要比较器实现来执行键比较。可以使用一个接受 comparer 参数的构造函数来指定 IComparer 泛型接口的实现；如果不指定实现，则使用默认的泛型比较器 Comparer.Default。如果类型 TKey 实现 System.IComparable 泛型接口，则默认比较器使用该实现。*/
    /// <summary>
    /// 任务配置，xmldata由具体实现来生成
    /// </summary>
    [Serializable]
    [PetaPoco.TableName("IJob")]
    [PetaPoco.PrimaryKey("JobId")]
    public class Job : IJob
    {
        private PetaPoco.Database odb = DbConfig.ConfigDb;

        public int JobId { get; set; }


        public string JobName { get; set; }

        public int GroupId { get; set; }


        public string XmlData { get; set; }

        /// <summary>
        /// 排序，按从小到大 自上而下排序
        /// </summary>
        public decimal ListOrder { get; set; }

        public DateTime CreateOn { get; set; }

        public DateTime ModifyOn { get; set; }

        private string error = "";
        [PetaPoco.Ignore]
        public string Error
        {
            get { return this.error; }
        }

        /// <summary>
        /// Selects the job
        /// </summary>
        /// <returns></returns>
        public bool SelectJob()
        {
            this.error = "";
            try
            {
                Job p = odb.Single<Job>("select * from IJob where JobId=" + this.JobId.ToString());
                if (p != null)
                {
                    this.CreateOn = p.CreateOn;
                    this.ModifyOn = p.ModifyOn;
                    this.JobId = p.JobId;
                    this.JobName = p.JobName;
                    this.ListOrder = p.ListOrder;
                    this.XmlData = p.XmlData;
                    this.GroupId = p.GroupId;
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

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <returns>添加Web发布配置的ID</returns>
        /// 
        public bool InsertJob()
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

        /// <summary>
        /// 更新Job表
        /// </summary>
        /// <returns>是否更新</returns>
        public bool UpdateJob()
        {
            this.error = "";
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
        /// 删除任务
        /// </summary>
        /// <returns></returns>
        public bool DeleteJob()
        {
            return this.DeleteJob(new List<int> { this.JobId });
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <returns>是否成功</returns>
        public bool DeleteJob(List<int> jobids)
        {
            List<string> jobidslst = new List<string>();
            foreach (int j in jobids) jobidslst.Add(j.ToString());
            this.error = "";
            try
            {
                this.odb.Execute(string.Format("delete from IJob where JobId in ({0})", string.Join(",", jobidslst.ToArray())));
                return true;
            }
            catch (Exception f)
            {
                error = f.Message + f.StackTrace;
            }
            return false;
        }

        public bool DeleteJobBySite(int groupid)
        {
            this.error = "";
            try
            {
                this.odb.Execute("delete from IJob where GroupId ={0}", new string[] { groupid.ToString() });
                return true;
            }
            catch (Exception f)
            {
                error = f.Message + f.StackTrace;
            }
            return false;
        }

        /// <summary>
        /// 返回null是出错的
        /// </summary>
        /// <returns></returns>
        public List<IJob> GetJobList()
        {
            this.error = "";
            try
            {
                List<IJob> ljb = new List<IJob>();
                foreach (Job ip in this.odb.Fetch<Job>("select * from IJob"))
                {
                    ljb.Add(ip);
                }
                return ljb;
            }
            catch (Exception f)
            {
                error = f.Message + f.StackTrace;
            }
            return null;
        }

    }
}