/******************************

 * 作者: 闪电黑客

 * 日期: 2021/07/08 06:30:29

 * 最后日期: 2021/07/08 06:30:29

 * 描述: 
    协程服务扩展：一些简单的协程扩展事件

******************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CoroutineSystem
{

    /// <summary>
    /// 协程事件
    /// </summary>
    public static class CoroutineEvent
    {
        private static CoroutineServer coroutineServer = CoroutineServer.Instance();


        /// <summary>
        /// 协程
        /// </summary>
        /// <param name="callBack">回调</param>
        public static void CoroutineCall(Action callBack)
        {
            coroutineServer.StartCoroutine(Callback(callBack));
        }
        /// <summary>
        /// 协程
        /// </summary>
        /// <param name="callBack">回调</param>
        public static void CoroutineCall(this MonoBehaviour mono, Action callBack)
        {
            mono.StartCoroutine(Callback(callBack));
        }
        private static IEnumerator Callback(Action callBack)
        {
            callBack();
            return null;
        }

        /// <summary>
        /// 协程等待
        /// </summary>
        /// <param name="wait">等待条件</param>
        /// <param name="callBack">回调</param>
        public static void CoroutineWait(Func<bool> wait, Action callBack)
        {
            coroutineServer.StartCoroutine(WaitCallback(wait, callBack));
        }
        /// <summary>
        /// 协程等待
        /// </summary>
        /// <param name="mono">运行的主线程</param>
        /// <param name="wait">等待条件</param>
        /// <param name="callBack">回调</param>
        public static void CoroutineWait(this MonoBehaviour mono, Func<bool> wait, Action callBack)
        {
            mono.StartCoroutine(WaitCallback(wait, callBack));
        }

        private static IEnumerator WaitCallback(Func<bool> wait, Action callBack)
        {
            while (!wait()) yield return null;
            callBack();
        }


        /// <summary>
        /// 协程延迟
        /// </summary>
        /// <param name="seconds">秒数</param>
        /// <param name="callBack">回调</param>
        public static void CoroutineDelay(float seconds, Action callBack)
        {
            coroutineServer.StartCoroutine(DelayCallback(seconds, callBack));
        }

        /// <summary>
        /// 协程延迟
        /// </summary>
        /// <param name="mono">运行的主线程</param>
        /// <param name="seconds">秒数</param>
        /// <param name="callBack">回调</param>
        public static void CoroutineDelay(this MonoBehaviour mono, float seconds, Action callBack)
        {
            mono.StartCoroutine(DelayCallback(seconds, callBack));
        }

        private static IEnumerator DelayCallback(float seconds, Action callBack)
        {
            yield return new WaitForSeconds(seconds);
            callBack();
        }


        /// <summary>
        /// 协程死循环
        /// </summary>
        /// <param name="seconds">秒数</param>
        /// <param name="callBack">回调</param>
        public static void CoroutineLoop(float seconds, Action callBack)
        {
            coroutineServer.StartCoroutine(LoopCallback(seconds, callBack));
        }


        private static IEnumerator LoopCallback(float seconds, Action callBack)
        {
            while (true)
            {
                yield return new WaitForSeconds(seconds);
                callBack();
            }
        }

        /// <summary>
        /// 协程死循环
        /// </summary>
        /// <param name="callBack">回调</param>
        public static void CoroutineLoop(Action callBack)
        {
            coroutineServer.StartCoroutine(LoopCallback(callBack));
        }

        private static IEnumerator LoopCallback(Action callBack)
        {
            while (true)
            {
                yield return null;
                callBack();
            }
        }



    }
}
