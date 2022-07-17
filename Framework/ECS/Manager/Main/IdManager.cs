using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// id管理器：单线程
    /// </summary>
    public class IdManager : SingletonBase<IdManager>
    {

        public override void OnInstance()
        {
            id = 0;
        }

        /// <summary>
        /// 当前id
        /// </summary>
        public ulong id = 0;

        /// <summary>
        /// 获取id后递增
        /// </summary>
        public static ulong GetID
        {
            get
            {
                return Instance.id++;
            }

        }

        public override void OnDispose()
        {
            instance = null;
        }
    }
}
