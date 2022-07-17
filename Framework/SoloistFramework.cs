using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{



    public class SoloistFramework : SingletonBase<SoloistFramework>
    {

        UpdateManager update;
        LateUpdateManager lateUpdate;
        FixedUpdateManager fixedUpdate;


        public void Start()
        {
            Debug.Log("启动！！！");

            EntityManager.GetInstance();//实体管理器单例,或许应该把根节点写在管理器里

            update = UpdateManager.GetInstance();//Update管理器
            lateUpdate = LateUpdateManager.GetInstance();
            fixedUpdate = FixedUpdateManager.GetInstance();

            EntityRoot.Root
                .GetComponent<Node>()
                .GetComponent<Node>()
                .GetComponent<Node>()
                .GetComponent<Node2>()
                .GetComponent<Node2>()
                .GetComponent<Node3>()


                ;//添加空节点测试


            Debug.Log(Print1(EntityRoot.Root, "\t"));
        }


        public string Print1(IEntity entity, string t)
        {
            string t1 = "\t" + t;
            string str = "";

            str += t1 + $"[{entity.Id}] " + entity.ToString() + "\n";

            if (entity.Children.Count > 0)
            {
                str += t1 + "   Children:\n";
                foreach (var item in entity.Children.Values)
                {
                    str += Print1(item, t1);
                }
            }
            if (entity.Components.Count > 0)
            {
                str += t1 + "   Components:\n";
                foreach (var item in entity.Components.Values)
                {
                    str += Print1(item, t1);
                }
            }
            return str;
        }

        public void Update()
        {
            update?.Update();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                EntityRoot.Root.RemoveAllChildren();
                EntityRoot.Root.RemoveAllComponent();

                EntityManager.Instance.Dispose();
                update = null;

                Debug.Log("回收全部！！！");
                Debug.Log(Print1(EntityRoot.Root, "\t"));
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                Start();
            }
        }
        public void LateUpdate()
        {
            lateUpdate?.Update();
        }

        public void FixedUpdate()
        {
            fixedUpdate?.Update();
        }

        public void OnDestroy()
        {

        }
    }
}
