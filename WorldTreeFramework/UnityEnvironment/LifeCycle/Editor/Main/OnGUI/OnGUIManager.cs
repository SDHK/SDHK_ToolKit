using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WorldTree
{
  
    public class OnGUIManager : Entity
    {
        public float deltaTime;
        public UnitDictionary<ulong, Entity> update1 = new UnitDictionary<ulong, Entity>();
        public UnitDictionary<ulong, Entity> update2 = new UnitDictionary<ulong, Entity>();
        public SystemGroup systems;
        public void Update()
        {
            while (update1.Count != 0 && IsActice)
            {
                ulong firstKey = update1.Keys.First();
                Entity entity = update1[firstKey];
                if (entity.IsActice)
                {
                    if (systems.TryGetValue(entity.Type, out UnitList<ISystem> systemList))
                    {
                        foreach (IUpdateSystem system in systemList)
                        {
                            system.Execute(entity,deltaTime);
                        }
                    }
                }
                update1.Remove(firstKey);
                update2.Add(firstKey, entity);
            }
            (update1, update2) = (update2, update1);
            
        }

        /*
         public class AEvent:EntityEventSystem<Entity,>
        {
        
        
         }

         */

    }
}
