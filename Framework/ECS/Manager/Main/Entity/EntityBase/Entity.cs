/****************************************

* 创 建 者：  闪电黑客
* 创建时间：  2022/5/6 21:27
* 描    述:

****************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{

    /// <summary>
    /// 泛型实体基类：提供获取和回收对象的方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Entity<T> : Entity
        where T : Entity<T>
    {
        /// <summary>
        /// 单位对象池：获取对象
        /// </summary>
        public static T GetObject()
        {
            return EntityPoolManager.Instance.Get<T>();
        }
    }

    //需要一个检查组件的方法
    /// <summary>
    /// 实体基类
    /// </summary>
    public abstract class Entity : IEntity
    {
        public bool IsDisposed { get; set; }
        public bool IsRecycle { get; set; }
        public bool IsComponent { get; set; }

        public ulong Id { get; set; }

        public Type Type { get; set; }

        /// <summary>
        /// 根节点
        /// </summary>
        public static IEntity Root;

        /// <summary>
        /// 父节点
        /// </summary>
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
