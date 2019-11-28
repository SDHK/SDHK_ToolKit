using SDHK_Tool.Static;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.6.11
 * 
 * 功能：在Update 里进行变化标记
 */

namespace SDHK_Tool.Dynamic
{
	/// <summary>
	/// T触发检测器
	/// </summary>
    public class SD_MarkerTrigger
    {
        private bool bit = false;
        private bool trigger = false;

        /// <summary>
        ///  T触发器：Switch间隔变成true，可改变触发标记状态（false/true）
        /// </summary>
        /// <param name="Switch">标记开关</param>
        /// <returns>return :　触发标记</returns>
        public bool isTrigger(bool Switch)
        {
            return SS_TriggerMarker.Trigger(Switch, ref bit, ref trigger);
        }

        /// <summary>
        /// 重置检测器
        /// </summary>
        public void Reset_Marker()
        {
            bit = false;
            trigger = false;
        }

    }
}

