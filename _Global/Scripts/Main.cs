using UnityEngine;
using WorldTree;

namespace Scripts
{

    class MainAddSystem : AddSystem<Main>
    {
        public override void OnAdd(Main self)
        {
            World.Log("MainAdd!!!");

            self.AddComponent<Node>();
        }
    }

    class MainUpdateSystem : UpdateSystem<Main>
    {
        public override void Update(Main self, float deltaTime)
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


    class MainRemoveSystem : RemoveSystem<Main>
    {
        public override void OnRemove(Main self)
        {
            World.Log("MainRemove");
        }
    }

}
