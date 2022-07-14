using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    public class SoloistFramework : SingletonBase<SoloistFramework>
    {
        Root root;
        UpdateSystemManager update;

        public override void OnInstance()
        {
            new Root();//实例化根节点

            EntityManager.GetInstance();//实体管理器单例,或许应该把根节点写在管理器里
            //update = UpdateSystemManager.GetInstance();

            Root.root.AddComponent<UpdateSystemManager>();//添加空节点测试
            Root.root.AddComponent<Node>();//添加空节点测试

        }

        public void Start()
        {

        }

        public void Update()
        {
            update.Update();
        }
        public void LateUpdate()
        {


        }
        public void FixedUpdate()
        {

        }

        public void OnDestroy()
        {

        }
    }
}
