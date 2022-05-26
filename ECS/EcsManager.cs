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