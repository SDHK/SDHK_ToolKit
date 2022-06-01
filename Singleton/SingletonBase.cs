﻿/****************************************

* 创 建 者：  闪电黑客
* 创建时间：  2022/5/7 9:23
* 描    述:

****************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Singleton
{
    public class SingletonBase<T>
    where T : SingletonBase<T>, new()
    {

        protected static T instance;//实例
        private static readonly object _lock = new object();//对象锁

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
                        instance = new T();
                        Debug.Log("[单例启动] : " + typeof(T).Name);
                        instance.OnInstance();
                    }
                }
            }
            return instance;
        }

        public virtual void OnInstance() { }

    }
}