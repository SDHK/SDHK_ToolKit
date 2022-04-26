
/******************************

 * Author: 闪电黑客

 * 日期: 2022/01/27 16:44:11

 * 最后日期: 2022/01/27 16:44:25

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System;
using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;
using XLua;

namespace WindowUI
{
    public class LuaWindow : IWindow
    {
        public LuaTable table;

        public StackStateMachine stackStateMachine { get; set; }
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