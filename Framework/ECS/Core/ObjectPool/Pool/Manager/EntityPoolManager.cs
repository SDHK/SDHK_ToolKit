/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/17 17:23

* 描述： 实体对象池管理器，偏向ECS实体。
* 为所有实体对象池的管理器。
* 
* 关于生成：
* 
* 实体设定是由对象池生成，
* 但对象池自己并不能实例自己，所以是通过new来实例的。
* 
* 并且由于是在域之后第一个生成，
* 其余生命周期管理器是通过对象池生成。
* 
* 对象池和管理器在生成时是没有任何生命周期事件可以触发的。
* 所以设定他们为半实体，面向对象的同时可以挂为节点。
*
*
* 关于回收：
* 
* 域在回收的时候会先回收全部节点，
* 然后释放掉对象池管理器和全部对象池，
* 
* 与生成同理，回收时是忽略自己和对象池的，所以也触发不了生命周期事件。
* 
* 最后只有全部释放掉。
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

    /// <summary>
    /// 实体对象池管理器
    /// </summary>
    public class EntityPoolManager : Entity
    {

        UnitDictionary<Type, EntityPool> pools= new  UnitDictionary<Type, EntityPool>();

        public EntityPoolManager()//通过构造函数来打破自己单例的死循环
        {
            id = IdManager.GetID;
            Type = GetType();
        }

        public override void OnDispose()
        {
            foreach (var item in pools)
            {
                item.Value.Dispose();
            }
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
                pool.Dispose();
                pools.Remove(type);
            }
        }
    }
}
