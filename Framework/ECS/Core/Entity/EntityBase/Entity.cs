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
    /// <summary>
    /// 实体基类
    /// </summary>
    public abstract class Entity : Unit
    {
        /// <summary>
        /// 回收标记
        /// </summary>
        public bool isRecycle;

        /// <summary>
        /// 组件标记
        /// </summary>
        public bool isComponent;

        /// <summary>
        /// Id
        /// </summary>
        public ulong id;

        /// <summary>
        /// 实体类型
        /// </summary>
        public Type Type;

        /// <summary>
        /// 根节点
        /// </summary>
        public EntityManager Root;

        /// <summary>
        /// 域节点
        /// </summary>
        public Entity Domain { get; set; }//节点换域需要递归子节点

        /// <summary>
        /// 父节点
        /// </summary>
        public Entity Parent;

        /// <summary>
        /// 父级是否活跃
        /// </summary>
        private bool isParentActive = true;

        /// <summary>
        /// 自身是否活跃
        /// </summary>
        private bool isActive = true;

        /// <summary>
        /// 递归设置子节点活跃状态
        /// </summary>
        public void SetParentActive(bool value)
        {
            isParentActive = value;

            if (Children.Count > 0)
            {
                foreach (var item in Children.Values)
                {
                    if (item.isParentActive != value)
                    {
                        item.SetParentActive(value);    //递归属性
                    }
                }
            }
            if (Components.Count > 0)
            {
                foreach (var item in Components.Values)
                {
                    if (item.isParentActive != value)
                    {
                        item.SetParentActive(value);    //递归属性
                    }
                }
            }
        }

        /// <summary>
        /// 自身是否活跃
        /// </summary>
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;

                if (Children.Count > 0)
                {
                    foreach (var item in Children.Values)
                    {
                        item.SetParentActive(value);    //递归属性

                    }
                }
                if (Components.Count > 0)
                {
                    foreach (var item in Components.Values)
                    {
                        item.SetParentActive(value);    //递归属性
                    }
                }

            }
        }

        /// <summary>
        /// 真正活跃状态
        /// </summary>
        public bool RealActive => isParentActive && isActive;


        /// <summary>
        /// 子节点
        /// </summary>
        private UnitDictionary<ulong, Entity> children;

        /// <summary>
        /// 组件节点
        /// </summary>
        private UnitDictionary<Type, Entity> components;

        public override string ToString()
        {
            return Type.Name;
        }

        public UnitDictionary<ulong, Entity> Children
        {
            get
            {
                if (children == null)
                {
                    children = UnitDictionary<ulong, Entity>.GetObject();
                }
                return children;
            }
            set { children = value; }
        }
        public UnitDictionary<Type, Entity> Components
        {
            get
            {
                if (components == null)
                {
                    components = UnitDictionary<Type, Entity>.GetObject();
                }
                return components;
            }

            set { components = value; }
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        public T To<T>()
        where T : Entity
        {
            return this as T;
        }

        /// <summary>
        /// 域节点
        /// </summary>
        public T DomainTo<T>()
        where T : Entity
        {
            return Domain as T;
        }

        /// <summary>
        /// 父节点
        /// </summary>
        public T ParentTo<T>()
        where T : Entity
        {
            return Parent as T;
        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        public void AddChildren(Entity entity)
        {
            if (entity != null)
            {
                if (Children.TryAdd(entity.id, entity))
                {
                    entity.Parent = this;
                    entity.Domain = Domain;
                    Root.Add(entity);
                }
            }
        }
        /// <summary>
        /// 添加新的子节点
        /// </summary>
        public T AddChildren<T>()
            where T : Entity
        {
            T entity = Root.pool.Get<T>();
            if (Children.TryAdd(entity.id, entity))
            {
                entity.Parent = this;
                entity.Domain = Domain;

                Root.Add(entity);
            }

            return entity;
        }

        /// <summary>
        /// 移除子节点
        /// </summary>

        public void RemoveChildren(Entity entity)
        {
            if (entity != null)
            {
                Root.pool.Recycle(entity);
                Root.Remove(entity);
                entity.RemoveAll();

                entity.Parent = null;
                entity.Domain = null;

                Children.Remove(entity.id);


                if (children.Count == 0)
                {
                    children.Recycle();
                    children = null;
                }
            }
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        public T AddComponent<T>()
            where T : Entity
        {
            Type type = typeof(T);

            T component = null;
            if (!Components.TryGetValue(type, out Entity entity))
            {
                component = Root.pool.Get<T>();

                component.Parent = this;
                component.Domain = Domain;

                component.isComponent = true;

                components.Add(type, component);
                Root.Add(component);
            }
            else
            {
                component = entity as T;
            }

            return component;
        }
        /// <summary>
        /// 添加组件
        /// </summary>
        public void AddComponent(Entity component)
        {
            Type type = component.Type;
            if (!Components.ContainsKey(type))
            {
                component.Parent = this;
                component.Domain = Domain;

                component.isComponent = true;
                components.Add(type, component);
                Root.Add(component);
            }
        }
        /// <summary>
        /// 移除组件
        /// </summary>
        public void RemoveComponent<T>()
            where T : Entity
        {
            Type type = typeof(T);
            if (Components.ContainsKey(type))
            {
                Entity component = components[type];
                Root.pool.Recycle(component);

                Root.Remove(component);

                component.RemoveAll();

                component.Parent = null;
                component.Domain = null;

                components.Remove(type);




                if (components.Count == 0)
                {
                    components.Recycle();
                    components = null;
                }
            }
        }
        /// <summary>
        /// 移除组件
        /// </summary>
        public void RemoveComponent(Entity component)
        {
            if (Components.ContainsValue(component))
            {
                Root.pool.Recycle(component);

                Root.Remove(component);
                component.RemoveAll();

                component.Parent = null;
                component.Domain = null;

                components.Remove(component.Type);
                if (components.Count == 0)
                {
                    components.Recycle();
                    components = null;
                }
            }

        }

        /// <summary>
        /// 移除全部子节点
        /// </summary>
        public void RemoveAllChildren()
        {
            while (Children.Count > 0)
            {
                RemoveChildren(Children.Last().Value);
            }
        }
        /// <summary>
        /// 移除全部组件
        /// </summary>
        public void RemoveAllComponent()
        {
            while (Components.Count > 0)
            {
                RemoveComponent(Components.Last().Value);
            }
        }
        /// <summary>
        /// 移除全部组件和子节点
        /// </summary>
        public void RemoveAll()
        {
            RemoveAllChildren();
            RemoveAllComponent();
        }

        /// <summary>
        /// 移除自己
        /// </summary>
        public void RemoveSelf()
        {
            if (Parent != null)
            {
                if (isComponent)
                {
                    Parent.RemoveComponent(this);
                }
                else
                {
                    Parent.RemoveChildren(this);
                }
            }
            else if (this == Domain)
            {
                RemoveAll();
            }
        }
    }

}
