/****************************************

* 作者： 闪电黑客
* 日期： 2022/8/26 0:23

* 描述： 主区域组件
* 
* 在 untiy运行时，启动后挂载
* 
* 用于启动unity环境的功能组件

*/
namespace WorldTree
{
    /// <summary>
    /// 主区域
    /// </summary>
    public class MainDomain : Entity { }


    class _Main : AddSystem<MainDomain>
    {
        public override void OnAdd(MainDomain self)
        {
            self.Domain = self;
            World.Log("Main区域启动！");
        }
    }

}
