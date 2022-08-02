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
    public class SystemManager : Entity, IUnit
    {
        //接口类型，（实例类型，实例方法）
        private UnitDictionary<Type, SystemGroup> InterfaceSystems;

        public SystemManager()
        {
            id = IdManager.GetID;
            Type = GetType();
            InterfaceSystems = UnitDictionary<Type, SystemGroup>.GetObject();
            Initialize();
        }

        private void Initialize()
        {
            var types = FindTypesIsInterface(typeof(ISystem));
            foreach (var itemType in types)//遍历实现接口的类
            {
                //实例化系统类
                ISystem system = Activator.CreateInstance(itemType, true) as ISystem;

                Type SystemType = system.SystemType;
                Type EntityType = system.EntityType;
                if (!InterfaceSystems.TryGetValue(SystemType, out SystemGroup systemGroup))
                {
                    systemGroup = SystemGroup.GetObject();
                    InterfaceSystems.Add(SystemType, systemGroup);
                }

                systemGroup.GetSystems(EntityType).Add(system);
            }
        }

        /// <summary>
        /// 获取系统组
        /// </summary>
        public SystemGroup GetSystemGroup<T>() where T : ISystem => GetSystemGroup(typeof(T));

        /// <summary>
        /// 获取系统组
        /// </summary>
        public SystemGroup GetSystemGroup(Type Interface)
        {
            if (!InterfaceSystems.TryGetValue(Interface, out SystemGroup systemGroup))
            {
                systemGroup = SystemGroup.GetObject();
            }
            return systemGroup;
        }


        /// <summary>
        /// 获取单类型系统列表
        /// </summary>
        public UnitList<ISystem> GetSystems<T>(Type type)
        {
            if (InterfaceSystems.TryGetValue(typeof(T), out SystemGroup systemGroup))
            {
                if (systemGroup.ContainsKey(type))
                {
                    return systemGroup[type];
                }
            }
            return null;
        }

        /// <summary>
        /// 释放自己
        /// </summary>
        public void Dispose()
        {
            if (isDisposed) return;
            OnDispose();
            isDisposed = true;
        }

        public void OnDispose()
        {
            InterfaceSystems.Clear();
            InterfaceSystems.Recycle();
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
