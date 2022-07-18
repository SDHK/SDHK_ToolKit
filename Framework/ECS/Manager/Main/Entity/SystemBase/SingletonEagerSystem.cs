
/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/18 9:35

* 描述： ECS模式的单例
* 实现思路为给根节点挂组件
* 

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 实体饿汉单例系统接口
    /// </summary>
    public interface ISingletonEagerSystem : ISystem
    {
        void Instance();
    }

    /// <summary>
    /// 实体饿汉单例系统：生成组件挂在根节点下
    /// </summary>
    public abstract class SingletonEagerSystem<T> : SystemBase<T>, ISingletonEagerSystem
        where T :class ,IEntity
    {
        public void Instance()
        {
            EntityRoot.Root.GetComponent<T>();
        }
    }
}
