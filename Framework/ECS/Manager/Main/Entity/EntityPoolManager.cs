/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/17 17:23

* 描述： 实体对象池管理器，半ECS组件

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
    /// 实体对象池管理器//  对象池回收是销毁自己!!!
    /// </summary>
    public class EntityPoolManager : SingletonEntityBase<EntityPoolManager>,IUnit
    {

        UnitDictionary<Type, PoolBase> pools;

        public EntityPoolManager()//通过构造函数来打破自己单例的死循环
        {
            if (Root is null)
            {
                new EntityRoot();
            }

            Id = IdManager.GetID;
            Type = GetType();
            instance = this;
            pools = UnitDictionary<Type, PoolBase>.GetObject();

            //注册生命周期系统
            SystemManager.Instance.RegisterSystems<INewSystem>();
            SystemManager.Instance.RegisterSystems<IGetSystem>();
            SystemManager.Instance.RegisterSystems<IRecycleSystem>();
            SystemManager.Instance.RegisterSystems<IDestroySystem>();
            Root.AddComponent(this);
        }

        /// <summary>
        /// 直接释放：释放后IsDisposed标记为true
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;
            OnDispose();
            IsDisposed = true;
        }

        public void OnDispose()
        {
            instance = null;
        }

        /// <summary>
        /// 获取单位
        /// </summary>
        public T Get<T>()
        where T : class, IEntity
        {
            Type type = typeof(T);
            if (pools.TryGetValue(type, out PoolBase pool))
            {
                return (pool as EntityPool<T>).Get();
            }
            else
            {
                EntityPool<T> newPool = new EntityPool<T>();
                pools.Add(type, newPool);
                AddComponent(newPool);
                return newPool.Get();
            }
        }

        


        /// <summary>
        /// 回收对象
        /// </summary>
        public void Recycle<T>(T obj)//禁止回收自己
        where T : class, IEntity
        {
            if (obj != this && !(obj is PoolBase))
            {
                if (pools.TryGetValue(obj.Type, out PoolBase pool))
                {
                    pool.Recycle(obj);
                }
                else
                {
                    EntityPool<T> newPool = new EntityPool<T>();
                    pools.Add(obj.Type, newPool);
                    AddComponent(newPool);
                    newPool.Recycle(obj);
                }
            }

        }

    }
}
