

/******************************

 * 作者: 闪电黑客
 * 日期: 2021/12/14 05:23:50

 * 描述:  
    
    泛型对象池管理器

*/
/****************************************

* 作者： 闪电黑客
* 日期： 2022/6/21 20:55

* 描述： 对象池结构大改。

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDHK;
using System.Runtime.InteropServices;
using UnityEngine.Profiling;
using System;

namespace SDHK
{

    /// <summary>
    /// 泛型对象池管理器
    /// </summary>
    public class ObjectPoolManager : SingletonBase<ObjectPoolManager>
    {
        private Dictionary<Type, IPool> pools = new Dictionary<Type, IPool>();

        /// <summary>
        /// 获取对象
        /// </summary>
        public T Get<T>()
        where T : class
        {
            Type type = typeof(T);
            if (pools.TryGetValue(type, out IPool pool))
            {
                return pool.GetObject() as T;
            }
            else//不存在则新建
            {
                ObjectPool<T> newPool = new ObjectPool<T>();
                pools.Add(type, newPool);
                return newPool.Get();
            }
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        public void Recycle<T>(T obj)
        where T : class
        {
            Type type = typeof(T);
            if (pools.TryGetValue(type, out IPool pool))
            {
                pool.Recycle(obj);
            }
            else//不存在则新建
            {
                ObjectPool<T> newPool = new ObjectPool<T>();
                pools.Add(type, newPool);
                newPool.Recycle(obj);
            }
        }

        /// <summary>
        /// 添加泛型对象池：假如池已存在，则替换并释放掉原来的。
        /// </summary>
        public void AddPool<T>(ObjectPool<T> pool)
        where T : class
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
        public ObjectPool<T> GetPool<T>()
        where T : class
        {
            Type type = typeof(T);
            if (pools.TryGetValue(type, out IPool pool))
            {
                return pool as ObjectPool<T>;
            }
            else
            {
                ObjectPool<T> unitPool = new ObjectPool<T>();
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
            if (pools.TryGetValue(type, out IPool pool))
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