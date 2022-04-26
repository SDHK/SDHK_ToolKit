using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Touch
{
    public static class GameObjectExtension
    {

        /// <summary>
        /// 获取TouchEventDown组件
        /// </summary>
        public static TouchEventDown TouchDown(this GameObject GameObj)
        {
            return GameObj.FillComponent<TouchEventDown>();
        }

        /// <summary>
        /// 获取TouchEventEnter组件
        /// </summary>
        public static TouchEventEnter TouchEnter(this GameObject GameObj)
        {
            return GameObj.FillComponent<TouchEventEnter>();
        }

        /// <summary>
        /// 获取TouchEventDrag组件
        /// </summary>
        public static TouchEventDrag TouchDrag(this GameObject GameObj)
        {
            return GameObj.FillComponent<TouchEventDrag>();
        }

        /// <summary>
        /// 获取TouchEventSelect组件
        /// </summary>
        public static TouchEventSelect TouchSelect(this GameObject GameObj)
        {
            return GameObj.FillComponent<TouchEventSelect>(); ;
        }

        /// <summary>
        /// 填充组件：假如组件没有则添加
        /// </summary>
        private static T FillComponent<T>(this GameObject GameObj)
        where T : Component
        {
            T t = GameObj.GetComponent<T>();
            if (t == null)
            {
                t = GameObj.AddComponent<T>();
            }
            return t;
        }

    }

}