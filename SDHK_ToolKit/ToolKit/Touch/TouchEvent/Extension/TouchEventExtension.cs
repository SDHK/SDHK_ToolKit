using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Touch
{

    public static class TouchEventExtension
    {

        #region TouchEventDown

        public static TouchEventDown OnDown(this TouchEventDown touchEventDown, Action<PointerEventData> TouchEvent)
        {
            touchEventDown.Down += TouchEvent;
            return touchEventDown;
        }

        public static TouchEventDown OnLongDown(this TouchEventDown touchEventDown, Action<PointerEventData> TouchEvent)
        {
            touchEventDown.LongDown += TouchEvent;
            return touchEventDown;
        }

        public static TouchEventDown OnDragUpdate(this TouchEventDown touchEventDown, Action<PointerEventData> TouchEvent)
        {
            touchEventDown.DragUpdate += TouchEvent;
            return touchEventDown;
        }


        public static TouchEventDown OnUp(this TouchEventDown touchEventDown, Action<PointerEventData> TouchEvent)
        {
            touchEventDown.Up += TouchEvent;
            return touchEventDown;
        }

        public static TouchEventDown OnShortUp(this TouchEventDown touchEventDown, Action<PointerEventData> TouchEvent)
        {
            touchEventDown.ShortUp += TouchEvent;
            return touchEventDown;
        }

        public static TouchEventDown OnLongUp(this TouchEventDown touchEventDown, Action<PointerEventData> TouchEvent)
        {
            touchEventDown.LongUp += TouchEvent;
            return touchEventDown;
        }


        public static TouchEventDown OnClick(this TouchEventDown touchEventDown, Action<PointerEventData> TouchEvent)
        {
            touchEventDown.Click += TouchEvent;
            return touchEventDown;
        }

        public static TouchEventDown OnShortClick(this TouchEventDown touchEventDown, Action<PointerEventData> TouchEvent)
        {
            touchEventDown.ShortClick += TouchEvent;
            return touchEventDown;
        }
        public static TouchEventDown OnLongClick(this TouchEventDown touchEventDown, Action<PointerEventData> TouchEvent)
        {
            touchEventDown.LongClick += TouchEvent;
            return touchEventDown;
        }


        public static TouchEventDown OnStay(this TouchEventDown touchEventDown, Action TouchEvent)
        {
            touchEventDown.Stay += TouchEvent;
            return touchEventDown;
        }

        public static TouchEventDown OnShortStay(this TouchEventDown touchEventDown, Action TouchEvent)
        {
            touchEventDown.ShortStay += TouchEvent;
            return touchEventDown;
        }

        public static TouchEventDown OnLongStay(this TouchEventDown touchEventDown, Action TouchEvent)
        {
            touchEventDown.LongStay += TouchEvent;
            return touchEventDown;
        }

        public static TouchEventDown OnScrollUpdate(this TouchEventDown touchEventDown, Action<float> TouchEvent)
        {
            touchEventDown.ScrollUpdate += TouchEvent;
            return touchEventDown;
        }

        #endregion

        //==============

        #region TouchEventEnter


        public static TouchEventEnter OnEnter(this TouchEventEnter touchEventEnter, Action<PointerEventData> TouchEvent)
        {
            touchEventEnter.Enter += TouchEvent;
            return touchEventEnter;
        }

        public static TouchEventEnter OnLongEnter(this TouchEventEnter touchEventEnter, Action<PointerEventData> TouchEvent)
        {
            touchEventEnter.LongEnter += TouchEvent;
            return touchEventEnter;
        }

        public static TouchEventEnter OnExit(this TouchEventEnter touchEventEnter, Action<PointerEventData> TouchEvent)
        {
            touchEventEnter.Exit += TouchEvent;
            return touchEventEnter;
        }

        public static TouchEventEnter OnShortExit(this TouchEventEnter touchEventEnter, Action<PointerEventData> TouchEvent)
        {
            touchEventEnter.ShortExit += TouchEvent;
            return touchEventEnter;
        }

        public static TouchEventEnter OnLongExit(this TouchEventEnter touchEventEnter, Action<PointerEventData> TouchEvent)
        {
            touchEventEnter.LongExit += TouchEvent;
            return touchEventEnter;
        }

        public static TouchEventEnter OnInside(this TouchEventEnter touchEventEnter, Action TouchEvent)
        {
            touchEventEnter.Inside += TouchEvent;
            return touchEventEnter;
        }

        public static TouchEventEnter OnShortInside(this TouchEventEnter touchEventEnter, Action TouchEvent)
        {
            touchEventEnter.ShortInside += TouchEvent;
            return touchEventEnter;
        }

        public static TouchEventEnter OnLongInside(this TouchEventEnter touchEventEnter, Action TouchEvent)
        {
            touchEventEnter.LongInside += TouchEvent;
            return touchEventEnter;
        }

        #endregion

        //==============

        #region TouchEventDrag


        public static TouchEventDrag OnDragInitialize(this TouchEventDrag touchEventDrag, Action<PointerEventData> TouchEvent)
        {
            touchEventDrag.DragInitialize += TouchEvent;
            return touchEventDrag;
        }

        public static TouchEventDrag OnDragStart(this TouchEventDrag touchEventDrag, Action<PointerEventData> TouchEvent)
        {
            touchEventDrag.DragStart += TouchEvent;
            return touchEventDrag;
        }

        public static TouchEventDrag OnDragUpdate(this TouchEventDrag touchEventDrag, Action<PointerEventData> TouchEvent)
        {
            touchEventDrag.DragUpdate += TouchEvent;
            return touchEventDrag;
        }

        public static TouchEventDrag OnDragEnd(this TouchEventDrag touchEventDrag, Action<PointerEventData> TouchEvent)
        {
            touchEventDrag.DragEnd += TouchEvent;
            return touchEventDrag;
        }

        public static TouchEventDrag OnDropEnd(this TouchEventDrag touchEventDrag, Action<PointerEventData> TouchEvent)
        {
            touchEventDrag.DropEnd += TouchEvent;
            return touchEventDrag;
        }

        # endregion

        //==============

        #region TouchEventSelect


        public static TouchEventSelect OnSelectStart(this TouchEventSelect touchEventSelect, Action<BaseEventData> TouchEvent)
        {
            touchEventSelect.SelectStart += TouchEvent;
            return touchEventSelect;
        }

        public static TouchEventSelect OnSelectUpdate(this TouchEventSelect touchEventSelect, Action<BaseEventData> TouchEvent)
        {
            touchEventSelect.SelectUpdate += TouchEvent;
            return touchEventSelect;
        }

        public static TouchEventSelect OnSelectEnd(this TouchEventSelect touchEventSelect, Action<BaseEventData> TouchEvent)
        {
            touchEventSelect.SelectEnd += TouchEvent;
            return touchEventSelect;
        }

        public static TouchEventSelect OnSelectMove(this TouchEventSelect touchEventSelect, Action<AxisEventData> TouchEvent)
        {
            touchEventSelect.SelectMove += TouchEvent;
            return touchEventSelect;
        }

        public static TouchEventSelect OnSelectSubmit(this TouchEventSelect touchEventSelect, Action<BaseEventData> TouchEvent)
        {
            touchEventSelect.SelectSubmit += TouchEvent;
            return touchEventSelect;
        }

        public static TouchEventSelect OnSelectCancel(this TouchEventSelect touchEventSelect, Action<BaseEventData> TouchEvent)
        {
            touchEventSelect.SelectCancel += TouchEvent;
            return touchEventSelect;
        }

        # endregion

    }
}