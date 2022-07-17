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

    class NodeStartSystem : NewSystem<Node>
    {
        public override void OnNew(Node self)
        {
            Debug.Log("OnNew!!!");
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

    public class Init : SingletonMonoEagerBase<Init>
    {

        SoloistFramework soloist;
        private void Start()
        {
            soloist = SoloistFramework.Instance;

            soloist.Start();
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
            soloist.OnInstance();

        }
        private void OnApplicationQuit()
        {

        }
    }
}
