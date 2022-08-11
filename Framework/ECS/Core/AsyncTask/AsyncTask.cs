
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
    public interface IAsyncTask : ICriticalNotifyCompletion
    {
        bool IsCompleted { get; }

    }

    /// <summary>
    /// 接口：异步等待事件节点
    /// </summary>
    public interface IAsyncTask<T> : IAsyncTask
    {
        T Result { get; }
        T GetResult();
    }

    public class AsyncTask : Entity, IAsyncTask
    {
        public AsyncTask GetAwaiter() => this; //await需要这个;

        public bool IsCompleted => true;


        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }
    
    
        public void UnsafeOnCompleted(Action continuation)
        {
        }

        public void GetResult()
        {

        }
        public void SetResult()
        { 
            
        }
    }

  
}
