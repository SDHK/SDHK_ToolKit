
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


    public class Node<T> : Entity
    {
        public async MyAwaitable<int> MyAwaitableMethod()
        {
            int result = 0;
            int arg1 = await this.GetMyAwaitable(1);
            Debug.Log(arg1);
            result += arg1;
            int arg2 = await this.GetMyAwaitable(2);
            Debug.Log(arg2);

            result += arg2;
            int arg3 = await this.GetMyAwaitable(3);
            Debug.Log(arg3);
            result += arg3;
            return result;
        }

        public async MyAwaitable<int> GetMyAwaitable(int arg)
        {
            return await new MyAwaitable<int>(arg);
        }
    }

    class NodeNewSystem : NewSystem<Node<int>>
    {
        public override async void OnNew(Node<int> self)
        {
            Debug.Log("OnNew1!!!");
            //await self.MyAwaitableMethod();

        }
    }
    class NodeAddSystem : AddSystem<Node<int>>
    {
        public override async void OnAdd(Node<int> self)
        {
            Debug.Log("OnAdd1!!!");

            do
            {
                Debug.Log("while1");
                await self.AddChildren<AsyncTask>();
                Debug.Log("while2");

            } while (!Input.GetKeyDown(KeyCode.A));

            Debug.Log("OnAdd2!!!");

            do
            {
                Debug.Log("while3");
                await self.AddChildren<AsyncTask>();
                Debug.Log("while4");

            } while (!Input.GetKeyDown(KeyCode.B));

            Debug.Log("OnAdd3!!!");

        }

    }
    class NodeGetSystem : GetSystem<Node<int>>
    {
        public override void OnGet(Node<int> self)
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




