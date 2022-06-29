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

namespace SDHK
{
    public interface ISystem
    {
        Type SystemType { get; }
        Type EntityType { get; }

    }





    public class System1 : UpdateSystem<Entity2>
    {
        public override void Update(Entity2 entity)
        {

        }
    }

}
