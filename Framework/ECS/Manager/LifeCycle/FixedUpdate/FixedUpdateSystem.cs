using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// FixedUpdate系统接口
    /// </summary>
    public interface IFixedUpdateSystem : ISystem
    {
        void Execute(IEntity self);
    }

    /// <summary>
    /// FixedUpdate系统基类
    /// </summary>
    public abstract class FixedUpdateSystem<T> : SystemBase<T>, IFixedUpdateSystem
       where T : class, IEntity
    {
        public void Execute(IEntity self) => FixedUpdate(self as T);
        public abstract void FixedUpdate(T self);
    }
}
