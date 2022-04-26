
/******************************

 * Author: 闪电黑客

 * 日期: 2022/01/28 21:48:56

 * 最后日期: 2022/02/05 21:23:05

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace SDHK_Extension
{
    public static class RectTransformExtension
    {

        /// <summary>
        /// 全拉伸：锚点拉伸到最大到四角，页面全展开
        /// </summary>
        public static RectTransform RectAllStretch(this RectTransform rtf)
        {
            rtf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rtf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
            rtf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rtf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);

            rtf.anchorMin = Vector2.zero;//设置锚点为全屏四角
            rtf.anchorMax = Vector2.one;//设置锚点为全屏四角

            return rtf;
        }
    }
}