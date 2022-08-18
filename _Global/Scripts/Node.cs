﻿
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
            do
            {

                self.Send(1);
                self.Call<Node, int>();

                Debug.Log("while1");
                await self.SendAsync();//延迟3秒
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



    //分在默认组的事件
    class TestEvent0 : EventCallSystem<Node, AsyncTask>
    {
        public override async AsyncTask Event(Node arg1)
        {
            Debug.Log("AsyncTask1");
            await arg1.AsyncDelay(1);
        }
    }

}




