using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 计数器
    /// </summary>
    public class CounterCall : Entity
    {
        public int count = 0;
        public int countOut = 0;
        public Action callback;
    }

    class CounterCallGetSystem : GetSystem<CounterCall>
    {
        public override void OnGet(CounterCall self)
        {
            self.count = 0;
        }
    }



    class CounterCallUpdateSystem : UpdateSystem<CounterCall>
    {
        public override void Update(CounterCall self)
        {
            self.count++;
            if (self.count >= self.countOut)
            {
                self.callback?.Invoke();
                self.RemoveSelf();
            }
        }
    }
}
