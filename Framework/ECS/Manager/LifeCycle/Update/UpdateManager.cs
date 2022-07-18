using SDHK;
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
    public class UpdateManager : SingletonEntityBase<UpdateManager>
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
                    foreach (IUpdateSystem system in systemList)
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


    public class UpdateManagerSingletonEagerSystem : SingletonEagerSystem<UpdateManager> { }


    public class UpdateManagerNewSystem : NewSystem<UpdateManager>
    {
        public override void OnNew(UpdateManager entity)
        {
            entity.systems = SystemManager.Instance.RegisterSystems<IUpdateSystem>();
        }
    }

    class UpdateManagerDestroySystem : DestroySystem<UpdateManager>
    {
        public override void OnDestroy(UpdateManager entity)
        {
            entity.systems.Clear();
            entity.systems.Recycle();
            entity.Dispose();
        }
    }

    public class UpdateManagerEntityListenerSystem : EntitySystem<UpdateManager>
    {
        public override void OnAddEntity(UpdateManager self, IEntity entity)
        {
            if (self.systems.ContainsKey(entity.Type))
            {
                self.update2.Add(entity.Id, entity);
            }
        }

        public override void OnRemoveEntity(UpdateManager self, IEntity entity)
        {
            if (self.systems.ContainsKey(entity.Type))
            {
                self.update1.Remove(entity.Id);
                self.update2.Remove(entity.Id);
            }
        }
    }








}