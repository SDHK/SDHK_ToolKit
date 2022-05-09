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
        //Entity��Ҫ���ձ��

        public SortedList<int, List<Type>> order = new SortedList<int, List<Type>>();// ִ��˳�� List������ֻ������������ȸ��£�����Ҫȫ�����

        public Dictionary<Type, Dictionary<long, Entity>> entities = new Dictionary<Type, Dictionary<long, Entity>>();//long���ݻ����Ƶ����ɾ


        //!Updateʱ�䲻ȷ����������
        public Dictionary<Type, Queue<long>> deletes = new Dictionary<Type, Queue<long>>();//��һ�����棬�������ɾ�� ����Updateɾ��ʵ������ִ��



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