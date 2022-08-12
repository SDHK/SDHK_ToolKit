using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{

    /// <summary>
    /// TaskUpdate系统接口
    /// </summary>
    public interface ITaskUpdateSystem : ISystem
    {
        /// <summary>
        /// 执行
        /// </summary>
        void Execute(Entity self);
    }

    /// <summary>
    /// TaskUpdate系统基类
    /// </summary>
    public abstract class TaskUpdateSystem<T> : SystemBase<T, ITaskUpdateSystem>, ITaskUpdateSystem
        where T : Entity
    {
        public void Execute(Entity self) => Update(self as T);
        public abstract void Update(T self);
    }
}
