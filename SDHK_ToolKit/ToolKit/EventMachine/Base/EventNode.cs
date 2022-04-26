
/******************************

 * Author: 闪电黑客

 * 日期: 2022/02/24 04:19:03

 * 最后日期: 2022/02/24 04:19:26

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System.Collections;
using System.Collections.Generic;
using ObjectFactory;
using UnityEngine;
namespace EventMachine
{
    public abstract class EventNode : IObjectPoolItem
    {
        public EventExecutor executor;
        public bool isDone = false;
        public void Done() => isDone = true;
        public abstract ObjectPoolBase thisPool { get; set; }

        public abstract void ObjectOnDestroy();
        public abstract void ObjectOnGet();
        public abstract void ObjectOnNew();
        public abstract void ObjectOnRecycle();
        public abstract void ObjectRecycle();
        public abstract void Update();
    }

}