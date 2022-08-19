
/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/18 9:35

* 描述： Soloist框架启动，脱离mono

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

        public override void OnInstance()
        {
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!");
        }

        public override void OnDispose()
        {
            instance = null;
        }

        public static string AllEntityString(Entity entity, string t)
        {
            string t1 = "\t" + t;
            string str = "";

            str += t1 + $"[{entity.id}] " + entity.ToString() + "\n";

            if (entity.children != null)
            {
                if (entity.children.Count > 0)
                {
                    str += t1 + "   Children:\n";
                    foreach (var item in entity.Children.Values)
                    {
                        str += AllEntityString(item, t1);
                    }
                }
            }

            if (entity.components != null)
            {
                if (entity.components.Count > 0)
                {
                    str += t1 + "   Components:\n";
                    foreach (var item in entity.Components.Values)
                    {
                        str += AllEntityString(item, t1);
                    }
                }
            }

            return str;
        }

    }
}
