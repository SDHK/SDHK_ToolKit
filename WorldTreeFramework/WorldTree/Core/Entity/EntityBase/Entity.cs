/****************************************

* 创 建 者：  闪电黑客
* 创建时间：  2022/5/6 21:27
* 描    述:

****************************************/

using System;
using System.Linq;

namespace WorldTree
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
        public long id;

        /// <summary>
        /// 实体类型
        /// </summary>
        public Type Type;

        /// <summary>
        /// 根节点
        /// </summary>
        public EntityManager Root;
 

        private Entity domain;
        /// <summary>
        /// 域节点：递归设置子节点
        /// </summary>
        public Entity Domain
        {
            get { return domain; }
            set
            {
                domain = value;
                if (children != null)
                {
                    if (children.Count > 0)
                    {
                        foreach (var item in children.Values)
                        {
                            if (item.domain != item)
                            {
                                item.Domain = value;    //递归属性
                            }
                        }
                    }
                }

                if (components != null)
                {
                    if (components.Count > 0)
                    {
                        foreach (var item in components.Values)
                        {
                            if (item.domain != item)
                            {
                                item.Domain = value;   //递归属性
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 父节点
        /// </summary>
        public Entity Parent;

        /// <summary>
        /// 活跃标记
        /// </summary>
        private bool activeMark = false;

        /// <summary>
        /// 活跃状态
        /// </summary>
        private bool active = false;

        /// <summary>
        /// 活跃状态
        /// </summary>
        public bool IsActice => active;

        /// <summary>
        /// 活跃标记
        /// </summary>
        public bool ActiveMark => activeMark;

        /// <summary>
        /// 设置激活状态
        /// </summary>
        public void SetActive(bool value)
        {
            if (activeMark != value)
            {
                activeMark = value;
                RefreshActive();
            }
        }

        /// <summary>
        /// 刷新激活状态：递归设置子节点
        /// </summary>
        private void RefreshActive()
        {
            var activeTag = active;
            active = (Parent == null) ? activeMark : Parent.active && activeMark;

            if (activeTag != active)
            {
                if (active)
                {
                    Root.Enable(this);
                }
                else
                {
                    Root.Disable(this);
                }
            }

            if (children != null)
            {
                if (children.Count > 0)
                {
                    foreach (var item in children.Values)
                    {
                        if (item.activeMark == true)
                        {
                            item.RefreshActive();    //递归属性
                        }
                    }
                }
            }
            if (components != null)
            {
                if (components.Count > 0)
                {
                    foreach (var item in components.Values)
                    {
                        if (item.activeMark == true)
                        {
                            item.RefreshActive();   //递归属性
                        }
                    }
                }

            }

        }


        public Entity()
        {
            Type = GetType();
        }

        /// <summary>
        /// 子节点
        /// </summary>
        public UnitDictionary<long, Entity> children;

        /// <summary>
        /// 组件节点
        /// </summary>
        public UnitDictionary<Type, Entity> components;

        public override string ToString()
        {
            return Type.ToString();
        }

        public UnitDictionary<long, Entity> Children
        {
            get
            {
                if (children == null)
                {
                    children = this.UnitPoolManager().Get<UnitDictionary<long, Entity>>();
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
                    components = this.UnitPoolManager().Get<UnitDictionary<Type, Entity>>();
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

            T entity = Root.EntityPoolManager.Get<T>();
            if (Children.TryAdd(entity.id, entity))
            {
                entity.Parent = this;
                entity.Domain = Domain;
                Root.Add(entity);
            }

            return entity;
        }

        /// <summary>
        /// 添加新的子节点
        /// </summary>
        public Entity AddChildren(Type type)
        {
            Entity entity = Root.EntityPoolManager.Get(type);
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
                Root.Remove(entity);
                entity.RemoveAll();

                entity.Parent = null;
                entity.Domain = null;

                children.Remove(entity.id);

                Root.EntityPoolManager.Recycle(entity);

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
                component = Root.EntityPoolManager.Get<T>();

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
        public Entity AddComponent(Type type)
        {
            if (!Components.TryGetValue(type, out Entity component))
            {
                component = Root.EntityPoolManager.Get(type);

                component.Parent = this;
                component.Domain = Domain;

                component.isComponent = true;

                components.Add(type, component);
                Root.Add(component);
            }
            return component;
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        public void AddComponent(Entity component)
        {
            if (component != null)
            {
                Type type = component.Type;
                if (Components.TryAdd(type, component))
                {
                    component.Parent = this;
                    component.Domain = Domain;
                    component.isComponent = true;
                    Root.Add(component);
                }
            }

        }

        /// <summary>
        /// 获取组件
        /// </summary>
        public Entity GetComponent(Type type)
        {
            Components.TryGetValue(type, out Entity component);
            return component;
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        public T GetComponent<T>()
            where T : Entity
        {
            Type type = typeof(T);
            Entity entity = null;
            if (components != null)
            {
                components.TryGetValue(type, out entity);
            }
            return entity as T;
        }

        /// <summary>
        /// 移除组件
        /// </summary>
        public void RemoveComponent<T>()
            where T : Entity
        {
            Type type = typeof(T);
            if (components.ContainsKey(type))
            {
                Entity component = components[type];

                Root.Remove(component);

                component.RemoveAll();

                component.Parent = null;
                component.Domain = null;

                components.Remove(type);

                Root.EntityPoolManager.Recycle(component);


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
            if (components.ContainsValue(component))
            {

                Root.Remove(component);
                component.RemoveAll();

                component.Parent = null;
                component.Domain = null;

                components.Remove(component.Type);

                Root.EntityPoolManager.Recycle(component);

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
            while (children != null)
            {
                if (children.Count != 0)
                {
                    RemoveChildren(children.Last().Value);
                }
                else
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 移除全部组件
        /// </summary>
        public void RemoveAllComponent()
        {
            while (components != null)
            {
                if (components.Count != 0)
                {
                    RemoveComponent(components.Last().Value);
                }
                else
                {
                    break;
                }
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
