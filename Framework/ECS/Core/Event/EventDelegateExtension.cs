/******************************

 * 作者: 闪电黑客

 * 日期: 2021/10/18 6:31:30

 * 最后日期: 2021/10/18 6:31:30

 * 描述: 
 
    EventDelegate事件委托 的静态扩展方法

    可添加 Action 和 Func 委托

    通过Call调用
    
    CallAction 调用同类型的多播委托
    CallFuncs 调用同类型的多播委托，并返回获取到的全部返回值List
    CallFunc 调用同类型的多播委托，并返回获取到的第一个返回值

    当前参数最多为 5
    
    
******************************/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{

    public static class EventDelegateExtension
    {
        #region Action


        #region Add
        public static void Add(this EventDelegate e, Action delegate_) { e.AddDelegate(delegate_); }
        public static void Add<T1>(this EventDelegate e, Action<T1> delegate_) { e.AddDelegate(delegate_); }
        public static void Add<T1, T2>(this EventDelegate e, Action<T1, T2> delegate_) { e.AddDelegate(delegate_); }
        public static void Add<T1, T2, T3>(this EventDelegate e, Action<T1, T2, T3> delegate_) { e.AddDelegate(delegate_); }
        public static void Add<T1, T2, T3, T4>(this EventDelegate e, Action<T1, T2, T3, T4> delegate_) { e.AddDelegate(delegate_); }
        public static void Add<T1, T2, T3, T4, T5>(this EventDelegate e, Action<T1, T2, T3, T4, T5> delegate_) { e.AddDelegate(delegate_); }
        #endregion

        #region Remove
        public static void Remove(this EventDelegate e, Action delegate_) { e.RemoveDelegate(delegate_); }
        public static void Remove<T1>(this EventDelegate e, Action<T1> delegate_) { e.RemoveDelegate(delegate_); }
        public static void Remove<T1, T2>(this EventDelegate e, Action<T1, T2> delegate_) { e.RemoveDelegate(delegate_); }
        public static void Remove<T1, T2, T3>(this EventDelegate e, Action<T1, T2, T3> delegate_) { e.RemoveDelegate(delegate_); }
        public static void Remove<T1, T2, T3, T4>(this EventDelegate e, Action<T1, T2, T3, T4> delegate_) { e.RemoveDelegate(delegate_); }
        public static void Remove<T1, T2, T3, T4, T5>(this EventDelegate e, Action<T1, T2, T3, T4, T5> delegate_) { e.RemoveDelegate(delegate_); }
        #endregion

        #region CallAction

        public static async Task CallActionAsync(this EventDelegate e)
        { 
            var events = e.Get<Func<Task>>();
            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                       await (events[i] as Func<Task>)();
                    }
                }
            }
        }
        public static void CallAction(this EventDelegate e)
        {
            var events = e.Get<Action>();

            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        (events[i] as Action)();
                    }
                }
            }
        }
        public static void CallAction<T1>(this EventDelegate e, T1 arg1)
        {
            var events = e.Get<Action<T1>>();

            if (events != null)
            {

                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        (events[i] as Action<T1>)(arg1);
                    }
                }
            }
        }
        public static void CallAction<T1, T2>(this EventDelegate e, T1 arg1, T2 arg2)
        {
            var events = e.Get<Action<T1, T2>>();

            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        (events[i] as Action<T1, T2>)(arg1, arg2);
                    }
                }
            }
        }
        public static void CallAction<T1, T2, T3>(this EventDelegate e, T1 arg1, T2 arg2, T3 arg3)
        {
            var events = e.Get<Action<T1, T2, T3>>();

            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        (events[i] as Action<T1, T2, T3>)(arg1, arg2, arg3);
                    }
                }
            }
        }
        public static void CallAction<T1, T2, T3, T4>(this EventDelegate e, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var events = e.Get<Action<T1, T2, T3, T4>>();

            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        (events[i] as Action<T1, T2, T3, T4>)(arg1, arg2, arg3, arg4);
                    }
                }
            }
        }
        public static void CallAction<T1, T2, T3, T4, T5>(this EventDelegate e, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var events = e.Get<Action<T1, T2, T3, T4, T5>>();

            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        (events[i] as Action<T1, T2, T3, T4, T5>)(arg1, arg2, arg3, arg4, arg5);
                    }
                }
            }
        }
        #endregion


        #endregion



        #region Func


        #region Add
        public static void Add<OutT>(this EventDelegate e, Func<OutT> delegate_) { e.AddDelegate(delegate_); }
        public static void Add<T1, OutT>(this EventDelegate e, Func<T1, OutT> delegate_) { e.AddDelegate(delegate_); }
        public static void Add<T1, T2, OutT>(this EventDelegate e, Func<T1, T2, OutT> delegate_) { e.AddDelegate(delegate_); }
        public static void Add<T1, T2, T3, OutT>(this EventDelegate e, Func<T1, T2, T3, OutT> delegate_) { e.AddDelegate(delegate_); }
        public static void Add<T1, T2, T3, T4, OutT>(this EventDelegate e, Func<T1, T2, T3, T4, OutT> delegate_) { e.AddDelegate(delegate_); }
        public static void Add<T1, T2, T3, T4, T5, OutT>(this EventDelegate e, Action<T1, T2, T3, T4, T5, OutT> delegate_) { e.AddDelegate(delegate_); }
        #endregion

        #region Remove
        public static void Remove<OutT>(this EventDelegate e, Func<OutT> delegate_) { e.RemoveDelegate(delegate_); }
        public static void Remove<T1, OutT>(this EventDelegate e, Func<T1, OutT> delegate_) { e.RemoveDelegate(delegate_); }
        public static void Remove<T1, T2, OutT>(this EventDelegate e, Func<T1, T2, OutT> delegate_) { e.RemoveDelegate(delegate_); }
        public static void Remove<T1, T2, T3, OutT>(this EventDelegate e, Func<T1, T2, T3, OutT> delegate_) { e.RemoveDelegate(delegate_); }
        public static void Remove<T1, T2, T3, T4, OutT>(this EventDelegate e, Func<T1, T2, T3, T4, OutT> delegate_) { e.RemoveDelegate(delegate_); }
        public static void Remove<T1, T2, T3, T4, T5, OutT>(this EventDelegate e, Action<T1, T2, T3, T4, T5, OutT> delegate_) { e.RemoveDelegate(delegate_); }
        #endregion

        #region CallFuncs
        public static List<OutT> CallFuncs<OutT>(this EventDelegate e)
        {
            var events = e.Get<Func<OutT>>();
            List<OutT> ts = new List<OutT>();
            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        ts.Add((events[i] as Func<OutT>)());
                    }
                }
            }
            return ts;
        }
        public static List<OutT> CallFuncs<T1, OutT>(this EventDelegate e, T1 arg1)
        {
            var events = e.Get<Func<T1, OutT>>();
            List<OutT> ts = new List<OutT>();
            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        ts.Add((events[i] as Func<T1, OutT>)(arg1));
                    }
                }
            }
            return ts;
        }
        public static List<OutT> CallFuncs<T1, T2, OutT>(this EventDelegate e, T1 arg1, T2 arg2)
        {
            var events = e.Get<Func<T1, T2, OutT>>();
            List<OutT> ts = new List<OutT>();
            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        ts.Add((events[i] as Func<T1, T2, OutT>)(arg1, arg2));
                    }
                }
            }
            return ts;
        }
        public static List<OutT> CallFuncs<T1, T2, T3, OutT>(this EventDelegate e, T1 arg1, T2 arg2, T3 arg3)
        {
            var events = e.Get<Func<T1, T2, T3, OutT>>();
            List<OutT> ts = new List<OutT>();
            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        ts.Add((events[i] as Func<T1, T2, T3, OutT>)(arg1, arg2, arg3));
                    }
                }
            }
            return ts;
        }
        public static List<OutT> CallFuncs<T1, T2, T3, T4, OutT>(this EventDelegate e, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var events = e.Get<Func<T1, T2, T3, T4, OutT>>();
            List<OutT> ts = new List<OutT>();
            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        ts.Add((events[i] as Func<T1, T2, T3, T4, OutT>)(arg1, arg2, arg3, arg4));
                    }
                }
            }
            return ts;
        }
        public static List<OutT> CallFuncs<T1, T2, T3, T4, T5, OutT>(this EventDelegate e, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var events = e.Get<Func<T1, T2, T3, T4, T5, OutT>>();
            List<OutT> ts = new List<OutT>();
            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        ts.Add((events[i] as Func<T1, T2, T3, T4, T5, OutT>)(arg1, arg2, arg3, arg4, arg5));
                    }
                }
            }
            return ts;
        }


        public static List<OutT> CallFuncs<OutT>(this EventDelegate e, out List<OutT> outT)
        {
            return outT = CallFuncs<OutT>(e);
        }
        public static List<OutT> CallFuncs<T1, OutT>(this EventDelegate e, T1 arg1, out List<OutT> outT)
        {
            return outT = CallFuncs<T1, OutT>(e, arg1);
        }
        public static List<OutT> CallFuncs<T1, T2, OutT>(this EventDelegate e, T1 arg1, T2 arg2, out List<OutT> outT)
        {
            return outT = CallFuncs<T1, T2, OutT>(e, arg1, arg2);
        }
        public static List<OutT> CallFuncs<T1, T2, T3, OutT>(this EventDelegate e, T1 arg1, T2 arg2, T3 arg3, out List<OutT> outT)
        {
            return outT = CallFuncs<T1, T2, T3, OutT>(e, arg1, arg2, arg3);
        }
        public static List<OutT> CallFuncs<T1, T2, T3, T4, OutT>(this EventDelegate e, T1 arg1, T2 arg2, T3 arg3, T4 arg4, out List<OutT> outT)
        {
            return outT = CallFuncs<T1, T2, T3, T4, OutT>(e, arg1, arg2, arg3, arg4);
        }
        public static List<OutT> CallFuncs<T1, T2, T3, T4, T5, OutT>(this EventDelegate e, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out List<OutT> outT)
        {
            return outT = CallFuncs<T1, T2, T3, T4, T5, OutT>(e, arg1, arg2, arg3, arg4, arg5);
        }
        #endregion

        #region CallFunc
        public static OutT CallFunc<OutT>(this EventDelegate e)
        {
            var events = e.Get<Func<OutT>>();
            OutT t = default(OutT);
            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        t = (events[i] as Func<OutT>)();
                    }
                }
            }
            return t;
        }
        public static OutT CallFunc<T1, OutT>(this EventDelegate e, T1 arg1)
        {
            var events = e.Get<Func<T1, OutT>>();
            OutT t = default(OutT);
            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        t = (events[i] as Func<T1, OutT>)(arg1);
                    }
                }
            }
            return t;
        }
        public static OutT CallFunc<T1, T2, OutT>(this EventDelegate e, T1 arg1, T2 arg2)
        {
            var events = e.Get<Func<T1, T2, OutT>>();
            OutT t = default(OutT);
            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        t = (events[i] as Func<T1, T2, OutT>)(arg1, arg2);
                    }
                }
            }
            return t;
        }
        public static OutT CallFunc<T1, T2, T3, OutT>(this EventDelegate e, T1 arg1, T2 arg2, T3 arg3)
        {
            var events = e.Get<Func<T1, T2, T3, OutT>>();
            OutT t = default(OutT);
            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        t = (events[i] as Func<T1, T2, T3, OutT>)(arg1, arg2, arg3);
                    }
                }
            }
            return t;
        }
        public static OutT CallFunc<T1, T2, T3, T4, OutT>(this EventDelegate e, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var events = e.Get<Func<T1, T2, T3, T4, OutT>>();
            OutT t = default(OutT);
            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        t = (events[i] as Func<T1, T2, T3, T4, OutT>)(arg1, arg2, arg3, arg4);
                    }
                }
            }
            return t;
        }
        public static OutT CallFunc<T1, T2, T3, T4, T5, OutT>(this EventDelegate e, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var events = e.Get<Func<T1, T2, T3, T4, T5, OutT>>();
            OutT t = default(OutT);
            if (events != null)
            {
                for (int i = events.Count - 1; i >= 0; i--)
                {
                    if (events[i] == null)
                    {
                        events.RemoveAt(i);
                    }
                    else
                    {
                        t = (events[i] as Func<T1, T2, T3, T4, T5, OutT>)(arg1, arg2, arg3, arg4, arg5);
                    }
                }
            }
            return t;
        }


        public static OutT CallFunc<OutT>(this EventDelegate e, out OutT outT)
        {
            return outT = CallFunc<OutT>(e);
        }
        public static OutT CallFunc<T1, OutT>(this EventDelegate e, T1 arg1, out OutT outT)
        {
            return outT = CallFunc<T1, OutT>(e, arg1);
        }
        public static OutT CallFunc<T1, T2, OutT>(this EventDelegate e, T1 arg1, T2 arg2, out OutT outT)
        {
            return outT = CallFunc<T1, T2, OutT>(e, arg1, arg2);
        }
        public static OutT CallFunc<T1, T2, T3, OutT>(this EventDelegate e, T1 arg1, T2 arg2, T3 arg3, out OutT outT)
        {
            return outT = CallFunc<T1, T2, T3, OutT>(e, arg1, arg2, arg3);
        }
        public static OutT CallFunc<T1, T2, T3, T4, OutT>(this EventDelegate e, T1 arg1, T2 arg2, T3 arg3, T4 arg4, out OutT outT)
        {
            return outT = CallFunc<T1, T2, T3, T4, OutT>(e, arg1, arg2, arg3, arg4);
        }
        public static OutT CallFunc<T1, T2, T3, T4, T5, OutT>(this EventDelegate e, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out OutT outT)
        {
            return outT = CallFunc<T1, T2, T3, T4, T5, OutT>(e, arg1, arg2, arg3, arg4, arg5);
        }
        #endregion


        #endregion


    }
}