/****************************************

* 创 建 者：  闪电黑客
* 创建时间：  2022/5/5 10:29
* 描    述:

****************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;



namespace SDHK_Tool.ECS
{
    public class Manager
    {
        List<Entity> entities = new List<Entity>();
    }

    //============================================

    public partial interface ISystem
    {
        void Call(Entity entity);
    }

    public partial class AddSystem : ISystem
    {
        public void Call(Entity entity)
        {
            Debug.Log("Add Entity");
        }
    }

    public partial class UpdateSystem : ISystem
    {
        public void Call(Entity e)
        {
            Debug.Log("Update Entity");

        }
    }
    //============================================

    public partial interface ISystem
    {
        void Call(Entity2 entity);
    }

    public partial class AddSystem : ISystem
    {
        public void Call(Entity2 entity)
        {
            Debug.Log("Add Entity1");
        }
    }
    public partial class UpdateSystem : ISystem
    {
        public void Call(Entity2 entity)
        {
            Debug.Log("Update Entity1");
        }
    }

    //============================================
    public partial interface ISystem
    {
        void Call(Entity3 entity);
    }

    public partial class AddSystem : ISystem
    {
        public void Call(Entity3 entity)
        {
            Debug.Log("Add Entity1");
        }
    }
    public partial class UpdateSystem : ISystem
    {
        public void Call(Entity3 e)
        {
            Debug.Log("Update Entity");

        }
    }

}
