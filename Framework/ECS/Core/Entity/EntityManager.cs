
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

namespace SDHK
{
    //时间外置获取
    //域的激活事件，交给update去遍历处理
    //异常处理


    /// <summary>
    /// 实体管理器
    /// </summary>
    public class EntityManager : Entity, IUnit
    {
        public UnitDictionary<long, Entity> allEntities = new UnitDictionary<long, Entity>();

        public UnitDictionary<Type, Entity> listeners = new UnitDictionary<Type, Entity>();//有监听器的实体

        private SystemGroup entitySystems;
        private SystemGroup singletonEagerSystems;

        private SystemGroup addSystems;
        private SystemGroup removeSystems;


        public IdManager idManager;
        public SystemManager systemManager;
        public UnitPoolManager UnitPool;
        public EntityPoolManager EntityPool;
        public EventManager eventManager;


        public EntityManager() : base()
        {
            //此时对象池没有，直接新建容器
            Components = new UnitDictionary<Type, Entity>();
            Children = new UnitDictionary<long, Entity>();

            //核心组件
            idManager = new IdManager();
            systemManager = new SystemManager();
            UnitPool = new UnitPoolManager();
            EntityPool = new EntityPoolManager();

            //赋予根节点
            Root = this;
            idManager.Root = this;
            systemManager.Root = this;
            UnitPool.Root = this;
            EntityPool.Root = this;

            //域节点指向自己
            Domain = this;

            //赋予id
            Root.id = idManager.GetId();
            idManager.id = idManager.GetId();
            systemManager.id = idManager.GetId();
            UnitPool.id = idManager.GetId();
            EntityPool.id = idManager.GetId();

            //实体管理器系统事件获取
            entitySystems = Root.systemManager.GetSystemGroup<IEntitySystem>();
            addSystems = Root.systemManager.GetSystemGroup<IAddSystem>();
            removeSystems = Root.systemManager.GetSystemGroup<IRemoveSystem>();
            singletonEagerSystems = systemManager.GetSystemGroup<ISingletonEagerSystem>();

            //核心组件添加
            AddComponent(idManager);
            AddComponent(systemManager);
            AddComponent(UnitPool);
            AddComponent(EntityPool);
            eventManager = AddComponent<EventManager>();

            //饿汉单例启动
            foreach (ISingletonEagerSystem singletonEager in singletonEagerSystems.Values)
            {
                singletonEager.Singleton(this);
            }
        }




        /// <summary>
        /// 回收时:对象池全部释放
        /// </summary>
        public override void OnDispose()
        {
            RemoveAll();
            listeners.Clear();
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
