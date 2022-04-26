
/******************************

 * Author: 闪电黑客

 * 日期: 2022/02/18 07:17:07

 * 最后日期: 2022/02/18 07:19:18

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System;
using System.Collections;
using System.Collections.Generic;
using ObjectFactory;
using UnityEngine;
namespace EventMachine
{
    public class EventAction : EventNode
    {
        private static ClassObjectPool<EventAction> pool = new ClassObjectPool<EventAction>()
        { objectDestoryClock = 600 }
        .RegisterManager();
        public override ObjectPoolBase thisPool { get; set; }

        private EventAction() { }

        private Action action;
        private Action<EventExecutor> action_;


        public static EventAction Get(Action action)
        {
            var node = EventAction.pool.Get();
            node.action = action;
            return node;
        }

        public static EventAction Get(Action<EventExecutor> action)
        {
            var node = EventAction.pool.Get();
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
            isDone = true;
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
        public static EventExecutor Action(this EventExecutor executor, Action<EventExecutor> action)
        {
            return executor.Add(EventAction.Get(action));
        }
        public static EventExecutor Action(this EventExecutor executor, Action action)
        {
            return executor.Add(EventAction.Get(action));
        }

    }

}