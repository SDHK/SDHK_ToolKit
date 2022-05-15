using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK_Tool.ECS
{
    //       AttributeTargets.All(说明这个特性可以标记在什么元素上，类、字段、属性、方法、返回值等元素，ALL代表所有)
    //       AllowMultiple(说明这个特性在同一个元素上使用的次数，默认一个元素只能标记一种特性，但可以多种特性并存)
    //       Inherited(说明这个特性是否可以继承)
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public  class EcsSystem : Attribute 
    {
        public int Layer { get;private set; }
        public EcsSystem(int layer = 0)
        {
            Layer = layer;
        }
    }

   
}
