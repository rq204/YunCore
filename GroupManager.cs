using System;
using System.Collections.Generic;
using System.Text;

namespace YunCore
{
    /// <summary>
    /// 站点分组管理，关于manager,全是内存中进行维护一个调用的实例
    /// </summary>
    public class GroupManager
    {
        /// <summary>
        /// 分组操作的锁
        /// </summary>
        private static readonly object groupLock = new object();

        /// <summary>
        /// 添加分组
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        public static void Add(YunCore.IGroup cs)
        {
            lock (groupLock)
            {
                grouplist.Add(cs.GroupId, cs);
            }
        }

        /// <summary>
        /// 编辑分组
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        public static void Edit(YunCore.IGroup cs)
        {
            lock (groupLock)
            {
                if (grouplist.ContainsKey(cs.GroupId)) grouplist[cs.GroupId] = cs;
            }
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public static void Remove(int groupid)
        {
            lock (groupLock)
            {
                grouplist.Remove(groupid);
            }
        }

        private static Dictionary<int, YunCore.IGroup> grouplist = new Dictionary<int, YunCore.IGroup>();
        public static Dictionary<int, YunCore.IGroup> GroupList
        {
            get { return grouplist; }
        }

        /// <summary>
        /// 最站分组的父节点id
        /// </summary>
        public static int MaxParentId
        {
            get
            {
                lock (groupLock)
                {
                    int max = 0;
                    Dictionary<int, YunCore.IGroup>.Enumerator en = grouplist.GetEnumerator();
                    while (en.MoveNext())
                    {
                        int pid = Convert.ToInt32(en.Current.Value.ParentId);
                        if (pid > max) max = pid;
                    }
                    return max;
                }
            }
        }

        /// <summary>
        /// 检查一个节点是否另一个节点的子节点
        /// </summary>
        /// <param name="groupid">节点</param>
        /// <param name="parentid">子节点</param>
        /// <returns></returns>
        public static bool IsChild(int groupid, int childid)
        {
            lock (groupLock)
            {
                Dictionary<int, YunCore.IGroup>.Enumerator en = grouplist.GetEnumerator();
                while (en.MoveNext())
                {
                    if (en.Current.Value.GroupId == childid)
                    {
                        if (en.Current.Value.ParentId == groupid) return true;
                        else if (en.Current.Value.ParentId != 0)
                        {
                            return IsChild(groupid, en.Current.Value.ParentId);
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 重新加载分组
        /// </summary>
        public static void ReloadGroup()
        {
            lock (groupLock)
            {
                List<IGroup> lst = Config.GetIGroup().GetGroupList();
                Dictionary<int, IGroup> dic = new Dictionary<int, IGroup>();
                foreach (IGroup si in lst) dic.Add(si.GroupId, si);
                grouplist = dic;
            }
        }

        /// <summary>
        /// 得到当前节点的子节点
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public static List<int> GetChildGroup(int groupid)
        {
            List<int> ls = new List<int>();
            lock (groupLock)
            {
                Dictionary<int, YunCore.IGroup>.Enumerator en = grouplist.GetEnumerator();
                while (en.MoveNext())
                {
                    if (en.Current.Value.ParentId == groupid && en.Current.Value.GroupId != en.Current.Value.ParentId)
                    {
                        ls.Add(en.Current.Key);
                        ls.AddRange(GetChildGroup(en.Current.Key));
                    }
                }
            }
            return ls;
        }
    }
}