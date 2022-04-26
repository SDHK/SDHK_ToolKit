

/******************************

 * Author: 闪电黑客

 * 日期: 2022/02/18 07:15:25

 * 最后日期: 2022/02/18 07:15:38

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System.Collections;
using System.Collections.Generic;
using ObjectFactory;
using UnityEngine;
using System;

namespace EventMachine
{

    /// <summary>
    /// 事件执行者
    /// </summary>
    public class EventExecutor : IObjectPoolItem
    {
        private static ClassObjectPool<EventExecutor> pool = new ClassObjectPool<EventExecutor>()
        { objectDestoryClock = 600 }
        .RegisterManager();
        public ObjectPoolBase thisPool { get; set; }

        private EventExecutor() { }
        public static EventExecutor Get() => pool.Get();

        public bool isRun = true;
        public bool isDone = false;
        public int waitCount = 0;//等待计数

        public List<EventExecutor> subExecutors = new List<EventExecutor>();
        public void WaitAdd(EventExecutor executor) => subExecutors.Add(executor);
        public void WaitRemove(EventExecutor executor) => subExecutors.Remove(executor);

        public Action<EventExecutor> OnDone = null;

        private List<EventNode> eventNodes = new List<EventNode>();//当前进程任务队列

        public void Run() => isRun = true;
        public void Stop() => isRun = false;

        public EventNode current => (eventNodes.Count > 0) ? eventNodes[0] : null;

        public void ObjectRecycle()
        {
            thisPool.Recycle(this);
        }
        public void ObjectOnNew() { }
        public void ObjectOnGet()
        {
            isRun = true;
            isDone = false;
            waitCount = 0;
            OnDone = null;
        }
        public void ObjectOnRecycle()
        {
            isRun = false;
            isDone = true;
            OnDone = null;

            foreach (var executor in subExecutors)
            {
                executor.ObjectRecycle();
            }
            subExecutors.Clear();

            foreach (var node in eventNodes)
            {
                node.ObjectRecycle();
            }
            eventNodes.Clear();
        }

        public void ObjectOnDestroy()
        {
        }

        /// <summary>
        /// 添加一个任务节点
        /// </summary>
        /// <param name="taskNode">任务节点</param>
        /// <returns>当前任务进程</returns>
        public EventExecutor Add(EventNode taskNode)
        {
            taskNode.executor = this;
            eventNodes.Add(taskNode);
            return this;
        }

        public void Update()
        {
            if (isRun && subExecutors.Count == 0)
            {
                if (eventNodes.Count > 0)
                {
                    isDone = false;
                    EventNode eventNode = eventNodes[0];

                    if (eventNode.isDone)
                    {
                        eventNode.ObjectRecycle();
                        eventNodes.RemoveAt(0);
                        Update();
                    }
                    else
                    {
                        eventNode.Update();
                    }
                }
                else
                {
                    isRun = false;
                    isDone = true;
                    OnDone?.Invoke(this);
                }
            }

        }


    }

}