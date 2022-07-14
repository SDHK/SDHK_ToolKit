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
    public abstract class UpdateSystem<T> : SystemBase<T>, IUpdateSystem
        where T : Entity
    {
        public void Execute(Entity entity) => Update(entity as T);
        public abstract void Update(T entity);
    }

    //调用SystemManager然后注册自己的添加实体的监听方法
    public class UpdateSystemManager : SingletonEntityBase<UpdateSystemManager>
    {
        public UnitDictionary<ulong, Entity> update1 = new UnitDictionary<ulong, Entity>();
        public UnitDictionary<ulong, Entity> update2 = new UnitDictionary<ulong, Entity>();

        public SystemGroup systems;

        public void Update()
        {

            while (update1.Count != 0)
            {
                Debug.Log("Update执行");

                ulong firstKey = update1.Keys.First();
                Entity entity = update1[firstKey];
                Type type = entity.type;

                if (systems.TryGetValue(type, out UnitList<ISystem> systemList))
                {
                    foreach (IUpdateSystem system in systemList)
                    {
                        system.Execute(entity);
                    }
                }
                update1.Remove(firstKey);
                update2.Add(firstKey, entity);
            }
            (update1, update2) = (update2, update1);
        }
    }

    public class UpdateSystemAwakeSystem : AwakeSystem<UpdateSystemManager>
    {
        public override void Awake(UpdateSystemManager entity)
        {
            entity.systems = SystemManager.Instance.RegisterSystems<IUpdateSystem>();
        }
    }

    public class UpdateSystemManagerSystem : EntityListenerSystem<UpdateSystemManager>
    {
        public override void OnAddEntitie(UpdateSystemManager self, Entity entity)
        {
            Type typeKey = entity.GetType();
            if (self.systems.ContainsKey(typeKey))
            {
                Debug.Log("添加"+ typeKey);
                self.update1.Add(entity.ID, entity);
                Debug.Log(self.update1.Count);
            }
        }

        public override void OnRemoveEntitie(UpdateSystemManager self, Entity entity)
        {
            Type typeKey = entity.GetType();
            if (self.systems.ContainsKey(typeKey))
            {
                Debug.Log("Remove" + typeKey);

                self.update1.Remove(entity.ID);
            }
        }
    }







}