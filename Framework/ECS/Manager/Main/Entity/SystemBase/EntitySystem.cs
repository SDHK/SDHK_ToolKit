
/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/18 9:35

* 描述： 实体监听系统
* 
* 思路为给管理器用的实体添加事件的监听
* 
* 这样就不需要每增加一个管理器，
* 就得去注册管理器的监听。
* 
* 在而是在添加任意实体的时候，判断有实现系统的实体
* 就能监听全局的实体添加移除事件
* 
* 自己 添加和移除时 不会监听到自己。
* 

*/

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
