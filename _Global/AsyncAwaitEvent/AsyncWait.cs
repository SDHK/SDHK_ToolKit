
/******************************

 * 作者: 闪电黑客

 * 日期: 2021/12/02 16:29:00

 * 最后日期: 。。。

 * 描述: 
    
    Async/Await的功能扩展
    等待 Func<bool> 返回true

    使用:
        await new AsyncWait(()=>true);
        
        await func;
    

******************************/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;



namespace AsyncAwaitEvent
{
    public static class AsyncWaitExtension
    {
        public static AsyncWait GetAwaiter(this Func<bool> func)
        {

            return new AsyncWait(func);
        }

     
    }

    /// <summary>
    /// AsyncAwait 的 Func(bool) 等待事件
    /// </summary>
    public class AsyncWait : IAsyncAwaitNode
    {
        public AsyncWait GetAwaiter() => this; //await需要这个;

        public bool IsCompleted => isDone;
        private bool isDone = false;

        private Func<bool> func;
        public AsyncWait(Func<bool> func)
        {
            this.func = func;
        }

        public void OnCompleted(Action continuation)
        {
            CoroutineFunc(continuation);
        }

        private async void CoroutineFunc(Action continuation)
        {
            while (!func())
            {
                await Task.Delay(10);
            }
            isDone = true;
            continuation();//继续运行
        }

        public void GetResult() { }
    }


}