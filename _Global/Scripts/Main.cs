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

            self.Root.AddComponent<TimerManager>();
            self.AddComponent<Node>();
            //self.TaskWait().Send(10);
            //self.TaskWait(10);

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
            if (Input.GetKeyDown(KeyCode.A))
            {
                self.GetComponent<Node>().SetActive(!self.GetComponent<Node>().ActiveMark);
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                self.GetComponent<Node>().GetComponent<Node1>().SetActive(!self.GetComponent<Node>().GetComponent<Node1>().ActiveMark);
            }
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
