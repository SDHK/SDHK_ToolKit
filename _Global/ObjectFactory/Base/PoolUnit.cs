﻿
/****************************************

* 作者： 闪电黑客
* 日期： 2022/5/18 15:07

* 描述： 

*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{

    public abstract class PoolUnit<T> : PoolUnit
        where T : PoolUnit<T>
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
    public abstract class PoolUnit : Unit, IPoolUnit
    {
        public PoolBase thisPool { get; set; }

        public bool IsRecycle { get; set; }


        public void Recycle()
        {
            thisPool.Recycle(this);
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
