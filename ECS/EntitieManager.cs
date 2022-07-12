using System;
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


    //单例本质应该是实体
    public class EntityManager : SingletonBase<EntityManager>
    {
        public Dictionary<long, Entity> allEntities = new Dictionary<long, Entity>();

        //管理器实例
        private UnitDictionary<Type, Entity> managers;//遍历实例执行方法
        //管理器系统方法集合
        private SystemGroup managerSystems;

        public override void OnInstance()
        {
            managerSystems = SystemManager.Instance.RegisterSystems<IEntitieSystem>();
            managers = UnitDictionary<Type, Entity>.GetObject();

        }

        public void Add(Entity entity)
        {
            Type typeKey = entity.GetType();

            foreach (var manager in managers)//广播给全部管理器
            {
                if (managerSystems.TryGetValue(manager.Key, out UnitList<ISystem> systems))
                {
                    foreach (IEntitieSystem system in systems)
                    {
                        system.AddEntitie(manager.Value, entity);
                    }
                }
            }

            if (managerSystems.ContainsKey(typeKey))//检测到系统存在，则说明这也是个管理器
            {
                managers.TryAdd(typeKey, entity);
            }
            //entity的Awake，或许直接写在entity里

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
