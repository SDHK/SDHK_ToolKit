﻿
/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/13 13:38:13

 * 最后日期: 2021/12/15 17:49:41

 * 最后修改: 闪电黑客

 * 描述:  

    泛型Mono对象池: 继承 ObjectPool<T>

    作用于继承了 MonoBehaviour、IObjectPoolItem 的类型

******************************/



using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Profiling;

namespace ObjectFactory
{

    /// <summary>
    /// 泛型Mono对象池
    /// </summary>
    public class MonoObjectPool<T> : ObjectPool<T>
    where T : MonoBehaviour, IObjectPoolItem
    {
        /// <summary>
        /// 池对象（不销毁）：用于储存回收的游戏对象
        /// </summary>
        public Transform poolTransform { get; private set; }

        /// <summary>
        /// 预制体
        /// </summary>
        public GameObject prefab { get; private set; }
        private string objName;

        /// <summary>
        /// 对象池构造 (预制体)
        /// </summary>
        public MonoObjectPool(GameObject prefabObj = null)
        {
            ObjectType = typeof(T);
            if (prefabObj != null)
            {
                prefab = prefabObj;
                objName = ObjectType.Name + "." + prefabObj.name;
            }
            else
            {
                objName = ObjectType.Name + ".MonoObject";
            }
            poolTransform = new GameObject(ToString()).transform;
            GameObject.DontDestroyOnLoad(poolTransform);

            NewObject = ObjectNew;
            DestroyObject = ObjectDestroy;

            objectOnNew += ObjectOnNew;
            objectOnDestroy += ObjectOnDestroy;
            objectOnGet += ObjectOnGet;
            objectOnRecycle += ObjectOnRecycle;


        }

        public override string ToString()
        {
            return "[MonoObjectPool<" + ObjectType.Name + ">] : " + objName;
        }

        /// <summary>
        /// 获取对象（设置父节点）
        /// </summary>
        public T Get(Transform parent)
        {
            destoryCountDown = objectDestoryClock;

            T obj = DequeueOrNewObject();
            obj.transform.parent = parent;
            objectOnGet?.Invoke(obj);
            Preload();

            return obj;
        }
        public override void Destroy()
        {
            base.Destroy();
            GameObject.Destroy(poolTransform.gameObject);
        }



        private T ObjectNew(ObjectPoolBase pool)
        {
            GameObject gameObj = (prefab == null) ? new GameObject(objName) : GameObject.Instantiate(prefab);
            gameObj.name = objName;
            T obj = gameObj.GetComponent<T>();
            if (obj == null)
            {
                obj = gameObj.AddComponent<T>();
            }
            obj.thisPool = pool;
            return obj;
        }

        private void ObjectDestroy(T obj)
        {
            GameObject.Destroy(obj.gameObject);
        }
        private void ObjectOnNew(T obj)
        {
            obj.transform.SetParent(poolTransform);
            obj.ObjectOnNew();
        }
        private void ObjectOnGet(T obj)
        {
            obj.gameObject.SetActive(true);
            obj.ObjectOnGet();
        }
        private void ObjectOnRecycle(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(poolTransform);
            obj.ObjectOnRecycle();
        }

        private void ObjectOnDestroy(T obj)
        {
            obj.ObjectOnDestroy();
        }

    }


}
