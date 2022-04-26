using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Touch
{
    public class TouchEventDrag : MonoBehaviour

        , IInitializePotentialDragHandler   //拖拽初始化：功能和Down一样
        , IBeginDragHandler
        , IDragHandler
        , IDropHandler          //拖拽放下：功能等同Click，并且与Click冲突后不被触发，功能冲突
        , IEndDragHandler    //拖拽结束：功能等同Up，但需要BeginDrag后才能触发，无法和DragInitialize照应

    {

        public bool ignoreMouse = false;//忽略鼠标

        public TouchPool touchPool = new TouchPool();


        public Action<PointerEventData> DragInitialize;    //拖拽初始化


        public Action<PointerEventData> DragStart;    //拖拽第一帧

        public Action<PointerEventData> DragUpdate;   //拖拽中
        public Action<PointerEventData> DragEnd;  //拖拽结束 (只有在拖拽第一帧开始后才能触发)
        public Action<PointerEventData> DropEnd;  //拖拽后放下（在原来对象上拖拽结束）,需要IDragHandler存在才能触发，与IPointerClickHandler冲突，导致不会触发。



        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (ignoreMouse && eventData.pointerId < 0) return;
            if (DragInitialize != null) DragInitialize(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (ignoreMouse && eventData.pointerId < 0) return;
            if (touchPool.Contains(eventData)) return;
            touchPool.Add(eventData);

            if (DragStart != null) DragStart(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (ignoreMouse && eventData.pointerId < 0) return;
            if (DragUpdate != null) DragUpdate(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {

            if (ignoreMouse && eventData.pointerId < 0) return;
            if (!touchPool.Contains(eventData)) return;

            touchPool.Remove(eventData);

            if (DragEnd != null) DragEnd(eventData);

        }

        public void OnDrop(PointerEventData eventData)
        {
            if (ignoreMouse && eventData.pointerId < 0) return;
            if (DropEnd != null) DropEnd(eventData);
        }

    }
}