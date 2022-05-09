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



namespace SDHK_Tool.ECS
{

    public abstract partial class SystemBase
    {
        public virtual bool Call(Entity entity) =>false;
    }


    public partial class AddSystem : SystemBase
    {
        public override bool Call(Entity entity)
        {
            Debug.Log("Add Entity");
            return true;
        }
    }

    public partial class UpdateSystem : SystemBase
    {
        public override bool Call(Entity e)
        {
            Debug.Log("Update Entity");
            return true;

        }
    }



    public abstract partial class SystemBase
    {
        public virtual bool Call(Entity2 entity) => false;
    }

    public partial class AddSystem : SystemBase
    {
       
        public override bool Call(Entity2 entity)
        {
            Debug.Log("Add Entity2");
            return true;
        }
    }

    public partial class UpdateSystem : SystemBase
    {
        public override bool Call(Entity2 e)
        {
            Debug.Log("Update Entity2");
            return true;
        }
    }

}
