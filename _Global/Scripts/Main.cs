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


            self.SetChildren<Node<int>>();

            //root.GetComponent<PathAsset>().Get("A/B/C");
            //root.GetComponent<UIDomain>().GetComponent<UIManager>();
            //root.GetComponent<UIManager>();


            //self.GetNode<GameDomain>();
        }
    }

    class MainEntitySystem : UpdateSystem<MainEntity>
    {
        public override void Update(MainEntity self)
        {
            Debug.Log("MainEntityUpdate!!!");
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
