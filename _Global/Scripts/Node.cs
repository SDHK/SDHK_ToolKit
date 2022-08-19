
/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/18 13:24

* 描述： 

*/

using AsyncAwaitEvent;
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

    class NodeNewSystem : NewSystem<Node>
    {
        public override void OnNew(Node self)
        {
            Debug.Log("OnNew1!!!");
        }
    }
    class NodeAddSystem : AddSystem<Node>
    {
        public override async void OnAdd(Node self)
        {

            //await self.SendAsync();
            do
            {
                Debug.Log("while1");

                Debug.Log(await self.CallAsync<Node, int>());

                Debug.Log("while2");
            } while (!Input.GetKeyDown(KeyCode.A));
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
            }

            Debug.Log("Update!!!");
        }


    }


    //异步事件测试
    class TestEvent0 : EventCallSystem<Node, AsyncTask>
    {
        public override async AsyncTask Event(Node self)
        {
            await self.AsyncDelay(1);
            Debug.Log("延迟1秒");
        }
    }

    class TestEvent1 : EventCallSystem<Node, AsyncTask<int>>
    {
        public override async AsyncTask<int> Event(Node self)
        {
            await self.AsyncDelay(3);//延迟3秒后返回
            return 101;
        }
    }

}




