using System.Collections.Generic;
using UnityEngine;
using SDHK_Tool.Static;
using UnityEngine.EventSystems;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.12.20
 *
 * 功能：TUIO触摸联动器
 *
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// TUIO触摸联动器
    /// </summary>
    public class SC_TUIO_Touch : MonoBehaviour
    {

        /// <summary>
        /// TUIO事件监听器
        /// </summary>
        [Tooltip("TUIO事件监听器")]
        public SC_TUIO_Listener tuio_Listener;

        [Space()]

        /// <summary>
        /// 触摸穿透器
        /// </summary>
        [Tooltip("触摸穿透器：挂上则可触发触摸事件")]
        public SC_TouchEvent_RayCast TouchRayCast;

        /// <summary>
        /// 触摸字典[无序]
        /// </summary>
        public Dictionary<int, PointerEventData> TouchPool = new Dictionary<int, PointerEventData>();

        // Use this for initialization
        void Start()
        {
            if (tuio_Listener != null && TouchRayCast != null)
            {
                tuio_Listener.TuioEnter = TouchEnter;
                tuio_Listener.TuioExit = TouchExit;
                tuio_Listener.TuioStay = TouchStay;
            }
        }

        public void TouchEnter(List<int> Ids)
        {
            foreach (var Id in Ids)
            {
                if (!TouchPool.ContainsKey(Id))
                {
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                    pointerEventData.pointerId = Id;
                    pointerEventData.position = tuio_Listener.Tuio_PointPool[Id];

                    TouchPool.Add(Id, pointerEventData);

                    SS_Touch.OnEnter(TouchRayCast.gameObject, pointerEventData);
                    SS_Touch.OnDown(TouchRayCast.gameObject, pointerEventData);
                    SS_Touch.OnBeginDrag(TouchRayCast.gameObject, pointerEventData);
                }
            }
        }

        public void TouchStay(List<int> Ids)
        {
            foreach (var Id in Ids)
            {
                if (TouchPool.ContainsKey(Id))
                {
                    TouchPool[Id].position = tuio_Listener.Tuio_PointPool[Id];
                    SS_Touch.OnDrag(TouchRayCast.gameObject, TouchPool[Id]);
                }
            }
        }

        public void TouchExit(List<int> Ids)
        {
            foreach (var Id in Ids)
            {
                if (TouchPool.ContainsKey(Id))
                {
                    TouchPool[Id].position = tuio_Listener.Tuio_PointPool[Id];

                    SS_Touch.OnUp(TouchRayCast.gameObject, TouchPool[Id]);
                    SS_Touch.OnExit(TouchRayCast.gameObject, TouchPool[Id]);
                    SS_Touch.OnEndDrag(TouchRayCast.gameObject, TouchPool[Id]);

                    TouchPool.Remove(Id);
                }
            }
        }
    }
}