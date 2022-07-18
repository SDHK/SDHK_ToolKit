
/******************************

 * Author: 闪电黑客

 * 日期: 2022/01/15 15:20:55

 * 最后日期: 2022/01/15 15:21:18

 * 最后修改: 闪电黑客

 * 描述:  

    路径资源类 由 Dictionary<string, object> 扩展的功能
    
    子节点可以无限扩张。

    让字典可以像文件夹一样储存获取对象
    
    主要功能：可以用多个键值组成的路径进行操作

******************************/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PathAssets
{
    public static class PathAsset
    {

        public static char[] pathSplits = { '/', '\\' };

        #region Get
        private static object Get(this Dictionary<string, object> dic, int index, params string[] keys)
        {
            string key = keys[index];

            if (dic.ContainsKey(key))
            {
                if (index == keys.Length - 1)
                {
                    return dic[keys[index]];

                }
                else
                {
                    var dataNode = dic[key] as Dictionary<string, object>;
                    if (dataNode != null)
                    {
                        return dataNode.Get(index + 1, keys);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }

        }
        public static object Get(this Dictionary<string, object> dic, string path)
        {
            return Get(dic, 0, path.Split(pathSplits));
        }

        public static object Get(this Dictionary<string, object> dic, params string[] keys)
        {
            return Get(dic, 0, keys);
        }
        public static T Get<T>(this Dictionary<string, object> dic, string path)
        {
            return (T)Get(dic, 0, path.Split(pathSplits));
        }

        public static T Get<T>(this Dictionary<string, object> dic, params string[] keys)
        {
            return (T)Get(dic, 0, keys);
        }


        #endregion

        #region Set


        private static void Set(this Dictionary<string, object> dic, int index, object value, params string[] keys)
        {
            string key = keys[index];

            if (index == keys.Length - 1)
            {
                if (dic.ContainsKey(key))
                {
                    dic[key] = value;
                }
                else
                {
                    dic.Add(key, value);
                }
            }
            else
            {
                if (dic.ContainsKey(key))
                {
                    var dataNode = dic[key] as Dictionary<string, object>;
                    if (dataNode != null)
                    {
                        dataNode.Set(index + 1, value, keys);
                    }
                }
                else
                {
                    Dictionary<string, object> dataNode = new Dictionary<string, object>();
                    dic.Add(key, dataNode);
                    dataNode.Set(index + 1, value, keys);
                }
            }
        }

        public static void Set(this Dictionary<string, object> dic, object value, string path)
        {
            dic.Set(0, value, path.Split(pathSplits));
        }

        public static void Set(this Dictionary<string, object> dic, object value, params string[] keys)
        {
            dic.Set(0, value, keys);
        }




        #endregion


        #region  Node


        public static Dictionary<string, object> Node(this Dictionary<string, object> dic, int index, params string[] keys)
        {
            Dictionary<string, object> pathData;
            string key = keys[index];
            if (dic.ContainsKey(key))
            {
                pathData = dic[key] as Dictionary<string, object>;
            }
            else
            {
                pathData = new Dictionary<string, object>();
                dic.Add(key, pathData);
            }

            if (index == keys.Length - 1)
            {
                return pathData;
            }
            else
            {
                return pathData.Node(index + 1, keys);
            }
        }
        public static Dictionary<string, object> Node(this Dictionary<string, object> dic, string path)
        {
            return dic.Node(0, path.Split(pathSplits));
        }
        public static Dictionary<string, object> Node(this Dictionary<string, object> dic, params string[] keys)
        {
            return dic.Node(0, keys);
        }



        #endregion


        #region  RemoveNode


        private static void RemoveNode(this Dictionary<string, object> dic, int index, params string[] keys)
        {
            string key = keys[index];
            if (index == keys.Length - 1)
            {
                dic.Remove(key);
            }
            else
            {
                if (dic.ContainsKey(key))
                {
                    dic.Node(key).RemoveNode(index + 1, keys);
                }
            }
        }
        public static void RemoveNode(this Dictionary<string, object> dic, string path)
        {
            dic.RemoveNode(0, path.Split(pathSplits));
        }

        public static void RemoveNode(this Dictionary<string, object> dic, params string[] keys)
        {
            dic.RemoveNode(0, keys);
        }



        #endregion


        #region  ContainsPath


        private static bool ContainsPath(this Dictionary<string, object> dic, int index, params string[] keys)
        {
            string key = keys[index];

            if (dic.ContainsKey(key))
            {
                if (index == keys.Length - 1)
                {
                    return true;
                }
                else
                {

                    return dic.Node(key).ContainsPath(index + 1, keys);
                }
            }
            else
            {
                return false;
            }
        }
        public static bool ContainsPath(this Dictionary<string, object> dic, string path)
        {
            return dic.ContainsPath(0, path.Split(pathSplits));
        }

        public static bool ContainsPath(this Dictionary<string, object> dic, params string[] keys)
        {
            return dic.ContainsPath(0, keys);
        }

        #endregion



        #region  NodeGet
        public static T NodeGet<T>(this Dictionary<string, object> dic, string path)
        {
            return (T)dic.Get<Dictionary<string, object>>(path)?.Get(typeof(T).ToString());
        }
        public static T NodeGet<T>(this Dictionary<string, object> dic, params string[] keys)
        {

            return (T)dic.Get<Dictionary<string, object>>(keys)?.Get(typeof(T).ToString());
        }

        #endregion


        #region  NodeSet
        public static void NodeSet<T>(this Dictionary<string, object> dic, T value, string path)
        {
            dic.Node(path).Set(value, typeof(T).ToString());
        }

        public static void NodeSet<T>(this Dictionary<string, object> dic, T value, params string[] keys)
        {
            dic.Node(keys).Set(value, typeof(T).ToString());
        }

        #endregion


        public static List<string> GetKeys(this Dictionary<string, object> dic)
        {
            return dic.Keys.ToList();
        }

    }
}