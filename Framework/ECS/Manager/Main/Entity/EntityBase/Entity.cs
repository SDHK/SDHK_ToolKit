/****************************************

* 创 建 者：  闪电黑客
* 创建时间：  2022/5/6 21:27
* 描    述:

****************************************/

using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{

    ///// <summary>
    ///// 泛型实体基类：提供获取和回收对象的方法
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public abstract class Entity<T> : Entity
    //    where T : Entity<T>
    //{
    //    /// <summary>
    //    /// 单位对象池：获取对象
    //    /// </summary>
    //    public static T GetObject()
    //    {
    //        return null;
    //    }
    //}

    //或许需要一个Edit的根节点，Main应该是域的概念：组件
    //需要一个bool激活标记
    //可以设置任意组件为域


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


        public EntityManager Root { get; set; }

        public EntityManager Domain { get; set; }//节点换域需要递归子节点


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


        public T To<T>()
        where T : class, IEntity
        {
            return this as T;
        }

        public void AddChildren(IEntity entity)
        {
            if (entity != null)
            {
                if (Children.TryAdd(entity.Id, entity))
                {
                    entity.Parent = this;
                    entity.Domain = Domain;
                    entity.Root = Root;
                    Domain.Add(entity);
                }
            }
        }

        public T GetChildren<T>()
            where T : class, IEntity
        {
            T entity = Domain.GetComponent<EntityPoolManager>().Get<T>();
            if (Children.TryAdd(entity.Id, entity))
            {
                entity.Parent = this;
                entity.Domain = Domain;
                entity.Root = Root;

                Domain.Add(entity);
            }

            return entity;
        }



        public void RemoveChildren(IEntity entity)
        {
            if (entity != null)
            {
                Domain.Remove(entity);
                entity.RemoveAll();

                entity.Parent = null;
                entity.Domain = null;
                entity.Root = null;

                Children.Remove(entity.Id);

                Domain.GetComponent<EntityPoolManager>().Recycle(entity);
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
                component = Domain.GetComponent<EntityPoolManager>().Get<T>();
                component.Parent = this;
                component.Domain = Domain;
                component.Root = Root;

                component.IsComponent = true;

                components.Add(type, component);
                Domain.Add(component);
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
                component.Domain = Domain;
                component.Root = Root;

                component.IsComponent = true;
                components.Add(type, component);
                Domain.Add(component);
            }
        }
        public void RemoveComponent<T>()
            where T : class, IEntity
        {
            Type type = typeof(T);
            if (Components.ContainsKey(type))
            {
                IEntity component = components[type];
                Domain.Remove(component);

                component.RemoveAll();

                component.Parent = null;
                component.Domain = null;
                component.Root = null;

                components.Remove(type);
                Domain.GetComponent<EntityPoolManager>().Recycle(component);
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
                Domain.Remove(component);
                component.RemoveAll();

                component.Parent = null;
                component.Domain = null;
                component.Root = null;

                components.Remove(component.Type);
                Domain.GetComponent<EntityPoolManager>().Recycle(component);
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
                Domain.Remove(this);
                RemoveAll();
                Domain.GetComponent<EntityPoolManager>().Recycle(this);
            }
        }
    }

}
