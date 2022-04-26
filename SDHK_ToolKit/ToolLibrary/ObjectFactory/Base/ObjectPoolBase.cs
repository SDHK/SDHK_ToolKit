﻿
/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/13 13:35:32

 * 最后日期: 2021/12/15 16:26:45

 * 最后修改: 闪电黑客

 * 文件相对于项目的路径: \RainbowButterfly\Assets\SDHK_ToolKit\ToolLibrary\ObjectPool1\Base\ObjectPoolBase.cs

 * 描述:  对象池抽象基类
    (5)

******************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectFactory
{
    /// <summary>
    /// 对象池抽象基类
    /// </summary>
    public abstract class ObjectPoolBase
    {
        /// <summary>
        /// 对象类型
        /// </summary>
        public Type ObjectType { get; set; }

        /// <summary>
        /// 当前保留对象数量
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// 预加载数量
        /// </summary>
        public int objectPreload = 0;

        /// <summary>
        /// 对象回收数量限制
        /// </summary>
        public int objectLimit = -1;

        /// <summary>
        /// 对象自动销毁计时设定
        /// </summary>
        public float objectDestoryClock = -1;

        /// <summary>
        /// 对象销毁的计时间隔
        /// </summary>
        public float objectDestoryIntervalClock = 0;


        /// <summary>
        /// 销毁倒计时
        /// </summary>
        protected float destoryCountDown = -1;
        /// <summary>
        /// 销毁间隔倒计时
        /// </summary>
        protected float destoryIntervalCountDown = -1;

        /// <summary>
        /// 获取对象
        /// </summary>
        public abstract object GetObject();

        /// <summary>
        /// 回收对象
        /// </summary>
        public abstract void Recycle(object obj);
        /// <summary>
        /// 清空对象池
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// 预加载
        /// </summary>
        public abstract void Preload();

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="deltaTime">刷新时间差</param>
        public abstract void Update(float deltaTime);

        /// <summary>
        /// 销毁对象池
        /// </summary>
        public abstract void Destroy();

    }

}