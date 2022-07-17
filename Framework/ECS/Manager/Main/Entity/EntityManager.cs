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
    public class EntityRoot : Entity
    {
        public EntityRoot()
        {
            Id = IdManager.GetID;
            Type = GetType();
            Root = this;
        }
    }

    //生命周期补全
    /// <summary>
    /// 实体管理器
    /// </summary>
    public class EntityManager : SingletonBase<EntityManager>
    {
        public UnitDictionary<ulong, IEntity> allEntities = new UnitDictionary<ulong, IEntity>();

        private UnitDictionary<Type, IEntity> listeners;//遍历实例执行方法

        private SystemGroup singletonEntitys;
        private SystemGroup entitySystems;


        private SystemManager systemManager;
        private EntityPoolManager poolManager;

        public override void OnInstance()
        {

            systemManager = SystemManager.Instance;
            listeners = UnitDictionary<Type, IEntity>.GetObject();


            //管理器系统
            entitySystems = systemManager.RegisterSystems<IEntitySystem>();
            //单例系统
            singletonEntitys = systemManager.RegisterSystems<ISingletonEagerSystem>();

            //实体对象池实例化
            poolManager = new EntityPoolManager();


            foreach (var singletonEntity in singletonEntitys)
            {
                foreach (ISingletonEagerSystem item in singletonEntity.Value)
                {
                    item.Instance();
                }
            }
        }

        public override void OnDispose()
        {
            listeners.Clear();
            listeners.Recycle();

            entitySystems.Clear();
            entitySystems.Recycle();

            singletonEntitys.Clear();
            singletonEntitys.Recycle();

            poolManager.Dispose();
            systemManager.Dispose();

            instance = null;
        }

        public void Add(IEntity entity)
        {
            Type typeKey = entity.Type;

            foreach (var manager in listeners)//广播给全部管理器
            {
                if (entitySystems.TryGetValue(manager.Key, out UnitList<ISystem> systems))
                {
                    foreach (IEntitySystem system in systems)
                    {
                        system.AddEntity(manager.Value, entity);
                    }
                }
            }


            if (entitySystems.ContainsKey(typeKey))//检测到系统存在，则说明这是个管理器
            {
                listeners.TryAdd(typeKey, entity);
            }


        }

        public void Remove(IEntity entity)
        {
            Type typeKey = entity.Type;
            if (entitySystems.ContainsKey(typeKey))//检测到系统存在，则说明这是个管理器
            {
                listeners.Remove(typeKey);
            }

            foreach (var manager in listeners)//广播给全部管理器
            {

                if (entitySystems.TryGetValue(manager.Key, out UnitList<ISystem> systems))
                {
                    foreach (IEntitySystem system in systems)
                    {
                        system.RemoveEntity(manager.Value, entity);
                    }
                }
            }
        }
    }
}
