/****************************************

* 创 建 者：  闪电黑客
* 创建时间：  2022/5/5 10:37
* 描    述:

****************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK_Tool.ECS
{

    public abstract class Entity 
    {
        public int ID { get; set; }
        public List<Entity> children = new List<Entity>();

        public abstract void Run(ISystem s);
    }


    public class Entity2 : Entity
    {
        public override void Run(ISystem s) => s.Call(this);
    }

    public class Entity3 : Entity
    {
        public override void Run(ISystem s) => s.Call(this);
    }
}
