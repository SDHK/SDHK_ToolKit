﻿/****************************************

* 作者： 闪电黑客
* 日期： 2022/6/26 16:29

* 描述： 生命周期系统
* 
* 通过反射获取全局继承了ISystem的接口的实现类
* 

*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{

    /// <summary>
    /// 系统集合组
    /// </summary>
    public class SystemGroup : UnitPoolItem<SystemGroup>
    {
        public Dictionary<Type, List<ISystem>> systems = new Dictionary<Type, List<ISystem>>();
        public List<ISystem> GetSystems(Type type)
        {
            if (!systems.TryGetValue(type, out List<ISystem> Isystems))
            {
                systems.Add(type, new List<ISystem>());
            }
            return systems[type];
        }
    }

    /// <summary>
    /// 系统管理器
    /// </summary>
    public class SystemManager : SingletonBase<SystemManager>
    {
        //接口类型，（实例类型，实例方法）
        private Dictionary<Type, SystemGroup> typeSystems;
        private Dictionary<Type, SystemGroup> ISystems;
       

        public override void OnInstance()
        {
            typeSystems = ObjectPoolManager.Instance.Get<Dictionary<Type, SystemGroup>>();
        }

        /// <summary>
        /// 注册系统
        /// </summary>
        public void RegisterSystems<T>() where T : ISystem => RegisterSystems(typeof(T));

        public void RegisterSystems(Type Interface)
        {
            var types = FindTypesIsInterface(Interface);//查找继承了接口的类

            if (!typeSystems.ContainsKey(Interface))
            {
                typeSystems.Add(Interface, SystemGroup.GetObject());
            }

            foreach (var itemType in types)//遍历实现接口的类
            {
                ISystem system = Activator.CreateInstance(itemType, true) as ISystem;
                typeSystems[Interface].GetSystems(system.EntityType).Add(system);
            }
        }

        /// <summary>
        /// 获取系统组
        /// </summary>
        public SystemGroup GetSystemGroup<T>()
        {
            typeSystems.TryGetValue(typeof(T), out SystemGroup systemGroup);
            return systemGroup;
        }


        /// <summary>
        /// 获取单类型系统列表
        /// </summary>
        public List<T> GetSystems<T>(Type type)
             where T : ISystem
        {
            if (typeSystems.TryGetValue(typeof(T), out SystemGroup systemGroup))
            {
                if (systemGroup.systems.ContainsKey(type))
                {
                    return systemGroup.systems[type] as List<T>;
                }
            }
            return null;
        }

        public override void OnDispose()
        {

        }

        /// <summary>
        /// 查找继承了接口的类型
        /// </summary>
        private static Type[] FindTypesIsInterface(Type Interface)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(T => T.GetInterfaces().Contains(Interface))).ToArray();
        }
    }
}
