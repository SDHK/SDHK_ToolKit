


/******************************

 * 作者: 闪电黑客

 * 日期: 2021/02/23 13:44:46

 * 最后日期: 2021/02/23 13:45:02

 * 描述: 计时器

******************************/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
using ObjectFactory;

namespace TimeClock
{

    public abstract class ClockBase : IObjectPoolItem
    {
        public bool isRun;
        public float time = 0;
        public float countDown = -1;
        public Action callBack;

        public abstract ObjectPoolBase thisPool { get; set; }

        public abstract void Update();
        public abstract void ObjectOnDestroy();
        public abstract void ObjectOnGet();
        public abstract void ObjectOnNew();
        public abstract void ObjectOnRecycle();
        public abstract void ObjectRecycle();
    }


    public class Clock : ClockBase
    {
        public static ClassObjectPool<Clock> pool = new ClassObjectPool<Clock>()
        {
            objectDestoryClock = 60
        };

        public override ObjectPoolBase thisPool { get; set; }

        public static Clock Get()
        {
            return pool.Get();
        }


        public Clock Set(float time, Action callBack)
        {
            this.time = time;
            this.callBack = callBack;
            return this;
        }
        public Clock Run()
        {
            if (!isRun)
            {
                isRun = true;
                this.countDown = time;
            }
            return this;
        }

        public Clock Stop()
        {

            isRun = false;
            this.countDown = 0;
            return this;
        }
        public Clock ResetRun()
        {
            isRun = true;
            this.countDown = time;
            return this;
        }

        public override void Update()
        {
            if (isRun)
            {
                if (countDown > 0)
                {
                    countDown -= Time.deltaTime;
                    if (countDown <= 0)
                    {
                        isRun = false;
                        callBack?.Invoke();
                    }
                }
            }
        }

        public override void ObjectRecycle()
        {
            thisPool.Recycle(this);
        }

        public override void ObjectOnNew()
        {
        }

        public override void ObjectOnGet()
        {
            ClockManager.Instance().AddClock(this);
        }

        public override void ObjectOnRecycle()
        {
            ClockManager.Instance().RemoveClock(this);

            isRun = false;
            time = 0;
            countDown = -1;
            callBack = null;
        }

        public override void ObjectOnDestroy()
        {
        }
    }
}