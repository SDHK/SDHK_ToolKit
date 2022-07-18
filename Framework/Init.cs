using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{

    public class Node : Entity
    {

    }

    public class Node2 : Entity
    {

    }
    public class Node3 : Entity
    {

    }

    class NodeNewSystem : NewSystem<Node>
    {
        public override void OnNew(Node self)
        {
            Debug.Log("OnNew!!!");
        }
    }
    class NodeAddSystem : AddSystem<Node>
    {
        public override void OnAdd(Node self)
        {
            Debug.Log("OnAdd!!!");
        }
    }
    class NodeGetSystem : GetSystem<Node>
    {
        public override void OnGet(Node self)
        {
            Debug.Log("OnGet!!!");
        }
    }
    class NodeRemoveSystem : RemoveSystem<Node>
    {
        public override void OnRemove(Node self)
        {
            Debug.Log("OnRemove!!!");
        }
    }
    class NodeRecycleSystem : RecycleSystem<Node>
    {
        public override void OnRecycle(Node self)
        {
            Debug.Log("OnRecycle!!!");
        }
    }
    class NodeDestroySystem : DestroySystem<Node>
    {
        public override void OnDestroy(Node self)
        {
            Debug.Log("OnDestroy!!!");
        }
    }


    class NodeUpdateSystem : UpdateSystem<Node>
    {
        public override void Update(Node self)
        {
            Debug.Log("Update!!!");
        }
    }

    class Node2LateUpdateSystem : LateUpdateSystem<Node2>
    {
        public override void LateUpdate(Node2 self)
        {
            Debug.Log("LateUpdate!!!");
        }
    }
    class Node3LateUpdateSystem : FixedUpdateSystem<Node3>
    {
        public override void FixedUpdate(Node3 self)
        {
            Debug.Log("FixedUpdate!!!");
        }
    }

    public class Init : MonoBehaviour
    {

        SoloistFramework soloist;
        private void Start()
        {
            soloist = SoloistFramework.Instance;

            Debug.Log(soloist.AllEntityString(EntityRoot.Root, "\t"));

        }

        private void Update()
        {
            soloist.Update();

        }

        private void LateUpdate()
        {
            soloist.LateUpdate();

        }
        private void FixedUpdate()
        {
            soloist.FixedUpdate();

        }

        private void OnDestroy()
        {
            soloist.Dispose();
            Debug.Log(soloist.AllEntityString(EntityRoot.Root, "\t"));
        }
        private void OnApplicationQuit()
        {

        }
    }
}
