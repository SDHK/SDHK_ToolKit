using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;


namespace TimeClock
{

    public class ClockManager : SingletonMonoBase<ClockManager>
    {
        private List<Clock> clocks = new List<Clock>();

        public void AddClock(Clock clock)
        {
            if (!clocks.Contains(clock))
            {
                clocks.Add(clock);
            }
        }

        public void RemoveClock(Clock clock)
        {
            clocks.Remove(clock);
        }

        private void Update()
        {
            for (int i = clocks.Count - 1; i >= 0; i--)
            {
                clocks[i].Update();
            }
        }




    }
}