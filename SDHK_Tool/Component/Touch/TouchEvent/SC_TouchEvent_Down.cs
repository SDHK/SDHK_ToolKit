using System;
using UnityEngine;
using UnityEngine.EventSystems;
using SDHK_Tool.Dynamic;
using System.Collections.Generic;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.7.16
 *
 * 2019.10.09 :触摸池合并
 * 
 * 功能：点击事件监听
 *
 */


namespace SDHK_Tool.Component
{
    
    /// <summary>
    /// 触摸事件监听器：【点击事件】
    /// </summary>
    public class SC_TouchEvent_Down : SB_TouchEvent
    
    , IPointerDownHandler
    , IPointerUpHandler
    , IPointerClickHandler
    , IBeginDragHandler
    , IEndDragHandler
    , IDragHandler
    {

        [Space()]
        [Tooltip("忽略拖拽事件影响")]
        public bool IgnoreDrag = false;	//忽略拖拽

        [Tooltip("触摸时间")]
        public float TouchTime = 0.5f;  //触摸时间

        /// <summary>
        /// 触摸事件：按下
        /// </summary>
        public Action TouchOnDown;

        /// <summary>
        /// 触摸事件：长按按下
        /// </summary>
        public Action TouchOnLongDown;

        /// <summary>
        /// 触摸事件：按住【Update】
        /// </summary>
        public Action TouchOnStay;

        /// <summary>
        /// 触摸事件：短按按住【Update】
        /// </summary>
        public Action TouchOnShortStay;

        /// <summary>
        /// 触摸事件：长按按住【Update】
        /// </summary>
        public Action TouchOnLongStay;



        /// <summary>
        /// 触摸事件：按下后抬起
        /// </summary>
        public Action TouchOnUp;

        /// <summary>
        /// 触摸事件：短按后抬起
        /// </summary>
        public Action TouchOnShortUp;

        /// <summary>
        /// 触摸事件：长按后抬起
        /// </summary>
        public Action TouchOnLongUp;



        /// <summary>
        /// 触摸事件：点击
        /// </summary>
        public Action TouchOnClick;

        /// <summary>
        /// 触摸事件：短点击
        /// </summary>
        public Action TouchOnShortClick;

        /// <summary>
        /// 触摸事件：长点击
        /// </summary>
        public Action TouchOnLongClick;


        private int touchCount = 0;		//触摸数量
        public int TouchCount { get { return touchCount; } }


        private SD_MarkerClock MarkerClock;

        private bool isDown = false;     //已按下

        private bool isTouch = true;

        private bool isLongDown = false; //长按触发


        void Awake()
        {
            MarkerClock = new SD_MarkerClock();
            MarkerClock.Reset_Marker();//SDHK临时修改:计时器的理念设定不完善
        }


        /// <summary>
        /// 添加触摸点
        /// </summary>
        /// <param name="eventData">触摸点</param>
        public void AddTouchData(PointerEventData eventData)
        {

            TouchIds.Add(eventData.pointerId);
            TouchPool.Add(eventData.pointerId, eventData);
        }

        /// <summary>
        /// 删除触摸点
        /// </summary>
        /// <param name="eventData">触摸点</param>
        public void RemoveTouchData(PointerEventData eventData)
        {

            TouchIds.Remove(eventData.pointerId);
            TouchPool.Remove(eventData.pointerId);
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (TouchPool.ContainsKey(eventData.pointerId)) return;
            if (IgnoreMouse && eventData.pointerId < 0) return;
            touchCount++;
            if (touchCount == 1)
            {
                isDown = true;
                MarkerClock.Reset_Marker();
            }

            //触摸池添加
            AddTouchData(eventData);

            if (TouchOnDown != null) TouchOnDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!TouchPool.ContainsKey(eventData.pointerId)) return;
            if (IgnoreMouse && eventData.pointerId < 0) return;
            touchCount--;

            if (TouchOnUp != null) TouchOnUp();

            if (touchCount == 0 && isTouch)
            {
                if (!isLongDown && TouchOnShortUp != null) TouchOnShortUp();
                if (isLongDown && TouchOnLongUp != null) TouchOnLongUp();
            }

            //触摸池删除
            RemoveTouchData(eventData);

            if (touchCount == 0)
            {
                isDown = false;
                isLongDown = false;
            }

            isTouch = true;

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!TouchPool.ContainsKey(eventData.pointerId)) return;
            if (IgnoreMouse && eventData.pointerId < 0) return;

            if (TouchOnClick != null) TouchOnClick();

            if (touchCount == 0 && isTouch)
            {
                if (!isLongDown && TouchOnShortClick != null) TouchOnShortClick();
                if (isLongDown && TouchOnLongClick != null) TouchOnLongClick();
            }

            isTouch = true;

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!IgnoreDrag) isTouch = false;
        }
        // public void

        public void OnEndDrag(PointerEventData eventData)
        {
            // isTouch = true;
        }
        public void OnDrag(PointerEventData eventData)//SDHK临时修改:OnBeginDrag触发太晚？ OnEndDrag的触发太早？ 导致isTouch失效，也可能是穿透器设定不完善
        {
            if (!IgnoreDrag) isTouch = false;
        }




        void Update()
        {
            if (isDown && TouchOnStay != null) TouchOnStay();

            if (isDown && isTouch)
            {
                if (MarkerClock.IF_Clock_System(TouchTime))
                {
                    isLongDown = true;
                    if (TouchOnLongDown != null) TouchOnLongDown();
                }

                if (!isLongDown && TouchOnShortStay != null) TouchOnShortStay();
                if (isLongDown && TouchOnLongStay != null) TouchOnLongStay();
            }

        }

    }
}
