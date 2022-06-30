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
        public void OnAddEntitie(Entity entity);
        public void OnRemoveEntitie(Entity entity);

    }

    public class EntitieManager : SingletonEagerBase<EntitieManager>
    {
        public Dictionary<long, Entity> allEntities = new Dictionary<long, Entity>();

        private List<IEntitieSystem> entitieSystems;

        public override void OnInstance()
        {
            SystemManager.Instance.RegisterSystems<IEntitieSystem>();
            entitieSystems = SystemManager.Instance.GetSystems<IEntitieSystem>(typeof(Entity));
        }

        public void Add(Entity entity)
        {
            allEntities.Add(entity.ID, entity);
            foreach (var item in entitieSystems)
            {
                item.OnAddEntitie(entity);
            }
        }
        public void Remove(Entity entity)
        {
            foreach (var item in entitieSystems)
            {
                item.OnRemoveEntitie(entity);
            }
            allEntities.Remove(entity.ID);
        }
    }
}
