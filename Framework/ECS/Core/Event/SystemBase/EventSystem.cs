using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 事件系统
    /// </summary>
    public interface IEventSystem : ISystem
    {
        Delegate GetDeleate();
    }

    public abstract class EventActionSystem<T> : SystemBase<Action<T>, IEventSystem>, IEventSystem
    {
        public Delegate GetDeleate() => (Action<T>)Event;
        public abstract void Event(T arg1);
    }


    public abstract class EventFuncSystem<T> : SystemBase<Func<T>, IEventSystem>, IEventSystem
    {
        public Delegate GetDeleate() => (Func<T>)Event;
        public abstract T Event();
    }

  
}
