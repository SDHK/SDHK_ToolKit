
/****************************************

* 作者： 闪电黑客
* 日期： 2022/6/27 9:47

* 描述： 继承实体是为了好扩展事件查看进度

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
    /// 接口：异步等待事件节点
    /// </summary>
    public interface IAsyncTask : ICriticalNotifyCompletion
    {
        bool IsCompleted { get; set; }
        void GetResult();
    }

    /// <summary>
    /// 接口：异步等待事件节点
    /// </summary>
    public interface IAsyncTask<T> : ICriticalNotifyCompletion
    {
        bool IsCompleted { get; set; }
        T Result { get; }
        void SetResult(T result);
        T GetResult();
    }

    public class AsyncTask : Entity, IAsyncTask
    {
        public AsyncTask GetAwaiter() => this;

        public Action continuation;

        public bool IsCompleted { get { return true; } set { } }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            IsCompleted = true;
            continuation();
            //this.continuation = continuation;

        }

        public void GetResult()
        {
           RemoveSelf();
        }
    }

    //class AsyncTaskUpdateSystem : TaskUpdateSystem<AsyncTask>
    //{
    //    public override void Update(AsyncTask self)
    //    {
    //        if (self.IsCompleted == false)
    //        {
    //            self.IsCompleted = true;
    //            self.continuation?.Invoke();
    //            self.RemoveSelf();
    //        }
    //    }
    //}
}
