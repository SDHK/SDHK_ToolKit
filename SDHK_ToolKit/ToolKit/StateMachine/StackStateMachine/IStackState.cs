using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;



namespace StateMachine
{

    /// <summary>
    /// 堆栈状态接口
    /// </summary>
    public interface IStackState
    {
        /// <summary>
        /// 栈状态机
        /// </summary>
        StackStateMachine stackStateMachine { get; set; }

        /// <summary>
        /// 栈顶提示
        /// </summary>
        void StackTopPrompt();

        /// <summary>
        /// 等待状态进入：可用于 入栈动画，调用 EnterDone() 让状态机继续运行
        /// </summary>
        void WaitStackStateEnter(Action EnterDone);

        /// <summary>
        /// 等待状态退出：可用于 出栈动画，调用 ExitDone() 让状态机继续运行
        /// </summary>
        void WaitStackStateExit(Action ExitDone);

        /// <summary>
        /// 堆栈状态进入
        /// </summary>
        void StackStateEnter();
        /// <summary>
        /// 堆栈状态退出
        /// </summary>
        void StackStateExit();
        /// <summary>
        /// 堆栈状态刷新
        /// </summary>
        void StackStateUpdate();
    }
}