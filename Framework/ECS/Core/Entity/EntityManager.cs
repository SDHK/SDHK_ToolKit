
/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/18 9:35

* 描述： 实体管理器
* 最重要管理器，是ECS的启动入口
* 
* 同时启动系统管理器和对象池管理器
* 
* 还有ECS的单例启动
* 
* 管理分发全局的实体与组件的生命周期
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
    /// 实体管理器
    /// </summary>
    public class EntityManager : Entity, IUnit
    {
        public UnitDictionary<ulong, Entity> allEntities = new UnitDictionary<ulong, Entity>();

        public UnitDictionary<Type, Entity> listeners = new UnitDictionary<Type, Entity>();//有监听器的实体//分域

        public SystemGroup entitySystems;

        public SystemManager systemManager;
        public UnitPoolManager unitPoolManager;
        public EntityPoolManager pool;

        private SystemGroup addSystems;
        private SystemGroup removeSystems;


        /// <summary>
        /// 初始化：对象池的新建
        /// </summary>
        public EntityManager() : base()
        {
            id = IdManager.GetID;
            Root = this;
            Domain = this;

            Components = new UnitDictionary<Type, Entity>();
            Children = new UnitDictionary<ulong, Entity>();

            systemManager = new SystemManager();
            pool = new EntityPoolManager();
            unitPoolManager = new UnitPoolManager();

            systemManager.Root = this;
            pool.Root = this;
            unitPoolManager.Root = this;

            entitySystems = Root.systemManager.GetSystemGroup<IEntitySystem>();
            addSystems = Root.systemManager.GetSystemGroup<IAddSystem>();
            removeSystems = Root.systemManager.GetSystemGroup<IRemoveSystem>();

            AddComponent(systemManager);
            AddComponent(unitPoolManager);
            AddComponent(pool);
            AddComponent<EventManager>();
        }




        /// <summary>
        /// 回收时:对象池全部释放
        /// </summary>
        public override void OnDispose()
        {

            RemoveAll();


            pool.RemoveSelf();//移除所有组件
            pool.Dispose();//全部释放
            unitPoolManager.RemoveSelf();//移除所有组件
            unitPoolManager.Dispose();

            systemManager.RemoveSelf();
            systemManager.Dispose();


            listeners.Clear();

            listeners = null;
            entitySystems = null;
            addSystems = null;
            removeSystems = null;

            pool = null;
            systemManager = null;
            unitPoolManager = null;

        }


        public void Add(Entity entity)
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

            //这个实体的添加事件
            if (addSystems.TryGetValue(entity.Type, out UnitList<ISystem> addsystem))
            {
                foreach (IAddSystem system in addsystem)
                {
                    system.Add(entity);
                }
            }

            if (entitySystems.ContainsKey(typeKey))//检测到系统存在，则说明这是个管理器
            {
                listeners.TryAdd(typeKey, entity);
            }


        }

        public void Remove(Entity entity)
        {
            Type typeKey = entity.Type;

            if (entitySystems.ContainsKey(typeKey))//检测到系统存在，则说明这是个管理器
            {
                listeners.Remove(typeKey);
            }

            //这个实体的移除事件
            if (removeSystems.TryGetValue(entity.Type, out UnitList<ISystem> removesystem))
            {
                foreach (IRemoveSystem system in removesystem)
                {
                    system.Remove(entity);
                }
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
