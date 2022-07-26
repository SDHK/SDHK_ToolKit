/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/17 17:23

* 描述： 实体对象池管理器，半ECS实体。
* 为所有实体对象池的管理器。
* 
* 但实体设定由对象池生成，单例会导致死循环
* 所以需要通过new来实例自己，并把自己当成组件添加到根节点
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
    /// <summary>
    /// 实体对象池管理器
    /// </summary>
    public class EntityPoolManager : Entity, IUnit
    {

        UnitDictionary<Type, EntityPool> pools;

        public EntityPoolManager()//通过构造函数来打破自己单例的死循环
        {
            Id = IdManager.GetID;
            Type = GetType();
            pools = UnitDictionary<Type, EntityPool>.GetObject();

            //注册生命周期系统
            SystemManager.Instance.RegisterSystems<INewSystem>();
            SystemManager.Instance.RegisterSystems<IGetSystem>();
            SystemManager.Instance.RegisterSystems<IRecycleSystem>();
            SystemManager.Instance.RegisterSystems<IDestroySystem>();
            Root.AddComponent(this);

        }

        public void Dispose()
        {
            if (IsDisposed) return;
            OnDispose();
            IsDisposed = true;
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
        where T : class, IEntity
        {
            Type type = typeof(T);
            return Get(type) as T;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        public IEntity Get(Type type) {

            if (pools.TryGetValue(type, out EntityPool pool))
            {
                return pool.GetObject() ;
            }
            else
            {
                EntityPool newPool = new EntityPool();
                newPool.ObjectType = type;

                pools.Add(type, newPool);
                AddChildren(newPool);
                return newPool.GetObject();
            }
        }


        /// <summary>
        /// 回收对象
        /// </summary>
        public void Recycle(IEntity obj)
        {
            if (obj != this && !(obj is EntityPool))//禁止回收自己和对象池
            {
                if (pools.TryGetValue(obj.Type, out EntityPool pool))
                {
                    pool.Recycle(obj);
                }
                else
                {
                    EntityPool newPool = new EntityPool();
                    pools.Add(obj.Type, newPool);
                    AddChildren(newPool);
                    newPool.Recycle(obj);
                }
            }
        }

      
    }
}
