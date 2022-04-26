/******************************

 * 作者: 闪电黑客

 * 日期: 2021/07/08 06:30:29

 * 最后日期: 2021/07/08 06:30:29

 * 描述: 
    协程服务：
    给 协程扩展 提供MonoBehaviour主线程用于执行协程
    将一部分简单协程事件转移至委托操作
    简化协程书写。

******************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;


namespace CoroutineSystem
{

    /// <summary>
    /// 协程服务:给 协程扩展 提供MonoBehaviour主线程用于执行
    /// </summary>
    public class CoroutineServer : SingletonMonoBase<CoroutineServer>
    {

    }



}