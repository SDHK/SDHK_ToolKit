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
    public interface ICallSystem : ISystem
    {
        Delegate GetDeleate();
    }


    public abstract class CallSystem<T> :SystemBase<Action<T>,ICallSystem>, ICallSystem
    {
        
        public Delegate GetDeleate() =>(Action<T>)Event;
        public abstract void Event(T self);
    }

    //需要将EventDelegate改为Entity版本
    public class EventManager:Entity
    {
        public SystemGroup systems;

        public UnitDictionary<object, EventDelegate> events;

    }

    class EventManagerAddSystem : AddSystem<EventManager>
    {
        public override void OnAdd(EventManager self)
        {
            //需要进行遍历
            self.systems = self.Root.systemManager.GetSystemGroup<ICallSystem>();

            //方法注册为事件，需要检测特性进行分类
            self.events[0].Add( (self.systems[typeof(Type)][0] as ICallSystem).GetDeleate);


            //self.Root.EventSystem.Get("测试").CallAction("参数",1,1.2f);

            //self.EventGet("测试").CallAction("参数",1,1.2f);

            //"测试".CallAction("参数",1,1.2f);

        }
    }
}
