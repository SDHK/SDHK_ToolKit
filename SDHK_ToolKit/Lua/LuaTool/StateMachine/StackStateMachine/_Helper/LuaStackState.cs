

/******************************

 * Author: 闪电黑客

 * 日期: 2022/01/27 19:04:19

 * 最后日期: 2022/01/27 19:04:32

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace StateMachine
{
    public class LuaStackState : IStackState
    {
        public LuaTable table;

        public StackStateMachine stackStateMachine { get; set; }

        public Action<LuaTable, Action> waitStackStateEnter;
        public void WaitStackStateEnter(Action EnterDone)
        {
            waitStackStateEnter?.Invoke(table, EnterDone);
        }

        public Action<LuaTable, Action> waitStackStateExit;
        public void WaitStackStateExit(Action ExitDone)
        {
            waitStackStateExit?.Invoke(table, ExitDone);
        }

        public Action<LuaTable> stackStateEnter;
        public void StackStateEnter()
        {
            stackStateEnter?.Invoke(table);

        }
        public Action<LuaTable> stackStateExit;
        public void StackStateExit()
        {
            stackStateExit?.Invoke(table);
        }
        public Action<LuaTable> stackStateUpdate;
        public void StackStateUpdate()
        {
            stackStateUpdate?.Invoke(table);
        }

        public Action<LuaTable> stackTopPrompt;
        public void StackTopPrompt()
        {
            stackTopPrompt?.Invoke(table);
        }

    }
}