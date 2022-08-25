
/****************************************

* 作者： 闪电黑客
* 日期： 2022/5/18 15:06

* 描述： 单位对象池
* 用于对继承了 IUnitPoolItem 接口的类进行回收
* 自己定义的类都继承 IUnitPoolItem ，方便统一管理

*/

using System;

namespace WorldTree
{


    class UnitPoolRemoveSystem : RemoveSystem<UnitPool>
    {
        public override void OnRemove(UnitPool self)
        {
            self.Dispose();//全部释放
        }
    }

    /// <summary>
    /// 单位对象池
    /// </summary>
    public class UnitPool : GenericPool<IUnitPoolItem>
    {
        /// <summary>
        /// 对象池构造
        /// </summary>
        public UnitPool(Type type) : base()
        {
            ObjectType = type;

            NewObject = ObjectNew;
            DestroyObject = ObjectDestroy;

            objectOnNew += ObjectOnNew;

            objectOnGet += ObjectOnGet;
            objectOnRecycle += ObjectOnRecycle;
        }

        /// <summary>
        /// 获取对象并转为指定类型
        /// </summary>
        public T Get<T>()
            where T :class,IUnitPoolItem
        {
            return Get() as T;
        }

        public override string ToString()
        {
            return $"[UnitPool<{ObjectType}>] :{Count} ";
        }

        private IUnitPoolItem ObjectNew(IPool pool)
        {
            IUnitPoolItem obj = Activator.CreateInstance(ObjectType, true) as IUnitPoolItem;
            obj.thisPool = pool;
            return obj;
        }
        private static void ObjectDestroy(IUnitPoolItem obj)
        {
            obj.Dispose();
        }

        private static void ObjectOnNew(IUnitPoolItem obj)
        {
            obj.OnNew();
        }
        private static void ObjectOnGet(IUnitPoolItem obj)
        {
            obj.IsRecycle = false;
            obj.OnGet();
        }
        private static void ObjectOnRecycle(IUnitPoolItem obj)
        {
            obj.IsRecycle = true;
            obj.OnRecycle();
        }


    }
}
