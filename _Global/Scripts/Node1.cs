using SDHK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class Node1 : Entity
    {
    }
    class Node1AddSystem : AddSystem<Node1>
    {
        public override async void OnAdd(Node1 self)
        {
          
        }
    }
    class Node1UpdateSystem : UpdateSystem<Node1>
    {
        public override void Update(Node1 self)
        {
            Debug.Log("Node1Update!!!");
        }
    }
}
