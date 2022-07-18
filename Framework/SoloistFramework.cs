
/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/18 9:35

* 描述： 框架启动，脱离mono

*/

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

        public override void OnInstance()
        {
            EntityManager.GetInstance();//实体管理器单例

            update = UpdateManager.GetInstance();//Update管理器
            lateUpdate = LateUpdateManager.GetInstance();
            fixedUpdate = FixedUpdateManager.GetInstance();

        }

        public void Update()
        {
            update.Update();
        }
        public void LateUpdate()
        {
            lateUpdate.Update();
        }

        public void FixedUpdate()
        {
            fixedUpdate.Update();
        }

        public override void OnDispose()
        {
            EntityManager.Instance.Dispose();
            update = null;
            lateUpdate = null;
            fixedUpdate = null;
            instance = null;
        }


        public string AllEntityString(IEntity entity, string t)
        {
            string t1 = "\t" + t;
            string str = "";

            str += t1 + $"[{entity.Id}] " + entity.ToString() + "\n";

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
