
/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/18 13:24

* 描述： 

*/

using AsyncAwaitEvent;
using WorldTree;
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
            World.Log("OnNew1!!!");
        }
    }
    class NodeAddSystem : AddSystem<Node>
    {
        public override async void OnAdd(Node self)
        {
            //await self.Event<EventCreate>().SendAsync(self);
            //await self.SendAsync();

            int i = 0;
            do
            {
                World.Log("while1");

                World.Log(await self.Event().CallAsync<Node, int, int>(self, i));

                World.Log("while2");

            } while (!Input.GetKey(KeyCode.A));
        }
    }


    class NodeGetSystem : GetSystem<Node>
    {
        public override void OnGet(Node self)
        {
            World.Log("OnGet!!!");
        }
    }
    class NodeRemoveSystem : RemoveSystem<Node>
    {
        public override void OnRemove(Node self)
        {
            World.Log("OnRemove!!!");
        }
    }
    class NodeRecycleSystem : RecycleSystem<Node>
    {
        public override void OnRecycle(Node self)
        {
            World.Log("OnRecycle!!!");
        }
    }
    class NodeDestroySystem : DestroySystem<Node>
    {
        public override void OnDestroy(Node self)
        {
            World.Log("OnDestroy!!!");
        }
    }


    class NodeUpdateSystem : UpdateSystem<Node>
    {
        public override void Update(Node self, float deltaTime)
        {
            World.Log("Update!!!");
        }


    }


    class EventCreate : EventDelegate { }

    [EventKey(typeof(EventCreate))]
    class TestEvent0 : EventCallSystem<Node, AsyncTask>
    {
        public override async AsyncTask Event(Node self)
        {
            await self.AsyncDelay(1);
            World.Log("延迟1秒");
        }
    }




    class TestEvent1 : EventCallSystem<Node, int, AsyncTask<int>>
    {
        public override async AsyncTask<int> Event(Node self, int index)
        {
            await self.AsyncDelay(1);//延迟3秒后返回
            return index + 100;
        }
    }

}




