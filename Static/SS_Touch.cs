using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.10.17
 * 
 * 功能：用于Touch的方法
 */

namespace SDHK_Tool.Static
{

    /// <summary>
    /// 对Touch的处理
    /// </summary>
    public static class SS_Touch
    {

        /// <summary>
        /// 触摸事件：按下
        /// </summary>
        /// <param name="gameObject">游戏物体</param>
        /// <param name="eventData">触摸点</param>
        public static void OnDown(GameObject gameObject, PointerEventData eventData)
        {
            ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.pointerDownHandler);
        }

        /// <summary>
        /// 触摸事件：抬起
        /// </summary>
        /// <param name="gameObject">游戏物体</param>
        /// <param name="eventData">触摸点</param>
        public static void OnUp(GameObject gameObject, PointerEventData eventData)
        {
            ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.pointerUpHandler);
        }

        /// <summary>
        /// 触摸事件：点击
        /// </summary>
        /// <param name="gameObject">游戏物体</param>
        /// <param name="eventData">触摸点</param>
        public static void OnClick(GameObject gameObject, PointerEventData eventData)
        {
            ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.pointerClickHandler);
        }

        /// <summary>
        /// 触摸事件：进入停留
        /// </summary>
        /// <param name="gameObject">游戏物体</param>
        /// <param name="eventData">触摸点</param>
        public static void OnEnter(GameObject gameObject, PointerEventData eventData)
        {
            ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.pointerEnterHandler);
        }

        /// <summary>
        /// 触摸事件：停留离开
        /// </summary>
        /// <param name="gameObject">游戏物体</param>
        /// <param name="eventData">触摸点</param>
        public static void OnExit(GameObject gameObject, PointerEventData eventData)
        {
            ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.pointerExitHandler);
        }

        /// <summary>
        /// 触摸事件：开始拖拽
        /// </summary>
        /// <param name="gameObject">游戏物体</param>
        /// <param name="eventData">触摸点</param>
        public static void OnBeginDrag(GameObject gameObject, PointerEventData eventData)
        {
            ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.beginDragHandler);
        }

        /// <summary>
        /// 触摸事件：结束拖拽
        /// </summary>
        /// <param name="gameObject">游戏物体</param>
        /// <param name="eventData">触摸点</param>
        public static void OnEndDrag(GameObject gameObject, PointerEventData eventData)
        {
            ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.endDragHandler);
        }

        /// <summary>
        /// 触摸事件：拖拽
        /// </summary>
        /// <param name="gameObject">游戏物体</param>
        /// <param name="eventData">触摸点</param>
        public static void OnDrag(GameObject gameObject, PointerEventData eventData)
        {
            ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.dragHandler);
        }

        /// <summary>
        /// 触摸事件：滚轮
        /// </summary>
        /// <param name="gameObject">游戏物体</param>
        /// <param name="eventData">触摸点</param>
        public static void OnScroll(GameObject gameObject, PointerEventData eventData)
        {
            ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.scrollHandler);
        }


        /// <summary>
        /// 触摸事件转移：进入停留
        /// </summary>
        /// <param name="Current">当前物体</param>
        /// <param name="Target">目标物体</param>
        /// <param name="eventData">触摸点</param>
        public static void Transfer_Enter(GameObject Current, GameObject Target, PointerEventData eventData)
        {

            ExecuteEvents.Execute(Current, eventData, ExecuteEvents.pointerExitHandler);//停留离开
            ExecuteEvents.Execute(Target, eventData, ExecuteEvents.pointerEnterHandler);//进入停留
            eventData.pointerEnter = Target;

        }

        /// <summary>
        /// 触摸事件转移：按下
        /// </summary>
        /// <param name="Current">当前物体</param>
        /// <param name="Target">目标物体</param>
        /// <param name="eventData">触摸点</param>
        public static void Transfer_Down(GameObject Current, GameObject Target, PointerEventData eventData)
        {
            ExecuteEvents.Execute(Current, eventData, ExecuteEvents.pointerUpHandler);//抬起
            ExecuteEvents.Execute(Target, eventData, ExecuteEvents.pointerDownHandler);//按下

            eventData.pointerDrag = Target;
            eventData.pointerPress = Target;

        }

        /// <summary>
        /// 触摸事件转移：开始拖拽
        /// </summary>
        /// <param name="Current">当前物体</param>
        /// <param name="Target">目标物体</param>
        /// <param name="eventData">触摸点</param>
        public static void Transfer_Drag(GameObject Current, GameObject Target, PointerEventData eventData)
        {
            ExecuteEvents.Execute(Current, eventData, ExecuteEvents.endDragHandler);//停止拖拽
            ExecuteEvents.Execute(Target, eventData, ExecuteEvents.beginDragHandler);//开始拖拽
            eventData.pointerDrag = Target;
        }

    }


}