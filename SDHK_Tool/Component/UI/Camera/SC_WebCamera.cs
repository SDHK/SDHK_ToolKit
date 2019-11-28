using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.9.27
 * 
 * 功能：调用照相机
 */

namespace SDHK_Tool.Component
{
    /// <summary>
    /// 用于调用摄像机拍照
    /// </summary>

    public class SC_WebCamera : MonoBehaviour
    {
        /// <summary>
        /// 渲染到图片
        /// </summary>
        [Tooltip("播放组件")]
        public RawImage rawImage;

        /// <summary>
        /// 照相尺寸
        /// </summary>
        [Tooltip("照相尺寸")]
        public Vector2 CameraSize;

        /// <summary>
        /// 拍照帧数
        /// </summary>
        [Tooltip("拍照帧数")]
        public float CameraFPS;

        /// <summary>
        /// 接收返回的图片数据  
        /// </summary>
        public WebCamTexture webCamera;

        /// <summary>
        /// 选择设备序号
        /// </summary>
        [Tooltip("选择设备序号")]
        public int DeviceIndex = 0;

        /// <summary>
        /// 设备列表
        /// </summary>
        [Tooltip("设备列表")]
        public WebCamDevice[] devices;

        // Use this for initialization
        void Start()
        {
            rawImage = GetComponent<RawImage>();
            WebCamDevice[] devices = WebCamTexture.devices;
            DeviceIndex = Mathf.Clamp(DeviceIndex, 0, devices.Length - 1);

            try
            {
                Initialize(); webCamera.Stop();
            }
            catch { }
        }

        /// <summary>
        /// 初始化摄像机
        /// </summary>
        public void Initialize()
        {
            Application.RequestUserAuthorization(UserAuthorization.WebCam);
            if (Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                webCamera = new WebCamTexture(devices[DeviceIndex].name, (int)CameraSize.x, (int)CameraSize.y, (int)CameraFPS);
                if (rawImage != null) rawImage.texture = webCamera;
                webCamera.Play();
            }
        }


        /// <summary>
        /// 切换设备
        /// </summary>
        public void SwitchDevice()
        {
            DeviceIndex++;
            DeviceIndex %= devices.Length;
            CameraStop();
            Initialize();
        }

        /// <summary>
        /// 获取照片 ：暂停后获取就不会出现照片错位 
        /// </summary>
        /// <returns>获取到的图形Texture2D</returns>
        public Texture2D Get_Texture2D()
        {
            Texture2D texture2D = new Texture2D(webCamera.width, webCamera.height, TextureFormat.ARGB32, true);
            texture2D.SetPixels(webCamera.GetPixels());
            texture2D.Apply();
            return texture2D;
        }

        /// <summary>
        /// 摄像机开始摄像
        /// </summary>
        public void CameraPlay()
        {
            webCamera.Play();
        }

        /// <summary>
        /// 摄像机关闭摄像
        /// </summary>
        public void CameraStop()
        {
            webCamera.Stop();
        }

        /// <summary>
        /// 摄像机暂停摄像
        /// </summary>
        public void CameraPause()
        {
            webCamera.Pause();
        }

    }

}