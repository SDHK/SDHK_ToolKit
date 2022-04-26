﻿

/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/14 05:23:50

 * 最后日期: 2021/12/15 17:53:47

 * 最后修改: 闪电黑客

 * 描述:  
    
    泛型对象池管理器
    
    给对象池提供 update 刷新

******************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using System.Runtime.InteropServices;
using UnityEngine.Profiling;
using XLua;

namespace ObjectFactory
{

    /// <summary>
    /// 泛型对象池管理器
    /// </summary>
    public class ObjectPoolManager : SingletonMonoBase<ObjectPoolManager>
    {
        public List<ObjectPoolBase> pools = new List<ObjectPoolBase>();

        /// <summary>
        /// 注册对象池
        /// </summary>
        public ObjectPoolBase Register(ObjectPoolBase objectPool)
        {
            if (!pools.Contains(objectPool))
            {
                pools.Add(objectPool);
            }
            return objectPool;
        }

        /// <summary>
        /// 移除对象池
        /// </summary>
        public void Remove(ObjectPoolBase objectPool)
        {
            pools.Remove(objectPool);
        }

        void FixedUpdate()
        {
            foreach (var pool in pools)
            {
                pool.Update(Time.fixedDeltaTime);
            }
        }
    }
    
    public static class ObjectPoolExtension
    {
        /// <summary>
        /// 注册对象池到管理器
        /// </summary>
        public static T RegisterManager<T>(this T objectPool)
        where T : ObjectPoolBase
        {
            return ObjectPoolManager.Instance().Register(objectPool) as T;
        }

        /// <summary>
        /// 从管理器移除对象池 
        /// </summary>
        public static void RemoveManager(this ObjectPoolBase objectPool)
        {
            ObjectPoolManager.Instance().Remove(objectPool);
        }
    }


}