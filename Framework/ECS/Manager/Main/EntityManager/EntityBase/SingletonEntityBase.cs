using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 实体单例化
    /// </summary>
    public class SingletonEntityBase<T> : Entity
        where T :class,IEntity
    {
        protected static T instance;//实例

        /// <summary>
        /// 实例组件
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance is null)
                {
                    instance= Root.GetComponent<T>();
                }
                return instance;
            }
        }

        /// <summary>
        /// 单例实例化
        /// </summary>
        public static T GetInstance()
        {
            return Instance;
        }
    }

}
