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

namespace SDHK
{

    /// <summary>
    /// 系统集合组
    /// </summary>
    public class SystemGroup : Dictionary<Type, List<ISystem>>, IUnitPoolItem
    {
        public PoolBase thisPool { get; set; }
        public bool IsRecycle { get; set; }
        public bool IsDisposed { get; set; }

        /// <summary>
        /// 单位对象池：获取对象
        /// </summary>
        public static SystemGroup GetObject()
        {
            return UnitPoolManager.Instance.Get<SystemGroup>();
        }
        /// <summary>
        /// 获取系统类列表
        /// </summary>
        public List<T> GetSystems<T>(Type type)
            where T : ISystem
        {
            List<ISystem> Isystems;
            if (!TryGetValue(type, out Isystems))
            {
                Isystems = new List<ISystem>();
                Add(type, Isystems);
            }
            return Isystems as List<T>;
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            OnDispose();
            IsDisposed = true;
        }

        public void OnDispose()
        {
        }

        public void OnGet()
        {
        }

        public void OnNew()
        {
        }

        public void OnRecycle()
        {
            foreach (var systemList in this)
            {
                systemList.Value.Clear();
                ObjectPoolManager.Instance.Recycle(systemList.Value);
            }
            Clear();
        }

        public  void Recycle()
        {
            if (thisPool != null)
            {
                if (!thisPool.IsDisposed)
                {
                    if (!IsRecycle)
                    {
                        thisPool.Recycle(this);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 系统管理器
    /// </summary>
    public class SystemManager : SingletonBase<SystemManager>
    {
        //接口类型，（实例类型，实例方法）
        private Dictionary<Type, SystemGroup> InterfaceSystems;
        private Dictionary<Type, SystemGroup> typeSystems;

        public override void OnInstance()
        {
            InterfaceSystems = ObjectPoolManager.Instance.Get<Dictionary<Type, SystemGroup>>();
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

                if (!typeSystems.ContainsKey(system.EntityType))
                {
                    typeSystems.Add(system.EntityType, SystemGroup.GetObject());
                }
                typeSystems[system.EntityType].GetSystems<ISystem>(Interface).Add(system);
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
        public List<T> GetSystems<T>(Type type)
             where T : ISystem
        {
            if (InterfaceSystems.TryGetValue(typeof(T), out SystemGroup systemGroup))
            {
                if (systemGroup.ContainsKey(type))
                {
                    return systemGroup[type] as List<T>;
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
