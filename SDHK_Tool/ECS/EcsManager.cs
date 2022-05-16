using Singleton;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SDHK_Tool.ECS
{
    public partial class EcsManager : SingletonMonoEagerBase<EcsManager>
    {
        public SortedList<int, List<ISystem>> systems = new SortedList<int, List<ISystem>>();//SystemBase.getType()
        public Dictionary<long, Entity> allEntities = new Dictionary<long, Entity>();

        public Dictionary<Type, List<long>> entitieList = new Dictionary<Type, List<long>>();//
        public Dictionary<Type, List<long>> entitieAddList = new Dictionary<Type, List<long>>();


        private void Update()
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