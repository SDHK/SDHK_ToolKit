using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{


    /// <summary>
    /// 监听系统
    /// </summary>
    public interface IEntitySystem : ISystem//或许可以成为单例组件
    {
        void AddEntity(IEntity self, IEntity entity);
        void RemoveEntity(IEntity self, IEntity entity);
    }

    /// <summary>
    /// 实体监听系统基类
    /// </summary>
    public abstract class EntitySystem<T> : SystemBase<T>, IEntitySystem
        where T : class, IEntity
    {
        public void AddEntity(IEntity self, IEntity entity) => OnAddEntity(self as T, entity);

        public void RemoveEntity(IEntity self, IEntity entity) => OnRemoveEntity(self as T, entity);

        /// <summary>
        /// 实体添加时
        /// </summary>
        public abstract void OnAddEntity(T self, IEntity entity);

        /// <summary>
        /// 实体移除时
        /// </summary>
        public abstract void OnRemoveEntity(T self, IEntity entity);
    }


}
