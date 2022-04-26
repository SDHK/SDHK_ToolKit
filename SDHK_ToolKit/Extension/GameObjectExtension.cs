using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace SDHK_Extension
{
    public static class GameObjectExtension
    {

        /// <summary>
        /// 假如组件变量为Null，从物体获得或添加 一个组件
        /// </summary>
        /// <param name="rootObj">目标物体</param>
        /// <param name="com">组件变量</param>
        public static T SetComponent<T>(this GameObject rootObj, ref T com)
        where T : Component
        {
            if (com == null)
            {
                if (rootObj != null)
                {
                    com = rootObj.GetComponent<T>();
                    if (com == null) com = rootObj.AddComponent<T>();
                }
            }
            return com;
        }

        /// <summary>
        /// 假如组件变量为Null，深度查询子物体，从子物体获得或添加 一个组件
        /// </summary>
        /// <param name="rootObj">目标物体</param>
        /// <param name="com">组件变量</param>
        /// <param name="childName">子物体名称</param>
        public static T SetComponent<T>(this GameObject rootObj, ref T com, string childName)
        where T : Component
        {
            if (com == null)
            {
                var gameTf = rootObj.FindChildDeep(childName);
                if (gameTf != null)
                {
                    com = gameTf.GetComponent<T>();
                    if (com == null) com = gameTf.AddComponent<T>();
                }
            }
            return com;
        }


        /// <summary>
        /// 深度查询子物体，从子物体获得一个组件，没有则添加一个
        /// </summary>
        /// <param name="rootObj">目标物体</param>
        /// <param name="type">组件类型</param>
        /// <param name="childName">子物体名称</param>
        public static Component SetComponent(this GameObject rootObj, Type type, string childName)
        {
            Component com = null;

            if (rootObj != null)
            {
                var gameTf = rootObj.FindChildDeep(childName);
                if (gameTf != null)
                {
                    com = gameTf.GetComponent(type);
                    if (com == null) com = gameTf.AddComponent(type);
                }
            }
            return com;
        }


        /// <summary>
        /// 深度查询子物体，从子物体获得一个组件，没有则添加一个
        /// </summary>
        /// <param name="rootObj">目标物体</param>
        /// <param name="childName">子物体名称</param>
        public static T SetComponent<T>(this GameObject rootObj, string childName)
        where T : Component
        {
            return rootObj.SetComponent(typeof(T), childName) as T;
        }


        /// <summary>
        /// 从物体获得或添加 一个组件
        /// </summary>
        /// <param name="rootObj">目标物体</param>
        /// <param name="type">组件类型</param>
        public static Component SetComponent(this GameObject rootObj, Type type)
        {
            Component com = null;

            if (rootObj != null)
            {
                com = rootObj.GetComponent(type);
                if (com == null) com = rootObj.AddComponent(type);
            }
            return com;
        }

        /// <summary>
        /// 从物体获得或添加 一个组件
        /// </summary>
        /// <param name="rootObj">目标物体</param>
        public static T SetComponent<T>(this GameObject rootObj)
        where T : Component
        {
            return rootObj.SetComponent(typeof(T)) as T;
        }

        /// <summary>
        /// 深度查找子物体
        /// </summary>
        /// <param name="childName">子物体名</param>
        public static GameObject FindChildDeep(this GameObject root, string childName)
        {
            Transform x = root.transform.Find(childName);//查找名字为childName的子物体
            if (x != null)
            {
                return x.gameObject;
            }

            for (int i = 0; i < root.transform.childCount; i++)
            {
                Transform childTF = root.transform.GetChild(i);
                x = childTF.FindChildDeep(childName);
                if (x != null)
                {
                    return x.gameObject;
                }
            }
            return null;
        }
    }
}