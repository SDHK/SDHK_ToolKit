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


    public abstract class EntitieSystem<T> : Entity, IEntitieSystem
        where T : Entity
    {
        public Type EntityType => typeof(T);

        public void AddEntitie(Entity self, Entity entity) => OnAddEntitie(self as T, entity);
        public void RemoveEntitie(Entity self, Entity entity) => OnRemoveEntitie(self as T, entity);

        public abstract void OnAddEntitie(T self, Entity entity);
        public abstract void OnRemoveEntitie(T self, Entity entity);
    }

    //单例本质应该是实体
    public class EntitieManager : SingletonEagerBase<EntitieManager>
    {
        public Dictionary<long, Entity> allEntities = new Dictionary<long, Entity>();

        //类型，系统方法集合
        private SystemGroup entitieSystems;
        //类型，实例
        private Dictionary<Type, List<Entity>> entities;//遍历实例执行方法
        public override void OnInstance()
        {
            entitieSystems = SystemManager.Instance.RegisterSystems<IEntitieSystem>();

            //!拿到的是系统方法，并不是实体
            //entitieSystems = SystemManager.Instance.GetSystemGroup<IEntitieSystem>(typeof(Entity));
        }

        public void Add(Entity entity)
        {
            entity.GetType();
            
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
