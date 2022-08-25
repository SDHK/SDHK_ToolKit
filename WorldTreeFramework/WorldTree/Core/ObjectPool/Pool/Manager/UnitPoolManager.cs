
/****************************************

* 作者： 闪电黑客
* 日期： 2022/5/24 18:52

* 描述： 单位对象池管理器

*/


using System;
using System.Collections.Generic;

namespace WorldTree
{
    public static class UnitPoolManagerExtension
    { 
        public static UnitPoolManager UnitPoolManager(this Entity self)
        {
            return self.Root.UnitPoolManager;
        }
    }

    class UnitPoolManagerRemoveSystem : RemoveSystem<UnitPoolManager>
    {
        public override void OnRemove(UnitPoolManager self)
        {
            self.Dispose();//全部释放
        }
    }

    /// <summary>
    /// 单位对象池管理器
    /// </summary>
    public class UnitPoolManager : Entity
    {
        private Dictionary<Type, UnitPool> pools = new Dictionary<Type, UnitPool>();

        public UnitPoolManager():base()
        {
            Components = new UnitDictionary<Type, Entity>();
            Children = new UnitDictionary<long, Entity>();
        }


        public override void OnDispose()
        {
            pools.Clear();
        }

        /// <summary>
        /// 获取单位
        /// </summary>
        public T Get<T>()
        where T : class, IUnitPoolItem
        {
            Type type = typeof(T);
            return GetPool(type).Get<T>();
        }

        /// <summary>
        /// 获取单位
        /// </summary>
        public IUnitPoolItem Get(Type type)
        {
            return GetPool(type).Get();
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
        public UnitPool GetPool<T>()
        where T : class, IUnitPoolItem
        {
            Type type = typeof(T);
            return GetPool(type);
        }

        /// <summary>
        /// 获取池
        /// </summary>
        public UnitPool GetPool(Type type)
        {
            if (!pools.TryGetValue(type, out UnitPool pool))
            {
                pool = new UnitPool(type);
                pool.id = Root.IdManager.GetId();
                pool.Root = Root;
                pools.Add(type, pool);
                AddChildren(pool);
            }
            return pool;
        }

        /// <summary>
        /// 释放池
        /// </summary>
        public void DisposePool<T>()
        {
            Type type = typeof(T);
            if (pools.TryGetValue(type, out UnitPool pool))
            {
                pools.Remove(type);
            }
        }

    }

}


