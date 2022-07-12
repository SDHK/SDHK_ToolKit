
/****************************************

* 作者： 闪电黑客
* 日期： 2022/6/27 9:47

* 描述： 

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    public class EventDelegateSystem
    {

        //Iupdate,<FuncType,eventlist>
        //反射全局获取类型和函数
        private Dictionary<Type, EventDelegate> events = new Dictionary<Type, EventDelegate>();

        // await System.Call(this,1);
    }
}
