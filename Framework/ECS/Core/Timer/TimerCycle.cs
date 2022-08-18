using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{
    /// <summary>
    /// 计时器：循环调用
    /// </summary>
    public class TimerCycle : Entity
    {
        public float time = 0;
        public float timeOutTime = 0;
        public Action callback;
    }

    class TimerCycleUpdateSystem : UpdateSystem<TimerCycle>
    {
        public override void Update(TimerCycle self)
        {
            self.time += Time.deltaTime * self.Domain.AddComponent<TimeDomain>().timeScale;
            if (self.time >= self.timeOutTime)
            {
                self.callback?.Invoke();
            }
        }
    }

    class TimerCycleGetSystem : GetSystem<TimerCycle>
    {
        public override void OnGet(TimerCycle self)
        {
            self.time = 0;
        }
    }
}
