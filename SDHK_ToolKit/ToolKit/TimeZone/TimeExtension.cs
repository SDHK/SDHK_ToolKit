/******************************

 * 作者: 闪电黑客

 * 日期: 2021/05/12 9:54:29

 * 最后日期: 2021/05/12 13:00:30

 * 描述: 
    时间区域：代替timeScale，划分不同区域的时间速度，实现游戏内各区域不同的时间速度


******************************/

using System.Collections.Generic;
using UnityEngine;

namespace TimeZone
{
    public static class TimeEx
    {
        public static Dictionary<string, float> timeZones = new Dictionary<string, float>();


        /// <summary>
        /// DeltaTime
        /// </summary>
        /// <param name="zoneName">时区名</param>
        /// <param name="Speed">速度比例（0~1）</param>
        public static float DeltaTime(this string zoneName, float Speed)
        {
            if (!timeZones.ContainsKey(zoneName))
            {
                timeZones.Add(zoneName, Speed);
            }
            else
            {
                timeZones[zoneName] = Speed;
            }

            return timeZones[zoneName] * Time.deltaTime;
        }

        public static float DeltaTime(this string zoneName)
        {
            return timeZones[zoneName] * Time.deltaTime;
        }




    }


}
