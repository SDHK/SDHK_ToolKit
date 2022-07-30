/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/17 17:23

* 描述： 实体对象池管理器，半ECS实体。
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

    public class EntityPoolManagerAddSystem : AddSystem<EntityPoolManager>
    {
        public override void OnAdd(EntityPoolManager self)
        {
            //注册生命周期系统
            self.Root.systemManager.RegisterSystems<INewSystem>();
            self.Root.systemManager.RegisterSystems<IGetSystem>();
            self.Root.systemManager.RegisterSystems<IRecycleSystem>();
            self.Root.systemManager.RegisterSystems<IDestroySystem>();
        }
    }

    /// <summary>
    /// 实体对象池管理器
    /// </summary>
    public class EntityPoolManager : Entity, IUnit
    {

        UnitDictionary<Type, EntityPool> pools;

        public EntityPoolManager()//通过构造函数来打破自己单例的死循环
        {
            id = IdManager.GetID;
            Type = GetType();
            pools = UnitDictionary<Type, EntityPool>.GetObject();
        }

        public void Dispose()
        {
            if (isDisposed) return;
            OnDispose();
            isDisposed = true;
        }

        public void OnDispose()
        {
            foreach (var item in pools)
            {
                item.Value.Dispose();
            }
            pools.Clear();
            pools.Recycle();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        public T Get<T>()
        where T :  Entity
        {
            Type type = typeof(T);
            return Get(type) as T;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        public Entity Get(Type type)
        {

            if (pools.TryGetValue(type, out EntityPool pool))
            {
                return pool.GetObject();
            }
            else
            {
                EntityPool newPool = new EntityPool(type);
                newPool.Root = Root;
                pools.Add(type, newPool);
                AddChildren(newPool);
                return newPool.GetObject();
            }
        }


        /// <summary>
        /// 回收对象
        /// </summary>
        public void Recycle(Entity obj)
        {
            if (obj != this && !(obj is EntityPool))//禁止回收自己和对象池
            {
                if (pools.TryGetValue(obj.Type, out EntityPool pool))
                {
                    pool.Recycle(obj);
                }
                else
                {
                    EntityPool newPool = new EntityPool(obj.Type);
                    newPool.Root = Root;
                    pools.Add(obj.Type, newPool);
                    AddChildren(newPool);
                    newPool.Recycle(obj);
                }
            }
        }


    }
}
