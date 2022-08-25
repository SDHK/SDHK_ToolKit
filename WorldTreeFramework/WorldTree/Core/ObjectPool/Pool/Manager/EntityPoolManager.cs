/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/17 17:23

* 描述： 实体对象池管理器。
* 为所有实体对象池的管理器。

*/
using System;

namespace WorldTree
{

    public static class EntityPoolManagerExtension
    {
        public static EntityPoolManager EntityPoolManager(this Entity self)
        {
            return self.Root.EntityPoolManager;
        }
    }

    class EntityPoolManagerRemove : RemoveSystem<EntityPoolManager>
    {
        public override void OnRemove(EntityPoolManager self)
        {
            self.Dispose();//全部释放
        }
    }

    /// <summary>
    /// 实体对象池管理器
    /// </summary>
    public class EntityPoolManager : Entity
    {

        UnitDictionary<Type, EntityPool> pools = new UnitDictionary<Type, EntityPool>();

        public override void OnDispose()
        {
            pools.Clear();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        public T Get<T>()
        where T : Entity
        {
            Type type = typeof(T);
            return GetPool(type).Get<T>();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        public Entity Get(Type type)
        {
            return GetPool(type).Get();
        }


        /// <summary>
        /// 回收对象
        /// </summary>
        public void Recycle(Entity obj)
        {
            if (obj != this && !(obj is EntityPool))//禁止回收自己和对象池
            {
                GetPool(obj.Type).Recycle(obj);
            }
        }

        /// <summary>
        /// 获取池
        /// </summary>
        public EntityPool GetPool<T>()
        where T : Entity
        {
            Type type = typeof(T);
            return GetPool(type);
        }
        /// <summary>
        /// 获取池
        /// </summary>
        public EntityPool GetPool(Type type)
        {
            if (!pools.TryGetValue(type, out EntityPool pool))
            {
                pool = new EntityPool(type);
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
            if (pools.TryGetValue(type, out EntityPool pool))
            {
                pools.Remove(type);
            }
        }
    }
}
