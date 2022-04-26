using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectFactory;

namespace StateMachine
{

    /// <summary>
    /// 焦点状态机
    /// </summary>
    public class FocusStateMachine : IObjectPoolItem
    {

        public ObjectPoolBase thisPool { get; set; }

        public static ClassObjectPool<FocusStateMachine> pool = new ClassObjectPool<FocusStateMachine>()
        {
            objectDestoryClock = 600
        };

        /// <summary>
        /// 从对象池获取状态机
        /// </summary>
        public static FocusStateMachine Get()
        {
            return pool.Get();
        }
        /// <summary>
        /// 当前状态
        /// </summary>
        public IFocusState State = null;
        private IFocusState nextState = null;

        public bool isRun = true;
        private bool isEnterDone = true;
        private bool isExitDone = true;

        /// <summary>
        /// 状态完成
        /// </summary>
        public bool IsDone => isEnterDone && isExitDone;
        /// <summary>
        /// 状态进入完成
        /// </summary>
        public bool IsEnterDone => isEnterDone;
        /// <summary>
        /// 状态退出完成
        /// </summary>
        public bool IsExitDone => isExitDone;



        /// <summary>
        /// 请求替换焦点
        /// </summary>
        public bool Set(IFocusState setState)
        {
            bool bit = IsDone && setState != State;
            if (bit)
            {
                nextState = setState;
                isExitDone = false;
                isEnterDone = false;

                if (State == null)
                {
                    isExitDone = true;
                    nextState.focusStateMachine = this;
                    nextState.WaitFocusStateEnter(EnterDone);
                }
                else
                {
                    State.WaitFocusStateExit(ExitDone);
                }
            }

            return bit;
        }

        //离开完成回调
        private void ExitDone()
        {
            isExitDone = true;
            State?.FocusStateExit();

            if (nextState == null)
            {
                isEnterDone = true;
                State = null;
            }
            else
            {
                nextState.WaitFocusStateEnter(EnterDone);
            }
        }

        //进入完成回调
        private void EnterDone()
        {
            isEnterDone = true;
            nextState?.FocusStateEnter();
            State = nextState;
            nextState = null;
        }

        /// <summary>
        /// 强制清空
        /// </summary>
        public void Clear()
        {
            State?.FocusStateExit();
            nextState?.FocusStateExit();
            State = null;
            nextState = null;
            isEnterDone = true;
            isExitDone = true;
        }

        /// <summary>
        /// 状态机刷新
        /// </summary>
        public void Update()
        {
            if (isRun) State?.FocusStateUpdate();
        }

        public void ObjectRecycle()
        {
            thisPool.Recycle(this);
        }

        public void ObjectOnNew()
        {
        }

        public void ObjectOnGet()
        {
            FocusStateMachineManager.Instance().Add(this);
        }

        public void ObjectOnRecycle()
        {
            Clear();
            FocusStateMachineManager.Instance().Remove(this);
        }
        public void ObjectOnDestroy()
        {
        }
    }
}
