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
    /// <summary>
    /// 实体对象池
    /// </summary>
    public class EntityPool<E> : GenericPool<E>, IEntity
        where E : class, IEntity
    {
        public bool IsRecycle { get; set; }
        public bool IsComponent { get; set; }


        private UnitList<ISystem> newSystem;
        private UnitList<ISystem> getSystem;
        private UnitList<ISystem> recycleSystem;
        private UnitList<ISystem> destroySystem;

        public EntityPool()
        {
            Id = IdManager.GetID;
            Type = GetType();

            ObjectType = typeof(E);

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

        private E ObjectNew(PoolBase pool)
        {
            E entity = Activator.CreateInstance(ObjectType, true) as E;
            entity.Id = IdManager.GetID;
            entity.Type = entity.GetType();

            return entity;
        }
        private void ObjectDestroy(E obj)
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

        private void ObjectOnNew(E obj)
        {
            if (newSystem != null)
            {
                foreach (INewSystem item in newSystem)
                {
                    item.New(obj);
                }
            }
        }

        private void ObjectOnGet(E obj)
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


        private void ObjectOnRecycle(E obj)
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

        private void ObjectOnDestroy(E obj)
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
                if (Children.TryAdd(entity.Id, entity))
                {
                    entity.Parent = this;
                    EntityManager.Instance.Add(entity);
                }
            }
        }

        public T GetChildren<T>()
            where T : class, IEntity
        {
            T entity = EntityPoolManager.Instance.Get<T>();
            if (Children.TryAdd(entity.Id, entity))
            {
                entity.Parent = this;
                EntityManager.Instance.Add(entity);
            }

            return entity;
        }



        public void RemoveChildren(IEntity entity)
        {
            if (entity != null)
            {
                EntityManager.Instance.Remove(entity);

                entity.Parent = null;
                Children.Remove(entity.Id);
                RemoveAll();

                EntityPoolManager.Instance.Recycle(entity);
                if (children.Count == 0)
                {
                    children.Recycle();
                    children = null;
                }
            }
        }


        public T GetComponent<T>()
            where T : class, IEntity
        {
            Type type = typeof(T);

            T component = null;
            if (!Components.TryGetValue(type, out IEntity entity))
            {
                component = EntityPoolManager.Instance.Get<T>();
                component.Parent = this;
                component.IsComponent = true;

                components.Add(type, component);
                EntityManager.Instance.Add(component);
            }
            else
            {
                component = entity as T;
            }

            return component;
        }

        public void AddComponent(IEntity component)
        {
            Type type = component.Type;
            if (!Components.ContainsKey(type))
            {
                component.Parent = this;
                component.IsComponent = true;
                components.Add(type, component);
                EntityManager.Instance.Add(component);
            }
        }
        public void RemoveComponent<T>()
            where T : class, IEntity
        {
            Type type = typeof(T);
            if (Components.ContainsKey(type))
            {
                IEntity component = components[type];
                EntityManager.Instance.Remove(component);

                component.Parent = null;

                components.Remove(type);
                RemoveAll();
                EntityPoolManager.Instance.Recycle(component);
                if (components.Count == 0)
                {
                    components.Recycle();
                    components = null;
                }
            }
        }

        public void RemoveComponent(IEntity component)
        {
            if (Components.ContainsValue(component))
            {
                EntityManager.Instance.Remove(component);
                component.Parent = null;
                components.Remove(component.Type);
                RemoveAll();
                EntityPoolManager.Instance.Recycle(component);
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

        public void RemoveAll()
        {
            RemoveAllChildren();
            RemoveAllComponent();
        }

        public void Recycle()
        {
            if (Parent != null)
            {
                if (IsComponent)
                {
                    Parent.RemoveComponent(this);
                }
                else
                {
                    Parent.RemoveChildren(this);
                }
            }
            else
            {
                EntityManager.Instance.Remove(this);
                RemoveAll();
                EntityPoolManager.Instance.Recycle(this);
            }
        }
    }
}
