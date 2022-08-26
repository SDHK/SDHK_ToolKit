using UnityEngine;
using WorldTree;

namespace Scripts
{
    class InitialDomainAddSystem : AddSystem<InitialDomain>
    {
        public override void OnAdd(InitialDomain self)
        {
            World.Log("MainAdd!!!");
            // singularity
            self.AddComponent<Node>();
        }
    }


    class InitialDomainUpdateSystem : UpdateSystem<InitialDomain>
    {
        public override void Update(InitialDomain self, float deltaTime)
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


    class InitialDomainRemoveSystem : RemoveSystem<InitialDomain>
    {
        public override void OnRemove(InitialDomain self)
        {
            World.Log("MainRemove");
        }
    }

}
