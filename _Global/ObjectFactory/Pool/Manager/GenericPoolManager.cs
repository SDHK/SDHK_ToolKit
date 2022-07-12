/****************************************

* 作者： 闪电黑客
* 日期： 2022/6/25 15:07

* 描述： 通用对象池管理器，需要自行添加对象池

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 通用对象池管理器
    /// </summary>
    public class GenericPoolManager : SingletonBase<GenericPoolManager>
    {
        private Dictionary<Type, PoolBase> pools = new Dictionary<Type, PoolBase>();

        /// <summary>
        /// 获取对象：通用对象池需要手动添加进来
        /// </summary>
        public T Get<T>()
        where T : class, IUnitPoolItem
        {
            Type type = typeof(T);
            if (pools.TryGetValue(type, out PoolBase pool))
            {
                return pool.GetObject() as T;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        public void Recycle<T>(T obj)
        where T : class
        {
            Type type = typeof(T);
            if (pools.TryGetValue(type, out PoolBase pool))
            {
                pool.Recycle(obj);
            }
        }

        /// <summary>
        /// 添加通用对象池：假如池已存在，则替换并释放掉原来的。
        /// </summary>
        public void AddPool<T>(GenericPool<T> pool)
        where T : class
        {
            if (pools.TryAdd(pool.ObjectType, pool))
            {
                pools[pool.ObjectType]?.Dispose();
                pools[pool.ObjectType] = pool;
            }
        }

        /// <summary>
        /// 获取通用对象池：不存在返回null
        /// </summary>
        public GenericPool<T> GetPool<T>()
        where T : class, IUnitPoolItem
        {
            Type type = typeof(T);
            if (pools.TryGetValue(type, out PoolBase pool))
            {
                return pool as UnitPool<T>;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 释放池
        /// </summary>
        public void DisposePool<T>()
        {
            Type type = typeof(T);
            if (pools.TryGetValue(type, out PoolBase pool))
            {
                pool.Dispose();
                pools.Remove(type);
            }
        }
        public override void OnDispose()
        {
            foreach (var pool in pools)
            {
                pool.Value.Dispose();
            }
            pools.Clear();
        }
    }
}
