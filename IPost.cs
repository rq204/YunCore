using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore
{
    /// <summary>
    /// 数据导出接口
    /// </summary>
    public interface IPost
    {
        //导出id
        int PostId { set; get; }

        /// <summary>
        /// 导出名称
        /// </summary>
        string PostName { get; set; }

        /// <summary>
        /// 导出类型
        /// </summary>
        PostType PostType { set; get; }

        /// <summary>
        /// 配置数据
        /// </summary>
        string XmlData { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateOn { set; get; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        DateTime ModifyOn { set; get; }


        /// <summary>
        /// 出错信息
        /// </summary>
        string Error { get; }

        /// <summary>
        /// 选择当前发布
        /// </summary>
        /// <returns></returns>
        bool SelectPost();

        /// <summary>
        /// 添加新记录
        /// </summary>
        /// <returns></returns>
        bool InsertPost();

        /// <summary>
        /// 更新Post表
        /// </summary>
        /// <returns>是否更新</returns>
        bool UpdatePost();

        /// <summary>
        /// 删除当前Post
        /// </summary>
        /// <returns></returns>
        bool DeletePost();

        /// <summary>
        /// 删除任务,返回null为成功
        /// </summary>
        /// <returns>是否成功</returns>
        bool DeletePost(string deleteids);

        /// <summary>
        /// 获取post列表
        /// </summary>
        /// <returns></returns>
        List<IPost> GetPostList();
    }
}