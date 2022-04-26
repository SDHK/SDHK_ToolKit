using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ScreenResolution
{
    /// <summary>
    /// 屏幕模式
    /// </summary>
    public enum ScreenMode
    {
        全屏,
        窗口,
        无边框窗口,
        边框窗口,
        黑边全屏
    }

    /// <summary>
    /// 屏幕数据
    /// </summary>
    [Serializable]
    public class ScreenData
    {
        /// <summary>
        /// 窗口模式
        /// </summary>
        [Tooltip("窗口模式")]
        public ScreenMode mode = ScreenMode.全屏;
        public bool isTop = false;

        /// <summary>  
        /// 窗口位置：X轴 ,0点在左上 
        /// </summary>  
        [Tooltip("窗口位置：X轴 ,0点在左上")]
        public int x = 0;
        /// <summary>  
        /// 窗口位置：Y轴 ,0点在左上
        /// </summary>  
        [Tooltip("窗口位置：Y轴 ,0点在左上")]
        public int y = 0;
        /// <summary>  
        /// 窗口宽度  
        /// </summary>  
        [Tooltip("窗口宽度")]
        public int width = 1920;
        /// <summary>  
        /// 窗口高度  
        /// </summary>  
        [Tooltip("窗口高度")]
        public int height = 1080;
    }
}