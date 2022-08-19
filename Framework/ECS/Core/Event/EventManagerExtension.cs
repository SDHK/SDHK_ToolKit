using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 根节点组件调用扩展
    /// </summary>
    public static class EventManagerExtension
    {
        /// <summary>
        /// 获取一组事件集合
        /// </summary>
        public static EventManager RootEventManager(this Entity self)
        {
            return self.Root.AddComponent<EventManager>();
        }


        public static T0 Send<T0, T1>(this T0 self, T1 arg1)
        where T0 : Entity
        {
            self.RootEventManager().Get().Send(self, arg1);
            return self;
        }

        public static async AsyncTask SendAsync<T0>(this T0 self)
         where T0 : Entity
        {
            await self.RootEventManager().Get().SendAsync(self);
        }

        public static async AsyncTask<OutT> CallAsync<T0, OutT>(this T0 self)
        where T0 : Entity
        {
            return await self.RootEventManager().Get().CallAsync<T0, OutT>(self);
        }


        public static OutT Call<T0, OutT>(this T0 self)
        where T0 : Entity
        {
            return self.RootEventManager().Get().Call<T0, OutT>(self);
        }

    }
}
