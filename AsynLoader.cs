using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YunCore
{
    /// <summary>
    /// 异步加载列表
    /// </summary>
    public class AsynLoader
    {
        private static bool loadOK = false;
        /// <summary>
        /// 是否完全加载完成
        /// </summary>
        public static bool LoadOK
        {
            get { return loadOK; }
        }

        private static bool isStart = false;

        /// <summary>
        /// 启动事件列表，最后要用loadOK来判断是否完成
        /// </summary>
        public static void StartLoader()
        {
            if (isStart) return;
            isStart = true;
            new System.Threading.Thread(() =>
                {
                    if (LoadAction != null) LoadAction();
                    loadOK = true;
                }).Start();
        }
        /// <summary>
        /// 要加载的事件列表
        /// </summary>
        public static Action LoadAction;
    }
}