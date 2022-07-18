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
    public class SystemManager : SingletonBase<SystemManager>
    {
        //接口类型，（实例类型，实例方法）
        private UnitDictionary<Type, SystemGroup> InterfaceSystems;

        public override void OnInstance()
        {
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
            //查找继承了接口的类
            var types = FindTypesIsInterface(Interface);

            foreach (var itemType in types)//遍历实现接口的类
            {
                ISystem system = Activator.CreateInstance(itemType, true) as ISystem;
                if (!InterfaceSystems.ContainsKey(Interface))
                {
                    InterfaceSystems.Add(Interface, SystemGroup.GetObject());
                }
                UnitList<ISystem> systems = InterfaceSystems[Interface].GetSystems(system.EntityType);

                if (!systems.Any((t) => t.GetType() == system.GetType()))
                {
                    systems.Add(system);
                }
            }
            if (InterfaceSystems.ContainsKey(Interface))
            {
                return InterfaceSystems[Interface];
            }
            else
            {
                return SystemGroup.GetObject();
            }
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

        public override void OnDispose()
        {
            InterfaceSystems.Clear();
            InterfaceSystems.Recycle();
            instance = null;
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
