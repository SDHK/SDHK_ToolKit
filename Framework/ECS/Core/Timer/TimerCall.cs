using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{
    //考虑全局暂停，所以只能用时间累加，不能用系统时间相减，进行时间差进行计算
    //时域+计时器，时域是float, 计时器的作用是回调事件

    public class TimerManager : Entity
    {
        public float time;

    }

    class TimerManagerAddSystem : AddSystem<TimerManager>
    {
        public override void OnAdd(TimerManager self)
        {

        }
    }

  



    /// <summary>
    /// 计时器：单次调用
    /// </summary>
    public class TimerCall : Entity
    {
        public float time = 0;
        public float timeOutTime = 0;
        public Action callback;
    }

    class TimerCallUpdateSystem : UpdateSystem<TimerCall>
    {
        public override void Update(TimerCall self)
        {
            self.time += Time.deltaTime * self.Domain.AddComponent<TimeDomain>().timeScale;
            if (self.time>= self.timeOutTime)
            {
                self.callback?.Invoke();
                self.RemoveSelf();
            }
        }
    }

    class TimerCallGetSystem : GetSystem<TimerCall>
    {
        public override void OnGet(TimerCall self)
        {
            self.time = 0;
        }
    }
}
