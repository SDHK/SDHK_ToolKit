/****************************************

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
using UnityEngine;

namespace SDHK
{

    /// <summary>
    /// 系统管理器
    /// </summary>
    public class SystemManager : SingletonBase<SystemManager>
    {
        //接口类型，（实例类型，实例方法）
        private UnitDictionary<Type, SystemGroup> InterfaceSystems;
        private UnitDictionary<Type, SystemGroup> typeSystems;

        public override void OnInstance()
        {
            InterfaceSystems = UnitDictionary<Type, SystemGroup>.GetObject();
            //typeSystems = UnitDictionary<Type, SystemGroup>.GetObject();
        }

        /// <summary>
        /// 注册系统
        /// </summary>
        public SystemGroup RegisterSystems<T>() where T : ISystem => RegisterSystems(typeof(T));

        public SystemGroup RegisterSystems(Type Interface)
        {
           
            //查找继承了接口的类
            var types = FindTypesIsInterface(Interface);

            foreach (var itemType in types)//遍历实现接口的类
            {
                ISystem system = Activator.CreateInstance(itemType, true) as ISystem;
                if (!InterfaceSystems.ContainsKey(Interface))
                {
                    InterfaceSystems.Add(Interface, SystemGroup.GetObject());
                }
                InterfaceSystems[Interface].GetSystems<ISystem>(system.EntityType).Add(system);

                //if (!typeSystems.ContainsKey(system.EntityType))
                //{
                //    typeSystems.Add(system.EntityType, SystemGroup.GetObject());
                //}
                //typeSystems[system.EntityType].GetSystems<ISystem>(Interface).Add(system);
            }
            if (InterfaceSystems.ContainsKey(Interface))
            {
                return InterfaceSystems[Interface];
            }
            else
            {
               
                return null;
            }
        }

        /// <summary>
        /// 获取系统组
        /// </summary>
        public SystemGroup GetSystemGroup<T>()
        {
            InterfaceSystems.TryGetValue(typeof(T), out SystemGroup systemGroup);
            return systemGroup;
        }


        /// <summary>
        /// 获取单类型系统列表
        /// </summary>
        public UnitList<T> GetSystems<T>(Type type)
             where T : ISystem
        {
            if (InterfaceSystems.TryGetValue(typeof(T), out SystemGroup systemGroup))
            {
                if (systemGroup.ContainsKey(type))
                {
                    return systemGroup[type] as UnitList<T>;
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
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(T => T.GetInterfaces().Contains(Interface) && !T.IsAbstract)).ToArray();
        }
    }
}
