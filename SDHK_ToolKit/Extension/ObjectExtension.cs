
/******************************

 * Author: 闪电黑客

 * 日期: 2022/01/15 15:18:25

 * 最后日期: 2022/01/15 15:19:05

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDHK_Extension
{
    public static class ObjectExtension
    {

        public static T To<T>(this object data, T def) => data is T ? (T)data : def;
        public static T As<T>(this object data) where T : new() => data is T ? (T)data : new T();

        public static bool IsNull(this Object target)
        {
            return target == null;
        }
    }
}