using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Singleton
{
    /// <summary>
    /// 泛型单例抽象基类
    /// </summary>
    public abstract class SingletonBase<T>
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

        
        public virtual void OnInstance(){}


    }





}