using SDHK;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SDHK
{
    /// <summary>
    /// Update系统接口
    /// </summary>
    public interface IUpdateSystem : ISystem
    {
        void Execute(IEntity entity);
    }
    /// <summary>
    /// Update系统基类
    /// </summary>
    public abstract class UpdateSystem<T> : SystemBase<T>, IUpdateSystem
        where T :class,IEntity
    {
        public void Execute(IEntity entity) => Update(entity as T);
        public abstract void Update(T entity);
    }




    /// <summary>
    /// Update生命周期管理器实体
    /// </summary>
    public class UpdateManager : SingletonEntityBase<UpdateManager>
    {
        public UnitDictionary<ulong, IEntity> update1 = new UnitDictionary<ulong, IEntity>();
        public UnitDictionary<ulong, IEntity> update2 = new UnitDictionary<ulong, IEntity>();
        public SystemGroup systems;
        public void Update()
        {
            while (update1.Count != 0)
            {
                ulong firstKey = update1.Keys.First();
                IEntity entity = update1[firstKey];
                Type type = entity.Type;

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

    /// <summary>
    /// 饿汉单例事件系统
    /// </summary>
    public class UpdateManagerSingletonEagerSystem : SingletonEagerSystem<UpdateManager> { }


    /// <summary>
    /// 组件添加事件系统
    /// </summary>
    public class UpdateManagerNewSystem : NewSystem<UpdateManager>
    {
        public override void OnNew(UpdateManager entity)
        {

            entity.systems = SystemManager.Instance.RegisterSystems<IUpdateSystem>();
        }
    }

    /// <summary>
    /// 实体监听事件系统
    /// </summary>
    public class UpdateManagerEntityListenerSystem : EntitySystem<UpdateManager>
    {
        public override void OnAddEntity(UpdateManager self, IEntity entity)
        {
            if (self.systems.ContainsKey(entity.Type))
            {
                self.update1.Add(entity.Id, entity);
            }
        }

        public override void OnRemoveEntity(UpdateManager self, IEntity entity)
        {
            if (self.systems.ContainsKey(entity.Type))
            {
                self.update1.Remove(entity.Id);
                self.update2.Remove(entity.Id);
            }
        }
    }








}