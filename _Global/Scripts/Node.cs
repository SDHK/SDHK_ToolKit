
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
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //召唤默认组事件
                self.EventGet().CallAction("事件召唤Q");
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                //召唤某个组的事件
                self.EventGet("分组1").CallAction("事件召唤W");
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                //召唤某个组的事件
                Debug.Log(self.EventGet().CallFunc<string>());
            }

            Debug.Log("Update!!!");
        }
    }
}

//分在默认组的事件
class TestEvent : EventActionSystem<string>
{
    public  override void Event(string arg1)
    {
        Debug.Log(arg1);
    }
}

//分在默认组的事件
class TestEvent0 : EventFuncSystem<string>
{
    public override string Event()
    {
        return "事件返回";
    }
}

//通过特性分组
[EventKey("分组1")]
class TestEvent1 : EventActionSystem<string>
{
    public override void Event(string arg1)
    {
        Debug.Log("分组1" + arg1);
    }
}