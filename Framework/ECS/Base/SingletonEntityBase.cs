using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    public class SingletonEntityBase<T> : Entity
        where T : Entity
    {
        /// <summary>
        /// 实例组件
        /// </summary>
        public static T Instance => root.AddComponent<T>();

        /// <summary>
        /// 单例实例化
        /// </summary>
        public static T GetInstance()
        {
            return Instance;
        }
    }

}
