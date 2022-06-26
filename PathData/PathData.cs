
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
using SDHK;

public class DataBox<T>
{
    private T value = default(T);
    
    public List<Action<T>> callBacks = new List<Action<T>>();

    public T Value
    {
        get => value;
        set
        {
            this.value = value;
            callBacks.ForEach(x => x(value));
        }
    }

    public void Add(Action<T> callBack)
    {
        callBacks.Add(callBack);
    }
}
public class IntData: DataBox<int>
{

}

public class FloatData : DataBox<float>
{

}

//?暂时无用，用于参考
namespace SDHK
{
    /// <summary>
    /// 全局路径数据：一个可以全局调用的 PathData
    /// </summary>
    public static class PathDataGlobal
    {
        public static char[] pathSplits = { '/', '\\' };
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PathData
    {
        public Dictionary<string, PathData> nodes = new Dictionary<string, PathData>();
        public object data;//使用dataBox基类

        public static implicit operator int(PathData d)
        {
            return int.Parse(d.data.ToString());
        }

        public static implicit operator PathData(int d)
        {
            return new PathData();
        }


        public PathData this[string key]
        {
            get
            {
                if (nodes.ContainsKey(key))
                {
                    return nodes[key];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (nodes.ContainsKey(key))
                {
                    nodes[key] = value;
                }
                else
                {
                    nodes.Add(key, value);
                }
            }
        }

    }


}