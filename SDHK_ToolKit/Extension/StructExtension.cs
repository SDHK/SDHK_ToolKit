
/******************************

 * Author: 闪电黑客

 * 日期: 2021/09/08 14:32:25

 * 最后日期: 2022/01/28 02:06:15

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace SDHK_Extension
{
    public static class StructExtension
    {

        public static Color A(this Color color, float a)
        {
            color.a = a;
            return color;
        }

    }
}