using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 实体饿汉单例系统接口
    /// </summary>
    public interface ISingletonEagerSystem : ISystem
    {
        void Instance();
    }

    /// <summary>
    /// 实体饿汉单例系统
    /// </summary>
    public abstract class SingletonEagerSystem<T> : SystemBase<T>, ISingletonEagerSystem
        where T :class ,IEntity
    {
        public void Instance()
        {
            EntityRoot.Root.GetComponent<T>();
        }
    }
}
