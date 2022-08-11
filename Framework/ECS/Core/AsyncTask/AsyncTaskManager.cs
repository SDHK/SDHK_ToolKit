
/****************************************

* 作者： 闪电黑客
* 日期： 2022/8/10 11:24

* 描述： 异步任务管理器

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    public class AsyncTaskManager:Entity
    {
        Dictionary<ulong ,AsyncTask> tasks = new Dictionary<ulong ,AsyncTask>();    

    }

    class AsyncTaskManagerUpdateSystem : UpdateSystem<AsyncTaskManager>
    {
        public override void Update(AsyncTaskManager self)
        {
            //await self.Task.Delyer();
        }
    }
}
