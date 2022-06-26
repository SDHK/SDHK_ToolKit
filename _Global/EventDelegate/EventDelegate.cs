
/******************************

 * 作者: 闪电黑客
 * 日期: 2021/10/18 6:31:30

 * 描述: 
 
    事件委托 ：继承了泛型对象池

    以委托的不同类型为键值，实现类似函数重载功能


    主要让"委托"有重载的功能（多播委托），
    可以有不同的类型参数，不同数量的参数，以及不同类型的返回值
    
*/
/****************************************

* 作者： 闪电黑客
* 日期： 2022/6/26 11:12

* 描述： 修改为继承 UnitPoolItem 获得对象池功能

*/


using System;
using System.Collections.Generic;


namespace SDHK
{

    /// <summary>
    /// 事件委托 
    /// </summary>
    public class EventDelegate : UnitPoolItem<EventDelegate>
    {
        private Dictionary<Type, List<Delegate>> events;

        /// <summary>
        /// 添加一个委托
        /// </summary>
        public void AddDelegate(Delegate action)
        {
            if (events == null)
            {
                events = ObjectPoolManager.Instance.Get<Dictionary<Type, List<Delegate>>>();
            }

            Type key = action.GetType();

            if (events.ContainsKey(key))
            {
                List<Delegate> delegates = ObjectPoolManager.Instance.Get<List<Delegate>>();
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
        public void RemoveDelegate(Delegate action)
        {
            Type key = action.GetType();
            if (events.ContainsKey(key))
            {
                var delegates = events[key];
                delegates.Remove(action);
                if (delegates.Count == 0)
                {
                    ObjectPoolManager.Instance.Recycle(delegates);
                    events.Remove(key);
                    if (events.Count == 0)
                    {
                        ObjectPoolManager.Instance.Recycle(events);
                        events = null;
                    }
                }
            }
        }

        /// <summary>
        /// 删除一个类型的全部委托
        /// </summary>
        public void Remove(Type key)
        {
            if (events.ContainsKey(key))
            {
                events[key].Clear();
                ObjectPoolManager.Instance.Recycle(events[key]);
                events.Remove(key);
                if (events.Count == 0)
                {
                    ObjectPoolManager.Instance.Recycle(events);
                    events = null;
                }
            }
        }

        /// <summary>
        ///  删除一个类型的全部委托
        /// </summary>
        public void Remove<T>()
        {
            events.Remove(typeof(T));
        }

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
        public void Clear()
        {
            foreach (var item in events)
            {
                var delegates = item.Value;
                delegates.Clear();
                ObjectPoolManager.Instance.Recycle(delegates);
            }
            events.Clear();
            ObjectPoolManager.Instance.Recycle(events);
            events = null;
        }

        public override void OnRecycle() { Clear(); }


    }
}