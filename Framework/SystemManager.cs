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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{

    /// <summary>
    /// 系统集合组
    /// </summary>
    public class SystemGroup
    {
        public Dictionary<Type, List<ISystem>> systems = new Dictionary<Type, List<ISystem>>();
    }

    /// <summary>
    /// 系统管理器
    /// </summary>
    public class SystemManager : SingletonBase<SystemManager>
    {
        private Dictionary<Type, SystemGroup> typeSystems = new Dictionary<Type, SystemGroup>();


        /// <summary>
        /// 注册系统
        /// </summary>
        public void RegisterSystems<T>() where T : ISystem => RegisterSystems(typeof(T));

        public void RegisterSystems(Type Interface)
        {
            var types = FindTypesIsInterface(Interface);//查找继承了接口的类
            if (!typeSystems.ContainsKey(Interface))
            {
                foreach (var itemType in types)//遍历实现接口的类
                {
                    if (typeSystems[Interface].systems.ContainsKey(itemType))//有问题
                    {
                        //接口，接收类的类型（壳），泛型参数类型（欠缺）
                        typeSystems[Interface].systems[itemType].Add(Activator.CreateInstance(itemType, true) as ISystem);
                    }
                }
            }
        }

        /// <summary>
        /// 获取系统列表
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

        /// <summary>
        /// 查找继承了接口的类型
        /// </summary>
        private static Type[] FindTypesIsInterface(Type Interface)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(T => T.GetInterfaces().Contains(Interface))).ToArray();
        }
    }
}
