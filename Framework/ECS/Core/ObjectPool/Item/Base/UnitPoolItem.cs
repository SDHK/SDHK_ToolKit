
/****************************************

* 作者： 闪电黑客
* 日期： 2022/5/18 15:07

* 描述： 池单位对象，抽象基类
* 
* 抽象基类将提供一个回收实例的方法，
* 并对其是否可以回收进行判断，
* 省去用对象池管理器回收的麻烦。
* 
* 
* 抽象泛型基类将提供一个静态获取实例的方法，
* 省去用对象池管理器获取的麻烦。
* 
* 

*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    ///// <summary>
    ///// 池单位抽象泛型基类：提供获取和回收对象的方法
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public abstract class UnitPoolItem<T> : UnitPoolItem
    //    where T : UnitPoolItem<T>
    //{
    //    /// <summary>
    //    /// 单位对象池：获取对象
    //    /// </summary>
    //    public static T GetObject()
    //    {
    //        return UnitPoolManager.Instance.Get<T>();
    //    }

    //}

    /// <summary>
    /// 池单位抽象基类：提供回收方法
    /// </summary>
    public abstract class UnitPoolItem : Unit, IUnitPoolItem
    {
        public IPool thisPool { get; set; }

        public bool IsRecycle { get; set; }


        public void Recycle()
        {
            if (thisPool!=null)
            {
                if (!thisPool.isDisposed)
                {
                    if (!IsRecycle)
                    {
                        thisPool.Recycle(this);
                    }
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
