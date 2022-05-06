/****************************************

* �� �� �ߣ�  ����ڿ�
* ����ʱ�䣺  2022/5/6 21:27
* ��    ��:

****************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Singleton
{
    public class SingletonBase<T>
    where T : SingletonBase<T>, new()
    {

        protected static T instance;//ʵ��
        private static readonly object _lock = new object();//������

        /// <summary>
        /// ����ʵ����
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
                        Debug.Log("[��������] : " + typeof(T).Name);
                        instance.OnInstance();
                    }
                }
            }
            return instance;
        }

        public virtual void OnInstance() { }

    }
}