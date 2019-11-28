using System;
using System.Collections.Generic;
using UnityEngine;


/*
 * 作者：闪电Y黑客
 * 
 * 日期：2019.10.16
 * 
 * 功能：用于游戏物体的处理
 */

namespace SDHK_Tool.Static
{
    /// <summary>
    /// 用于游戏物体处理
    /// </summary>
    public static class SS_GameObject
    {

        /// <summary>
        /// 判断游戏物体是否为要忽略的层
        /// </summary>
        /// <param name="ThisGameObject">游戏物体</param>
        /// <param name="IgnoreLayer">要忽略的层</param>
        /// <returns>bool</returns>
        public static bool If_IgnoreLayer(GameObject ThisGameObject, string[] IgnoreLayer)
        {
            for (int i = 0; i < IgnoreLayer.Length; i++)//触摸事件穿透传递
            {
                if (ThisGameObject.layer == LayerMask.NameToLayer(IgnoreLayer[i])) return true;
            }
            return false;
        }


        /// <summary>
        /// 获取游戏物体：根据最近的组件查找父物体
        /// </summary>
		/// <param name="thisGameObject">当前物体</param>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns>游戏物体</returns>
        public static GameObject GetParent_In_Component<T>(GameObject thisGameObject)
        {
            Transform thisTransform = thisGameObject.transform;
            if (thisTransform.parent != null)
            {
                thisTransform = thisTransform.parent;
                for (; thisTransform.GetComponent<T>() == null && thisTransform.parent != null;)
                {
                    thisTransform = thisTransform.parent;
                }
                return thisTransform.gameObject;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取游戏物体集合：根据最近的组件查找父物体
        /// </summary>
        /// <param name="thisGameObject">当前物体</param>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns>游戏物体集合</returns>
        public static List<GameObject> GetParents_In_Component<T>(GameObject thisGameObject)
        {
            Transform thisTransform = thisGameObject.transform;

            List<GameObject> gameObjects = new List<GameObject>();

            if (thisTransform.parent != null)
            {
                thisTransform = thisTransform.parent;

                for (; thisTransform.parent != null;)
                {
                    if (thisTransform.GetComponent<T>() != null) gameObjects.Add(thisTransform.gameObject);
                    thisTransform = thisTransform.parent;
                }
                return gameObjects;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取组件:在最接近的父物体
        /// </summary>
        /// <param name="thisGameObject">当前物体</param>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns>组件</returns>
        public static T GetComponent_In_Parent<T>(GameObject thisGameObject)
        {
            Transform thisTransform = thisGameObject.transform;
            if (thisTransform.parent != null)
            {
                thisTransform = thisTransform.parent;
                for (; thisTransform.GetComponent<T>() == null && thisTransform.parent != null;)
                {
                    thisTransform = thisTransform.parent;
                }
                return thisTransform.GetComponent<T>();
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 获取组件集合:在最接近的父物体
        /// </summary>
        /// <param name="thisGameObject">当前物体</param>
        /// <typeparam name="T">组件类型</typeparam>
        /// <returns>组件集合</returns>
        public static T[] GetComponents_In_Parent<T>(GameObject thisGameObject)
        {
            Transform thisTransform = thisGameObject.transform;
            if (thisTransform.parent != null)
            {
                thisTransform = thisTransform.parent;
                for (; thisTransform.GetComponents<T>().Length <= 0 && thisTransform.parent != null;)
                {
                    thisTransform = thisTransform.parent;
                }
                return thisTransform.GetComponents<T>();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 物体集合剔除
        /// </summary>
        /// <param name="OriginList">原集合</param>
        /// <param name="ExceptList">要剔除的集合</param>
        /// <param name="IF_Func">判断方法:相等返回true</param>
        /// <typeparam name="T">集合类型</typeparam>
        /// <returns>剔除的集合</returns>
        public static List<T> List_Except<T>(List<T> OriginList, List<T> ExceptList, Func<T, T, bool> IF_Func)
        {
            List<T> newList = new List<T>();
            bool bit = false;
            for (int i = 0; i < OriginList.Count; i++)
            {
                for (int i1 = 0; i1 < ExceptList.Count; i1++)
                {
                    if (IF_Func(OriginList[i], ExceptList[i1]))
                    {
                        bit = true;
                        break;
                    }
                }
                if (!bit) newList.Add(OriginList[i]);
                bit = false;
            }
            return newList;
        }
        /// <summary>
        /// 物体集合剔除
        /// </summary>
        /// <param name="OriginList">原集合</param>
        /// <param name="ExceptList">要剔除的集合</param>
        /// <param name="IF_Func">判断方法:相等返回true</param>
        /// <typeparam name="T">集合类型</typeparam>
        /// <returns>剔除的集合</returns>
        public static List<T> List_Except<T>(T[] OriginList, T[] ExceptList, Func<T, T, bool> IF_Func)
        {
            List<T> newList = new List<T>();
            bool bit = false;
            for (int i = 0; i < OriginList.Length; i++)
            {
                for (int i1 = 0; i1 < ExceptList.Length; i1++)
                {
                    if (IF_Func(OriginList[i], ExceptList[i1]))
                    {
                        bit = true;
                        break;
                    }
                }
                if (!bit) newList.Add(OriginList[i]);
                bit = false;
            }
            return newList;
        }

        /// <summary>
        /// 物体集合交集
        /// </summary>
        /// <param name="originList">原集合</param>
        /// <param name="IntersectList">新集合</param>
        /// <param name="IF_Func">判断方法:相等返回true</param>
        /// <typeparam name="T">集合类型</typeparam>
        /// <returns>交集集合</returns>
        public static List<T> List_Intersect<T>(List<T> originList, List<T> IntersectList, Func<T, T, bool> IF_Func)
        {
            List<T> newList = new List<T>();
            for (int i = 0; i < originList.Count; i++)
            {
                for (int i1 = 0; i1 < IntersectList.Count; i1++)
                {
                    if (IF_Func(originList[i], IntersectList[i1]))
                    {
                        newList.Add(originList[i]);
                    }
                }
            }
            return newList;
        }

        /// <summary>
        /// 物体集合交集
        /// </summary>
        /// <param name="originList">原集合</param>
        /// <param name="IntersectList">新集合</param>
        /// <param name="IF_Func">判断方法:相等返回true</param>
        /// <typeparam name="T">集合类型</typeparam>
        /// <returns>交集集合</returns>
        public static List<T> List_Intersect<T>(T[] originList, T[] IntersectList, Func<T, T, bool> IF_Func)
        {
            List<T> newList = new List<T>();
            for (int i = 0; i < originList.Length; i++)
            {
                for (int i1 = 0; i1 < IntersectList.Length; i1++)
                {
                    if (IF_Func(originList[i], IntersectList[i1]))
                    {
                        newList.Add(originList[i]);
                    }
                }
            }
            return newList;
        }


       


    }
}