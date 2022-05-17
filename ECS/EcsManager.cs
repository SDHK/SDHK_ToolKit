using Singleton;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDHK
{
    /*
     * 文件夹式分组：需要PathData结构
     * 全局/敌人/武器/子弹
     * 
     
     */

    public class EntityGroup
    {
        public bool isRun = true;
        public SortedList<int, List<ISystem>> systems = new SortedList<int, List<ISystem>>();//SystemBase.getType()
        public Dictionary<Type, List<Entity>> entities = new Dictionary<Type, List<Entity>>();
        public Dictionary<Type, List<Entity>> entitieExecutes = new Dictionary<Type, List<Entity>>();

        public void AddSystem()
        {

        }
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

        public void Update(Type type)
        {
            if (isRun)
            {

            }
        }
    }

    public partial class EcsManager//单独执行层
    {
        public Dictionary<long, Entity> allEntities = new Dictionary<long, Entity>();

        public SortedList<int, EntityGroup> entityGroupUpdate = new SortedList<int, EntityGroup>();
        public SortedList<int, EntityGroup> entityGroupLateUpdate = new SortedList<int, EntityGroup>();
        public SortedList<int, EntityGroup> entityGroupFixedUpdate = new SortedList<int, EntityGroup>();



        private void Update()
        {
            //Guid.NewGuid();
        }
        private void LateUpdate()
        {
        }

        public void FixedUpdate()
        {

        }

        private void Foreach(ISystem ecsSystem)
        {
            //for (int i = 0; i < order.Count; i++)
            //{
            //    for (int j = 0; j < entities[order[i]].Count; j++)
            //    {
            //        if (entities[order[i]][j].Run(ecsSystem)) break;
            //    }
            //}
        }

        public static void Swap<T>(ref T t1, ref T t2)
        {
            (t1, t2) = (t2, t1);
        }
    }
}