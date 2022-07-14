
/****************************************

* 作者： 闪电黑客
* 日期： 2022/5/24 18:52

* 描述： 单位对象池管理器

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 单位对象池管理器
    /// </summary>
    public class UnitPoolManager : SingletonBase<UnitPoolManager>
    {
        private Dictionary<Type, PoolBase> pools = new Dictionary<Type, PoolBase>();

        /// <summary>
        /// 获取单位
        /// </summary>
        public T Get<T>()
        where T : class, IUnitPoolItem
        {
            Type type = typeof(T);
            if (pools.TryGetValue(type, out PoolBase pool))
            {
                return pool.GetObject() as T;
            }
            else//不存在则新建
            {
                UnitPool<T> newPool = new UnitPool<T>();
                pools.Add(type, newPool);
                return newPool.Get();
            }
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        public void Recycle<T>(T obj)
        where T : class, IUnitPoolItem
        {
            Type type = typeof(T);
            if (pools.TryGetValue(type, out PoolBase pool))
            {
                pool.Recycle(obj);
            }
            else//不存在则新建
            {
                UnitPool<T> newPool = new UnitPool<T>();
                pools.Add(type, newPool);
                newPool.Recycle(obj);
            }
        }

        /// <summary>
        /// 添加单位池：假如池已存在，则替换并释放掉原来的。
        /// </summary>
        public void AddPool<T>(UnitPool<T> pool)
        where T : class, IUnitPoolItem
        {
            if (pools.TryAdd(pool.ObjectType, pool))
            {
                pools[pool.ObjectType]?.Dispose();
                pools[pool.ObjectType] = pool;
            }
        }

        /// <summary>
        /// 获取池
        /// </summary>
        public UnitPool<T> GetPool<T>()
        where T : class, IUnitPoolItem
        {
            Type type = typeof(T);
            if (pools.TryGetValue(type, out PoolBase pool))
            {
                return pool as UnitPool<T>;
            }
            else
            { 
                UnitPool<T> unitPool = new UnitPool<T>();
                pools.Add(type, unitPool); 
                return unitPool;
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


