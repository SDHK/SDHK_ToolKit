
/****************************************

* 作者： 闪电黑客
* 日期： 2022/5/17 11:25

* 描述： 统一的释放功能和标记

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 单位基类
    /// </summary>
    public abstract class Unit : IUnit
    {
        public bool IsDisposed { get; set; }


        /// <summary>
        /// 直接释放：释放后IsDisposed标记为true
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;
            OnDispose();
            IsDisposed = true;
        }

        public virtual void OnDispose() { }
        
    }

}
