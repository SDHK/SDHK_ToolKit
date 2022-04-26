
/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/05 06:18:13

 * 最后日期: 2021/12/21 07:42:37

 * 最后修改: 闪电黑客

 * 描述:  //!暂时无用，用于参考

    可以用路径格式进行存取的数据结构。

    子节点可以无限扩张。

    可以当成是 Dictionary<string, Object>()

    设计目的 
    
    使得 数据存取 可以像 文件读取 一样操作。

******************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Singleton;

//?暂时无用，用于参考
namespace PathAssetsTest
{
    /// <summary>
    /// 全局路径数据：一个可以全局调用的 PathData
    /// </summary>
    public static class PathDataGlobal
    {
        public static char[] pathSplits = { '/', '\\' };
    }

    /// <summary>
    /// 路径数据 , 继承 Dictionary ( object, object ) 
    /// </summary>
    [Serializable]
    public class PathData : Dictionary<object, object>
    {

        // 转换：为 Dictionary<object, object>
        public Dictionary<object, object> ToDictionary()
        {
            return this.ToDictionary(tb => tb.Key, tb =>
            {
                return (tb.Value is PathData) ? (tb.Value as PathData).ToDictionary() : tb.Value;
            });
        }
        public PathData SetData(Dictionary<object, object> dic)
        {
            foreach (var item in dic)
            {
                if (item.Value is Dictionary<object, object>)
                {
                    this.Add(item.Key, new PathData().SetData(item.Value as Dictionary<object, object>));
                }
                else
                {
                    this.Add(item.Key, item.Value);
                }
            }
            return this;
        }


        public KeyValuePair<object, object> this[int index]
        {
            get
            {
                if (0 <= index && index < Count)
                {
                    return this.ElementAt(index);
                }
                else
                {
                    return default;
                }
            }

        }


        #region  this_Item

        public object this[string path]
        {
            get
            {
                return this[path.Split(PathDataGlobal.pathSplits).ToList()];
            }
            set
            {
                this[path.Split(PathDataGlobal.pathSplits).ToList()] = value;
            }
        }
        public object this[params string[] keys]
        {
            get
            {
                return this[keys.ToList()];
            }
            set
            {
                this[keys.ToList()] = value;
            }
        }
        public object this[List<string> keys]
        {
            get //根据路径节点获取object，不存在返回null
            {
                if (keys.Count == 1)
                {
                    return this[keys[0] as object];
                }
                else
                {
                    if (this.ContainsKey(keys[0]))
                    {
                        var dataNode = this[keys[0] as object] as PathData;
                        if (dataNode != null)
                        {
                            keys.RemoveAt(0);
                            return dataNode[keys];
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {

                        return null;
                    }
                }
            }

            set //将一个object储存到路径节点，自动创建不存在的节点。
            {
                if (keys.Count == 1)
                {
                    if (this.ContainsKey(keys[0]))
                    {
                        this[keys[0]] = value;
                    }
                    else
                    {
                        this.Add(keys[0], value);
                    }
                }
                else
                {
                    if (this.ContainsKey(keys[0]))
                    {
                        var dataNode = this[keys[0]] as PathData;
                        keys.RemoveAt(0);
                        if (dataNode != null)
                        {
                            dataNode[keys] = value;
                        }
                    }
                    else
                    {
                        PathData dataNode = new PathData();
                        this.Add(keys[0], dataNode);
                        keys.RemoveAt(0);
                        dataNode[keys] = value;
                    }
                }
            }
        }

        public object this[Type typeKey]
        {
            get
            {
                return this[typeKey.ToString() as object];
            }
            set
            {
                string key = typeKey.ToString();

                if (this.ContainsKey(key))
                {
                    this[key as object] = value;
                }
                else
                {
                    this.Add(key, value);
                }
            }
        }
        public object this[Type typeKey, string path]
        {
            get
            {
                var keys = path.Split(PathDataGlobal.pathSplits).ToList();
                keys.Add(typeKey.ToString());
                return this[keys];
            }
            set
            {
                var keys = path.Split(PathDataGlobal.pathSplits).ToList();
                keys.Add(typeKey.ToString());
                this[keys] = value;
            }
        }
        public object this[Type typeKey, params string[] keys]
        {
            get
            {
                var keyList = keys.ToList();
                keyList.Add(typeKey.ToString());
                return this[keyList];
            }
            set
            {
                var keyList = keys.ToList();
                keyList.Add(typeKey.ToString());
                this[keyList] = value;
            }
        }

        #endregion

        #region  Node

        public PathData Node(string path)
        {
            return this.Node(path.Split(PathDataGlobal.pathSplits).ToList());
        }
        public PathData Node(params string[] keys)
        {
            return this.Node(keys.ToList());
        }
        public PathData Node(List<string> keys)
        {
            PathData pathData;
            if (this.ContainsKey(keys[0]))
            {
                pathData = this[keys[0]] as PathData;
            }
            else
            {
                pathData = new PathData();
                this.Add(keys[0], pathData);
            }

            if (keys.Count == 1)
            {
                return pathData;
            }
            else
            {
                keys.RemoveAt(0);
                return pathData.Node(keys);
            }
        }

        public PathData Node(Type typeKey)
        {
            string key = typeKey.ToString();

            if (this.ContainsKey(key))
            {
                return this[key as object] as PathData;
            }
            else
            {
                PathData node = new PathData();
                this.Add(key, node);
                return node;
            }
        }
        public PathData Node(Type typeKey, string path)
        {
            var keys = path.Split(PathDataGlobal.pathSplits).ToList();
            keys.Add(typeKey.ToString());
            return this.Node(keys);
        }
        public PathData Node(Type typeKey, params string[] keys)
        {
            var keyList = keys.ToList();
            keyList.Add(typeKey.ToString());
            return this.Node(keyList);
        }

        public PathData Node<T>()
        {
            return Node(typeof(T));
        }
        public PathData Node<T>(string path)
        {
            return Node(typeof(T), path);
        }
        public PathData Node<T>(params string[] keys)
        {
            return Node(typeof(T), keys);
        }

        #endregion

        #region  Get

        public object Get(string path)
        {
            return this[path];
        }
        public object Get(params string[] keys)
        {
            return this[keys];
        }

        public T Get<T>(string path)
        {
            return (T)this[path.Split(PathDataGlobal.pathSplits).ToList()];
        }
        public T Get<T>(params string[] keys)
        {
            return (T)this[keys.ToList()];
        }

        public void Set(object value, string path)
        {
            this[path] = value;
        }
        public void Set(object value, params string[] keys)
        {
            this[keys] = value;
        }

        #endregion

        #region  NodeGet

        public object NodeGet(Type typeKey)
        {
            return this[typeKey];
        }
        public object NodeGet(Type typeKey, string path)
        {
            return this[typeKey, path];
        }
        public object NodeGet(Type typeKey, params string[] keys)
        {
            return this[typeKey, keys];
        }

        public T NodeGet<T>()
        {
            return (T)this[typeof(T)];
        }
        public T NodeGet<T>(string path)
        {
            return (T)this[typeof(T), path];
        }
        public T NodeGet<T>(params string[] keys)
        {
            return (T)this[typeof(T), keys];
        }

        #endregion

        #region  NodeSet

        public void NodeSet(object value, Type typeKey)
        {
            this[typeKey] = value;
        }
        public void NodeSet(object value, Type typeKey, string path)
        {
            this[typeKey, path] = value;
        }
        public void NodeSet(object value, Type typeKey, params string[] keys)
        {
            this[typeKey, keys] = value;
        }

        public void NodeSet<T>(object value)
        {
            this[typeof(T)] = value;
        }
        public void NodeSet<T>(object value, string path)
        {
            this[typeof(T), path] = value;
        }
        public void NodeSet<T>(object value, params string[] keys)
        {
            this[typeof(T), keys] = value;
        }

        #endregion

        #region  RemoveNode

        public void RemoveNode(string path)
        {
            RemoveNode(path.Split(PathDataGlobal.pathSplits).ToList());
        }
        public void RemoveNode(params string[] keys)
        {
            RemoveNode(keys.ToList());
        }
        public void RemoveNode(List<string> keys)
        {
            if (keys.Count == 1)
            {
                this.Remove(keys[0]);
            }
            else
            {
                if (this.ContainsKey(keys[0]))
                {
                    string key = keys[0];
                    keys.RemoveAt(0);
                    this.Node(key).RemoveNode(keys);
                }
            }
        }

        public void RemoveNode(Type typekey)
        {
            this.Remove(typekey.ToString());
        }
        public void RemoveNode(Type typekey, string path)
        {
            var keys = path.Split(PathDataGlobal.pathSplits).ToList();
            keys.Add(typekey.ToString());
            this.RemoveNode(keys);
        }
        public void RemoveNode(Type typekey, params string[] keys)
        {
            var keyList = keys.ToList();
            keyList.Add(typekey.ToString());
            this.RemoveNode(keyList);
        }

        public void RemoveNode<T>()
        {
            RemoveNode(typeof(T));
        }
        public void RemoveNode<T>(string path)
        {
            RemoveNode(typeof(T), path);
        }
        public void RemoveNode<T>(params string[] keys)
        {
            RemoveNode(typeof(T), keys);
        }

        #endregion

        #region  ContainsPath

        public bool ContainsPath(string path)
        {
            return ContainsPath(path.Split(PathDataGlobal.pathSplits).ToList());
        }
        public bool ContainsPath(params string[] keys)
        {
            return ContainsPath(keys.ToList());
        }
        public bool ContainsPath(List<string> keys)
        {
            if (this.ContainsKey(keys[0]))
            {
                if (keys.Count == 1)
                {
                    return true;
                }
                else
                {
                    string key = keys[0];
                    keys.RemoveAt(0);
                    return this.Node(key).ContainsPath(keys);
                }
            }
            else
            {
                return false;
            }
        }


        public bool ContainsPath(Type typekey)
        {
            return this.ContainsKey(typekey.ToString());
        }
        public bool ContainsPath(Type typekey, string path)
        {
            var keys = path.Split(PathDataGlobal.pathSplits).ToList();
            keys.Add(typekey.ToString());
            return this.ContainsPath(keys);
        }
        public bool ContainsPath(Type typekey, params string[] keys)
        {
            var keyList = keys.ToList();
            keyList.Add(typekey.ToString());
            return this.ContainsPath(keyList);
        }


        public bool ContainsPath<T>()
        {
            return this.ContainsPath(typeof(T));
        }
        public bool ContainsPath<T>(string path)
        {
            return ContainsPath(typeof(T), path);
        }
        public bool ContainsPath<T>(params string[] keys)
        {
            return ContainsPath(typeof(T), keys);
        }

        #endregion
    }


}