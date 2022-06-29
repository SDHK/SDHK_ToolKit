using SDHK;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDHK
{

    //用于标记
    public interface IUpdateSystem : ISystem
    {
        void Execute(Entity entity);
    }
    public abstract class UpdateSystem<T> : IUpdateSystem
        where T : Entity
    {
        public Type SystemType => throw new NotImplementedException();

        public Type EntityType => throw new NotImplementedException();

        public void Execute(Entity entity) => Update(entity as T);
        public abstract void Update(T entity);
    }

    //调用SystemManager然后注册自己的添加实体的监听方法
    public class UpdateSystem : SingletonEagerBase<UpdateSystem>//!!
    {
        public Queue<long> update1 = new Queue<long>();
        public Queue<long> update2 = new Queue<long>();

        public override void OnInstance()
        {
            SystemManager.Instance.RegisterSystems<IUpdateSystem>();
        }

        public void OnAddEntitie(Entity entity)
        {
        }

        public void OnRemoveEntitie(Entity entity)
        {
        }


        public void Update()
        {

        }
    }




    public partial class EcsManager : SingletonEagerBase<EcsManager>
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