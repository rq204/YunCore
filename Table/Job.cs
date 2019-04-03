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
    /* SortedDictionary ��Ҫ�Ƚ���ʵ����ִ�м��Ƚϡ�����ʹ��һ������ comparer �����Ĺ��캯����ָ�� IComparer ���ͽӿڵ�ʵ�֣������ָ��ʵ�֣���ʹ��Ĭ�ϵķ��ͱȽ��� Comparer.Default��������� TKey ʵ�� System.IComparable ���ͽӿڣ���Ĭ�ϱȽ���ʹ�ø�ʵ�֡�*/
    /// <summary>
    /// �������ã�xmldata�ɾ���ʵ��������
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
        /// ���򣬰���С���� ���϶�������
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
                    this.error = "û�в��ҵ���¼";
                }
            }
            catch (Exception f)
            {
                error = f.Message + f.StackTrace;
            }
            return false;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns>���Web�������õ�ID</returns>
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
        /// ����Job��
        /// </summary>
        /// <returns>�Ƿ����</returns>
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
        /// ɾ������
        /// </summary>
        /// <returns></returns>
        public bool DeleteJob()
        {
            return this.DeleteJob(new List<int> { this.JobId });
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <returns>�Ƿ�ɹ�</returns>
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
        /// ����null�ǳ����
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