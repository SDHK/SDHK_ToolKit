using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDHK_Tool.Component;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.10.11
 * 
 * 功能：给触摸组件切换当前模式
 *
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// 画布模式切换器:编辑器模式下运行
    /// </summary>

    [ExecuteInEditMode]
    public class SC_TouchCanvasMode : MonoBehaviour
    {

        [Tooltip("模式刷新：点击一次即可")]
        public bool refresh = true;

        /// <summary>
        /// 触摸忽略鼠标事件
        /// </summary>
        [Tooltip("忽略鼠标")]
        public bool IgnoreMouse = false;

        private Canvas canvas;

        private void Update()
        {
            if (refresh)
            {
                Refresh();
                refresh = false;
            }
        }

        /// <summary>
        /// 触摸切换刷新模式
        /// </summary>
        [ContextMenu("画布模式刷新")]

        public void Refresh()
        {
            if (GetComponent<Canvas>() == null) return;
            canvas = GetComponent<Canvas>();

            foreach (var TouchTransform in this.GetComponentsInChildren<SC_TouchTransform>())//搜寻所有子物体组件赋值画布模式
            {
                TouchTransform.renderMode = canvas.renderMode;
                TouchTransform.PlaneDistance = canvas.planeDistance;
            }

            foreach (var TouchBase in this.GetComponentsInChildren<SB_Touch>())//搜寻所有子物体组件鼠标事件忽略属性
            {
                TouchBase.IgnoreMouse = IgnoreMouse;
            }
        }

    }
}