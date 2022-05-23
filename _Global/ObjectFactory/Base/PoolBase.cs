/****************************************

 * 作者：闪电黑客
 * 日期: 2021/12/13 13:35:32 （第5次重写）
 
 * 描述:  
 * 对原先对象池工具的重写，
 * 增加计时销毁设定，并抽象大部分方法

*/
/****************************************

 * 作者： 闪电黑客
 * 日期： 2022/5/17 10:34 （第6次修改）

 * 描述:  
 * 改为继承Unit统一了销毁功能
 * 重命名类由 ObjectPoolBase 改为 PoolBase
 * 分离计时功能

*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDHK
{
    /// <summary>
    /// 计时对象池抽象基类
    /// </summary>
    public abstract class PoolBase:Unit
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
        public int minLimit = 0;

        /// <summary>
        /// 对象回收数量限制
        /// </summary>
        public int maxLimit = -1;

        /// <summary>
        /// 获取对象
        /// </summary>
        public abstract object GetObject();

        /// <summary>
        /// 回收对象
        /// </summary>
        public abstract void Recycle(object obj);
        
        /// <summary>
        /// 释放全部对象
        /// </summary>
        public abstract void DisposeAll();

        /// <summary>
        /// 释放一个对象
        /// </summary>
        public abstract void DisposeOne();

        /// <summary>
        /// 预加载
        /// </summary>
        public abstract void Preload();



    }

}