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
            //self.Domain.GetComponent<GameDomain>().GetChildren<Node>();

            Debug.Log(self.Domain);
            Debug.Log(self.Domain.GetComponent<GameDomain>().Domain);
            //self.Domain.GetComponent<UiDomain>();

            //root.GetComponent<PathAsset>().Get("A/B/C");
            //root.GetComponent<UIDomain>().GetComponent<UIManager>();
            //root.GetComponent<UIManager>();


            //self.GetNode<GameDomain>();
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
