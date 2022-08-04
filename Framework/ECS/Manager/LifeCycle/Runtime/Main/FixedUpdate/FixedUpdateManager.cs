using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// FixedUpdate生命周期管理器实体
    /// </summary>
    public class FixedUpdateManager : Entity
    {
        public UnitDictionary<ulong, Entity> update1 = new UnitDictionary<ulong, Entity>();
        public UnitDictionary<ulong, Entity> update2 = new UnitDictionary<ulong, Entity>();
        public SystemGroup systems;

        public void Update()
        {
            while (update1.Count != 0 && RealActive)
            {
                ulong firstKey = update1.Keys.First();
                Entity entity = update1[firstKey];
                if (entity.RealActive)
                {
                    if (systems.TryGetValue(entity.Type, out UnitList<ISystem> systemList))
                    {
                        foreach (IFixedUpdateSystem system in systemList)
                        {
                            system.Execute(entity);
                        }
                    }
                }
                update1.Remove(firstKey);
                update2.Add(firstKey, entity);
            }
            (update1, update2) = (update2, update1);
        }
    }


    class FixedUpdateManagerNewSystem : NewSystem<FixedUpdateManager>
    {
        public override void OnNew(FixedUpdateManager self)
        {
            self.systems = self.RootGetSystemGroup<IFixedUpdateSystem>();
        }
    }

    class FixedUpdateManagerDestroySystem : DestroySystem<FixedUpdateManager>
    {
        public override void OnDestroy(FixedUpdateManager self)
        {
            self.systems = null;
        }
    }

    class FixedUpdateManagerEntityListenerSystem : EntitySystem<FixedUpdateManager>
    {
        public override void OnAddEntity(FixedUpdateManager self, Entity entity)
        {
            if (self.systems.ContainsKey(entity.Type))
            {
                self.update2.Add(entity.id, entity);
            }
        }

        public override void OnRemoveEntity(FixedUpdateManager self, Entity entity)
        {
            if (self.systems.ContainsKey(entity.Type))
            {
                self.update1.Remove(entity.id);
                self.update2.Remove(entity.id);
            }
        }
    }
}
