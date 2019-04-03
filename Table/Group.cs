using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;
using YunCore;


namespace YunCore.Table
{
    /// <summary>
    /// 分组，任务是归于分组下的，属于无限级分组
    /// </summary>
    [Serializable]
    [PetaPoco.TableName("IGroup")]
    [PetaPoco.PrimaryKey("GroupId")]
    public class Group : IGroup
    {
        private PetaPoco.Database odb = DbConfig.ConfigDb;

        public int GroupId { set; get; }

        public string GroupName { set; get; }

        public string GroupMemo { set; get; }

        public int ParentId { set; get; }

        /// <summary>
        /// 排序，按从小到大 自上而下排序
        /// </summary>
        public decimal ListOrder { get; set; }

        public DateTime CreateOn { set; get; }

        public DateTime ModifyOn { set; get; }

        private string error = "";
        [PetaPoco.Ignore]
        public string Error
        {
            get
            {
                return this.error;
            }
        }


        /// <summary>
        /// 加载站点
        /// </summary>
        /// <returns></returns>
        public bool SelectGroup()
        {
            this.error = "";
            try
            {
                Group p = this.odb.Single<Group>("select * from Group where GroupId='" + this.GroupId + "'");
                if (p != null)
                {
                    this.CreateOn = p.CreateOn;
                    this.ModifyOn = p.ModifyOn;
                    this.GroupId = p.GroupId;
                    this.GroupMemo = p.GroupMemo;
                    this.GroupName = p.GroupName;
                    this.ListOrder = p.ListOrder;
                    this.ParentId = p.ParentId;
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
        /// 添加站点
        /// </summary>
        /// <returns></returns>
        public bool InsertGroup()
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
        /// 更新Site表
        /// </summary>
        /// <returns>是否更新</returns>
        public bool UpdateGroup()
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
        /// 删除站点
        /// </summary>
        /// <returns>是否成功</returns>
        public bool DeleteGroup()
        {
            return DeleteGroup(this.GroupId.ToString());
        }


        /// <summary>
        /// 删除站点
        /// </summary>
        /// <returns>是否成功</returns>
        public bool DeleteGroup(string groupids)
        {
            this.error = "";
            try
            {
                this.odb.Execute("delete from IGroup where GroupId in ({0})", new string[] { groupids });
                return true;
            }
            catch (Exception f)
            {
                error = f.Message + f.StackTrace;
            }
            return false;
        }

        public List<IGroup> GetGroupList()
        {
            error = "";
            try
            {
                List<IGroup> ips = new List<IGroup>();
                foreach (IGroup ip in this.odb.Fetch<Group>("select * from IGroup"))
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