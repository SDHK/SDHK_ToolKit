
/****************************************

* 作者： 闪电黑客
* 日期： 2022/5/17 11:19

* 描述： 统一销毁标记

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 单位接口
    /// </summary>
    public interface IUnit: IDisposable
    {
        bool IsDisposed { get; }
    }
}
