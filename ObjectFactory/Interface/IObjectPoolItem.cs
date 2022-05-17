
/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/13 13:40:18

 * 最后日期: 2021/12/15 18:32:23

 * 最后修改: 闪电黑客

 * 描述:  
  
    泛型对象接口

******************************/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SDHK
{

    /// <summary>
    /// 对象池对象接口
    /// </summary>
    public interface IObjectPoolItem
    {
        /// <summary>
        /// 产生此类的对象池
        /// </summary>
        PoolBase thisPool { get; set; }

        /// <summary>
        /// 对象回收
        /// </summary>
        void ObjectRecycle();

        /// <summary>
        /// 对象新建时
        /// </summary>
        void ObjectOnNew();

        /// <summary>
        /// 对象获取时
        /// </summary>
        void ObjectOnGet();

        /// <summary>
        /// 对象回收时
        /// </summary>
        void ObjectOnRecycle();

        /// <summary>
        /// 对象删除时
        /// </summary>
        void ObjectOnDestroy();
    }
   


}