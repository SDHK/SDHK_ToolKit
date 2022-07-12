﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    //划分事件
    public interface IEntitieSystem : ISystem
    {
        public void AddEntitie(Entity entity, Entity arg);
        public void RemoveEntitie(Entity entity, Entity arg);
    }


    public abstract class EntitySystem<T> : Entity, IEntitieSystem
        where T : Entity
    {
        public Type EntityType => typeof(T);

        public void AddEntitie(Entity self, Entity entity) => OnAddEntitie(self as T, entity);
        public void RemoveEntitie(Entity self, Entity entity) => OnRemoveEntitie(self as T, entity);

        public abstract void OnAddEntitie(T self, Entity entity);
        public abstract void OnRemoveEntitie(T self, Entity entity);
    }


    /// <summary>
    /// 实体集合组
    /// </summary>
    public class EntityGroup : Dictionary<Type, Entity>, IUnitPoolItem
    {
        public PoolBase thisPool { get; set; }
        public bool IsRecycle { get; set; }
        public bool IsDisposed { get; set; }

        /// <summary>
        /// 单位对象池：获取对象
        /// </summary>
        public static EntityGroup GetObject()
        {
            return UnitPoolManager.Instance.Get<EntityGroup>();
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            OnDispose();
            IsDisposed = true;
        }

        public void OnDispose()
        {
        }

        public void OnGet()
        {
        }

        public void OnNew()
        {
        }

        public void OnRecycle()
        {
            Clear();
        }

        public void Recycle()
        {
            if (thisPool != null)
            {
                if (!thisPool.IsDisposed)
                {
                    if (!IsRecycle)
                    {
                        thisPool.Recycle(this);
                    }
                }
            }
        }
    }



    //单例本质应该是实体
    public class EntityManager : SingletonBase<EntityManager>
    {
        public Dictionary<long, Entity> allEntities = new Dictionary<long, Entity>();

        //类型，系统方法集合
        private SystemGroup managerSystems;
        //类型，实例
        private EntityGroup entitys;//遍历实例执行方法
        public override void OnInstance()
        {
            managerSystems = SystemManager.Instance.RegisterSystems<IEntitieSystem>();

            entitys = EntityGroup.GetObject();
            //!拿到的是系统方法，并不是实体
            //entitieSystems = SystemManager.Instance.GetSystemGroup<IEntitieSystem>(typeof(Entity));
        }

        public void Add(Entity entity)
        {
            Type typeKey = entity.GetType();

            if (managerSystems.ContainsKey(typeKey))//存在则说明这是个管理器
            {
                entitys.Add(typeKey, entity);
                //if (entitys.TryGetValue(typeKey, out List<Entity> entitys))
                //{


                //}
            }

            //allEntities.Add(entity.ID, entity);
            //foreach (var item in entitieSystems)
            //{
            //    item.OnAddEntitie(entity);
            //}
        }
        public void Remove(Entity entity)
        {
            //foreach (var item in entitieSystems)
            //{
            //    item.OnRemoveEntitie(entity);
            //}
            //allEntities.Remove(entity.ID);
        }
    }
}