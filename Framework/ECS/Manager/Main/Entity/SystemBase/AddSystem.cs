using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 添加事件系统接口
    /// </summary>
    public interface IAddSystem : ISystem
    {
        void Add(IEntity self);
    }

    /// <summary>
    /// 添加事件系统
    /// </summary>
    public abstract class AddSystem<T> : SystemBase<T>, IAddSystem
        where T :class, IEntity
    {
        public void Add(IEntity self) => OnAdd(self as T);
        public abstract void OnAdd(T self);

    }
}
