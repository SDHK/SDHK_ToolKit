using SDHK_Tool.Static;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.6.11
 * 
 * 功能：在Update 里进行计数标记
 */

namespace SDHK_Tool.Dynamic
{

    /// <summary>
    /// 计数标记器
    /// </summary>
    public class SD_MarkerCount
    {
        private float StartCount = 0;

        private SD_MarkerEdge MarkerEdge;

        public SD_MarkerCount()
        {
            MarkerEdge = new SD_MarkerEdge();
        }


        /// <summary>
        /// 计数器：判断累加数值(一次性)
        /// 到达指定值返回一帧true
        /// </summary>
        /// <param name="targetCount">设定数</param>
		/// <param name="Cumulative">累加数</param>
        /// <returns>return :　触发标记</returns>
        public bool IF_Count(float targetCount, float Cumulative)
        {
            return MarkerEdge.isEdge(SS_TriggerMarker.Count(ref StartCount, targetCount, Cumulative));
        }

        /// <summary>
        /// 计数器：判断累加数值
        /// 循环触发：到达指定值返回一帧true，并重置时间再次计时
        /// 持续触发：到达指定值后，一直返回true
        /// </summary>
        /// <param name="targetCount">设定数</param>
		/// <param name="Cumulative">累加数</param>
        /// <param name="Switch">true/false（循环触发/持续触发）</param>
        /// <returns>return :　触发标记</returns>
        public bool IF_Count(float targetCount, float Cumulative, bool Switch)
        {
            bool bit = SS_TriggerMarker.Count(ref StartCount, targetCount, Cumulative);
            if (bit && Switch) Reset_Marker();

            return (Switch) ? MarkerEdge.isEdge(bit) : bit;
        }

        /// <summary>
        /// 获取累加数值
        /// </summary>
        /// <returns>累加数值</returns>
        public float Get_Count()
        {
            return StartCount;
        }

        /// <summary>
        /// 重置检测器
        /// </summary>
        public void Reset_Marker()
        {
            StartCount = 0;
            MarkerEdge.Reset_Marker();
        }




    }


}

