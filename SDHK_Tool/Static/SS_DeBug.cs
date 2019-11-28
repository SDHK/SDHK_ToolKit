using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 *
 * 日期：2019.6.20
 * 
 * 功能：一些调试测试用的静态方法
 * 
 */

namespace SDHK_Tool.Static
{

    /// <summary>
    /// 调试静态类
    /// </summary>
    public class SS_DeBug
    {
		
        /// <summary>
        /// 函数效率检测
        /// </summary>
        /// <param name="action">要检测的方法委托</param>
        /// <param name="Num">循环次数(默认为1000次)</param>
        /// <returns>执行时间(毫秒)</returns>
        public static long Efficiency(Action action, int Num = 1000)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < Num; i++)
            {
                action();
            }
            sw.Stop();
            return sw.ElapsedMilliseconds;

            // Profiler.BeginSample("TestMethod");//定位热点功能
            // Profiler.EndSample();
        }




    }

}