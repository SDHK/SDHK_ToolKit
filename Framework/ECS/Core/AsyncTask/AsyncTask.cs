
/****************************************

* 作者： 闪电黑客
* 日期： 2022/6/27 9:47

* 描述：
* 
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{

    public interface IAsyncTask : ICriticalNotifyCompletion
    {
        bool IsCompleted { get; set; }
        IAsyncTask GetResult();
        void SetResult();
    }


    public interface IAsyncTask<T> : ICriticalNotifyCompletion
    {
        bool IsCompleted { get; set; }
        T Result { get; }
        void SetResult(T result);
        T GetResult();
    }


    public static class AsyncTaskExtension
    {
        public static AsyncTask AsyncYield(this Entity self, int count = 0)
        {
            AsyncTask asyncTask = self.AddChildren<AsyncTask>();
            var counter = asyncTask.AddComponent<CounterCall>();
            counter.countOut = count;
            counter.callback = asyncTask.SetResult;
            return asyncTask;
        }

        public static AsyncTask AsyncDelay(this Entity self, float time)
        {
            AsyncTask asyncTask = self.AddChildren<AsyncTask>();
            var timer = asyncTask.AddComponent<TimerCall>();
            timer.timeOutTime = time;
            timer.callback = asyncTask.SetResult;
            return asyncTask;
        }
    }

    [AsyncMethodBuilder(typeof(AsyncTaskMethodBuilder))]
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

        public IAsyncTask GetResult()
        {
            return this;
        }
        public void SetResult()
        {
            continuation?.Invoke();
            RemoveSelf();
        }
    }

    public struct AsyncTaskMethodBuilder
    {
        private AsyncTask task;
        // 1. Static Create method.

        [DebuggerHidden]
        public static AsyncTaskMethodBuilder Create()
        {
            AsyncTaskMethodBuilder builder = new AsyncTaskMethodBuilder() { task = EntityManager.Instance.AddChildren<AsyncTask>() };
            return builder;
        }

        // 2. TaskLike Task property.
        [DebuggerHidden]
        public AsyncTask Task => this.task;

        // 3. SetException
        [DebuggerHidden]
        public void SetException(Exception exception)
        {
            this.task.SetException(exception);
        }

        // 4. SetResult
        [DebuggerHidden]

        public void SetResult()
        {
            this.task.SetResult();
        }

        // 5. AwaitOnCompleted
        [DebuggerHidden]

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        // 6. AwaitUnsafeOnCompleted
        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        // 7. Start
        [DebuggerHidden]
        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        // 8. SetStateMachine
        [DebuggerHidden]
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }


}
