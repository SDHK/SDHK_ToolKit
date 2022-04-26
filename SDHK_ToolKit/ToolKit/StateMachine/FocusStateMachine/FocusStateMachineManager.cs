
/******************************

 * Author: 闪电黑客

 * 日期: 2022/01/28 21:48:57

 * 最后日期: 2022/03/04 15:21:00

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;

namespace StateMachine
{
    public class FocusStateMachineManager : SingletonMonoBase<FocusStateMachineManager>
    {
        public List<FocusStateMachine> focusStateMachines = new List<FocusStateMachine>();

        public FocusStateMachine Add(FocusStateMachine focusStateMachine)
        {
            if (!focusStateMachines.Contains(focusStateMachine))
            {
                focusStateMachines.Add(focusStateMachine);
            }
            return focusStateMachine;
        }


        public void Remove(FocusStateMachine focusStateMachine)
        {
            focusStateMachines.Remove(focusStateMachine);
        }

        private void Update()
        {
            for (int i = 0; i < focusStateMachines.Count; i++)
            {
                focusStateMachines[i].Update();
            }

        }
    }
}