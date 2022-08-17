using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{
    //需要全局暂停的考虑，所以只能用时间累计，不能用现实时间和时间差进行计算
    //时域应该与计时器无关，计时器的作用是回调

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
    /// 时钟周期
    /// </summary>
    public class TimerCycle : Entity
    {
        public float time;
        public Action callback;
    }



    public class Timer : Entity
    {
        public float time = 0;

        //Timer.Speed
        //Timer.isActive

        //Timer.Delay(1,a)
        //Timer.Loop(1,a)
        //Timer.Countdown(1,a) 



        //Timer.TimeOut()


    }
}
