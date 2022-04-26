

using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.11.08
 * 
 * 功能：UI辅助线画线，可以查看UI的边框,挂在任意物体上即可生效
 * 
 *
 */

/// <summary>
/// UI辅助线画线
/// </summary>
namespace SDHK_Tool.Component
{
#if UNITY_EDITOR
    public class SC_DebugUILine : MonoBehaviour
    {
        /// <summary>
        /// 画线颜色
        /// </summary>
        [SerializeField]
        public Color color = Color.green;

        static Vector3[] fourCorners = new Vector3[4];
        void OnDrawGizmos()
        {
            foreach (MaskableGraphic g in GameObject.FindObjectsOfType<MaskableGraphic>())
            {
                if (g.raycastTarget)
                {
                    RectTransform rectTransform = g.transform as RectTransform;
                    rectTransform.GetWorldCorners(fourCorners);
                    Gizmos.color = color;
                    for (int i = 0; i < 4; i++)
                        Gizmos.DrawLine(fourCorners[i], fourCorners[(i + 1) % 4]);
                }
            }
        }
    }
#endif
}



