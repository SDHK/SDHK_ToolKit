
/******************************

 * Author: 闪电黑客

 * 日期: 2022/02/24 04:17:41

 * 最后日期: 2022/02/24 04:20:36

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;


namespace EventMachine
{
    public partial class EventExecutorManager : SingletonMonoBase<EventExecutorManager>
    {
        private List<EventExecutor> updates = new List<EventExecutor>();
        private List<EventExecutor> lateUpdate = new List<EventExecutor>();
        private List<EventExecutor> fixedUpdate = new List<EventExecutor>();

        public EventExecutor RunUpdate()
        {
            EventExecutor eventExecutor = EventExecutor.Get();
            AddUpdate(eventExecutor);
            return eventExecutor;
        }
        public EventExecutor RunLateUpdate()
        {
            EventExecutor eventExecutor = EventExecutor.Get();
            AddLateUpdate(eventExecutor);
            return eventExecutor;
        }
        public EventExecutor RunFixedUpdate()
        {
            EventExecutor eventExecutor = EventExecutor.Get();
            AddFixedUpdate(eventExecutor);
            return eventExecutor;
        }


        public EventExecutor AddUpdate(EventExecutor eventExecutor)
        {
            if (!updates.Contains(eventExecutor))
            {
                updates.Add(eventExecutor);
            }
            return eventExecutor;
        }

        public EventExecutor AddLateUpdate(EventExecutor eventExecutor)
        {
            if (!lateUpdate.Contains(eventExecutor))
            {
                lateUpdate.Add(eventExecutor);
            }
            return eventExecutor;
        }

        public EventExecutor AddFixedUpdate(EventExecutor eventExecutor)
        {
            if (!fixedUpdate.Contains(eventExecutor))
            {
                fixedUpdate.Add(eventExecutor);
            }
            return eventExecutor;
        }

        private void Update()
        {
            for (int i = 0; i < updates.Count;)
            {
                updates[i].Update();
                if (updates[i].isDone)
                {
                    updates[i].ObjectRecycle();
                    updates.RemoveAt(i);
                    Update();
                }
                else
                {
                    i++;
                }
            }
        }

        private void LateUpdate()
        {
            for (int i = 0; i < lateUpdate.Count;)
            {
                lateUpdate[i].Update();
                if (lateUpdate[i].isDone)
                {
                    lateUpdate[i].ObjectRecycle();
                    lateUpdate.RemoveAt(i);
                    LateUpdate();
                }
                else
                {
                    i++;
                }
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < fixedUpdate.Count;)
            {
                fixedUpdate[i].Update();
                if (fixedUpdate[i].isDone)
                {
                    fixedUpdate[i].ObjectRecycle();
                    fixedUpdate.RemoveAt(i);
                    FixedUpdate();
                }
                else
                {
                    i++;
                }
            }
        }

    }

    public static partial class EventExecutorExtension
    {
        public static EventExecutor RunUpdate(this EventExecutor executor)
        {
            return EventExecutorManager.Instance().AddUpdate(executor);
        }
        public static EventExecutor RunLateUpdate(this EventExecutor executor)
        {
            return EventExecutorManager.Instance().AddLateUpdate(executor);
        }
        public static EventExecutor RunFixedUpdate(this EventExecutor executor)
        {
            return EventExecutorManager.Instance().AddFixedUpdate(executor);
        }

        public static EventExecutor Wait(this EventExecutor executor, EventExecutor subExecutor)
        {
            executor.WaitAdd(subExecutor);
            subExecutor.OnDone += executor.WaitRemove;
            return subExecutor;
        }

        public static EventExecutor WaitUpdate(this EventExecutor executor)
        {
            return Wait(executor, EventExecutorManager.Instance().RunUpdate());
        }
        public static EventExecutor WaitLateUpdate(this EventExecutor executor)
        {
            return Wait(executor, EventExecutorManager.Instance().RunLateUpdate());
        }

        public static EventExecutor WaitFixedUpdate(this EventExecutor executor)
        {
            return Wait(executor, EventExecutorManager.Instance().RunFixedUpdate());
        }

    }
}