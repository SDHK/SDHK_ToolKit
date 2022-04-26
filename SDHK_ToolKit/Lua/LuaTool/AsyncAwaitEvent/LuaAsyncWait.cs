
/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/24 22:44:56

 * 最后日期: 2021/12/24 22:45:28

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using CoroutineSystem;
using ObjectFactory;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace AsyncAwaitEvent
{
    public static class LuaAsyncAwait
    {

        public async static void AsyncTask(Task async, Action onFinished = null)
        {
            await async;
            onFinished?.Invoke();
        }

        public async static void AsyncOperation(AsyncOperation async, Action onFinished = null)
        {
            await async;
            onFinished?.Invoke();
        }

        public async static void AsyncDelay(int millisecondsDelay, Action onFinished = null)
        {
            await Task.Delay(millisecondsDelay);
            onFinished?.Invoke();
        }

        public static async void AsyncWait(Func<bool> boolFunc, Action onFinished = null)
        {
            await boolFunc;
            onFinished?.Invoke();
        }
    }

}