
/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/13 04:33:09

 * 最后日期: 2021/12/15 17:46:40

 * 最后修改: 闪电黑客

 * 描述:  

    游戏物体对象池: 继承 ObjectPool<T>

    作用于 GameObject 类型

******************************/



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SDHK
{

    public class GameObjectPool : GenericPool<GameObject>
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
        public GameObjectPool(GameObject prefabObj = null)
        {

            ObjectType = typeof(GameObject);

            if (prefabObj != null)
            {
                prefab = prefabObj;
                objName = prefabObj.name;
            }
            else
            {
                objName = "GameObject";
            }
            poolTransform = new GameObject(ToString()).transform;
            GameObject.DontDestroyOnLoad(poolTransform);

            NewObject = ObjectNew;
            DestroyObject = ObjectDestroy;

            objectOnNew += ObjectOnNew;
            objectOnGet += ObjectOnGet;
            objectOnRecycle += ObjectOnRecycle;

        }
        public override string ToString()
        {
            return "[GameObjectPool] : " + objName;
        }

        /// <summary>
        /// 获取对象（设置父节点）
        /// </summary>
        public GameObject Get(Transform parent)
        {
            destoryCountDown = objectDestoryClock;

            GameObject gameObject = DequeueOrNewObject();
            gameObject.transform.parent = parent;
            objectOnGet?.Invoke(gameObject);
            Preload();

            return gameObject;
        }

        //public override void Destroy()
        //{
        //    base.Destroy();
        //    GameObject.Destroy(poolTransform.gameObject);
        //}


        private GameObject ObjectNew(PoolBase pool)
        {
            return (prefab == null) ? new GameObject(objName) : GameObject.Instantiate(prefab);
        }
        private void ObjectDestroy(GameObject gameObject)
        {
            GameObject.Destroy(gameObject);
        }

        private void ObjectOnNew(GameObject gameObject)
        {
            gameObject.SetActive(poolTransform);
        }
        private void ObjectOnGet(GameObject gameObject)
        {
            gameObject.SetActive(true);
        }
        private void ObjectOnRecycle(GameObject gameObject)
        {
            gameObject.SetActive(false);
            gameObject.transform.SetParent(poolTransform);

        }






    }



}