using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    //划分事件
    public interface IEntityListenerSystem : ISystem
    {
        public void AddEntity(Entity entity, Entity arg);
        public void RemoveEntity(Entity entity, Entity arg);
    }

    public abstract class EntityListenerSystem<T> : SystemBase<T>, IEntityListenerSystem
        where T : Entity
    {
        public void AddEntity(Entity self, Entity entity) => OnAddEntitie(self as T, entity);
        public void RemoveEntity(Entity self, Entity entity) => OnRemoveEntitie(self as T, entity);

        public abstract void OnAddEntitie(T self, Entity entity);
        public abstract void OnRemoveEntitie(T self, Entity entity);
    }

    //root.GetComponent<UpdateManager>().Update(),组件或单例

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

        public override void OnInstance()
        {
            listenersSystems = SystemManager.Instance.RegisterSystems<IEntityListenerSystem>();
            listeners = UnitDictionary<Type, Entity>.GetObject();

        }

        public void Add(Entity entity)
        {
            Type typeKey = entity.GetType();

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

            if (listenersSystems.ContainsKey(typeKey))//检测到系统存在，则说明这是个管理器
            {
                listeners.TryAdd(typeKey, entity);
            }
            //entity的Awake，或许直接写在entity里
        }

        public void Remove(Entity entity)
        {
            Type typeKey = entity.GetType();
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
