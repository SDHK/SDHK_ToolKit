
/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/12 16:23

* 描述： 

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SDHK
{
    /// <summary>
    /// 单位链表：对象池管理可回收
    /// </summary>
    public class UnitList<T> : List<T>, IUnitPoolItem
    {
        public PoolBase thisPool { get; set; }
        public bool IsRecycle { get; set; }
        public bool IsDisposed { get; set; }

        /// <summary>
        /// 单位对象池：获取对象
        /// </summary>
        public static UnitList<T> GetObject()
        {
            return UnitPoolManager.Instance.Get<UnitList<T>>();
        }

        public void Dispose()
        {
        }

        public void OnDispose()
        {
        }

        public void OnGet()
        {
        }

        public void OnNew()
        {
        }

        public void OnRecycle()
        {
            if (typeof(T) == typeof(IUnitPoolItem))
            {
                foreach (var item in this)
                {
                    (item as IUnitPoolItem)?.Recycle();
                }
            }
            Clear();
        }


        public void Recycle()
        {
            if (thisPool != null)
            {
                if (!thisPool.IsDisposed)
                {
                    if (!IsRecycle)
                    {
                        thisPool.Recycle(this);
                    }
                }
            }
        }


    }
}
