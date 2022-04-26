using System.Collections;
using System.Collections.Generic;
using SDHK_Tool.Component;
using SDHK_Tool.Static;
using UnityEngine;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.11.20
 * 
 * 功能：滚动列表回弹器
 * 
 * 需要SC_TouchTransform计算器，并挂载SC_TouchMotor，其移动目标为SC_ScrollGroup物体
 *
 */



namespace SDHK_Tool.Component
{
    /// <summary>
    /// 滚动列表触摸回弹器
    /// </summary>
    public class SC_ScrollTouch_SpringBack : MonoBehaviour
    {
        /// <summary>
        /// 触摸位置计算器
        /// </summary>
        [Tooltip("触摸位置计算器")]

        public SC_TouchTransform touchTransform;

        /// <summary>
        /// 回弹灵敏度
        /// </summary>
        [Tooltip("回弹灵敏度")]
        public float SpringSensitivity = 0.5f;


        private SC_ScrollGroup scrollGroup; //滚动列表

        private SC_TouchEvent_Down touchEvent_Down; //点击事件监听器

        private bool isSpringBack = false;  //回弹标记

        private Vector3 PositionLate;   //上一帧位置



        // Use this for initialization
        void Start()
        {
            if (touchTransform != null)
            {
                if (touchTransform.TouchObject.GetComponent<SC_ScrollGroup>() == null) return;

                scrollGroup = touchTransform.TouchObject.GetComponent<SC_ScrollGroup>();

                touchEvent_Down = (touchTransform.gameObject.GetComponent<SC_TouchEvent_Down>() != null)
                ? touchTransform.gameObject.GetComponent<SC_TouchEvent_Down>()
                : touchTransform.gameObject.AddComponent<SC_TouchEvent_Down>()
                ;
                touchEvent_Down.TouchOnUp += OnUp;

                PositionLate = touchTransform.TouchObject.transform.localPosition;
            }
        }

        public void OnUp()
        {
            if (touchTransform.TouchMotor != null && touchEvent_Down.TouchPool.Count <= 1)
            {
                isSpringBack = true;
            }
        }

        public void Spring()
        {
            touchTransform.TouchMotor.Refresh();
            touchTransform.Calculation_Position.x = SS_Mathf.Recent_Value(touchTransform.TouchObject.transform.localPosition.x, scrollGroup.GroupBox.x);
            touchTransform.Calculation_Position.y = SS_Mathf.Recent_Value(touchTransform.TouchObject.transform.localPosition.y, scrollGroup.GroupBox.y);
            isSpringBack = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (isSpringBack)
            {
                if ((touchTransform.TouchObject.transform.localPosition - PositionLate).magnitude < SpringSensitivity)//判断距离
                {
                    Spring();
                }
            }

            PositionLate = touchTransform.TouchObject.transform.localPosition;
        }

    }

}