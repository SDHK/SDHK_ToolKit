
/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/02 14:33:43

 * 最后日期: 2021/12/29 15:03:31

 * 最后修改: 闪电黑客

 * 描述:  
 
    Async/Await的功能扩展
    在异步函数内，随意切换协程与线程两种执行方式

    使用:

        await AsyncMode.ToUnity; //使后续代码，切换为unity主线程执行
    
        await AsyncMode.ToThread; //使后续代码，切换为线程执行：内部会通过线程池申请个新线程。
    

    感觉可以脱离回调地狱了... ヽ(✿ﾟ▽ﾟ)ノ

******************************/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace AsyncAwaitEvent
{

    /// <summary>
    /// AsyncAwait 的 执行模式
    /// </summary>
    public class AsyncMode : IAsyncAwaitNode
    {
        #region static
        public static int UnityThreadID { get; private set; }
        public static int CurrentThreadID => Thread.CurrentThread.ManagedThreadId;
        public static SynchronizationContext ContextUnity { get; private set; }
        public static SynchronizationContext ContextThread { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]//初始化自动执行
        public static void AsyncModeInitlize()
        {
            UnityThreadID = Thread.CurrentThread.ManagedThreadId;//获取unity的线程ID
            ContextUnity = SynchronizationContext.Current;//获取unity的线程上下文Context
            ContextThread = new SynchronizationContext();
        }

        /// <summary>
        /// 切换到Untiy主线程执行
        /// </summary>
        public static AsyncMode ToUnity => new AsyncMode(ContextUnity);

        /// <summary>
        /// 切换到线程执行
        /// </summary>
        public static AsyncMode ToThread => new AsyncMode(ContextThread);

        # endregion

        public AsyncMode GetAwaiter() => this; //await需要这个;

        private SynchronizationContext context;//此节点的上下文


        public bool IsCompleted => context == SynchronizationContext.Current;
        public AsyncMode(SynchronizationContext context)
        {
            this.context = context;
        }

        public void OnCompleted(Action continuation)
        {
            context.Post(PostCallBack, continuation);//启动一个线程回调
        }

        private void PostCallBack(object continuation)
        {
            ((Action)continuation)();//继续运行
        }

        public void GetResult() { }

    }
}