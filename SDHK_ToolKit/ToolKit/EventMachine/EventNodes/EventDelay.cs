
/******************************

 * Author: 闪电黑客

 * 日期: 2022/02/18 10:50:20

 * 最后日期: 2022/02/18 10:50:38

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncAwaitEvent;
using ObjectFactory;
using UnityEngine;
namespace EventMachine
{
    public class EventDelay : EventNode
    {
        private static ClassObjectPool<EventDelay> pool = new ClassObjectPool<EventDelay>()
        { objectDestoryClock = 600 }
        .RegisterManager();
        public override ObjectPoolBase thisPool { get; set; }
        private EventDelay() { }

        private int delayTime;
        public double clockTime;
        private DateTime SystemTime;

        private Action<EventExecutor> update;

        public static EventDelay Get(int secondsDelay, Action<EventExecutor> update)
        {
            var node = EventDelay.pool.Get();
            node.update = update;
            node.delayTime = secondsDelay;
            return node;
        }

        public override void ObjectOnNew() { }
        public override void ObjectOnGet() { SystemTime = DateTime.MinValue; }

        public override void ObjectOnRecycle()
        {
            executor = null;
            isDone = false;
            update = null;
        }

        public override void Update()
        {
            if (SystemTime == DateTime.MinValue)
            {
                SystemTime = DateTime.Now;
            }

            clockTime = (DateTime.Now - SystemTime).TotalSeconds;
            update?.Invoke(executor);

            if (clockTime > delayTime)
            {
                isDone = true;
            }
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

        public static EventExecutor Delay(this EventExecutor executor, int secondsDelay, Action<EventExecutor> update)
        {
            return executor.Add(EventDelay.Get(secondsDelay, update));
        }
        public static EventExecutor Delay(this EventExecutor executor, int secondsDelay)
        {
            return executor.Add(EventDelay.Get(secondsDelay, null));
        }

    }

}