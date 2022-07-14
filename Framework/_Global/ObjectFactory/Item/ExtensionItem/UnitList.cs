
/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/12 16:23

* 描述： 单位列表，这个列表可由对象池管理生成和回收
* 
* 其余和普通的List一样使用

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDHK
{
    /// <summary>
    /// 单位列表：可由对象池管理回收
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

        /// <summary>
        /// 直接释放：释放后IsDisposed标记为true
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;
            OnDispose();
            IsDisposed = true;
        }

        public virtual void OnDispose()
        {
        }

        public virtual void OnGet()
        {
        }

        public virtual void OnNew()
        {
        }

        public virtual void OnRecycle()
        {
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
