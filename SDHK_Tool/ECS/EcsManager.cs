using Singleton;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SDHK_Tool.ECS
{
    public class EcsManager : SingletonMonoEagerBase<EcsManager>
    {

        public Dictionary<long, Entity> allEntities = new Dictionary<long, Entity>();

        public Dictionary<Type, List<long>> entitieList = new Dictionary<Type, List<long>>();//
        public List<long> entitieAddList = new List<long>();
        

        private void Update()
        {
            Foreach(new AddSystem());
        }

        private void Foreach(SystemBase ecsSystem)
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