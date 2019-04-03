using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore
{
    /// <summary>
    /// 接口操作类
    /// </summary>
    public interface IJob
    {
        /// <summary>
        /// 任务id
        /// </summary>
        int JobId { get; set; }

        /// <summary>
        /// 任务名
        /// </summary>
        string JobName { get; set; }

        /// <summary>
        /// 分组ID
        /// </summary>
        int GroupId { get; set; }

        /// <summary>
        /// 应用配置文件
        /// </summary>
        string XmlData { get; set; }

        /// <summary>
        /// 排序，按从小到大 自上而下排序 算法设计：移动到两个节点之间时 利用两个节点的listorder值相除得到新节点的listorder值
        /// </summary>
        decimal ListOrder { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        DateTime ModifyOn { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateOn { set; get; }

        /// <summary>
        /// 出错信息
        /// </summary>
        string Error { get; }

        /// <summary>
        /// 选择当前任务
        /// </summary>
        /// <returns></returns>
        bool SelectJob();

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <returns></returns>
        bool InsertJob();

        /// <summary>
        /// 更新Job表
        /// </summary>
        /// <returns>是否更新</returns>
        bool UpdateJob();

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <returns>是否成功</returns>
        bool DeleteJob();

        /// <summary>
        /// 删除任务,返回null为成功
        /// </summary>
        /// <returns>是否成功</returns>
        bool DeleteJob(List<int> jobids);

        /// <summary>
        /// 按分组id删除任务,返回null为成功
        /// </summary>
        /// <returns>是否成功</returns>
        bool DeleteJobBySite(int siteid);

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <returns></returns>
        List<IJob> GetJobList();
    }
}
