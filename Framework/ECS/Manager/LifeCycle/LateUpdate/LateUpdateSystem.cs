using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{

    /// <summary>
    /// LateUpdate系统接口
    /// </summary>
    public interface ILateUpdateSystem : ISystem
    {
        void Execute(IEntity self);
    }
    /// <summary>
    /// LateUpdate系统基类
    /// </summary>
    public abstract class LateUpdateSystem<T> : SystemBase<T>, ILateUpdateSystem
        where T : class, IEntity
    {
        public void Execute(IEntity self) => LateUpdate(self as T);
        public abstract void LateUpdate(T self);
    }
}
