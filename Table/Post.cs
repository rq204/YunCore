using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using YunCore;

namespace YunCore.Table
{
    /// <summary>
    /// 数据导出接口，是包含了数据库，文件，web，soap等方式
    /// </summary>
    [Serializable]
    [PetaPoco.TableName("IPost")]
    [PetaPoco.PrimaryKey("PostId")]
    public class Post : IPost
    {
        PetaPoco.Database odb = YunCore.Table.DbConfig.ConfigDb;

        public int PostId { get; set; }

        public string PostName { get; set; }

        public PostType PostType { get; set; }

        public string XmlData { set; get; }

        public DateTime ModifyOn { get; set; }

        public DateTime CreateOn { get; set; }

        private string error = "";
        [PetaPoco.Ignore]
        public string Error
        {
            get { return this.error; }
        }

        public bool SelectPost()
        {
            error = "";
            try
            {
                Post p = this.odb.Single<Post>("select * from IPost where PostId=" + this.PostId + "");
                if (p != null)
                {
                    this.CreateOn = p.CreateOn;
                    this.ModifyOn = p.ModifyOn;
                    this.PostId = p.PostId;
                    this.PostName = p.PostName;
                    this.PostType = p.PostType;
                    this.XmlData = p.XmlData;
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

        public bool InsertPost()
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

        public bool UpdatePost()
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
        public bool DeletePost()
        {
            return DeletePost(this.PostId.ToString());
        }

        public bool DeletePost(string postids)
        {
            error = "";
            try
            {
                this.odb.Execute("delete from IPost where PostId in ({0})", new string[] { postids });
                return true;
            }
            catch (Exception f)
            {
                error = f.Message + f.StackTrace;
            }
            return false;
        }


        public List<IPost> GetPostList()
        {
            try
            {
                List<IPost> ips = new List<IPost>();
                foreach (IPost ip in this.odb.Fetch<Post>("select * from IPost"))
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