using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore
{
    /// <summary>
    /// 日志类型，比如重复，成功，失败，
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 重复
        /// </summary>
        Repeat,
        /// <summary>
        /// 成功
        /// </summary>
        Sucess,
        /// <summary>
        /// 失败
        /// </summary>
        Faild
    }
}
