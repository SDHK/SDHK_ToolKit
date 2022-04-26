using System;
using System.Collections;
using System.Collections.Generic;

namespace EventDelegate_
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 获取对象绑定的事件委托 
        /// </summary>
        /// <param name="key">绑定的对象</param>
        public static EventDelegate EventGet(this Object key)
        {
            return EventDelegateService.Instance().Get(key);
        }

        /// <summary>
        /// 移除对象绑定的事件委托
        /// </summary>
        /// <param name="key">绑定的对象</param>
        public static void EventRemove(this Object key)
        {
            EventDelegateService.Instance().Remove(key);
        }

    }
}