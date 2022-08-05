
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
        public async override void OnAdd(Node self)
        {
            Debug.Log("OnAdd!!!");
            await self.RootGetEvent().CallActionAsync();
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



            if (Input.GetKeyDown(KeyCode.E))
            {
                //召唤某个组的事件

            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                //召唤某个组的事件
                self.RootGetEvent("分组1").CallAction("事件召唤W", 1, 0.5f, Vector3.one, Color.red);
            }

            Debug.Log("Update!!!");
        }
    }
}

//通过特性分组
[EventKey("分组1")]
class TestEvent1 : EventActionSystem<string, int, float, Vector3, Color>
{
    public async override void Event(string arg1, int index, float f, Vector3 vector, Color color)
    {

        Debug.Log($"分组1: {arg1}|{index}|{f}|{vector}|{color}");
        await Task.Delay(1000);
        Debug.Log($"分组延迟: {arg1}|{index}|{f}|{vector}|{color}");

    }
}



//分在默认组的事件
class TestEvent0 : EventActionAsyncSystem
{
    public override async Task Event()
    {
        Debug.Log("异步1");
        await Task.Delay(1000);
        Debug.Log("异步2");
    }
}

