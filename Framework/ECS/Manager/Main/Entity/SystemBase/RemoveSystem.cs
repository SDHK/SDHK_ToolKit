using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 移除事件系统接口
    /// </summary>
    public interface IRemoveSystem : ISystem
    { 
        void Remove(IEntity self);
    }
    /// <summary>
    /// 移除事件系统
    /// </summary>
    public abstract class RemoveSystem<T> : SystemBase<T>, IRemoveSystem
        where T :class, IEntity
    {
        public void Remove(IEntity self) => OnRemove(self as T);
        public abstract void OnRemove(T self);
    }
}
