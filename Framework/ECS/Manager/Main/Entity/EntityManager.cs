
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
    /// 根节点实体
    /// </summary>
    public class RootEntity : Entity
    {
        public RootEntity()
        {

            Debug.Log("NewRoot");

            if (Root==null)
            {
                Debug.Log("Root");
                Id = IdManager.GetID;
                Type = GetType();
                Root = this;
            }
        }
    }

    // 改成实体并定义为域,需要一个主域?作为主生命周期，
    // 大域后 ，分生命周期组（层）

    /// <summary>
    /// 实体管理器
    /// </summary>
    public class EntityManager : SingletonBase<EntityManager>
    {
        public UnitDictionary<ulong, IEntity> allEntities = new UnitDictionary<ulong, IEntity>();

        private UnitDictionary<Type, IEntity> listeners;//有监听器的实体

        private RootEntity rootEntity;

        private SystemGroup singletonEntitys;
        private SystemGroup entitySystems;


        private SystemManager systemManager;
        private EntityPoolManager poolManager;


        private SystemGroup addSystems;
        private SystemGroup removeSystems;

        public override void OnInstance()
        {
            rootEntity= new RootEntity();
            systemManager = SystemManager.Instance;
            listeners = UnitDictionary<Type, IEntity>.GetObject();


            //管理器系统
            entitySystems = systemManager.RegisterSystems<IEntitySystem>();
            //单例系统
            singletonEntitys = systemManager.RegisterSystems<ISingletonEagerSystem>();

            addSystems = systemManager.RegisterSystems<IAddSystem>();
            removeSystems = systemManager.RegisterSystems<IRemoveSystem>();

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
            rootEntity.RemoveAll();

            listeners.Clear();
            listeners.Recycle();

            entitySystems.Clear();
            entitySystems.Recycle();

            singletonEntitys.Clear();
            singletonEntitys.Recycle();

            addSystems.Clear();
            addSystems.Recycle();

            removeSystems.Clear();
            removeSystems.Recycle();

            poolManager.Dispose();
            systemManager.Dispose();
            IdManager.Instance.Dispose();

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

        public void Remove(IEntity entity)
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
