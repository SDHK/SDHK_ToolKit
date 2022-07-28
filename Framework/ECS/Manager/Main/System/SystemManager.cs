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
    /// 执行系统
    /// </summary>
    public interface ICallSystem : ISystem//或许可以成为单例组件
    {
        void Call(IEntity self);
    }



    /// <summary>
    /// 系统管理器
    /// </summary>
    public class SystemManager : Entity, IUnit
    {
        //接口类型，（实例类型，实例方法）
        private UnitDictionary<Type, SystemGroup> InterfaceSystems;

        public SystemManager()
        {
            Id = IdManager.GetID;
            Type = GetType();
            InterfaceSystems = UnitDictionary<Type, SystemGroup>.GetObject();
        }

        /// <summary>
        /// 注册系统
        /// </summary>
        public SystemGroup RegisterSystems<T>() where T : ISystem => RegisterSystems(typeof(T));

        /// <summary>
        /// 注册系统
        /// </summary>
        public SystemGroup RegisterSystems(Type Interface)
        {
            if (!InterfaceSystems.TryGetValue(Interface, out SystemGroup systemGroup))
            {
                systemGroup = SystemGroup.GetObject();
                InterfaceSystems.Add(Interface, systemGroup);

                //查找继承了接口的类
                var types = FindTypesIsInterface(Interface);

                foreach (var itemType in types)//遍历实现接口的类
                {
                    //实例化系统类
                    ISystem system = Activator.CreateInstance(itemType, true) as ISystem;
                    systemGroup.GetSystems(system.EntityType).Add(system);
                }
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
            if (IsDisposed) return;
            OnDispose();
            IsDisposed = true;
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


        public void CallSystems<T>(IEntity entity)
               where T : ISystem
        {

            if (InterfaceSystems.TryGetValue(typeof(T), out SystemGroup systemGroup))
            {

                foreach (ICallSystem system in systemGroup.GetSystems(entity.Type))
                {
                    system.Call(entity);
                }
            }
        }
    }
}
