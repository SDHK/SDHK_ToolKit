

/******************************

 * 作者: 闪电黑客

 * 日期: 2021/10/18 6:31:30

 * 最后日期: 2021/10/18 6:31:30

 * 描述: 
 
    事件委托 ：继承了泛型对象池

    以委托的不同类型为键值，实现类似函数重载功能


    主要让"委托"有重载的功能（多播委托），
    可以有不同的类型参数，不同数量的参数，以及不同类型的返回值
    
    
******************************/


using System;
using System.Collections.Generic;
using ObjectFactory;


namespace EventDelegate_
{


    /// <summary>
    /// 事件委托 ：内置对象池，通过GetObject()获取实例
    /// </summary>
    public class EventDelegate : IObjectPoolItem
    {
        private static ObjectPool<EventDelegate> Pool = new ClassObjectPool<EventDelegate>()
        { objectDestoryClock = 300, objectLimit = 100 }//5分钟
        .RegisterManager();

        public ObjectPoolBase thisPool { get; set; }

        public static EventDelegate GetObject() => Pool.Get();


        private Dictionary<Type, List<Delegate>> events = new Dictionary<Type, List<Delegate>>();



        /// <summary>
        /// 添加一个委托
        /// </summary>
        public void AddDelegate(Delegate action)
        {
            Type key = action.GetType();
            if (!events.ContainsKey(key))
            {
                List<Delegate> delegates = new List<Delegate>();
                delegates.Add(action);
                events.Add(key, delegates);
            }
            else
            {
                events[key].Add(action);
            }
        }

        /// <summary>
        /// 删除一个委托
        /// </summary>
        public void RemoveDelegate(Delegate action) { events[action.GetType()].Remove(action); }

        /// <summary>
        /// 删除一个类型的全部委托
        /// </summary>
        public void Remove(Type key) { events.Remove(key); }

        /// <summary>
        ///  删除一个类型的全部委托
        /// </summary>
        public void Remove<T>() { events.Remove(typeof(T)); }

        /// <summary>
        /// 获取一个类型的全部委托
        /// </summary>
        public List<Delegate> Get(Type key) { return events.ContainsKey(key) ? events[key] : null; }

        /// <summary>
        /// 获取一个类型的全部委托
        /// </summary>
        public List<Delegate> Get<T>() { return Get(typeof(T)); }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear() { events.Clear(); }

        /// <summary>
        /// 回收
        /// </summary>
        public void Recycle() { Pool.Recycle(this); }

        public void ObjectOnNew() { }
        public void ObjectOnGet() { }
        public void ObjectOnRecycle() { Clear(); }
        public void ObjectOnClear() { Clear(); }

        public void ObjectRecycle()
        {
            throw new NotImplementedException();
        }

        public void ObjectOnDestroy()
        {
            throw new NotImplementedException();
        }
    }
}