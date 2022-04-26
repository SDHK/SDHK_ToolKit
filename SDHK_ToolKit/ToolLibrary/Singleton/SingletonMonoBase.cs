﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Singleton
{
    /// <summary>
    ///  Mono泛型单例抽象基类
    /// </summary>
    public abstract class SingletonMonoBase<T> : MonoBehaviour
    where T : SingletonMonoBase<T>
    {
        protected static T instance;//实例
        private static readonly object _lock = new object();//对象锁

        public static bool isInstance => instance != null;

        /// <summary>
        /// 单例实例化
        /// </summary>
        public static T Instance()
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        var gameObj = new GameObject(typeof(T).Name);
                        instance = gameObj.AddComponent<T>();
                        UnityEngine.Object.DontDestroyOnLoad(gameObj);
                        Debug.Log("[单例启动][Mono] : " + gameObj.name);
                        instance.OnInstance();
                    }
                }
            }

            return instance;
        }
        private void Start()
        {
            if (instance == null)
            {
                instance = this as T;
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                Debug.Log("[单例启动][Mono] : " + gameObject.name);
                instance.OnInstance();
            }
        }

        public virtual void OnInstance() { }




    }








}