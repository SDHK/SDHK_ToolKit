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
        public virtual void Call(Entity entity) => Debug.Log("跳过 Entity");
    }


    public partial class AddSystem : SystemBase
    {
        public override void Call(Entity entity)
        {
            Debug.Log("Add Entity");
        }
    }

    public partial class UpdateSystem : SystemBase
    {
        public override void Call(Entity e)
        {
            Debug.Log("Update Entity");

        }
    }



    public abstract partial class SystemBase
    {
        public virtual void Call(Entity2 entity) => Debug.Log("跳过 Entity2");
    }

    public partial class AddSystem : SystemBase
    {

        public override void Call(Entity2 entity)
        {
            Debug.Log("Add Entity2");
        }
    }

    public partial class UpdateSystem : SystemBase
    {
        public override void Call(Entity2 e)
        {
            Debug.Log("Update Entity2");
        }
    }

}
