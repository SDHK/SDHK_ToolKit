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

        public Entity parent;

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


        public void AddChildren(Entity entity)
        {
            if (entity != null)
            {
                entity.parent = this;
                Children.TryAdd(entity.ID, entity);
            }
        }
        public void RemoveChildren(Entity entity)
        {

            if (entity != null)
            {
                Children.Remove(entity.ID);
                if (children.Count == 0)
                {
                    ObjectPoolManager.Instance.Recycle(children);
                    children = null;
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
