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

namespace SDHK
{
    //Entity需要回收标记
    public abstract class Entity : UnitPoolItem
    {
        public long ID { get; set; }

        /// <summary>
        /// 根节点
        /// </summary>
        public static Entity root;

        /// <summary>
        /// 父节点
        /// </summary>
        public Entity parent { get; private set; }

        public Dictionary<long, Entity> children = new Dictionary<long, Entity>();  //实体
        public Dictionary<Type, Entity> components = new Dictionary<Type, Entity>(); //组件

        public Dictionary<long, Entity> Children
        {
            get
            {
                if (children == null)
                {
                    children = ObjectPoolManager.Instance.Get<Dictionary<long, Entity>>();
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
                    components = ObjectPoolManager.Instance.Get<Dictionary<Type, Entity>>();
                }
                return components;
            }
        }

        public void AddChildren(Entity entity)
        {
            if (entity != null)
            {
                entity.parent = this;
                Children.TryAdd(entity.ID, entity);
                EntityManager.Instance.Add(entity);
            }
        }
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
                    ObjectPoolManager.Instance.Recycle(children);
                    children = null;
                }
            }
        }

        public void AddComponent<T>()
            where T : Entity
        {
            Type type = typeof(T);
            if (!Components.ContainsKey(type))
            {
                T t = UnitPoolManager.Instance.Get<T>();
                Components.Add(type, t);
            }
        }

        public void RemoveComponent<T>()
            where T : Entity
        {
            Type type = typeof(T);
            if (Components.ContainsKey(type))
            {
                components[type].Recycle();
                components.Remove(type);
                if (components.Count == 0)
                {
                    ObjectPoolManager.Instance.Recycle(components);
                    components = null;
                }
            }
        }

    }





    public class Entity2 : Entity
    {
    }

    public class Entity3 : Entity
    {

    }
}
