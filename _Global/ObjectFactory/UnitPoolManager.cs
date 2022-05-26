using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    public class EntityMonoGroup
    {
        public bool isRun = true;

        public EntityGroup Update = new EntityGroup();
        public EntityGroup LateUpdate = new EntityGroup();
        public EntityGroup FixedUpdate = new EntityGroup();

        public UnitPoolManager unitPools;
        //计时器
    }

    public class EntityGroup
    {
        public SortedList<int, List<ISystem>> systems = new SortedList<int, List<ISystem>>();//SystemBase.getType()//System不能在组里

        public Dictionary<Type, List<Entity>> entities = new Dictionary<Type, List<Entity>>();
        public Dictionary<Type, List<Entity>> entitieExecutes = new Dictionary<Type, List<Entity>>();

        public void Add(Entity entity)
        {
            Type type = entity.GetType();
            if (!entities.ContainsKey(type))
            {
                entities.Add(type, new List<Entity>());
                entitieExecutes.Add(type, new List<Entity>());
            }
            if (!entities[type].Contains(entity))
            {
                entities[type].Add(entity);
            }
        }

        public void remove(Entity entity)
        {
            Type type = entity.GetType();
            if (entities.ContainsKey(type))
            {
                entities[type].Remove(entity);
                entitieExecutes[type].Remove(entity);
            }
        }

        public void Swap(Type type)
        {
            List<Entity> swap;
            swap = entities[type];
            entities[type] = entitieExecutes[type];
            entitieExecutes[type] = swap;
        }

    }




    public class UnitPoolManager
    {
        Dictionary<Type, PoolBase> pools = new Dictionary<Type, PoolBase>();

        public void Get<T>()
            where T : class, IPoolUnit
        {


        }
        public void Set<T>(UnitPool<T> pool)
        where T : class, IPoolUnit
        {

        }

        public void Add<T>(UnitPool<T> pool)
        where T : class, IPoolUnit
        {

        }



    }
}
