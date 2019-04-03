using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore
{
    /// <summary>
    /// 数据统计接口
    /// </summary>
    public interface IStats
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        int StatsId { get; set; }

        /// <summary>
        /// 任务id
        /// </summary>
        int JobId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        DateTime EndTime { get; set; }

        /// <summary>
        /// 新记录
        /// </summary>
        int Appends { get; set; }

        /// <summary>
        /// 重复数
        /// </summary>
        int Repeats { get; set; }

        /// <summary>
        /// 发布成功数
        /// </summary>
        int Posts { get; set; }

        /// <summary>
        /// 增加统计数据
        /// </summary>
        bool InsertStats();

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        List<IStats> GetStatsList();
    }
}
