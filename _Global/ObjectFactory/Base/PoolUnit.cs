
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
