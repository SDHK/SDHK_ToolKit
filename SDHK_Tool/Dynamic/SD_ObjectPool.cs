using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.11.29
 * 
 * 功能：泛型对象池
 *
 */

namespace SDHK_Tool.Dynamic
{
    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public class SD_ObjectPool<T>
    {
        /// <summary>
        /// 工作池:激活的对集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>类</returns>
        public List<T> ObjectPool_Work = new List<T>();

        /// <summary>
        /// 闲置池:闲置的对集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>类</returns>
        public List<T> ObjectPool_Idle = new List<T>();

        /// <summary>
        /// 工作委托:工作状态启动后回调函数
        /// </summary>
        public Action<T> Object_Work;
        /// <summary>
        /// 闲置委托:闲置状态启动后回调函数
        /// </summary>
        public Action<T> Object_Idle;

        /// <summary>
        /// 新建委托:新建对象后回调函数
        /// </summary>
        public Action<T> Object_New;
        /// <summary>
        /// 删除委托:删除对象前回调函数
        /// </summary>
        public Action<T> Object_Del;


        private Func<T> ObjectPool_New;//实例化对象的方法
        private Action<T> ObjectPool_Del;//删除对象的方法

        /// <summary>
        /// 初始化对象池
        /// </summary>
        /// <param name="New">实例化委托</param>
        /// <param name="Del">删除委托</param>
        public SD_ObjectPool(Func<T> New, Action<T> Del)
        {
            ObjectPool_New = New;//实例化委托
            ObjectPool_Del = Del;//删除委托
        }

        /// <summary>
        /// 获取对象（若没有闲置对象则新建对象为工作对象）
        /// </summary>
        /// <returns>对象</returns>
        public T Get_Object()
        {
            T obj;

            if (ObjectPool_Idle.Count > 0)
            {
                obj = ObjectPool_Idle[0];
                ObjectPool_Work.Add(obj);
                ObjectPool_Idle.RemoveAt(0);
            }
            else
            {
                obj = ObjectPool_New();
                if (Object_New != null) Object_New(obj);
                ObjectPool_Work.Add(obj);
            }

            if (Object_Work != null) Object_Work(obj);

            return obj;
        }

        /// <summary>
        /// 闲置对象(将工作对象设置为闲置对象)
        /// </summary>
        /// <param name="Object">要闲置的对象</param>
        public void Set_Object(T Object)
        {
            ObjectPool_Work.Remove(Object);
            ObjectPool_Idle.Add(Object);//从工作变成闲置状态
            if (Object_Idle != null) Object_Idle(Object);
        }

        /// <summary>
        /// 从对象池中删除一个对象
        /// </summary>
        /// <param name="Object">要删除的对象</param>
        public void Clear_ObjectPool(T Object)
        {
            ObjectPool_Work.Remove(Object);
            ObjectPool_Idle.Remove(Object);
            if (Object_Del != null) Object_Del(Object);
            ObjectPool_Del(Object);

        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public void Clear_ObjectPool()
        {
            if (Object_Del != null)
            {
                ObjectPool_Work.ForEach(Object_Del);
                ObjectPool_Idle.ForEach(Object_Del);
            }

            ObjectPool_Work.ForEach(ObjectPool_Del);
            ObjectPool_Idle.ForEach(ObjectPool_Del);

            ObjectPool_Work.Clear();
            ObjectPool_Idle.Clear();
        }

    }

}