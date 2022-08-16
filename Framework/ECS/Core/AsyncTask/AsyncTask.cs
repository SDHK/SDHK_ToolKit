
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


    public static class AsyncTaskYieldExtension
    {

        public static AsyncTaskYield TaskYield(this Entity self)
        {
            return self.Root.AddComponent<AsyncTaskYield>();
        }

    }

    public class AsyncTaskYield : Entity, IAsyncTask
    {
        public UnitList<Action> continuations;

        public AsyncTaskYield GetAwaiter() => this;

        public bool IsCompleted { get; set; }

        public void GetResult()
        {
        }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public void SetResult()
        {
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            continuations.Add(continuation);
        }

        public override string ToString()
        {
            return $"AsyncTaskYield : {continuations.Count}";
        }
    }

    class AsyncTaskAddSystem : AddSystem<AsyncTaskYield>
    {
        public override void OnAdd(AsyncTaskYield self)
        {
            self.continuations = UnitList<Action>.GetObject();
        }
    }


    class AsyncTaskRemoveSystem : RemoveSystem<AsyncTaskYield>
    {
        public override void OnRemove(AsyncTaskYield self)
        {
            self.continuations.Recycle();
        }
    }

    class AsyncTaskYieldUpdateSystem : UpdateSystem<AsyncTaskYield>
    {
        public override void Update(AsyncTaskYield self)
        {
            while (self.continuations.Count > 0)
            {
                self.continuations[0]?.Invoke();
                self.continuations.RemoveAt(0);
            }
            //foreach (var continuation in self.continuations)
            //{
            //    continuation?.Invoke();
            //}
        }
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
