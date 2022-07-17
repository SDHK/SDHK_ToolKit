/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/17 17:23

* 描述： 实体对象池，半ECS组件

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{

    /// <summary>
    /// 实体对象池,组件，需要挂在manager下
    /// </summary>
    public class EntityPool<T> : GenericPool<T>, IEntity
        where T : class, IEntity
    {
        public bool IsRecycle { get; set; }

        private UnitList<ISystem> newSystem;
        private UnitList<ISystem> getSystem;
        private UnitList<ISystem> recycleSystem;
        private UnitList<ISystem> destroySystem;

        public EntityPool()
        {
            Id = IdManager.GetID;
            Type = GetType();

            ObjectType = typeof(T);

            NewObject = ObjectNew;
            DestroyObject = ObjectDestroy;

            objectOnNew += ObjectOnNew;
            objectOnGet += ObjectOnGet;
            objectOnRecycle += ObjectOnRecycle;
            objectOnDestroy += ObjectOnDestroy;

            newSystem = SystemManager.Instance.GetSystems<INewSystem>(ObjectType);
            getSystem = SystemManager.Instance.GetSystems<IGetSystem>(ObjectType);
            recycleSystem = SystemManager.Instance.GetSystems<IRecycleSystem>(ObjectType);
            destroySystem = SystemManager.Instance.GetSystems<IDestroySystem>(ObjectType);
        }

        public override string ToString()
        {
            return "[EntityPool<" + ObjectType.Name + ">] ";
        }

        private T ObjectNew(PoolBase pool)
        {
            T entity = Activator.CreateInstance(ObjectType, true) as T;
            entity.Id = IdManager.GetID;
            entity.Type = entity.GetType();

            return entity;
        }
        private void ObjectDestroy(T obj)
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

        private void ObjectOnNew(T obj)
        {
            if (newSystem != null)
            {
                foreach (INewSystem item in newSystem)
                {
                    item.New(obj);
                }
            }
        }

        private void ObjectOnGet(T obj)
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


        private void ObjectOnRecycle(T obj)
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

        private void ObjectOnDestroy(T obj)
        {
            if (destroySystem != null)
            {
                foreach (IDestroySystem item in destroySystem)
                {
                    item.Destroy(obj);
                }
            }
        }


        public ulong Id { get; set; }
        public Type Type { get; set; }
        public IEntity Parent { get; set; }


        private UnitDictionary<ulong, IEntity> children;
        private UnitDictionary<Type, IEntity> components;

        public UnitDictionary<ulong, IEntity> Children
        {
            get
            {
                if (children == null)
                {
                    children = UnitDictionary<ulong, IEntity>.GetObject();
                }
                return children;
            }
        }
        public UnitDictionary<Type, IEntity> Components
        {
            get
            {
                if (components == null)
                {
                    components = UnitDictionary<Type, IEntity>.GetObject();
                }
                return components;
            }
        }


        public void AddChildren(IEntity entity)
        {
            if (entity != null)
            {
                entity.Parent = this;
                Children.TryAdd(entity.Id, entity);
                EntityManager.Instance.Add(entity);
            }
        }

        public T1 GetChildren<T1>()
           where T1 : class, IEntity
        {
            T1 entity = EntityPoolManager.Instance.Get<T1>();
            if (Children.TryAdd(entity.Id, entity))
            {
                EntityManager.Instance.Add(entity);
            }

            return entity;
        }

        public void AddComponent(IEntity entity)
        {
            Type type = entity.Type;
            if (!Components.ContainsKey(type))
            {
                components.Add(type, entity);
                EntityManager.Instance.Add(entity);
            }
        }

        public void RemoveChildren(IEntity entity)
        {
            if (entity != null)
            {
                entity.Parent = null;
                EntityManager.Instance.Remove(entity);
                EntityPoolManager.Instance.Recycle(entity);

                Children.Remove(entity.Id);
                entity.RemoveAllChildren();
                entity.RemoveAllComponent();
                if (children.Count == 0)
                {
                    children.Recycle();
                    children = null;
                }
            }
        }

        public T1 GetComponent<T1>()
            where T1 : class, IEntity
        {
            Type type = typeof(T1);

            T1 t = null;
            if (!Components.TryGetValue(type, out IEntity entity))
            {
                t = EntityPoolManager.Instance.Get<T1>();
                components.Add(type, t);
                EntityManager.Instance.Add(t);
            }
            else
            {
                t = entity as T1;
            }

            return t;
        }

        public void RemoveComponent<T1>()
            where T1 : class, IEntity
        {
            Type type = typeof(T);
            if (Components.ContainsKey(type))
            {
                IEntity entity = components[type];
                components.Remove(type);
                entity.RemoveAllChildren();
                entity.RemoveAllComponent();
                EntityManager.Instance.Remove(entity);
                EntityPoolManager.Instance.Recycle(entity);

                if (components.Count == 0)
                {
                    components.Recycle();
                    components = null;
                }
            }
        }

        public void RemoveComponent(IEntity entity)
        {
            if (Components.ContainsValue(entity))
            {
                EntityManager.Instance.Remove(entity);
                components.Remove(entity.Type);
                entity.RemoveAllChildren();
                entity.RemoveAllComponent();
                EntityPoolManager.Instance.Recycle(entity);

                if (components.Count == 0)
                {
                    components.Recycle();
                    components = null;
                }
            }

        }

        public void RemoveAllChildren()
        {
            while (Children.Count > 0)
            {
                RemoveChildren(Children.First().Value);
            }
        }

        public void RemoveAllComponent()
        {
            while (Components.Count > 0)
            {
                RemoveComponent(Components.First().Value);
            }
        }
    }
}
