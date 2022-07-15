using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{

    public class Node : Entity<Node>
    {

    }
    public class NodeStartSystem : StartSystem<Node>
    {
        public override void Start(Node entity)
        {
            Debug.Log("Start!!!");
        }
    }
    public class NodeUpdateSystem : UpdateSystem<Node>
    {
        public override void Update(Node entity)
        {
            Debug.Log("Update!!!");
        }
    }


    public class SoloistFramework : SingletonBase<SoloistFramework>
    {
        
        UpdateManager update;

        public void Start()
        {
          
            EntityManager.GetInstance();//实体管理器单例,或许应该把根节点写在管理器里
            Root.root.AddComponent<Node>();//添加空节点测试
            update = UpdateManager.GetInstance();//Update管理器
            
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
