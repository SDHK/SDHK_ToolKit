
/******************************

 * Author: 闪电黑客

 * 日期: 2022/02/18 09:43:25

 * 最后日期: 2022/02/18 09:43:39

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ObjectFactory;
namespace EventMachine
{
    public class EventLoop : EventNode
    {
        private static ClassObjectPool<EventLoop> pool = new ClassObjectPool<EventLoop>()
        { objectDestoryClock = 600 }
              .RegisterManager();
        public override ObjectPoolBase thisPool { get; set; }

        private EventLoop() { }

        private Action action;
        private Action<EventExecutor> action_;

        public static EventLoop Get(Action action)
        {
            var node = EventLoop.pool.Get();
            node.action = action;
            return node;
        }
        public static EventLoop Get(Action<EventExecutor> action)
        {
            var node = EventLoop.pool.Get();
            node.action_ = action;
            return node;
        }


        public override void ObjectOnNew() { }
        public override void ObjectOnGet() { }

        public override void ObjectOnRecycle()
        {
            executor = null;
            action = null;
            action_ = null;
            isDone = false;
        }

        public override void Update()
        {
            action?.Invoke();
            action_?.Invoke(executor);
        }

        public override void ObjectRecycle()
        {
            thisPool.Recycle(this);
        }

        public override void ObjectOnDestroy()
        {
        }
    }

    public static partial class EventExecutorExtension
    {

        public static EventExecutor Loop(this EventExecutor executor, Action<EventExecutor> loopUpdate)
        {
            return executor.Add(EventLoop.Get(loopUpdate));
        }
        public static EventExecutor Loop(this EventExecutor executor, Action loopUpdate)
        {
            return executor.Add(EventLoop.Get(loopUpdate));
        }

    }
}