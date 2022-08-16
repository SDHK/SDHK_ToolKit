using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using System.Security;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Diagnostics.Tracing;
using System.Threading;
using System.Security.Permissions;


namespace SDHK
{
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
            //if (this.IsCompleted)
            //{
            //    continuation();
            //}
        }
    }

    public class MyAwaitableTaskMethodBuilder<T>
    {

        public static MyAwaitableTaskMethodBuilder<T> Create()
      => new MyAwaitableTaskMethodBuilder<T>();

        public MyAwaitableTaskMethodBuilder()
            => this.Task = new MyAwaitable<T>();

      

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



    [AsyncMethodBuilder(typeof(TaskLikeMethodBuilder))]
    public readonly struct YieldAwaitable1
    {

        public YieldAwaiter1 GetAwaiter() => default(YieldAwaiter1);

        public readonly struct YieldAwaiter1 : ICriticalNotifyCompletion, INotifyCompletion
        {
            //public YieldAwaiter1 GetAwaiter() => this;
            public bool IsCompleted => true;

            public void GetResult() { }
            public void OnCompleted(Action continuation) { }
            public void UnsafeOnCompleted(Action continuation) { }
        }
    }


    public sealed class TaskLikeMethodBuilder
    {
        public TaskLikeMethodBuilder()
        {
        }

        public static TaskLikeMethodBuilder Create()
            => new TaskLikeMethodBuilder();

        public void SetResult()
        {
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            //stateMachine.MoveNext();
        }

        public YieldAwaitable1 Task => default(YieldAwaitable1);

        // AwaitOnCompleted, AwaitUnsafeOnCompleted, SetException 
        // and SetStateMachine are empty

    }



}
