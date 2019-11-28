using System;
using System.Threading;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.6.11
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
		public bool isStart = false;
        public Thread thread;

		//线程添加
        public SD_Thread(Action<SD_Thread> action)
        {
            thread = new Thread(() => { action(this); })
            { IsBackground = true };//设置为后台线程
        }

        /// <summary>
        /// 线程启动
        /// </summary>
        public void Start()
        {
            isStart = true;
            thread.Start();
        }

        /// <summary>
        /// 线程退出
        /// </summary>
        public void End()
        {
            isStart = false;
        }

    }
}