using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 活跃禁用事件系统接口
    /// </summary>
    public interface IDisableSystem : ISystem
    {
        void Disable(Entity self);
    }

    /// <summary>
    /// 活跃禁用事件系统
    /// </summary>
    public abstract class DisableSystem<T> : SystemBase<T, IDisableSystem>, IDisableSystem
        where T : Entity
    {
        public void Disable(Entity self) => OnDisable(self as T);
        public abstract void OnDisable(T self);
    }
}
