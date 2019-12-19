using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.7.10
 * 
 * 功能：触摸事件 UI变色器
 *
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// 触摸UI变色器
    /// </summary>
    public class SC_TouchColor : SB_Touch
    , IPointerDownHandler
    , IPointerEnterHandler
    , IPointerUpHandler
    , IPointerExitHandler
    {

        [Space()]

        [Tooltip("点击颜色")]
        [SerializeField]
        public Color TouchDown = new Color(0.7f, 0.7f, 0.7f, 1);

        [Tooltip("停留颜色")]
        [SerializeField]
        public Color TouchEnter = new Color(0.9f, 0.9f, 0.9f, 1);

        // [SerializeField]
        public Color TouchExit = Color.white;

        private Color imageColor;

        private Image image;
        private RawImage rawimage;

        /// <summary>
        /// 点击触摸id顺序链表[顺序列表]
        /// </summary>
        private List<int> TouchDownIds = new List<int>();

        /// <summary>
        /// 点击触摸id顺序链表[顺序列表]
        /// </summary>
        private List<int> TouchEnterIds = new List<int>();

        void Awake()
        {
            if (GetComponent<Image>() != null) { image = GetComponent<Image>(); TouchExit = image.color; }

            if (GetComponent<RawImage>() != null) { rawimage = GetComponent<RawImage>(); TouchExit = rawimage.color; }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (IgnoreMouse && eventData.pointerId < 0) return;
            if (TouchDownIds.Contains(eventData.pointerId)) return;

            TouchDownIds.Add(eventData.pointerId);

            if (TouchDownIds.Count == 1)
            {
                imageColor = TouchDown;
                Set_Color();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IgnoreMouse && eventData.pointerId < 0) return;
            if (TouchEnterIds.Contains(eventData.pointerId)) return;

            TouchEnterIds.Add(eventData.pointerId);
            if (TouchDownIds.Count == 0 && TouchEnterIds.Count == 1)
            {
                imageColor = TouchEnter;
                Set_Color();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (IgnoreMouse && eventData.pointerId < 0) return;
            if (!TouchDownIds.Contains(eventData.pointerId)) return;

            TouchDownIds.Remove(eventData.pointerId);
            if (TouchDownIds.Count == 0 && TouchEnterIds.Count > 0)
            {
                imageColor = TouchEnter;
                Set_Color();
            }
            if (TouchDownIds.Count == 0 && TouchEnterIds.Count == 0)
            {
                imageColor = TouchExit;
                Set_Color();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IgnoreMouse && eventData.pointerId < 0) return;
            if (!TouchEnterIds.Contains(eventData.pointerId)) return;
            TouchEnterIds.Remove(eventData.pointerId);
            if (TouchDownIds.Count == 0 && TouchEnterIds.Count == 0)
            {
                imageColor = TouchExit;
                Set_Color();
            }
        }

        public void Set_Color()
        {
            if (image != null) image.color = imageColor;
            if (rawimage != null) rawimage.color = imageColor;
        }


    }
}