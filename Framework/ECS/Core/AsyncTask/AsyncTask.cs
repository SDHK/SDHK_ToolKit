
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
        void SetException(Exception exception);
    }


    public interface IAsyncTask<T> : ICriticalNotifyCompletion
    {
        bool IsCompleted { get; set; }
        void SetResult(T result);
        T GetResult();
        void SetException(Exception exception);

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

        [DebuggerHidden]
        private async AsyncTaskVoid InnerCoroutine()
        {
            await this;
        }

        [DebuggerHidden]
        public void Coroutine()
        {
            InnerCoroutine().Coroutine();
        }

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


    [AsyncMethodBuilder(typeof(AsyncTaskMethodBuilder<>))]
    public class AsyncTask<T> : Entity, IAsyncTask<T>
    {
        public AsyncTask<T> GetAwaiter() => this;
        public Action continuation;
        public bool IsCompleted { get; set; }

        public T Result;


        [DebuggerHidden]
        private async AsyncTaskVoid InnerCoroutine()
        {
            await this;
        }

        [DebuggerHidden]
        public void Coroutine()
        {
            InnerCoroutine().Coroutine();
        }


        public T GetResult()
        {
            return Result;
        }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);

        }
        public void UnsafeOnCompleted(Action continuation)
        {
            this.continuation = continuation;
        }
        public void SetResult(T result)
        {
            Result = result;
            continuation?.Invoke();
            RemoveSelf();
        }

        public void SetException(Exception exception)
        {
        }


    }






}
