
/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/13 13:37:00

 * 最后日期: 2021/12/15 17:23:49

 * 最后修改: 闪电黑客

 * 描述:  
 
    泛型对象池 ,继承 ObjectPoolBase

    1.预加载数：objectPreload = 0 ，在 池新建时 和 Get时 , 将池内对象数量保持到设定数值。
    
    2.限制数：objectLimit = -1 ，为-1时则不限制对象数量。
        数量超过限制时，对象将不再被回收保留，而是被回收后立即销毁。
    
    3.自动销毁倒计时时间（秒）：objectDestoryClock = -1，为-1时不起效，
        设定倒计时，内部计时结束后自动销毁池内对象，Get 和 Recycle 操作会重置倒计时。
    
    4.销毁间隔时间（秒）：objectDestoryIntervalClock = 0 ，为0时间隔为1帧。
        开始自动销毁时，每间隔一段时间销毁一个对象。
    

    其中 计时功能 需要 update 刷新调用才会起效。



    生命周期大致为：
    
    Get     NewObject
            objectOnNew
            objectOnGet
    
    Recycle objectOnRecycle
            objectOnDestroy
            DestroyObject



******************************/



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;

namespace SDHK
{
    /// <summary>
    /// object对象池
    /// </summary>
    public class ObjectPool : GenericPool<object> {
        public ObjectPool() : base() { }
        public ObjectPool(Type type) { ObjectType = type; }
    }

    /// <summary>
    /// 泛型通用对象池
    /// </summary>
    public class GenericPool<T> : PoolBase
    {
        /// <summary>
        /// 对象池
        /// </summary>
        private Queue<T> objetPool = new Queue<T>();

        /// <summary>
        /// 当前保留对象数量
        /// </summary>
        public override int Count => objetPool.Count;

        /// <summary>
        /// 实例化对象的方法
        /// </summary>
        public Func<PoolBase, T> NewObject;
        /// <summary>
        /// 销毁对象的方法
        /// </summary>
        public Action<T> DestroyObject;


        /// <summary>
        /// 对象新建时
        /// </summary>
        public Action<T> objectOnNew;

        /// <summary>
        /// 对象获取时
        /// </summary>
        public Action<T> objectOnGet;

        /// <summary>
        /// 对象回收时
        /// </summary>
        public Action<T> objectOnRecycle;

        /// <summary>
        /// 对象销毁时
        /// </summary>
        public Action<T> objectOnDestroy;


        //用于继承的子类可实现无参数构造函数
        protected GenericPool() { ObjectType = typeof(T); }

        /// <summary>
        /// 对象池构造 （实例化对象的委托，销毁对象的委托）
        /// </summary>
        public GenericPool(Func<PoolBase, T> objectNew, Action<T> objectDestroy = null)
        {
            ObjectType = typeof(T);
            this.NewObject = objectNew;
            this.DestroyObject = objectDestroy;
        }

        public override string ToString()
        {
            return "[ObjectPool<" + ObjectType.Name + ">]";
        }

        /// <summary>
        /// 从队列获取一个对象，假如队列无对象则新建对象
        /// </summary>
        public T DequeueOrNewObject()
        {
            lock (objetPool)
            {
                T obj = default(T);

                while (objetPool.Count > 0 && obj.Equals(default(T)))
                {
                    obj = objetPool.Dequeue();
                }

                if (obj.Equals(default(T)))
                {
                    if (NewObject != null)
                    {
                        obj = NewObject(this);
                    }
                    objectOnNew?.Invoke(obj);
                }

                return obj;
            }
        }


        /// <summary>
        /// 获取对象
        /// </summary>
        public T Get()
        {
            destoryCountDown = objectDestoryClock;

            T obj = DequeueOrNewObject();
            objectOnGet?.Invoke(obj);
            Preload();
            return obj;
        }


        /// <summary>
        /// 回收对象
        /// </summary>
        public void Recycle(T obj)
        {
            lock (objetPool)
            {
                if (obj != null)
                {

                    if (objectLimit == -1 || objetPool.Count < objectLimit)
                    {
                        if (!objetPool.Contains(obj))
                        {
                            objectOnRecycle?.Invoke(obj);
                            objetPool.Enqueue(obj);
                        }
                    }
                    else
                    {
                        objectOnRecycle?.Invoke(obj);
                        objectOnDestroy?.Invoke(obj);
                        DestroyObject?.Invoke(obj);
                    }
                }
            }
        }
        public override object GetObject()
        {
            return (object)Get();
        }

        public override void Recycle(object obj)
        {
            Recycle((T)obj);
        }

        public override void Clear()
        {
            lock (objetPool)
            {
                for (int i = objetPool.Count - 1; i >= 0; i--)
                {
                    var obj = objetPool.Dequeue();
                    objectOnDestroy?.Invoke(obj);
                    DestroyObject?.Invoke(obj);
                }
                objetPool.Clear();
            }
        }

        public override void Preload()
        {
            lock (objetPool)
            {
                while (objetPool.Count < objectPreload)
                {
                    T obj = NewObject(this);
                    objectOnNew?.Invoke(obj);
                    objetPool.Enqueue(obj);
                }
            }
        }

        public override void Update(float deltaTime)
        {
            if (objectDestoryClock != -1)
            {
                if (objetPool.Count > 0)
                {
                    if (destoryCountDown == -1)
                    {
                        if (destoryIntervalCountDown > 0)//回收间隔倒计时计时
                        {
                            destoryIntervalCountDown -= deltaTime;
                        }
                        else
                        {
                            var obj = objetPool.Dequeue();//清除回调
                            objectOnDestroy?.Invoke(obj);
                            DestroyObject?.Invoke(obj);
                            destoryIntervalCountDown = objectDestoryIntervalClock;
                        }
                    }
                    else
                    {
                        if (destoryCountDown > 0)//回收倒计时
                        {
                            destoryCountDown -= deltaTime;
                        }
                        else
                        {
                            destoryCountDown = -1;
                        }
                    }
                }
            }
        }

        protected override void OnDispose()
        {
            Clear();
            NewObject = null;
            DestroyObject = null;
            objectOnNew = null;
            objectOnGet = null;
            objectOnRecycle = null;
            objectOnDestroy = null;
        }
    }
}