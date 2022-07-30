using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 实体根
    /// </summary>
    public abstract class EntityRoot : EntityDomain, IUnit
    {
        public SystemManager systemManager;
        public EntityPoolManager pool;

        public EntityRoot()
        {
            Type = GetType();
            Root = this;
            systemManager = new SystemManager();
            pool = new EntityPoolManager();
            pool.Root = Root;
            OnNew();
            AddComponent(pool);
            AddComponent(systemManager);
        }

        /// <summary>
        /// 释放自己
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;
            OnDispose();
            IsDisposed = true;
        }
        public void OnDispose()
        {
            RemoveAll();
            OnRecycle();
            pool.RemoveSelf();//移除所有组件
            pool.Dispose();//全部释放
            systemManager.Dispose();
        }
    }
}
