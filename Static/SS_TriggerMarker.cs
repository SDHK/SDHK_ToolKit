using System;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.6.11
 * 
 * 功能：状态标记
 */

namespace SDHK_Tool.Static
{
    /// <summary>
    /// 标记器
    /// </summary>
    public static class SS_TriggerMarker
    {
        /// <summary>
        /// 边缘检测：检测Switch是否发生变化,触发标记变成true一次 ; 用!Switch[与]运算过滤上下边缘
        /// </summary>
        /// <param name="Switch">主判断</param>
        /// <param name="bit">标记位</param>
        /// <returns>return :　触发标记</returns>
        public static bool Edge(bool Switch, ref bool bit)
        {
            bool T = Switch && !bit;
            bit = Switch;
            return T;
        }

        /// <summary>
        /// T触发器：Switch间隔变成true，可改变触发标记状态（false/true）
        /// </summary>
        /// <param name="Switch">主判断</param>
        /// <param name="bit">标记位</param>
        /// <param name="trigger">触发标记</param>
        /// <returns>return :　触发标记</returns>
        public static bool Trigger(bool Switch, ref bool bit, ref bool trigger)
        {
            if (Switch && !bit) trigger = !trigger;
            bit = Switch;
            return trigger;
        }

        /// <summary>
        /// 累加计数器：判断累加数值，需要手动清零
        /// </summary>
        /// <param name="StartCount">计数位</param> 
        /// <param name="TargetCount">设定数</param>
        /// <param name="Cumulative">累加数</param>
        /// <returns>return :　触发标记</returns>
        public static bool Count(ref float StartCount, float TargetCount, float Cumulative)
        {
            if (StartCount <= TargetCount)
            {
                StartCount += Cumulative;
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 计时器：判断系统时间:DateTime.Now
        /// </summary>
        /// <param name="StartTime">计时位</param>
        /// <param name="TargetSeconds">定时位（秒）</param>
        /// <returns>return :　触发标记</returns>
        public static bool Clock_System(DateTime StartTime, float TargetSeconds)
        {

            return ((DateTime.Now - StartTime).TotalSeconds > TargetSeconds);
        }

        /// <summary>
        /// 计时器：判断游戏时间:Time.unscaledTime
        /// </summary>
        /// <param name="StartTime">计时位</param>
        /// <param name="TargetSeconds">定时位（秒）</param>
        /// <returns>return :　触发标记</returns>
        public static bool Clock_Game(float StartTime, float TargetSeconds)
        {

            return ((Time.unscaledTime - StartTime) > TargetSeconds);
        }

        /// <summary>
        /// 计时器：判断游戏世界时间:Time.time（被TimeScale影响）
        /// </summary>
        /// <param name="StartTime">计时位</param>
        /// <param name="TargetSeconds">定时位（秒）</param>
        /// <returns>return :　触发标记</returns>
        public static bool Clock_GameWorld(float StartTime, float TargetSeconds)
        {
            return ((Time.time - StartTime) > TargetSeconds);
        }


    }
}

