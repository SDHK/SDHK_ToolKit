using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{
    /// <summary>
    /// 根节点实体
    /// </summary>
    public class Root : Entity
    {
        public Root() { root = this; }
    }

    /// <summary>
    /// 实体管理器
    /// </summary>
    public class EntityManager : SingletonBase<EntityManager>
    {
        public UnitDictionary<ulong, Entity> allEntities = new UnitDictionary<ulong, Entity>();

        private UnitDictionary<Type, Entity> listeners;//遍历实例执行方法
        
        private SystemGroup singletonEntitys;
        private SystemGroup listenersSystems;
        private SystemGroup startSystems;

        public override void OnInstance()
        {
            listeners = UnitDictionary<Type, Entity>.GetObject();

            singletonEntitys = SystemManager.Instance.RegisterSystems<ISingletonEagerSystem>();

            listenersSystems = SystemManager.Instance.RegisterSystems<IEntityListenerSystem>();
            startSystems = SystemManager.Instance.RegisterSystems<IStartSystem>();


            foreach (var singletonEntity in singletonEntitys)
            {
                foreach (ISingletonEagerSystem item in singletonEntity.Value)
                {
                    item.Instance();
                } 
            }

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

            if (startSystems.TryGetValue(typeKey, out UnitList<ISystem> awakes))
            {
                foreach (IStartSystem system in awakes)
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
