using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// Update系统接口
    /// </summary>
    public interface IUpdateSystem : ISystem
    {
        void Execute(IEntity self);
    }

    /// <summary>
    /// Update系统基类
    /// </summary>
    public abstract class UpdateSystem<T> : SystemBase<T>, IUpdateSystem
        where T : class, IEntity
    {
        public void Execute(IEntity self) => Update(self as T);
        public abstract void Update(T self);
    }
}
