
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


    public interface IAsyncTask : ICriticalNotifyCompletion
    {
        bool IsCompleted { get; set; }
        void GetResult();
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

    [AsyncMethodBuilder(typeof(MyAwaitableTaskMethodBuilder<>))]
    public class MyAwaitable<T> : INotifyCompletion
    {
        private Action _continuation;

        public MyAwaitable()
        { }

        public MyAwaitable(T value)
        {
            this.Value = value;
            this.IsCompleted = true;
        }

        public MyAwaitable<T> GetAwaiter() => this;

        public bool IsCompleted { get; private set; }

        public T Value { get; private set; }

        public Exception Exception { get; private set; }

        public T GetResult()
        {
            if (!this.IsCompleted) throw new Exception("Not completed");
            //if (this.Exception != null)
            //{
            //    ExceptionDispatchInfo.Throw(this.Exception);
            //}
            return this.Value;
        }

        internal void SetResult(T value)
        {
            if (this.IsCompleted) throw new Exception("Already completed");
            this.Value = value;
            this.IsCompleted = true;
            this._continuation?.Invoke();
        }

        internal void SetException(Exception exception)
        {
            this.IsCompleted = true;
            this.Exception = exception;
        }

        void INotifyCompletion.OnCompleted(Action continuation)
        {
            this._continuation = continuation;
            if (this.IsCompleted)
            {
                continuation();
            }
        }
    }

    public class MyAwaitableTaskMethodBuilder<T>
    {
        public MyAwaitableTaskMethodBuilder()
            => this.Task = new MyAwaitable<T>();

        public static MyAwaitableTaskMethodBuilder<T> Create()
        => new MyAwaitableTaskMethodBuilder<T>();

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
            => stateMachine.MoveNext();

        public void SetStateMachine(IAsyncStateMachine stateMachine) { }

        public void SetException(Exception exception)
            => this.Task.SetException(exception);

        public void SetResult(T result)
            => this.Task.SetResult(result);

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter,
            ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
            => this.GenericAwaitOnCompleted(ref awaiter, ref stateMachine);

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter,
            ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
            => this.GenericAwaitOnCompleted(ref awaiter, ref stateMachine);

        public void GenericAwaitOnCompleted<TAwaiter, TStateMachine>(
            ref TAwaiter awaiter,
            ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
            => awaiter.OnCompleted(stateMachine.MoveNext);

        public MyAwaitable<T> Task { get; }
    }


}
