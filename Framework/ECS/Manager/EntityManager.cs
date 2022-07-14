﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{
    public class Root : Entity
    {
        public Root() { root = this; }
    }

    /// <summary>
    /// 实体监听系统接口
    /// </summary>
    public interface IEntityListenerSystem : ISystem
    {
        public void AddEntity(Entity entity, Entity arg);
        public void RemoveEntity(Entity entity, Entity arg);
    }
    /// <summary>
    /// 实体监听系统基类
    /// </summary>
    public abstract class EntityListenerSystem<T> : SystemBase<T>, IEntityListenerSystem
        where T : Entity
    {
        public void AddEntity(Entity self, Entity entity) => OnAddEntitie(self as T, entity);
        public void RemoveEntity(Entity self, Entity entity) => OnRemoveEntitie(self as T, entity);

        /// <summary>
        /// 实体添加时
        /// </summary>
        public abstract void OnAddEntitie(T self, Entity entity);
        
        /// <summary>
        /// 实体移除时
        /// </summary>
        public abstract void OnRemoveEntitie(T self, Entity entity);
    }

    public interface IAwakeSystem : ISystem
    {
        void Execute(Entity entity);
    }

    public abstract class AwakeSystem<T> : SystemBase<T>, IAwakeSystem
        where T : Entity
    {
        public void Execute(Entity entity) => Awake(entity as T);
        public abstract void Awake(T entity);

    }

    /// <summary>
    /// 实体管理器
    /// </summary>
    public class EntityManager : SingletonBase<EntityManager>
    {
        public UnitDictionary<ulong, Entity> allEntities = new UnitDictionary<ulong, Entity>();

        //监听类实例
        private UnitDictionary<Type, Entity> listeners;//遍历实例执行方法
        //监听类系统方法集合
        private SystemGroup listenersSystems;
        private SystemGroup awakeSystems;

        public override void OnInstance()
        {
            listenersSystems = SystemManager.Instance.RegisterSystems<IEntityListenerSystem>();
            awakeSystems = SystemManager.Instance.RegisterSystems<IAwakeSystem>();


            listeners = UnitDictionary<Type, Entity>.GetObject();

        }

        public void Add(Entity entity)
        {
            Type typeKey = entity.type;

            foreach (var manager in listeners)//广播给全部管理器
            {
                if (listenersSystems.TryGetValue(manager.Key, out UnitList<ISystem> systems))
                {
                    foreach (IEntityListenerSystem system in systems)
                    {
                        system.AddEntity(manager.Value, entity);
                    }
                }
            }

            //entity的Awake，或许直接写在entity里
            if (awakeSystems.TryGetValue(typeKey, out UnitList<ISystem> awakes))
            {
                foreach (IAwakeSystem system in awakes)
                {
                    system.Execute(entity);
                }
            }

            if (listenersSystems.ContainsKey(typeKey))//检测到系统存在，则说明这是个管理器
            {
                listeners.TryAdd(typeKey, entity);
            }


        }

        public void Remove(Entity entity)
        {
            Type typeKey = entity.type;
            if (listenersSystems.ContainsKey(typeKey))//检测到系统存在，则说明这是个管理器
            {
                listeners.Remove(typeKey);
            }

            foreach (var manager in listeners)//广播给全部管理器
            {
                if (listenersSystems.TryGetValue(manager.Key, out UnitList<ISystem> systems))
                {
                    foreach (IEntityListenerSystem system in systems)
                    {
                        system.RemoveEntity(manager.Value, entity);
                    }
                }
            }
        }
    }
}
