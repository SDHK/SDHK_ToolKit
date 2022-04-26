using System;
using SDHK_Tool.Static;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.9.26
 * 
 * 功能：进行计时标记
 */

namespace SDHK_Tool.Dynamic
{
    /// <summary>
    /// 计时标记器
    /// </summary>
    public class SD_MarkerClock
    {
        private DateTime StartTime_System;
        private float StartTime_GameWorld;
        private float StartTime_Game;

        private SD_MarkerEdge MarkerEdge;


        public SD_MarkerClock()
        {
            MarkerEdge = new SD_MarkerEdge();
            StartTime_System = DateTime.Now;
            StartTime_GameWorld = Time.time;
            StartTime_Game = Time.unscaledTime;
        }

        /// <summary>
        /// 计时器[系统时间]：判断执行时间(一次性)
        /// 到达指定时间返回一帧true
        /// </summary>
        /// <param name="targetSeconds">定时</param>
        /// <returns>return :　触发标记</returns>
        public bool IF_Clock_System(float targetSeconds)
        {
            return MarkerEdge.isEdge(SS_TriggerMarker.Clock_System(StartTime_System, targetSeconds));
        }

        /// <summary>
        /// 计时器[游戏世界时间]：判断执行时间(一次性)（被TimeScale影响）
        /// 到达指定时间返回一帧true
        /// </summary>
        /// <param name="targetSeconds">定时</param>
        /// <returns>return :　触发标记</returns>
        public bool IF_Clock_GameWorld(float targetSeconds)
        {
            return MarkerEdge.isEdge(SS_TriggerMarker.Clock_GameWorld(StartTime_GameWorld, targetSeconds));
        }


        /// <summary>
        /// 计时器[游戏时间]：判断执行时间(一次性)
        /// 到达指定时间返回一帧true
        /// </summary>
        /// <param name="targetSeconds">定时</param>
        /// <returns>return :　触发标记</returns>
        public bool IF_Clock_Game(float targetSeconds)
        {
            return MarkerEdge.isEdge(SS_TriggerMarker.Clock_Game(StartTime_Game, targetSeconds));
        }


        /// <summary>
        /// 计时器[系统时间]：判断执行时间
        /// 循环触发：到达指定时间返回一帧true，并重置时间再次计时
        /// 持续触发：到达指定时间后，一直返回true
        /// </summary>
        /// <param name="targetSeconds">定时</param>
        ///	<param name="Switch">true/false（循环触发/持续触发）</param>
        /// <returns>return :　触发标记</returns>
        public bool IF_Clock_System(float targetSeconds, bool Switch)
        {
            bool bit = SS_TriggerMarker.Clock_System(StartTime_System, targetSeconds);
            if (bit && Switch) Reset_Marker();
            return (Switch) ? MarkerEdge.isEdge(bit) : bit;
        }

        /// <summary>
        /// 计时器[游戏世界时间]：判断执行时间（被TimeScale影响）
        /// 循环触发：到达指定时间返回一帧true，并重置时间再次计时
        /// 持续触发：到达指定时间后，一直返回true
        /// </summary>
        /// <param name="targetSeconds">定时</param>
        ///	<param name="Switch">true/false（循环触发/持续触发）</param>
        /// <returns>return :　触发标记</returns>
        public bool IF_Clock_GameWorld(float targetSeconds, bool Switch)
        {
            bool bit = SS_TriggerMarker.Clock_GameWorld(StartTime_GameWorld, targetSeconds);
            if (bit && Switch) Reset_Marker();
            return (Switch) ? MarkerEdge.isEdge(bit) : bit;
        }

        /// <summary>
        /// 计时器[游戏时间]：判断执行时间
        /// 循环触发：到达指定时间返回一帧true，并重置时间再次计时
        /// 持续触发：到达指定时间后，一直返回true
        /// </summary>
        /// <param name="targetSeconds">定时</param>
        ///	<param name="Switch">true/false（循环触发/持续触发）</param>
        /// <returns>return :　触发标记</returns>
        public bool IF_Clock_Game(float targetSeconds, bool Switch)
        {
            bool bit = SS_TriggerMarker.Clock_Game(StartTime_Game, targetSeconds);
            if (bit && Switch) Reset_Marker();
            return (Switch) ? MarkerEdge.isEdge(bit) : bit;
        }


        /// <summary>
        /// 获取倒计时时间[系统时间]
        /// </summary>
        /// <returns>倒计时时间</returns>
        public double Get_Clock_System()
        {
            return (DateTime.Now - StartTime_System).TotalSeconds;
        }

        /// <summary>
        /// 获取倒计时时间[游戏世界时间]（被TimeScale影响）
        /// </summary>
        /// <returns>倒计时时间</returns>
        public double Get_Clock_GameWorld()
        {
            return Time.time - StartTime_GameWorld;
        }

        /// <summary>
        /// 获取倒计时时间[游戏时间]
        /// </summary>
        /// <returns>倒计时时间</returns>
        public double Get_Clock_Game()
        {
            return Time.unscaledTime - StartTime_Game;
        }

        /// <summary>
        /// 重置检测器
        /// </summary>  
        public void Reset_Marker()
        {
            StartTime_System = DateTime.Now;
            StartTime_GameWorld = Time.time;
            StartTime_Game = Time.unscaledTime;
            MarkerEdge.Reset_Marker();
        }


    }
}