
/******************************

 * Author: 闪电黑客

 * 日期: 2021/10/19 19:42:54

 * 最后日期: 2021/12/21 10:30:59

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System.Collections;
using System.Collections.Generic;
using ObjectFactory;
using UnityEngine;
using XLua;


namespace WindowUI
{
    public static class WindowBaseExtension
    {
        private static WindowManager windowManager = WindowManager.Instance();

        /// <summary>
        /// 请求管理器通过对象池生成窗口并显示
        /// </summary>
        public static T WindowShow<T>(this ObjectPool<T> monoObject)
        where T : WindowBase
        {
            if (windowManager.IsDone)
            {
                var window = monoObject.Get();
                windowManager.Show(window);
                return window;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 请求管理器关闭窗口
        /// </summary>
        public static void WindowClose(this IWindow window)
        {
            windowManager.Close(window);
        }

    }


}


