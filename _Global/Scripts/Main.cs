using SDHK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    class MainEntityNewSystem : AddSystem<MainEntity>
    {
        public override void OnAdd(MainEntity self)
        {
            Debug.Log("MainAdd!!!");
            self.GetComponent<GameDomain>();
        }
    }


    class MainEntityRemoveSystem : RemoveSystem<MainEntity>
    {
        public override void OnRemove(MainEntity self)
        {
            Debug.Log("MainRemove");
        }
    }

}
