
/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/18 9:35

* 描述： Mono的启动端，一切从这里开始

*/

using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{
    /// <summary>
    /// 主节点
    /// </summary>
    public class MainEntity : Entity { }



    public class Init : MonoBehaviour
    {
        public EntityManager root;

        UpdateManager update;
        LateUpdateManager lateUpdate;
        FixedUpdateManager fixedUpdate;

        private void Start()
        {
            root = new EntityManager();

            update = root.SetComponent<UpdateManager>();
            lateUpdate = root.SetComponent<LateUpdateManager>();
            fixedUpdate = root.SetComponent<FixedUpdateManager>();

            root.SetComponent<MainEntity>();
            
            Debug.Log(SoloistFramework.AllEntityString(root, "\t"));
           
        }

        private void Update()
        {
            update.Update();
        }

        private void LateUpdate()
        {
            lateUpdate.Update();
        }
        private void FixedUpdate()
        {
            fixedUpdate.Update();
        }

        private void OnDestroy()
        {
            root.Dispose();
            update = null;
            lateUpdate = null;
            fixedUpdate = null;
            Debug.Log(SoloistFramework.AllEntityString(root, "\t"));
        }
        private void OnApplicationQuit()
        {

        }


        public static string AllEntityString(Entity entity, string t)
        {
            string t1 = "\t" + t;
            string str = "";

            str += t1 + $"[{entity.id}] " + entity.ToString() + "\n";

            if (entity.Children.Count > 0)
            {
                str += t1 + "   Children:\n";
                foreach (var item in entity.Children.Values)
                {
                    str += AllEntityString(item, t1);
                }
            }
            if (entity.Components.Count > 0)
            {
                str += t1 + "   Components:\n";
                foreach (var item in entity.Components.Values)
                {
                    str += AllEntityString(item, t1);
                }
            }
            return str;
        }

    }
}
