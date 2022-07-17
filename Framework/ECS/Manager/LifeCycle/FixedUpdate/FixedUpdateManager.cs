using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    public class FixedUpdateManager : SingletonEntityBase<FixedUpdateManager>
    {
        public UnitDictionary<ulong, IEntity> update1 = new UnitDictionary<ulong, IEntity>();
        public UnitDictionary<ulong, IEntity> update2 = new UnitDictionary<ulong, IEntity>();
        public SystemGroup systems;

        public void Update()
        {
            while (update1.Count != 0)
            {
                ulong firstKey = update1.Keys.First();
                IEntity entity = update1[firstKey];

                if (systems.TryGetValue(entity.Type, out UnitList<ISystem> systemList))
                {
                    foreach (IFixedUpdateSystem system in systemList)
                    {
                        system.Execute(entity);
                    }
                }
                update1.Remove(firstKey);
                update2.Add(firstKey, entity);
            }
            (update1, update2) = (update2, update1);
        }
    }


    class FixedUpdateManagerSingletonEagerSystem : SingletonEagerSystem<FixedUpdateManager> { }


    class FixedUpdateManagerNewSystem : NewSystem<FixedUpdateManager>
    {
        public override void OnNew(FixedUpdateManager entity)
        {
            entity.systems = SystemManager.Instance.RegisterSystems<IFixedUpdateSystem>();
        }
    }

    class FixedUpdateManagerEntityListenerSystem : EntitySystem<FixedUpdateManager>
    {
        public override void OnAddEntity(FixedUpdateManager self, IEntity entity)
        {
            if (self.systems.ContainsKey(entity.Type))
            {
                self.update2.Add(entity.Id, entity);
            }
        }

        public override void OnRemoveEntity(FixedUpdateManager self, IEntity entity)
        {
            if (self.systems.ContainsKey(entity.Type))
            {
                self.update1.Remove(entity.Id);
                self.update2.Remove(entity.Id);
            }
        }
    }
}
