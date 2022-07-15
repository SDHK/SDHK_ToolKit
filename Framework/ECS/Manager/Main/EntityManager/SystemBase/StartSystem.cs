using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 开始系统接口
    /// </summary>
    public interface IStartSystem : ISystem
    {
        void Execute(Entity entity);
    }

    /// <summary>
    /// 开始系统
    /// </summary>
    public abstract class StartSystem<T> : SystemBase<T>, IStartSystem
        where T : Entity
    {
        public void Execute(Entity entity) => Start(entity as T);
        public abstract void Start(T entity);

    }
}
