/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/17 17:23

* 描述： 实体对象池，半ECS实体。
* 掌管实体的生命周期
* 
* 但实体设定由对象池生成，生成自己会导致死循环。
* 所以这个组件由管理器通过New生成后挂为管理器子物体。
*
* 设定为实体的目的是为了可以挂组件添加功能，例如计时销毁，或生成后的计数回收
*
*

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{

    public class EntityPoolAddSystem : AddSystem<EntityPool>
    {
        public override void OnAdd(EntityPool self)
        {
            //注册生命周期系统
            self.newSystem = self.Root.systemManager.GetSystems<INewSystem>(self.ObjectType);
            self.getSystem = self.Root.systemManager.GetSystems<IGetSystem>(self.ObjectType);
            self.recycleSystem = self.Root.systemManager.GetSystems<IRecycleSystem>(self.ObjectType);
            self.destroySystem = self.Root.systemManager.GetSystems<IDestroySystem>(self.ObjectType);
        }
    }


    /// <summary>
    /// 实体类型对象池
    /// </summary>
    public class EntityPool : Entity, IUnit
    {

        #region 对象池功能
        /// <summary>
        /// 对象类型
        /// </summary>
        public Type ObjectType { get; set; }

        /// <summary>
        /// 对象池
        /// </summary>
        private Queue<IEntity> objetPool = new Queue<IEntity>();

        /// <summary>
        /// 当前保留对象数量
        /// </summary>
        public int Count => objetPool.Count;
        /// <summary>
        /// 预加载数量
        /// </summary>
        public int minLimit = 0;

        /// <summary>
        /// 对象回收数量限制
        /// </summary>
        public int maxLimit = -1;

        /// <summary>
        /// 从队列获取一个对象，假如队列无对象则新建对象
        /// </summary>
        public IEntity DequeueOrNewObject()
        {
            lock (objetPool)
            {
                IEntity obj = null;

                while (objetPool.Count > 0 && obj == null)
                {
                    obj = objetPool.Dequeue();
                }

                if (obj == null)
                {
                    obj = ObjectNew();
                    ObjectOnNew(obj);
                }

                return obj;
            }
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        public IEntity GetObject()
        {
            IEntity obj = DequeueOrNewObject();
            ObjectOnGet(obj);
            return obj;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        public void Recycle(IEntity obj)
        {
            lock (objetPool)
            {
                if (obj != null)
                {

                    if (maxLimit == -1 || objetPool.Count < maxLimit)
                    {
                        if (!objetPool.Contains(obj))
                        {
                            ObjectOnRecycle(obj);
                            objetPool.Enqueue(obj);
                        }
                    }
                    else
                    {
                        ObjectOnRecycle(obj);
                        ObjectOnDestroy(obj);
                        ObjectDestroy(obj);
                    }
                }
            }
        }

        /// <summary>
        /// 释放所有
        /// </summary>
        public void DisposeAll()
        {
            lock (objetPool)
            {
                while (objetPool.Count > 0)
                {
                    var obj = objetPool.Dequeue();
                    ObjectOnDestroy(obj);
                    ObjectDestroy(obj);
                }
            }
        }

        /// <summary>
        /// 释放一个
        /// </summary>
        public void DisposeOne()
        {
            lock (objetPool)
            {
                if (objetPool.Count > 0)
                {
                    var obj = objetPool.Dequeue();
                    ObjectOnDestroy(obj);
                    ObjectDestroy(obj);
                }
            }
        }

        /// <summary>
        /// 预加载
        /// </summary>
        public void Preload()
        {
            lock (objetPool)
            {
                while (objetPool.Count < minLimit)
                {
                    IEntity obj = ObjectNew();
                    ObjectOnNew(obj);
                    objetPool.Enqueue(obj);
                }
            }
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
            DisposeAll();
        }
        #endregion

        #region ECS生命周期事件

        public UnitList<ISystem> newSystem;
        public UnitList<ISystem> getSystem;
        public UnitList<ISystem> recycleSystem;
        public UnitList<ISystem> destroySystem;

        public EntityPool(Type type)
        {
            Id = IdManager.GetID;
            Type = GetType();
            ObjectType = type;

        }

        public override string ToString()
        {
            return "[EntityPool<" + ObjectType.Name + ">] ";
        }

        private IEntity ObjectNew()
        {
            IEntity entity = Activator.CreateInstance(ObjectType, true) as IEntity;
            entity.Id = IdManager.GetID;
            entity.Type = entity.GetType();
            entity.Root = Root;

            return entity;
        }
        private void ObjectDestroy(IEntity obj)
        {
            if (obj != null)
            {
                if (obj is IDisposable && !obj.IsDisposed)
                {
                    (obj as IDisposable).Dispose();
                }
            }
            obj.IsDisposed = true;

        }

        private void ObjectOnNew(IEntity obj)
        {
            if (newSystem != null)
            {
                foreach (INewSystem item in newSystem)
                {
                    item.New(obj);
                }
            }
        }

        private void ObjectOnGet(IEntity obj)
        {
            obj.IsRecycle = false;
            if (getSystem != null)
            {
                foreach (IGetSystem item in getSystem)
                {
                    item.Get(obj);
                }
            }

        }

        private void ObjectOnRecycle(IEntity obj)
        {
            obj.IsRecycle = true;
            if (recycleSystem != null)
            {
                foreach (IRecycleSystem item in recycleSystem)
                {
                    item.Recycle(obj);
                }
            }
        }

        private void ObjectOnDestroy(IEntity obj)
        {
            if (destroySystem != null)
            {
                foreach (IDestroySystem item in destroySystem)
                {
                    item.Destroy(obj);
                }
            }
        }

        #endregion
    }
}
