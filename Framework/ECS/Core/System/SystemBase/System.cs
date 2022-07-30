/****************************************

* 创 建 者：  闪电黑客
* 创建时间：  2022/5/6 21:31
* 描    述:

****************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace SDHK
{

    /// <summary>
    /// 系统接口
    /// </summary>
    public interface ISystem
    {
        Type EntityType { get; }
    }

    /// <summary>
    /// 系统基类
    /// </summary>
    public abstract class SystemBase<T> : ISystem
    {
        public Type EntityType => typeof(T);
    }
}
