

/******************************

 * 作者: 闪电黑客

 * 日期: 2021/07/15 14:44:29

 * 最后日期: 。。。

 * 描述: 
    按键输入管理器

    int扩展：

    扩展了鼠标滚轮按键码

    

******************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputKeys
{
    public static class IntExtension
    {
        /// <summary>
        /// 判断对应枚举值按键返回数值
        /// </summary>
        public static float InputValue(this int inputCode)
        {
            float value = 0;
            switch ((MouseCode)inputCode)
            {
                case MouseCode.ScrollUp: value = Input.GetAxis("Mouse ScrollWheel"); break;
                case MouseCode.ScrollDown: value = Input.GetAxis("Mouse ScrollWheel"); break;

                default: value = Input.GetKey((KeyCode)inputCode) ? 1 : 0; break;
            }
            return value;
        }
        /// <summary>
        /// 判断对应枚举值按键按住
        /// </summary>
        public static bool InputKey(this int inputCode)
        {

            switch ((MouseCode)inputCode)
            {
                case MouseCode.ScrollUp: return Input.GetAxis("Mouse ScrollWheel") > 0;
                case MouseCode.ScrollDown: return Input.GetAxis("Mouse ScrollWheel") < 0;
            }
            return Input.GetKey((KeyCode)inputCode);
        }
        /// <summary>
        /// 判断对应枚举值按键按下
        /// </summary>
        public static bool InputKeyDown(this int inputCode)
        {
            switch ((MouseCode)inputCode)
            {
                case MouseCode.ScrollUp: return Input.GetAxis("Mouse ScrollWheel") > 0;
                case MouseCode.ScrollDown: return Input.GetAxis("Mouse ScrollWheel") < 0;
            }
            return Input.GetKeyDown((KeyCode)inputCode);
        }
        /// <summary>
        /// 判断对应枚举值按键抬起
        /// </summary>
        public static bool InputKeyUp(this int inputCode)
        {
            switch ((MouseCode)inputCode)
            {
                case MouseCode.ScrollUp: return Input.GetAxis("Mouse ScrollWheel") <= 0;
                case MouseCode.ScrollDown: return Input.GetAxis("Mouse ScrollWheel") >= 0;
            }
            return Input.GetKeyUp((KeyCode)inputCode);
        }
    }

}