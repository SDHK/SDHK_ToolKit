/******************************

 * 作者: 闪电黑客

 * 日期: 2021/07/03 7:17:50

 * 最后日期: 2021/07/03 7:17:50

 * 描述: 
    有限状态机管理器

    主要功能：
        方便运行管理状态机

******************************/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;



namespace FSM
{
    public class FSM_Manager : SingletonMonoBase<FSM_Manager>
    {

        public Dictionary<string, FiniteStateMachine> Machines = new Dictionary<string, FiniteStateMachine>();


        //禁止new实例化
        private FSM_Manager() { }

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                Debug.Log("FSM Manager 启动！");
            }
        }


        /// <summary>
        /// 获取状态机
        /// </summary>
        /// <param name="key">键</param>
        public static FiniteStateMachine GetMachine(string key)
        {
            if (Instance().Machines.ContainsKey(key))
            {
                return instance.Machines[key];
            }
            else
            {
                FiniteStateMachine machine = new FiniteStateMachine();
                Instance().Machines.Add(key, machine);
                return machine;
            }
        }

        /// <summary>
        /// 移除状态机
        /// </summary>
        /// <param name="key">键</param>
        public static FiniteStateMachine RemoveMachine(string key)
        {
            if (Instance().Machines.ContainsKey(key))
            {
                FiniteStateMachine machine = instance.Machines[key];
                Instance().Machines.Remove(key);
                return machine;

            }
            else
            {
                return null;
            }
        }

        void Update()
        {
            foreach (var Machine in Machines)
            {
                Machine.Value.Update();
            }
        }
    }
}