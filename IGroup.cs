using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore
{
    /// <summary>
    /// 分组实现
    /// </summary>
    public interface IGroup
    {
        /// <summary>
        /// 分组id
        /// </summary>
        int GroupId { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        string GroupName { get; set; }

        /// <summary>
        /// 分组描述
        /// </summary>
        string GroupMemo { get; set; }

        /// <summary>
        /// 上级节点
        /// </summary>
        int ParentId { get; set; }

        /// <summary>
        /// 排序，按从小到大 自上而下排序
        /// </summary>
        decimal ListOrder { get; set; }
        /// <summary>
        /// 最后修改
        /// </summary>
        DateTime ModifyOn { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateOn { set; get; }

        /// <summary>
        /// 分组操作中出错信息
        /// </summary>
        string Error { get; }

        /// <summary>
        /// 加载站点
        /// </summary>
        /// <returns></returns>
        bool SelectGroup();

        /// <summary>
        /// 添加站点
        /// </summary>
        /// <returns></returns>
        bool InsertGroup();

        /// <summary>
        /// 更新Site表
        /// </summary>
        /// <returns>是否更新</returns>
        bool UpdateGroup();

        /// <summary>
        /// 删除站点
        /// </summary>
        /// <returns>是否成功</returns>
        bool DeleteGroup();

        /// <summary>
        /// 删除站点
        /// </summary>
        /// <returns>是否成功</returns>
        bool DeleteGroup(string groupids);

        /// <summary>
        /// 获取分组列表
        /// </summary>
        /// <returns></returns>
        List<IGroup> GetGroupList();
    }
}
