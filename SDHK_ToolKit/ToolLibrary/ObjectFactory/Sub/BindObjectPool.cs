
/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/14 03:43:06

 * 最后日期: 2021/12/15 17:23:26

 * 最后修改: 闪电黑客

 * 描述:  

    泛型绑定对象池: 继承 ObjectPool<T>

    作用于继承了 IBindObjectPoolItem 的类型

******************************/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectFactory
{

    /// <summary>
    /// 泛型绑定对象池
    /// </summary>
    public class BindObjectPool<T> : ObjectPool<T>
    where T : class, IBindObjectPoolItem
    {
        /// <summary>
        /// 池对象（不销毁）：用于储存回收的游戏对象
        /// </summary>
        public Transform poolTransform { get; private set; }

        /// <summary>
        /// 预制体
        /// </summary>
        public GameObject prefab { get; private set; }

        //游戏对象名称
        private string objName;

        /// <summary>
        /// 对象池构造 (预制体)
        /// </summary>
        public BindObjectPool(GameObject prefabObj = null)
        {
            ObjectType = typeof(T);
            if (prefabObj != null)
            {
                prefab = prefabObj;
                objName = ObjectType.Name + "." + prefabObj.name;
            }
            else
            {
                objName = ObjectType.Name + ".BindObject";
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
            return "[BindObjectPool<" + ObjectType.Name + ">] : " + objName;
        }
        
        /// <summary>
        /// 获取对象（设置父节点）
        /// </summary>
        public T Get(Transform parent)
        {
            destoryCountDown = objectDestoryClock;

            T obj = DequeueOrNewObject();
            obj.bindGameObject.transform.parent = parent;
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
            T obj = Activator.CreateInstance(typeof(T), true) as T;
            obj.bindGameObject = gameObj;
            obj.thisPool = pool;
            return obj;
        }

        private void ObjectDestroy(T obj)
        {
            GameObject.Destroy(obj.bindGameObject);
        }
        private void ObjectOnNew(T obj)
        {
            obj.bindGameObject.SetActive(poolTransform);
            obj.ObjectOnNew();
        }
        private void ObjectOnGet(T obj)
        {
            obj.bindGameObject.SetActive(true);
            obj.ObjectOnGet();
        }
        private void ObjectOnRecycle(T obj)
        {
            obj.bindGameObject.SetActive(false);
            obj.bindGameObject.transform.SetParent(poolTransform);
            obj.ObjectOnRecycle();
        }
        private void ObjectOnDestroy(T obj)
        {
            obj.ObjectOnDestroy();
        }
    }
}