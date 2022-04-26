


using System;
using UnityEngine;
using UnityEngine.EventSystems;



namespace Touch
{
    public class TouchEventEnter : MonoBehaviour
        , IPointerEnterHandler
        , IPointerExitHandler

    {

        public bool ignoreMouse = false;//忽略鼠标

        public float touchTime = 0.5f;  //触摸时间


        [SerializeField]
        private bool startTime = false;
        [SerializeField]
        private bool isLongEnter = false;


        public TouchPool touchPool = new TouchPool();



        public Action<PointerEventData> Enter;         //进入
        public Action<PointerEventData> LongEnter;     //长进入


        public Action<PointerEventData> Exit;         //离开
        public Action<PointerEventData> ShortExit;    //短停留后离开
        public Action<PointerEventData> LongExit;     //长停留后离开


        public Action Inside;             //停留
        public Action ShortInside;        //短停留
        public Action LongInside;         //长停留

        private DateTime timeClock = DateTime.MinValue;



        // Start is called before the first frame update
        void Start()
        {

        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (ignoreMouse && eventData.pointerId < 0) return;
            if (touchPool.Contains(eventData)) return;

            touchPool.Add(eventData);

            if (Enter != null) Enter(eventData);

            //===


            if (touchPool.Count == 1)
            {
                startTime = true;
                isLongEnter = false;
                timeClock = DateTime.Now;
            }

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (ignoreMouse && eventData.pointerId < 0) return;
            if (!touchPool.Contains(eventData)) return;

            touchPool.Remove(eventData);

            if (Exit != null) Exit(eventData);

            //===

            if (touchPool.Count == 0)
            {
                startTime = false;
                timeClock = DateTime.MinValue;

                if (!isLongEnter && ShortExit != null) ShortExit(eventData);
                if (isLongEnter && LongExit != null) LongExit(eventData);
            }

        }


        // Update is called once per frame
        void Update()
        {
            if (startTime)
            {
                if (!isLongEnter && Clock_System(timeClock, touchTime))
                {
                    isLongEnter = true;
                    if (LongEnter != null) LongEnter(touchPool[0]);
                }

                if (Inside != null) Inside();
                if (!isLongEnter && ShortInside != null) ShortInside();
                if (isLongEnter && LongInside != null) LongInside();
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