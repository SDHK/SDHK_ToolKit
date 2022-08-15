
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

    /// <summary>
    /// 异步任务Update 生命周期管理器实体 
    /// </summary>
    public class AsyncTaskManager : Entity
    {
        public Dictionary<ulong, Entity> update1 = new Dictionary<ulong, Entity>();
        public Dictionary<ulong, Entity> update2 = new Dictionary<ulong, Entity>();
        public SystemGroup systems;
    }

    class AsyncTaskManagerUpdateSystem : UpdateSystem<AsyncTaskManager>
    {
        public override void Update(AsyncTaskManager self)
        {
            while (self.update1.Count != 0 && self.RealActive)
            {
                ulong firstKey = self.update1.Keys.First();
                Entity entity = self.update1[firstKey];
                if (entity.RealActive)
                {
                    if (self.systems.TryGetValue(entity.Type, out UnitList<ISystem> systemList))
                    {
                        foreach (ITaskUpdateSystem system in systemList)
                        {
                            system.Execute(entity);
                        }
                    }
                }
                self.update1.Remove(firstKey);
                self.update2.TryAdd(firstKey, entity);
            }
            (self.update1, self.update2) = (self.update2, self.update1);
        }
    }

    class AsyncTaskManagerNewSystem : NewSystem<AsyncTaskManager>
    {
        public override void OnNew(AsyncTaskManager self)
        {
            self.systems = self.RootGetSystemGroup<ITaskUpdateSystem>();
        }
    }


    class AsyncTaskManagerEntityListenerSystem : EntitySystem<AsyncTaskManager>
    {
        public override void OnAddEntity(AsyncTaskManager self, Entity entity)
        {
            if (self.systems.ContainsKey(entity.Type))
            {
                self.update2.Add(entity.id, entity);
            }
        }

        public override void OnRemoveEntity(AsyncTaskManager self, Entity entity)
        {
            if (self.systems.ContainsKey(entity.Type))
            {
                self.update1.Remove(entity.id);
                self.update2.Remove(entity.id);
            }
        }
    }

}
