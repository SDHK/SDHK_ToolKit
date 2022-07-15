using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 实体监听系统接口
    /// </summary>
    public interface IEntityListenerSystem : ISystem
    {
        public void AddEntity(Entity entity, Entity arg);
        public void RemoveEntity(Entity entity, Entity arg);
    }
    /// <summary>
    /// 实体监听系统基类
    /// </summary>
    public abstract class EntityListenerSystem<T> : SystemBase<T>, IEntityListenerSystem
        where T : Entity
    {
        public void AddEntity(Entity self, Entity entity) => OnAddEntitie(self as T, entity);
        public void RemoveEntity(Entity self, Entity entity) => OnRemoveEntitie(self as T, entity);

        /// <summary>
        /// 实体添加时
        /// </summary>
        public abstract void OnAddEntitie(T self, Entity entity);

        /// <summary>
        /// 实体移除时
        /// </summary>
        public abstract void OnRemoveEntitie(T self, Entity entity);
    }
}
