
/****************************************

* 作者： 闪电黑客
* 日期： 2022/5/17 11:25

* 描述：单位类
* 用于自定义类的最基层
* 统一自定义类的释放功能和释放标记

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
        public bool isDisposed { get; set; }


        /// <summary>
        /// 直接释放：释放后IsDisposed标记为true
        /// </summary>
        public void Dispose()
        {
            if (isDisposed) return;
            OnDispose();
            isDisposed = true;
        }

        public virtual void OnDispose() { }
        
    }

}
