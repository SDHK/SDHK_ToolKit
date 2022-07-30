
/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/18 13:24

* 描述： 

*/

using SDHK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
{

    public class Node : Entity
    {

    }

    class NodeNewSystem : AddSystem<Node>
    {
        public override void OnAdd(Node self)
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
}
