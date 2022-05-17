

/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/13 13:39:02

 * 最后日期: 2021/12/15 17:39:37

 * 最后修改: 闪电黑客

 * 描述:  

    泛型对象池: 继承 ObjectPool<T>

    作用于继承了 IObjectPoolItem 的类型

******************************/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SDHK
{
    /// <summary>
    /// 泛型对象池
    /// </summary>
    public class ClassObjectPool<T> : GenericPool<T>
    where T : class, IObjectPoolItem
    {
        /// <summary>
        /// 对象池构造
        /// </summary>
        public ClassObjectPool()
        {
            ObjectType = typeof(T);

            NewObject = ObjectNew;

            objectOnNew += ObjectOnNew;
            objectOnDestroy += ObjectOnDestroy;
            objectOnGet += ObjectOnGet;
            objectOnRecycle += ObjectOnRecycle;
        }

        public override string ToString()
        {
            return "[ClassObjectPool<" + ObjectType.Name + ">] ";
        }

        private  T ObjectNew(PoolBase pool)
        {
            T obj = Activator.CreateInstance(ObjectType, true) as T;
            obj.thisPool = pool;
            return obj;
        }

        private static void ObjectOnNew(T obj)
        {
            obj.ObjectOnNew();
        }
        private static void ObjectOnGet(T obj)
        {
            obj.ObjectOnGet();
        }
        private static void ObjectOnRecycle(T obj)
        {
            obj.ObjectOnRecycle();
        }
        private static void ObjectOnDestroy(T obj)
        {
            obj.ObjectOnDestroy();
        }

    }

}
