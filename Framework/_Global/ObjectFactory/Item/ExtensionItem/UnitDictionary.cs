
/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/12 16:45

* 描述： 单位字典，这个字典可由对象池管理生成和回收
* 
* 其余和普通的字典一样使用

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{

    /// <summary>
    /// 单位字典：可由对象池管理回收
    /// </summary>
    public class UnitDictionary<TKey,TValue>:Dictionary<TKey, TValue>,IUnitPoolItem
    {
        public IPool thisPool { get; set; }
        public bool IsRecycle { get; set; }
        public bool IsDisposed { get; set; }

        /// <summary>
        /// 单位对象池：获取对象
        /// </summary>
        public static  UnitDictionary<TKey, TValue> GetObject()
        {
            return UnitPoolManager.Instance.Get<UnitDictionary<TKey, TValue>>();
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
