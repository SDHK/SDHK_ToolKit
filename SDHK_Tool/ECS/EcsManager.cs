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
        //Entity需要回收标记

        public SortedList<int, List<Type>> order = new SortedList<int, List<Type>>();// 执行顺序 List的内容只增不减，如果热更新，则需要全部清除

        public Dictionary<Type, Dictionary<long, Entity>> entities = new Dictionary<Type, Dictionary<long, Entity>>();//long内容会随机频繁增删


        //!Update时间不确定，不能用
        public Dictionary<Type, Queue<long>> deletes = new Dictionary<Type, Queue<long>>();//另一个储存，用来标记删除 ，在Update删除实体会打乱执行



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

    }
}