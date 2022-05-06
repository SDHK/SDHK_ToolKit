﻿/****************************************

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

}
