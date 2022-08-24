using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 活跃启用事件系统接口
    /// </summary>
    public interface IEnableSystem : ISystem
    {
        void Enable(Entity self);
    }

    /// <summary>
    /// 活跃启用事件系统
    /// </summary>
    public abstract class EnableSystem<T> : SystemBase<T, IEnableSystem>, IEnableSystem
        where T : Entity
    {
        public void Enable(Entity self) => OnEnable(self as T);
        public abstract void OnEnable(T self);
    }
}
