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

        public EntityRoot()
        {
            Type = GetType();
            Root = this;
            Domain = this;
            systemManager = new SystemManager();
            OnNew();
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
            systemManager.Dispose();
        }
    }
}
