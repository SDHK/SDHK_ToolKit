/****************************************

* 创 建 者：  闪电黑客
* 创建时间：  2022/5/6 21:27
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
        public int Layer = 0;
        public Dictionary<long, Entity> entities = new Dictionary<long, Entity>();  //实体
        public Dictionary<Type, Entity> components = new Dictionary<Type, Entity>(); //组件

        public abstract void Run(SystemBase s);

       
    }








    

    public class Entity2 : Entity
    {
        public override void Run(SystemBase s) => s.Call(this);
    }

    public class Entity3 : Entity
    {
        public override void Run(SystemBase s) => s.Call(this);
    }
}
