using System;
using System.Threading;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.6.11
 * 
 * 2020.2.14 挂起功能添加
 *
 * 功能：可让线程安全退出
 */

namespace SDHK_Tool.Dynamic
{

    /// <summary>
    /// 标记式线程
    /// </summary>
    public class SD_Thread
    {
        /// <summary>
        /// 线程启动标记：线程启动\结束
        /// </summary>
		public bool isStart = false;

        /// <summary>
        /// 线程运行标记：线程运行\挂起
        /// </summary>
        public bool isRun = false;

        /// <summary>
        /// 线程句柄
        /// </summary>
        public Thread thread;

        /// <summary>
        /// 新建线程
        /// </summary>
        /// <param name="action">委托方法</param>
        public SD_Thread(Action<SD_Thread> action)
        {
            thread = new Thread(() =>
            {
                while (isStart)
                {
                    if (isRun)
                    {
                        action(this);
                        isRun = false;
                    }
                }
            })
            { IsBackground = true };//设置为后台线程

            isStart = true;
            thread.Start();
        }

        /// <summary>
        /// 线程运行
        /// </summary>
        public void Run()
        {
            isRun = true;
        }

        /// <summary>
        /// 线程挂起
        /// </summary>
        public void Stop()
        {
            isRun = false;
        }

        /// <summary>
        /// 线程退出
        /// </summary>
        public void End()
        {
            isRun = false;
            isStart = false;
            thread.Abort();
            thread.Join();
        }

    }
}