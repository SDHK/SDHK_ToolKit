
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


    public class Node<T> : Entity
    {

    }

    class NodeNewSystem : NewSystem<Node<int>>
    {
        public override  void OnNew(Node<int> self)
        {
            Debug.Log("OnNew1!!!");
        }
    }
    class NodeAddSystem : AddSystem<Node<int>>
    {
        public  override async void OnAdd(Node<int> self)
        {
            Debug.Log("OnAdd1!!!");

            do
            {
                await self.AddChildren<AsyncTask>();
                //await Task.Yield();
            } while (!Input.GetKeyDown(KeyCode.A));

            Debug.Log("OnAdd2!!!");
            //do
            //{
            //    await self.AddChildren<AsyncTask>();
            //} while (!Input.GetKeyDown(KeyCode.S));

            Debug.Log("OnAdd3!!!");

        }
    }
    class NodeGetSystem : GetSystem<Node<int>>
    {
        public override  void OnGet(Node<int> self)
        {
            Debug.Log("OnGet!!!");
        }
    }
    class NodeRemoveSystem : RemoveSystem<Node<int>>
    {
        public override void OnRemove(Node<int> self)
        {
            Debug.Log("OnRemove!!!");
        }
    }
    class NodeRecycleSystem : RecycleSystem<Node<int>>
    {
        public override void OnRecycle(Node<int> self)
        {
            Debug.Log("OnRecycle!!!");
        }
    }
    class NodeDestroySystem : DestroySystem<Node<int>>
    {
        public override void OnDestroy(Node<int> self)
        {
            Debug.Log("OnDestroy!!!");
        }
    }


    class NodeUpdateSystem : UpdateSystem<Node<int>>
    {
        public override void Update(Node<int> self)
        {



            if (Input.GetKeyDown(KeyCode.E))
            {
                //召唤某个组的事件

            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                //召唤某个组的事件
                self.RootGetEvent("分组1").Send("事件召唤W", 1, 0.5f, Vector3.one, Color.red);
            }

            Debug.Log("Update!!!");
        }
    }



    //分在默认组的事件
    class TestEvent0 : EventSendSystem<Node<int>, int>
    {
        public override void Event(Node<int> arg1, int arg2)
        {
            Debug.Log("事件：" + arg2);
        }
    }

}




