using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.03.30 
 * 
 * 功能：动态类单例模式基类
 *
 */

/// <summary>
/// 动态类单例模式:抽象基类
/// </summary>
/// <typeparam name="T">单例类</typeparam>
public class SD_Singleton<T> where T : class
{
    // 由于单例基类不能实例化，故设计为抽象类

    // 这里采用实现5的方案，实际可采用上述任意一种方案
    class Nested
    {
        // 创建模板类实例，参数2设为true表示支持私有构造函数
        internal static readonly T instance = Activator.CreateInstance(typeof(T), true) as T;
    }
    // private static T instance = null;

    //实例类
    public static T Instance { get { return Nested.instance; } }
}




