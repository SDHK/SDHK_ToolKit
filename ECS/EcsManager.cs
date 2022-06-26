using SDHK;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDHK
{
    public class EntitieManager : SingletonBase<EcsManager>
    {
        public Dictionary<long, Entity> allEntities = new Dictionary<long, Entity>();

        public void AddChild(Entity entity)
        {

        }
        public void RemoveChild(Entity entity)
        {

        }
    }

    public partial class EcsManager : SingletonBase<EcsManager>
    {
        public Dictionary<long, Entity> allEntities = new Dictionary<long, Entity>();

        public Queue<long> update1 = new Queue<long>();
        public Queue<long> update2 = new Queue<long>();

        public List<ISystem> systems = new List<ISystem>();//SystemBase.getType()//System不能在组里



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