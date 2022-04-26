using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    /// <summary>
    /// 焦点状态接口
    /// </summary>
    public interface IFocusState
    {
        /// <summary>
        /// 栈状态机
        /// </summary>
        FocusStateMachine focusStateMachine { get; set; }

        /// <summary>
        /// 等待焦点状态进入：可用于 进入动画，调用 EnterDone() 让状态机继续运行
        /// </summary>
        void WaitFocusStateEnter(Action EnterDone);

        /// <summary>
        /// 等待焦点状态退出：可用于 退出动画，调用 ExitDone() 让状态机继续运行
        /// </summary>
        void WaitFocusStateExit(Action ExitDone);

        /// <summary>
        /// 焦点状态进入
        /// </summary>
        void FocusStateEnter();
        /// <summary>
        /// 焦点状态退出
        /// </summary>
        void FocusStateExit();
        /// <summary>
        /// 焦点状态刷新
        /// </summary>
        void FocusStateUpdate();
    }
}