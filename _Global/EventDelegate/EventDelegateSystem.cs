/****************************************

* 作者： 闪电黑客
* 日期： 2022/6/25 20:27

* 描述： 
* 绑定类型的是直接呼叫
* 绑定对象的是队列呼叫，可以撤销
* 暂时无用

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
