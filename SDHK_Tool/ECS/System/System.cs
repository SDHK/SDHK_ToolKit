/****************************************

* 创 建 者：  闪电黑客
* 创建时间：  2022/5/6 21:31
* 描    述:

****************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//数据监听器

namespace SDHK_Tool.ECS
{
    public interface ISystem
    {
        Type SystemType { get; }
        Type EntityType { get; }
        void Execute(Entity entity);
    }

    //用于标记
    public interface IUpdateSystem : ISystem
    {
    }


    public abstract class UpdateSystem<T> : IUpdateSystem
    where T : Entity
    {
        public  Type SystemType => typeof(IUpdateSystem);
        public  Type EntityType => typeof(T);
        public  void Execute(Entity entity)=> Update(entity as T);
        public abstract void Update(T entity);
    }

    [EcsSystem(1)]
    public class System1 : UpdateSystem<Entity2>
    {
        public override void Update(Entity2 entity)
        {

        }
    }

}
