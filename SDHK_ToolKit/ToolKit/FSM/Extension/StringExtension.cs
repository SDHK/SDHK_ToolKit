/******************************

 * 作者: 闪电黑客

 * 日期: 2021/07/03 7:17:50

 * 最后日期: 2021/07/03 7:17:50

 * 描述: 
    有限状态机管理器:字符串扩展

    简化书写

******************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FSM
{
    public static class StringExtension
    {
        /// <summary>
        /// 获取一个有限状态机，为其注册状态
        /// </summary>
        /// <param name="machineName">状态机名字</param>
        /// <param name="key">状态键值</param>
        /// <param name="state">状态</param>
        public static FiniteStateMachine FSM_AddState(this string machineName, string key, IFiniteState state)
        {
            return FSM_Manager.GetMachine(machineName).Add(key, state);
        }

        /// <summary>
        /// 对一个有限状态机，移除一个状态
        /// </summary>
        /// <param name="machineName">状态机名字</param>
        /// <param name="key">状态键值</param>
        public static FiniteStateMachine FSM_RemoveState(this string machineName, string key)
        {
            return FSM_Manager.GetMachine(machineName).Remove(key);
        }

        /// <summary>
        /// 移除一个有限状态机
        /// </summary>
        /// <param name="machineName">状态机名字</param>
        public static FiniteStateMachine FSM_RemoveState(this string machineName)
        {
            return FSM_Manager.RemoveMachine(machineName);
        }

        /// <summary>
        /// 对一个有限状态机：切换启动当前状态
        /// </summary>
        /// <param name="machineName">状态机名字</param>
        /// <param name="key">状态键值</param>
        public static FiniteStateMachine FSM_SwitchState(this string machineName, string key)
        {
            return FSM_Manager.GetMachine(machineName).SwitchState(key);
        }



    }
}