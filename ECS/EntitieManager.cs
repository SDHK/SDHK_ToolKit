using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{

    public interface IEntitieSystem : ISystem
    {
        public void OnAddEntitie(Entity entity);
        public void OnRemoveEntitie(Entity entity);

    }


    public class EntitieManager : SingletonEagerBase<EntitieManager>
    {
        public Dictionary<long, Entity> allEntities = new Dictionary<long, Entity>();

        public override void OnInstance()
        {
            SystemManager.Instance.RegisterSystems<IEntitieSystem>();
        }

        public void Add(Entity entity)
        {
            allEntities.Add(entity.ID, entity);
            foreach (var item in SystemManager.Instance.GetSystems<IEntitieSystem>(entity.GetType()))
            {
                item.OnAddEntitie(entity);
            }
        }
        public void Remove(Entity entity)
        {
            foreach (var item in SystemManager.Instance.GetSystems<IEntitieSystem>(entity.GetType()))
            {
                item.OnRemoveEntitie(entity);
            }
            allEntities.Remove(entity.ID);
        }
    }
}
