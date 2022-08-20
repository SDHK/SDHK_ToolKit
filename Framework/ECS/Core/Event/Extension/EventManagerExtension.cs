using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{


    /// <summary>
    /// 根节点组件调用扩展
    /// </summary>
    public static class EventManagerExtension
    {
        /// <summary>
        /// 获取事件管理器
        /// </summary>
        public static EventManager EventManager(this Entity self)
        {
            return self.Root.AddComponent<EventManager>();
        }

        /// <summary>
        /// 获取默认事件
        /// </summary>
        public static EventDelegate Event(this Entity self)
        {
            return self.Root.AddComponent<EventManager>().AddComponent<EventDelegate>();
        }
        /// <summary>
        /// 移除默认事件
        /// </summary>
        public static void EventRemove(this Entity self)
        {
            self.Root.AddComponent<EventManager>().RemoveComponent<EventDelegate>();
        }

        /// <summary>
        /// 获取分组事件
        /// </summary>
        public static EventDelegate Event<Key>(this Entity self)
        where Key : EventDelegate
        {
            return self.Root.AddComponent<EventManager>().AddComponent<Key>();
        }

        /// <summary>
        /// 移除分组事件
        /// </summary>
        public static void EventRemove<Key>(this Entity self)
        where Key : EventDelegate
        {
            self.Root.AddComponent<EventManager>().RemoveComponent<Key>();
        }

    }
}
