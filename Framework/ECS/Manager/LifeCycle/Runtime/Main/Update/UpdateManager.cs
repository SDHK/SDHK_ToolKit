﻿using SDHK;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDHK
{

    /// <summary>
    /// Update生命周期管理器实体 
    /// </summary>
    public class UpdateManager : Entity
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
                        foreach (IUpdateSystem system in systemList)
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



    class UpdateManagerNewSystem : NewSystem<UpdateManager>
    {
        public override void OnNew(UpdateManager self)
        {
            self.systems = self.RootGetSystemGroup<IUpdateSystem>();
        }
    }

    class UpdateManagerDestroySystem : DestroySystem<UpdateManager>
    {
        public override void OnDestroy(UpdateManager self)
        {
            self.systems = null;
        }
    }

    class UpdateManagerEntityListenerSystem : EntitySystem<UpdateManager>
    {
        public override void OnAddEntity(UpdateManager self, Entity entity)
        {
            if (self.systems.ContainsKey(entity.Type))
            {
                self.update2.Add(entity.id, entity);
            }
        }

        public override void OnRemoveEntity(UpdateManager self, Entity entity)
        {
            if (self.systems.ContainsKey(entity.Type))
            {
                self.update1.Remove(entity.id);
                self.update2.Remove(entity.id);
            }
        }
    }








}