﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// LateUpdate生命周期管理器实体
    /// </summary>
    public class LateUpdateManager : Entity
    {
        public UnitDictionary<long, Entity> update1 = new UnitDictionary<long, Entity>();
        public UnitDictionary<long, Entity> update2 = new UnitDictionary<long, Entity>();
        public SystemGroup systems;

        public void Update()
        {
            while (update1.Count != 0 && IsActice)
            {
                long firstKey = update1.Keys.First();
                Entity entity = update1[firstKey];
                if (entity.IsActice)
                {
                    if (systems.TryGetValue(entity.Type, out UnitList<ISystem> systemList))
                    {
                        foreach (ILateUpdateSystem system in systemList)
                        {
                            system.Execute(entity);
                        }
                    }
                }
                update1.Remove(firstKey);
                if (!entity.isRecycle)
                {
                    update2.Add(firstKey, entity);
                }
            }
            (update1, update2) = (update2, update1);
        }
    }


    class LateUpdateManagerNewSystem : NewSystem<LateUpdateManager>
    {
        public override void OnNew(LateUpdateManager self)
        {
            self.systems = self.RootGetSystemGroup<ILateUpdateSystem>();
        }
    }

    class LateUpdateManagerDestroySystem : DestroySystem<LateUpdateManager>
    {
        public override void OnDestroy(LateUpdateManager self)
        {
            self.systems = null;
        }
    }

    class LateUpdateManagerEntitySystem : EntitySystem<LateUpdateManager>
    {
        public override void OnAddEntity(LateUpdateManager self, Entity entity)
        {
            if (self.systems.ContainsKey(entity.Type))
            {
                self.update2.Add(entity.id, entity);
            }
        }

        public override void OnRemoveEntity(LateUpdateManager self, Entity entity)
        {
            if (self.systems.ContainsKey(entity.Type))
            {
                self.update1.Remove(entity.id);
                self.update2.Remove(entity.id);
            }
        }
    }
}
