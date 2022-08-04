﻿
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
    /// 根节点组件调用扩展
    /// </summary>
    public static class EventManagerExtension
    {

        /// <summary>
        /// 获取一组事件集合
        /// </summary>
        public static EventDelegate RootGetEvent(this Entity self, object key)
        {
            return self.Root.SetComponent<EventManager>().Get(key);
        }

        /// <summary>
        /// 删除一组事件集合
        /// </summary>
        public static void RootRemoveEvent(this Entity self, object key)
        {
             self.Root.SetComponent<EventManager>().Remove(key);
        }

        /// <summary>
        /// 获取默认组事件集合
        /// </summary>
        public static EventDelegate RootGetEvent(this Entity self)
        {
            return self.Root.SetComponent<EventManager>().Get("");
        }

        /// <summary>
        /// 删除默认组事件集合
        /// </summary>
        public static void RootRemoveEvent(this Entity self)
        {
             self.Root.SetComponent<EventManager>().Remove("");
        }
    }

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
        /// <param name="key">绑定的对象</param>
        public EventDelegate Get(object key)
        {
            if (eventDelegates.ContainsKey(key))
            {
                return eventDelegates[key];
            }
            else
            {
                EventDelegate eventDelegate = EventDelegate.GetObject();
                eventDelegates.Add(key, eventDelegate);
                return eventDelegate;
            }
        }

        /// <summary>
        /// 移除对象绑定的事件委托
        /// </summary>
        /// <param name="key">绑定的对象</param>
        public void Remove(object key)
        {
            if (eventDelegates.ContainsKey(key))
            {
                eventDelegates[key].Recycle();
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
            self.eventDelegates = UnitPoolManager.Instance.Get<UnitDictionary<object, EventDelegate>>();

            foreach (var systems in self.systemGroup.Values)
            {
                foreach (IEventSystem system in systems)
                {
                    //反射属性获取键值
                    object key = "";
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
