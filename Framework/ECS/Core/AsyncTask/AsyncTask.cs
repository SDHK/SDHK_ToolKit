
/****************************************

* 作者： 闪电黑客
* 日期： 2022/6/27 9:47

* 描述： 继承实体是为了好扩展事件查看进度

*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{

    public interface IAsyncTask : ICriticalNotifyCompletion
    {
        bool IsCompleted { get; set; }
        void GetResult();
        void SetResult();
    }


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
        public Exception Exception { get; private set; }

        public bool IsCompleted { get; set; }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            this.continuation = continuation;
        }
        public void SetException(Exception exception)
        {
            this.Exception = exception;
        }

        public void GetResult()
        {
        }
        public void SetResult()
        {
            continuation?.Invoke();
            RemoveSelf();
        }
    }

    class AsyncTaskUpdate : TaskUpdateSystem<AsyncTask>
    {
        public override void Update(AsyncTask self)
        {
            self.SetResult();
        }
    }

}
