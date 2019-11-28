using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.7.16
 *
 * 2019.10.09 触摸池合并
 * 
 * 功能：拖拽事件监听
 *
 */

namespace SDHK_Tool.Component
{
    /// <summary>
    /// 触摸事件监听器：【拖拽事件】
    /// </summary>
    public class SC_TouchEvent_Drag : SB_TouchEvent
    , IBeginDragHandler
    , IDragHandler
    , IEndDragHandler
    , IDropHandler
    , IScrollHandler
    {
        [Space()]

        /// <summary>
        /// 触摸事件：拖拽第一帧
        /// </summary>
        public Action TouchOnStartDrag;

        /// <summary>
        /// 触摸事件：拖拽中
        /// </summary>
        public Action TouchOnDrag;

        /// <summary>
        /// 触摸事件：拖拽结束
        /// </summary>
        public Action TouchOnEndDrag;

        /// <summary>
        /// 触摸事件：在原对象上拖拽结束
        /// </summary>
        public Action TouchOnEndDrop;

        public Action<float> TouchOnScroll; //鼠标滚轮


        private void Start()
        {

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


        public void OnBeginDrag(PointerEventData eventData)
        {
            if (TouchPool.ContainsKey(eventData.pointerId)) return;
            if (IgnoreMouse && eventData.pointerId < 0) return;

            //拖拽池添加
            AddTouchData(eventData);

            if (TouchOnStartDrag != null) TouchOnStartDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (IgnoreMouse && eventData.pointerId < 0) return;
            if (TouchOnDrag != null) TouchOnDrag();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!TouchPool.ContainsKey(eventData.pointerId)) return;
            if (IgnoreMouse && eventData.pointerId < 0) return;

            if (TouchOnEndDrag != null) TouchOnEndDrag();

            //拖拽池删除
            RemoveTouchData(eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (IgnoreMouse && eventData.pointerId < 0) return;
            if (TouchOnEndDrop != null) TouchOnEndDrop();
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (IgnoreMouse && eventData.pointerId < 0) return;
            if (TouchOnScroll != null) TouchOnScroll(eventData.scrollDelta.y);
        }


    }
}