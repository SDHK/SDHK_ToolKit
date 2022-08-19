
/****************************************

* 作者： 闪电黑客
* 日期： 2022/8/2 15:28

* 描述： 事件管理器

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{

    /// <summary>
    /// 事件管理器
    /// </summary>
    public class EventManager : Entity
    {
        public SystemGroup systemGroup;

        public UnitDictionary<object, EventDelegate> eventDelegates;

        /// <summary>
        /// 获取对象绑定的事件委托 
        /// </summary>
        public EventDelegate Get()
        {
            return Get(this.Type);
        }

        /// <summary>
        /// 获取对象绑定的事件委托 
        /// </summary>
        public EventDelegate Get(object key)
        {
            if (eventDelegates.ContainsKey(key))
            {
                return eventDelegates[key];
            }
            else
            {
                EventDelegate eventDelegate = AddChildren<EventDelegate>();
                eventDelegates.Add(key, eventDelegate);
                return eventDelegate;
            }
        }
        /// <summary>
        /// 移除对象绑定的事件委托
        /// </summary>
        public void Remove()
        {
            Remove(this.Type);
        }

        /// <summary>
        /// 移除对象绑定的事件委托
        /// </summary>
        public void Remove(object key)
        {
            if (eventDelegates.ContainsKey(key))
            {
                RemoveChildren(eventDelegates[key]);
                eventDelegates.Remove(key);
            }
        }

    }

    class EventManagerAddSystem : AddSystem<EventManager>
    {
        public override void OnAdd(EventManager self)
        {
            //进行遍历分类
            self.systemGroup = self.RootGetSystemGroup<IEventSystem>();
            self.eventDelegates = self.RootUnitPoolManager().Get<UnitDictionary<object, EventDelegate>>();

            foreach (var systems in self.systemGroup.Values)
            {
                foreach (IEventSystem system in systems)
                {
                    //反射属性获取键值
                    object key = self.Type;
                    object[] attributes = system.GetType().GetCustomAttributes(typeof(EventKeyAttribute), true);
                    if (attributes.Length != 0)
                    {
                        key = (attributes[0] as EventKeyAttribute)?.key;
                    }
                    //分组注册事件
                    self.Get(key).AddDelegate(system.GetDeleate());
                }
            }
        }
    }


    class EventManagerRemoveSystem : RemoveSystem<EventManager>
    {
        public override void OnRemove(EventManager self)
        {
            self.systemGroup = null;
            self.eventDelegates.Recycle();
            self.eventDelegates = null;
        }
    }
}
