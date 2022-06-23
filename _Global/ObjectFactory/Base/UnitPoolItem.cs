﻿
/****************************************

* 作者： 闪电黑客
* 日期： 2022/5/18 15:07

* 描述： 池单位对象抽象基类

*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 池单位抽象泛型基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UnitPoolItem<T> : UnitPoolItem
        where T : UnitPoolItem<T>
    {
        public static UnitPool<T> unitPool = new UnitPool<T>();
        public static T Get()//需要通过管理器获取
        {
            return unitPool.Get();
        }

    }

    /// <summary>
    /// 池单位抽象基类
    /// </summary>
    public abstract class UnitPoolItem : Unit, IUnitPoolItem
    {
        public PoolBase thisPool { get; set; }

        public bool IsRecycle { get; set; }


        public void Recycle()
        {
            if (thisPool!=null)
            {
                if (!thisPool.IsDisposed)
                {
                    thisPool.Recycle(this);
                }
            }
        }

        public virtual void OnGet()
        {
        }

        public virtual void OnNew()
        {
        }

        public virtual void OnRecycle()
        {

        }

    }
}
