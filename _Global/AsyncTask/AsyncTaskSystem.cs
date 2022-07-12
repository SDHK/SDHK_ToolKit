
/****************************************

* 作者： 闪电黑客
* 日期： 2022/6/27 9:47

* 描述： 

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{

    /// <summary>
    /// 接口：异步等待事件节点（无返回结果）
    /// </summary>
    public interface IAsyncAwaitNode : INotifyCompletion
    {
        bool IsCompleted { get; }

    }

    /// <summary>
    /// 接口：异步等待事件节点
    /// </summary>
    public interface IAsyncAwaitNode<out T> : IAsyncAwaitNode
    {
        T Result { get; }
        T GetResult();
    }

    public class AsyncTask : UnitPoolItem<AsyncTask>, IAsyncAwaitNode
    {
        public AsyncTask GetAwaiter() => this; //await需要这个;

        public bool IsCompleted => true;

        public AsyncTask()
        {
        }

        public void OnCompleted(Action continuation)
        {
            continuation();//继续运行
        }
    
        public void GetResult()
        {
        }
    }

    public class AsyncTaskSystem
    {

    }
}
