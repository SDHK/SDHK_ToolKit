
/****************************************

* 作者： 闪电黑客
* 日期： 2022/5/18 15:06

* 描述： 单位对象池

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
            ObjectType = typeof(T);

            NewObject = ObjectNew;

            objectOnNew += ObjectOnNew;
            objectOnDestroy += ObjectOnDestroy;
            objectOnGet += ObjectOnGet;
            objectOnRecycle += ObjectOnRecycle;
        }

        public override string ToString()
        {
            return "[UnitPool<" + ObjectType.Name + ">] ";
        }

        private T ObjectNew(PoolBase pool)
        {
            T obj = Activator.CreateInstance(ObjectType, true) as T;
            obj.thisPool = pool;
            return obj;
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
        private static void ObjectOnDestroy(T obj)
        {
            obj.Dispose();
        }

    }
}
