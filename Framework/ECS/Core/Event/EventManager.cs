﻿
/****************************************

* 作者： 闪电黑客
* 日期： 2022/8/2 15:28

* 描述： 事件管理器

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{

    /// <summary>
    /// 事件管理器
    /// </summary>
    public class EventManager : Entity
    {
        public SystemGroup systemGroup;
    }

    class EventManagerAddSystem : AddSystem<EventManager>
    {
        public override void OnAdd(EventManager self)
        {
            //进行遍历分类
            self.systemGroup = self.RootGetSystemGroup<IEventSystem>();

            foreach (var systems in self.systemGroup.Values)
            {
                foreach (IEventSystem system in systems)
                {
                    //反射属性获取键值
                    Type key = typeof(EventDelegate);
                    object[] attributes = system.GetType().GetCustomAttributes(typeof(EventKeyAttribute), true);
                    if (attributes.Length != 0)
                    {
                        key = (attributes[0] as EventKeyAttribute)?.key;
                    }
                    //分组注册事件
                    self.AddComponent(key).To<EventDelegate>().AddDelegate(system.GetDeleate());
                }
            }
        }
    }
}
