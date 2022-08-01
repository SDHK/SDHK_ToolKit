using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{

    /// <summary>
    /// 新建事件系统接口
    /// </summary>
    public interface INewSystem : ISystem
    {
        public void New(Entity self);
    }

    /// <summary>
    /// 新建事件系统
    /// </summary>
    public abstract class NewSystem<T> : SystemBase<T, INewSystem>, INewSystem
        where T :  Entity
    {
        public void New(Entity self) => OnNew(self as T);

        public abstract void OnNew(T self);
    }

    /// <summary>
    /// 获取事件系统接口
    /// </summary>
    public interface IGetSystem : ISystem
    {
        public void Get(Entity self);

    }

    /// <summary>
    /// 获取事件系统
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GetSystem<T> : SystemBase<T, IGetSystem>, IGetSystem
        where T : Entity
    {
        public void Get(Entity self) => OnGet(self as T);

        public abstract void OnGet(T self);
    }

    /// <summary>
    /// 回收事件系统接口
    /// </summary>
    public interface IRecycleSystem : ISystem
    {
        public void Recycle(Entity self);
    }

    /// <summary>
    /// 回收事件系统
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RecycleSystem<T> : SystemBase<T, IRecycleSystem>, IRecycleSystem
        where T :  Entity
    {
        public void Recycle(Entity self) => OnRecycle(self as T);

        public abstract void OnRecycle(T self);
    }

    /// <summary>
    /// 释放事件系统接口
    /// </summary>
    public interface IDestroySystem : ISystem
    {
        public void Destroy(Entity self);
    }
    /// <summary>
    /// 释放事件系统
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DestroySystem<T> : SystemBase<T, IDestroySystem>, IDestroySystem
        where T : Entity
    {
        public void Destroy(Entity self) => OnDestroy(self as T);

        public abstract void OnDestroy(T self);

    }
}
