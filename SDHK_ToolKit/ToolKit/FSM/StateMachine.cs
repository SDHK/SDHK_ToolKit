/******************************

 * 作者: 闪电黑客

 * 日期: 2021/07/03 7:17:50

 * 最后日期: 2021/07/03 7:17:50

 * 描述: 
    有限状态机

    主要功能：
        有过渡间隔的功能
        通过修改键值改变当前状态

******************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FSM
{

    /// <summary>
    /// 有限状态机
    /// </summary>
    public class FiniteStateMachine
    {
        /// <summary>
        /// 状态字典集合
        /// </summary>
        public Dictionary<string, IFiniteState> states = new Dictionary<string, IFiniteState>();

        /// <summary>
        /// 状态键
        /// </summary>
        public string stateKey = "";

        /// <summary>
        /// 状态机运行控制
        /// </summary>
        public bool isRun = true;


        private bool isTransition = false;//过渡标记
        private string currentKey = "";//当前状态


        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="key">状态键值</param>
        public FiniteStateMachine SwitchState(string key)
        {
            stateKey = key;
            return this;
        }

        /// <summary>
        /// 注册添加状态
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="state">状态</param>
        public FiniteStateMachine Add(string key, IFiniteState state)
        {
            state.StateMachine = this;
            states.Add(key, state);
            state.StateAdd();
            return this;
        }

        /// <summary>
        /// 移除状态
        /// </summary>
        /// <param name="key">键</param>
        public FiniteStateMachine Remove(string key)
        {
            if (states.ContainsKey(key))
            {
                states[key].StateRemove();
                states.Remove(key);
            }
            return this;
        }



        /// <summary>
        /// 状态机刷新
        /// </summary>
        public void Update()
        {
            if (isRun)
            {

                if (stateKey != currentKey)//假如 要切换状态
                {
                    if (states.ContainsKey(currentKey) ? states[currentKey].IsRun : false)//当前状态正在运行
                    {
                        if (!isTransition)//假如 未过渡
                        {
                            isTransition = true;    //标记为正在过渡
                            states[currentKey].StateExit();//当前状态退出
                        }
                    }
                    else //当前状态运行结束
                    {
                        isTransition = false;    //标记为过渡结束
                        currentKey = stateKey;   //切换状态

                        if (states.ContainsKey(currentKey))
                        {
                            states[currentKey].IsRun = true;
                            states[currentKey].StateEnter();//当前状态开始运行
                        }
                    }
                }
                else //假如 非切换状态
                {
                    if (states.ContainsKey(currentKey))
                    {
                        states[currentKey].StateUpdate();//当前状态更新
                    }
                }

            }
        }
    }
}