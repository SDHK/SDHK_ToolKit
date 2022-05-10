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

        public SortedList<int, List<Type>> order = new SortedList<int, List<Type>>();// ִ��˳�� List������ֻ������������ȸ��£�����Ҫȫ�����
        
        public Dictionary<long, Entity> allEntities = new Dictionary<long, Entity>();


        public Queue<int> orderList = new Queue<int>(); 
        public Queue<int> orderQueue = new Queue<int>(); 

        public Dictionary<Type, List<long>> entitieList = new Dictionary<Type, List<long>>();//long���ݻ����Ƶ����ɾ
        public List<int> entitieQueue = new List<int>();
        

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

        public static void Replace<T>(ref T t1, ref T t2)
        {
            (t1, t2) = (t2, t1);
        }
    }
}