using SDHK_Tool.Static;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.6.11
 * 
 * 功能：在Update 里进行变化标记
 *
 */

namespace SDHK_Tool.Dynamic
{
	/// <summary>
	/// 边缘检测器
	/// </summary>
    public class SD_MarkerEdge
    {
        private bool bit = false;

		/// <summary>
        /// 边缘检测：检测Switch是否发生变化,触发标记变成true一次 ;  用!Switch[与]运算过滤上下边缘
        /// </summary>
        /// <param name="Switch">标记开关</param>
        /// <returns>return :　触发标记</returns>
        public bool isEdge(bool Switch)
        {
            return SS_TriggerMarker.Edge(Switch, ref bit);
        }

		/// <summary>
        /// 重置检测器
        /// </summary>
        public void Reset_Marker()
        {
            bit = false;
        }

    }
}

