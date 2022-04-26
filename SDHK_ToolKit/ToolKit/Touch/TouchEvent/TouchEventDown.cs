using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Touch
{
    public class TouchEventDown : MonoBehaviour

        , IPointerDownHandler
        , IPointerUpHandler
        , IPointerClickHandler

        , IBeginDragHandler
        , IDragHandler

        , IScrollHandler

    {

        public bool ignoreMouse = false;//忽略鼠标

        public bool ignoreDrag = false; //忽略拖拽影响

        public float touchTime = 0.5f;  //触摸时间

        [SerializeField]
        private bool startTime = false;
        [SerializeField]
        private bool isLongDown = false;


        public TouchPool touchPool = new TouchPool();

        public Action<PointerEventData> Down;
        public Action<PointerEventData> LongDown;

        public Action<PointerEventData> DragUpdate;   //拖拽中


        public Action<PointerEventData> Up;
        public Action<PointerEventData> ShortUp;
        public Action<PointerEventData> LongUp;



        public Action<PointerEventData> Click;
        public Action<PointerEventData> ShortClick;
        public Action<PointerEventData> LongClick;

        public Action<float> ScrollUpdate;  //滚轮事件



        public Action Stay;
        public Action ShortStay;
        public Action LongStay;



        private DateTime timeClock = DateTime.MinValue;



        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!ignoreDrag) startTime = false;//拖拽事件会终止计时
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (ignoreMouse && eventData.pointerId < 0) return;
            if (DragUpdate != null) DragUpdate(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (ignoreMouse && eventData.pointerId < 0) return;//鼠标忽略
            if (touchPool.Contains(eventData)) return;//存在则忽略

            touchPool.Add(eventData);

            if (Down != null) Down(eventData);

            //===

            if (touchPool.Count == 1)//第一个触摸点击时启动计时
            {
                startTime = true;
                isLongDown = false;
                timeClock = DateTime.Now;
            }

        }



        public void OnPointerUp(PointerEventData eventData)
        {
            if (ignoreMouse && eventData.pointerId < 0) return;//鼠标忽略
            if (!touchPool.Contains(eventData)) return;//不存在则忽略

            touchPool.Remove(eventData);

            if (Up != null) Up(eventData);

            //===

            if (touchPool.Count == 0)//最后一个触摸抬起后关闭计时
            {
                startTime = false;
                timeClock = DateTime.MinValue;

                if (!isLongDown && ShortUp != null) ShortUp(eventData);
                if (isLongDown && LongUp != null) LongUp(eventData);
            }

        }

        public void OnPointerClick(PointerEventData eventData)
        {

            if (ignoreMouse && eventData.pointerId < 0) return;//鼠标忽略
            if (Click != null) Click(eventData);

            //===

            if (touchPool.Count == 0)//最后一个触摸抬起后
            {
                if (!isLongDown && ShortClick != null) ShortClick(eventData);
                if (isLongDown && LongClick != null) LongClick(eventData);
            }

        }

        public void OnScroll(PointerEventData eventData)
        {
            if (ignoreMouse && eventData.pointerId < 0) return;
            if (ScrollUpdate != null) ScrollUpdate(eventData.scrollDelta.y);
        }





        // Update is called once per frame
        void Update()
        {
            if (startTime)
            {
                if (!isLongDown && Clock_System(timeClock, touchTime))
                {
                    isLongDown = true;
                    if (LongDown != null) LongDown(touchPool[0]);
                }

                if (Stay != null) Stay();
                if (!isLongDown && ShortStay != null) ShortStay();
                if (isLongDown && LongStay != null) LongStay();
            }

        }



        /// <summary>
        /// 计时器：判断系统时间:DateTime.Now
        /// </summary>
        /// <param name="StartTime">计时位</param>
        /// <param name="TargetSeconds">定时位（秒）</param>
        /// <returns>return :　触发标记</returns>
        private bool Clock_System(DateTime StartTime, float TargetSeconds)
        {
            return ((DateTime.Now - StartTime).TotalSeconds > TargetSeconds);
        }


    }

}