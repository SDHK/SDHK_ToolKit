/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/17 17:23

* 描述： 实体对象池。
* 掌管实体的生命周期
*

*/
using System;

namespace SDHK
{

    class EntityPoolAddSystem : AddSystem<EntityPool>
    {
        public override void OnAdd(EntityPool self)
        {
            //注册生命周期系统
            self.newSystem = self.RootGetSystems<INewSystem>(self.ObjectType);
            self.getSystem = self.RootGetSystems<IGetSystem>(self.ObjectType);
            self.recycleSystem = self.RootGetSystems<IRecycleSystem>(self.ObjectType);
            self.destroySystem = self.RootGetSystems<IDestroySystem>(self.ObjectType);
        }
    }

    class EntityPoolRemoveSystem : RemoveSystem<EntityPool>
    {
        public override void OnRemove(EntityPool self)
        {
            self.Dispose();//全部释放
        }
    }

    /// <summary>
    /// 实体类型对象池
    /// </summary>
    public class EntityPool : GenericPool<Entity>
    {
        public UnitList<ISystem> newSystem;
        public UnitList<ISystem> getSystem;
        public UnitList<ISystem> recycleSystem;
        public UnitList<ISystem> destroySystem;

        public EntityPool(Type type) : base()
        {

            ObjectType = type;

            NewObject = ObjectNew;
            DestroyObject = ObjectDestroy;

            objectOnNew = ObjectOnNew;
            objectOnGet = ObjectOnGet;
            objectOnRecycle = ObjectOnRecycle;
            objectOnDestroy = ObjectOnDestroy;
        }

        /// <summary>
        /// 获取对象并转为指定类型
        /// </summary>
        public T Get<T>()
            where T : Entity
        {
            return Get() as T;
        }

        public override string ToString()
        {
            return $"[EntityPool<{ObjectType}>] : {Count} ";
        }

        private Entity ObjectNew(IPool pool)
        {
            Entity entity = Activator.CreateInstance(ObjectType, true) as Entity;
            entity.id = Root.IdManager.GetId();
            entity.Root = Root;

            return entity;
        }

        private void ObjectDestroy(Entity obj)
        {
            obj.Dispose();
        }

        private void ObjectOnNew(Entity obj)
        {
            if (newSystem != null)
            {
                foreach (INewSystem item in newSystem)
                {
                    item.New(obj);
                }
            }
        }

        private void ObjectOnGet(Entity obj)
        {
            obj.isRecycle = false;
            if (getSystem != null)
            {
                foreach (IGetSystem item in getSystem)
                {
                    item.Get(obj);
                }
            }

        }

        private void ObjectOnRecycle(Entity obj)
        {
            obj.isRecycle = true;

            if (recycleSystem != null)
            {
                foreach (IRecycleSystem item in recycleSystem)
                {
                    item.Recycle(obj);
                }
            }
        }

        private void ObjectOnDestroy(Entity obj)
        {
            if (destroySystem != null)
            {
                foreach (IDestroySystem item in destroySystem)
                {
                    item.Destroy(obj);
                }
            }
        }

    }
}
