
/****************************************

* 作者： 闪电黑客
* 日期： 2022/5/18 15:06

* 描述： 单位对象池
* 用于对继承了 IUnitPoolItem 接口的类进行回收
* 自己定义的类都继承 IUnitPoolItem ，方便统一管理

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 单位对象池
    /// </summary>
    public class UnitPool<T> : GenericPool<T>
      where T : class, IUnitPoolItem
    {
        /// <summary>
        /// 对象池构造
        /// </summary>
        public UnitPool()
        {
            id = IdManager.GetID;
            Type = GetType();
            ObjectType = typeof(T);

            NewObject = ObjectNew;
            DestroyObject = ObjectDestroy;

            objectOnNew += ObjectOnNew;

            objectOnGet += ObjectOnGet;
            objectOnRecycle += ObjectOnRecycle;
        }

        public override string ToString()
        {
            return "[UnitPool<" + ObjectType + ">] ";
        }

        private T ObjectNew(IPool pool)
        {
            T obj = Activator.CreateInstance(ObjectType, true) as T;
            obj.thisPool = pool;
            return obj;
        }
        private static void ObjectDestroy(T obj)
        {
            obj.Dispose();
        }

        private static void ObjectOnNew(T obj)
        {
            obj.OnNew();
        }
        private static void ObjectOnGet(T obj)
        {
            obj.IsRecycle = false;
            obj.OnGet();
        }
        private static void ObjectOnRecycle(T obj)
        {
            obj.IsRecycle = true;
            obj.OnRecycle();
        }


    }
}
