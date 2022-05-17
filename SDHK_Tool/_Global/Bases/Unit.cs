
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
        private bool isDisposed = false;
        /// <summary>
        /// 是否已经释放
        /// </summary>
        public bool IsDisposed => isDisposed;

        /// <summary>
        /// 释放时
        /// </summary>
        protected abstract void OnDispose();

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            if (isDisposed) return;
            OnDispose();
            isDisposed = true;
        }
    }

}
