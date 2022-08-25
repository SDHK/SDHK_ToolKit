using WorldTree;

namespace Scripts
{
    public class Node1 : Entity
    {
    }
    class Node1AddSystem : AddSystem<Node1>
    {
        public override  void OnAdd(Node1 self)
        {
          
        }
    }
    class Node1UpdateSystem : UpdateSystem<Node1>
    {
        public override void Update(Node1 self, float deltaTime)
        {
            World.Log("Node1Update!!!");
        }
    }

    class Node1EnableSystem : EnableSystem<Node1>
    {
        public override void OnEnable(Node1 self)
        {
            World.Log("Node1OnEnable!!!");
        }
    }

    class Node1DisbleSystem : DisableSystem<Node1>
    {
        public override void OnDisable(Node1 self)
        {
            World.Log("Node1OnDisble!!!");

        }
    }
}
