/******************************

 * 作者: 闪电黑客

 * 日期: 2021/10/18 6:31:30

 * 最后日期: 2021/10/18 6:31:30

 * 描述: 
 
    事件委托服务，全局单例

    可以全局调用获取，提供绑定对象的事件委托

    以Object为键值区分，达到在任何对象上绑定事件委托，进行调用的功能
    
    
******************************/


using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;

namespace EventDelegate_
{
    /// <summary>
    /// 事件委托绑定服务
    /// </summary>
    public class EventDelegateService : SingletonBase<EventDelegateService>
    {
        public Dictionary<Object, EventDelegate> eventDelegates = new Dictionary<Object, EventDelegate>();

        /// <summary>
        /// 获取对象绑定的事件委托 
        /// </summary>
        /// <param name="key">绑定的对象</param>
        public EventDelegate Get(Object key)
        {
            if (Instance().eventDelegates.ContainsKey(key))
            {
                return instance.eventDelegates[key];
            }
            else
            {
                EventDelegate eventDelegate = EventDelegate.GetObject();
                instance.eventDelegates.Add(key, eventDelegate);
                return eventDelegate;
            }
        }

        /// <summary>
        /// 移除对象绑定的事件委托
        /// </summary>
        /// <param name="key">绑定的对象</param>
        public void Remove(Object key)
        {
            if (Instance().eventDelegates.ContainsKey(key))
            {
                instance.eventDelegates[key].Recycle();
                instance.eventDelegates.Remove(key);
            }
        }

    }
}