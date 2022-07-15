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
            return UnitPoolManager.Instance.Get<T>();
        }
    }

    /// <summary>
    /// 实体基类
    /// </summary>
    public abstract class Entity : UnitPoolItem
    {
        public ulong ID { get; set; }

        public Type type = null;

        /// <summary>
        /// 根节点
        /// </summary>
        public static Entity root;

        /// <summary>
        /// 父节点
        /// </summary>
        public Entity parent { get; private set; }

        public UnitDictionary<ulong, Entity> children = new UnitDictionary<ulong, Entity>(); //实体
        public UnitDictionary<Type, Entity> components = new UnitDictionary<Type, Entity>(); //组件

        public Dictionary<ulong, Entity> Children
        {
            get
            {
                if (children == null)
                {
                    children = UnitDictionary<ulong, Entity>.GetObject();
                }
                return children;
            }
        }
        public Dictionary<Type, Entity> Components
        {
            get
            {
                if (components == null)
                {
                    components = UnitDictionary<Type, Entity>.GetObject();
                }
                return components;
            }
        }

        public override void OnNew()//懒汉注册OnNew管理器,或许需要一个ECS模式的对象池
        {
            ID = IdManager.GetID;
            type = GetType();
        }
        public override void OnGet()
        {
        }

        public override void OnRecycle()
        {
        }

        public override void OnDispose()
        {
        }

        /// <summary>
        /// 添加子实体
        /// </summary>
        public void AddChildren(Entity entity)
        {
            if (entity != null)
            {
                entity.parent = this;
                Children.TryAdd(entity.ID, entity);
                EntityManager.Instance.Add(entity);
            }
        }

        /// <summary>
        /// 移除子实体
        /// </summary>
        public void RemoveChildren(Entity entity)
        {
            if (entity != null)
            {
                entity.parent = null;
                EntityManager.Instance.Remove(entity);
                entity.Recycle();
                Children.Remove(entity.ID);
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

            T t;
            if (!Components.TryGetValue(type, out Entity entity))
            {
                t = UnitPoolManager.Instance.Get<T>();
                Components.Add(type, t);
                EntityManager.Instance.Add(t);
            }
            else
            {
                t = entity as T;
            }

            return t;
        }


        /// <summary>
        /// 添加组件
        /// </summary>
        public void AddComponent(Entity entity)
        {
            Type type = entity.GetType();
            if (!Components.ContainsKey(type))
            {
                Components.Add(type, entity);
                EntityManager.Instance.Add(entity);
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
                EntityManager.Instance.Remove(components[type]);
                components[type].Recycle();
                components.Remove(type);
                if (components.Count == 0)
                {
                    components.Recycle();
                    components = null;
                }
            }
        }

    }

}
