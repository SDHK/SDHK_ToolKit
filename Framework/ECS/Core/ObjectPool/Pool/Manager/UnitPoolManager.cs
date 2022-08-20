
/****************************************

* 作者： 闪电黑客
* 日期： 2022/5/24 18:52

* 描述： 单位对象池管理器
* 
* 由于单位对象池管理的是自定义的类而不是实体
* 自定义类并不能拿到实体根节点，所以需要单例
* 
* 
* 
* 

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{
    public static class UnitPoolManagerExtension
    { 
        public static UnitPoolManager UnitPoolManager(this Entity self)
        {
            return self.Root.GetComponent<UnitPoolManager>();
        }
    }

    /// <summary>
    /// 单位对象池管理器
    /// </summary>
    public class UnitPoolManager : Entity
    {
        private Dictionary<Type, IPool> pools = new Dictionary<Type, IPool>();

        public UnitPoolManager():base()//通过构造函数来打破自己单例的死循环
        {

            id = IdManager.GetID;

            Components = new UnitDictionary<Type, Entity>();
            Children = new UnitDictionary<ulong, Entity>();
        }


        public override void OnDispose()
        {
            foreach (var pool in pools)
            {
                pool.Value.Dispose();
            }
            pools.Clear();
        }

        /// <summary>
        /// 获取单位
        /// </summary>
        public T Get<T>()
        where T : class, IUnitPoolItem
        {
            return GetPool<T>().Get();
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        public void Recycle<T>(T obj)
        where T : class, IUnitPoolItem
        {
            if (!(obj is IPool))//禁止回收对象池
            {
                GetPool<T>().Recycle(obj);
            }
        }


        /// <summary>
        /// 获取池
        /// </summary>
        public UnitPool<T> GetPool<T>()
        where T : class, IUnitPoolItem
        {
            Type type = typeof(T);

            if (!pools.TryGetValue(type, out IPool pool))
            {
                UnitPool<T> unitPool = new UnitPool<T>();
                unitPool.Root = Root;
                pools.Add(type, unitPool);
                AddChildren(unitPool);
                return unitPool;
            }
            return pool as UnitPool<T>;
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

    }

}


