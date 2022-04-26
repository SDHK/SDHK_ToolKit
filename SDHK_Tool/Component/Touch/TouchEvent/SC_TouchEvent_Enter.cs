using System;
using System.Collections.Generic;
using SDHK_Tool.Dynamic;
using UnityEngine;
using UnityEngine.EventSystems;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.7.16
 *
 * 2019.10.09 触摸池合并
 * 
 * 功能：经过事件监听
 *
 */

namespace SDHK_Tool.Component
{
    /// <summary>
    /// 触摸事件监听器：【经过事件】
    /// </summary>
    public class SC_TouchEvent_Enter : SB_TouchEvent
    , IPointerEnterHandler
    , IPointerExitHandler
    {

        [Space()]

        [Tooltip("触摸时间")]
        public float TouchTime = 0.5f;  //触摸时间

        /// <summary>
        /// 触摸事件：进入停留
        /// </summary>
        public Action TouchOnEnter;         //进入

        /// <summary>
        /// 触摸事件：进入长停留
        /// </summary>
        public Action TouchOnLongEnter;     //进入停留

        /// <summary>
        /// 触摸事件：进入停留【Update】
        /// </summary>
        public Action TouchOnInside;        //停留【Update】

        /// <summary>
        /// 触摸事件：进入短停留【Update】
        /// </summary>
        public Action TouchOnShortInside;   //短停留【Update】

        /// <summary>
        /// 触摸事件：进入长停留【Update】
        /// </summary>
        public Action TouchOnLongInside;    //长停留【Update】

        /// <summary>
        /// 触摸事件：离开
        /// </summary>
        public Action TouchOnExit;        //离开

        /// <summary>
        /// 触摸事件：短停留后离开
        /// </summary>
        public Action TouchOnShortExit;   //短停留后离开

        /// <summary>
        /// 触摸事件：长停留后离开
        /// </summary>
        public Action TouchOnLongExit;    //长停留后离开

        private int touchCount = 0;      //触摸数量
        public int TouchCount { get { return touchCount; } }

        private SD_MarkerClock MarkerClock;

        private bool isEnter = false;     //已进入

        private bool isLongEnter = false; //长停留触发


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



        public void OnPointerEnter(PointerEventData eventData)
        {
            if (TouchPool.ContainsKey(eventData.pointerId)) return;
            if (IgnoreMouse && eventData.pointerId < 0) return;
            touchCount++;
            if (touchCount == 1)
            {
                isEnter = true;
                MarkerClock.Reset_Marker();
            }

            //停留池添加
            AddTouchData(eventData);

            if (TouchOnEnter != null) TouchOnEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!TouchPool.ContainsKey(eventData.pointerId)) return;
            if (IgnoreMouse && eventData.pointerId < 0) return;
            touchCount--;

            if (TouchOnExit != null) TouchOnExit();

            if (touchCount == 0)
            {
                if (!isLongEnter && TouchOnShortExit != null) TouchOnShortExit();
                if (isLongEnter && TouchOnLongExit != null) TouchOnLongExit();
            }

            //停留池删除
            RemoveTouchData(eventData);

            if (touchCount == 0)
            {
                isEnter = false;
                isLongEnter = false;
            }
        }

        void Update()
        {

            if (isEnter)
            {
                if (TouchOnInside != null) TouchOnInside();

                if (MarkerClock.IF_Clock_Game(TouchTime))
                {
                    isLongEnter = true;
                    if (TouchOnLongEnter != null) TouchOnLongEnter();
                }

                if (!isLongEnter && TouchOnShortInside != null) TouchOnShortInside();
                if (isLongEnter && TouchOnLongInside != null) TouchOnLongInside();
            }
        }
    }

}