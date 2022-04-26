using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;


namespace StateMachine
{
    public class StackStateMachineManager : SingletonMonoBase<StackStateMachineManager>
    {
        public List<StackStateMachine> stackStateMachines = new List<StackStateMachine>();


        public StackStateMachine Add(StackStateMachine stackStateMachine)
        {
            if (!stackStateMachines.Contains(stackStateMachine))
            {
                stackStateMachines.Add(stackStateMachine);
            }
            return stackStateMachine;
        }


        public void Remove(StackStateMachine stackStateMachine)
        {

            stackStateMachines.Remove(stackStateMachine);
        }

        private void Update()
        {

            for (int i = 0; i < stackStateMachines.Count; i++)
            {
                stackStateMachines[i].Update();
            }
        }
    }
}