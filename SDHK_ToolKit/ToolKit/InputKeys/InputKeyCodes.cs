
/******************************

 * 作者: 闪电黑客

 * 日期: 2021/07/15 14:44:29

 * 最后日期: 。。。

 * 描述: 
    组合键类：通过int值代表按键码

    组合键功能：按下，抬起，按住。组合键相等
    
    新增鼠标滚轮：上:ScrollUp，下:ScrollDown

    

******************************/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputKeys
{
    /// <summary>
    /// 鼠标按键码：unity没有提供鼠标滚轮的按键码
    /// </summary>
    public enum MouseCode
    {
        None,
        /// <summary>
        /// 鼠标上滚
        /// </summary>
        ScrollUp = 1001,
        /// <summary>
        /// 鼠标下滚
        /// </summary>
        ScrollDown,
    }

    [Serializable]
    public class InputKeyCodes
    {
        /// <summary>
        /// 组合键限制数：用于限制绑定组合键的数量
        /// </summary>
        public int limit = 1;
        public List<int> Codes = new List<int>();

        /// <summary>
        /// 清空组合键列表
        /// </summary>
        public void Clear()
        {
            Codes.Clear();
        }



        /// <summary>
        /// 打印组合键名字
        /// </summary>
        public override string ToString()
        {
            string keysName = "[";

            if (Codes.Count > 0)
            {
                for (int i = 0; i < Codes.Count - 1; i++)
                {
                    keysName += ((Codes[i] > 1000) ? (MouseCode)Codes[i] + "+" : (KeyCode)Codes[i] + "+");
                }

                keysName += (Codes[Codes.Count - 1] > 1000 ? (MouseCode)Codes[Codes.Count - 1] + "]" : (KeyCode)Codes[Codes.Count - 1] + "]");
            }
            else
            {
                keysName += KeyCode.None + "]";
            }

            return keysName;
        }

        /// <summary>
        /// 判断组合键按住
        /// </summary>
        public bool IsKeys()
        {
            bool isKey = true;

            for (int i = 0; i < Codes.Count; i++)
            {
                if (!Codes[i].InputKey())
                {
                    isKey = false;
                }
            }
            return isKey;
        }

        /// <summary>
        /// 判断组合键按下：条件是最后一键为最后按下的，才生效
        /// </summary>
        public bool IsKeysDown()
        {
            bool isKey = true;
            for (int i = 0; i < Codes.Count - 1; i++)
            {
                if (!Codes[i].InputKey())
                {
                    isKey = false;
                }
            }
            return isKey && Codes[Codes.Count - 1].InputKeyDown();
        }

        /// <summary>
        /// 判断组合键抬起：与按下判断没有关联,全部对应键按下后，任意键抬起即生效。即按下判断未触发也能触发抬起
        /// </summary>
        public bool IsKeysUp()
        {
            int iskey = 0;
            int isKeyUp = 0;

            for (int i = 0; i < Codes.Count; i++)
            {
                if (Codes[i].InputKey())
                {
                    iskey++;
                }
                else if (Codes[i].InputKeyUp())
                {
                    isKeyUp = 1;
                }
            }

            if (iskey == Codes.Count - 1)
            {
                return Codes.Count == (isKeyUp + iskey);
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 判断组合键列表相等：列表遍历对比
        /// </summary>
        public bool IsEqual(InputKeyCodes rhs)
        {
            bool status = true;
            if (rhs != null)
                if (Codes.Count == rhs.Codes.Count)
                {
                    for (int i = 0; i < Codes.Count; i++)
                    {
                        if (Codes[i].ToString() != rhs.Codes[i].ToString())
                        {
                            status = false;
                        }
                    }
                }
                else
                {
                    status = false;
                }

            return status;
        }
    }

}