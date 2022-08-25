using UnityEngine;
using WorldTree;

namespace Scripts
{
    class MainDomainAddSystem : AddSystem<MainDomain>
    {
        public override void OnAdd(MainDomain self)
        {
            World.Log("MainAdd!!!");

            self.AddComponent<Node>();
        }
    }


    class MainUpdateSystem : UpdateSystem<MainDomain>
    {
        public override void Update(MainDomain self, float deltaTime)
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


    class MainRemoveSystem : RemoveSystem<MainDomain>
    {
        public override void OnRemove(MainDomain self)
        {
            World.Log("MainRemove");
        }
    }

}
