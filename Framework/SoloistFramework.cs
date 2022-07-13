using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    public class SoloistFramework:SingletonBase<SoloistFramework>
    {
        Root root;
        public override void OnInstance()
        {
            EntityManager.GetInstance();//实体管理器单例,或许应该把根节点写在管理器里
            root = new Root();
        }

        public void Start()
        {
            
        }

        public void Update()
        {
        
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
