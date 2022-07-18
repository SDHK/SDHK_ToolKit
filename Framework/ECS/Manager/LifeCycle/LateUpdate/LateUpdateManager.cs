using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// LateUpdate生命周期管理器实体
    /// </summary>
    public class LateUpdateManager : SingletonEntityBase<LateUpdateManager>
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
                    foreach (ILateUpdateSystem system in systemList)
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


    class LateUpdateManagerSingletonEagerSystem : SingletonEagerSystem<LateUpdateManager> { }


    class LateUpdateManagerNewSystem : NewSystem<LateUpdateManager>
    {
        public override void OnNew(LateUpdateManager entity)
        {

            entity.systems = SystemManager.Instance.RegisterSystems<ILateUpdateSystem>();
        }
    }

    class LateUpdateManagerDestroySystem : DestroySystem<LateUpdateManager>
    {
        public override void OnDestroy(LateUpdateManager entity)
        {
            entity.systems.Clear();
            entity.systems.Recycle();
            entity.Dispose();
        }
    }

    class LateUpdateManagerEntityListenerSystem : EntitySystem<LateUpdateManager>
    {
        public override void OnAddEntity(LateUpdateManager self, IEntity entity)
        {
            if (self.systems.ContainsKey(entity.Type))
            {
                self.update2.Add(entity.Id, entity);
            }
        }

        public override void OnRemoveEntity(LateUpdateManager self, IEntity entity)
        {
            if (self.systems.ContainsKey(entity.Type))
            {
                self.update1.Remove(entity.Id);
                self.update2.Remove(entity.Id);
            }
        }
    }
}
