
/****************************************

* 作者： 闪电黑客
* 日期： 2022/8/1 9:47

* 描述： 用于重要管理器的实体单例基类

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{
    /// <summary>
    /// 实体单例基类：懒汉式 (面向对象的单例，但挂在实体点上)
    /// </summary>
    public class EntitySingletonBase1<T> : Entity
        where T : EntitySingletonBase1<T>, new()
    {
        protected static T instance;//实例
        private static readonly object _lock = new object();//对象锁

        /// <summary>
        /// 实例组件
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new T();
                            Debug.Log("[单例实体启动] : " + typeof(T).Name);
                            instance.OnInstance();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// 单例实例化
        /// </summary>
        public static T GetInstance()
        {
            return Instance;
        }

        /// <summary>
        /// 单例实例时
        /// </summary>
        public virtual void OnInstance() { }
        public override void OnDispose()
        {
            instance = null;
        }
    }

}
