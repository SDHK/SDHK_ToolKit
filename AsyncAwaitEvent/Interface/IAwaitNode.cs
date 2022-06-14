using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AsyncAwaitEvent
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
}