
/******************************

 * Author: 闪电黑客

 * 日期: 2022/01/27 18:55:33

 * 最后日期: 2022/01/27 18:55:47

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
    public class LuaFocusState : IFocusState
    {
        public LuaTable table;
        public FocusStateMachine focusStateMachine { get; set; }

        public Action<LuaTable, Action> waitFocusStateEnter;
        public void WaitFocusStateEnter(Action EnterDone)
        {
            waitFocusStateEnter?.Invoke(table, EnterDone);
        }

        public Action<LuaTable, Action> waitFocusStateExit;
        public void WaitFocusStateExit(Action ExitDone)
        {
            waitFocusStateExit?.Invoke(table, ExitDone);
        }

        public Action<LuaTable> focusStateEnter;
        public void FocusStateEnter()
        {
            focusStateEnter?.Invoke(table);
        }
        public Action<LuaTable> focusStateExit;
        public void FocusStateExit()
        {
            focusStateExit?.Invoke(table);
        }

        public Action<LuaTable> focusStateUpdate;
        public void FocusStateUpdate()
        {
            focusStateUpdate?.Invoke(table);
        }

    }
}