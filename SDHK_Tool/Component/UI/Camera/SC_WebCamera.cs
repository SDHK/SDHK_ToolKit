using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using SDHK_Tool.Static;
using UnityEngine;
using UnityEngine.UI;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.9.27
 *
 * 2020.8.1 添加自适应与画面旋转
 * 
 * 功能：调用照相机
 */
namespace SDHK_Tool.Component
{
    /// <summary>
    /// 用于调用摄像机拍照
    /// </summary>
    [DisallowMultipleComponent]//不允许多个
    public class SC_WebCamera : MonoBehaviour
    {
        /// <summary>
        /// 根目录
        /// </summary>
        [Tooltip("根目录")]
        public FileLocation fileLocation;

        /// <summary>
        /// 文件路径
        /// </summary>
        [Tooltip("文件路径")]
        public string FliePath = "CamInfo.json";


        /// <summary>
        /// 启动后拍摄
        /// </summary>
        [Tooltip("启动后拍摄")]
        public bool PlayOnAwake = true;

        /// <summary>
        /// 显示偏转角度
        /// </summary>
        [Tooltip("显示图片偏转角度")]
        public float ImageAngle = 0;

        /// <summary>
        /// 画面翻转
        /// </summary>
        [Tooltip("画面翻转")]
        public TextureFlip Flip = TextureFlip.无翻转;

        /// <summary>
        /// 照相尺寸：设置一个大概像素（例如1920*1080）组件将自动匹配设备像素
        /// </summary>
        [Tooltip("照相尺寸：设置一个大概像素（例如1920*1080）\n组件将自动匹配设备像素")]
        public Vector2 CameraSize = new Vector2(1920, 1080);


        /// <summary>
        /// 拍照帧数
        /// </summary>
        [Tooltip("拍照帧数")]
        public int CameraFPS = 30;

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

        /// <summary>
        /// 拍摄的画面 
        /// </summary>
        [System.NonSerialized]
        public WebCamTexture webCamera;

        /// <summary>
        /// 用于显示的Raw
        /// </summary>
        [System.NonSerialized]
        public RawImage rawImage;

        private void Awake()
        {
            GameObject ImageObject = new GameObject("RamImage");
            rawImage = ImageObject.AddComponent<RawImage>();
            ImageObject.transform.SetParent(transform);
            ImageObject.transform.localPosition = Vector3.zero;

#if UNITY_EDITOR //编辑器模式

            Set_File(); //编辑模式下运行一次刷新一次文件
            Get_File(); //运行模式下启动时读取文件设置

#else //非编辑器模式

            Get_File (); //运行模式下启动时读取文件设置

#endif

        }

        // Use this for initialization
        void Start()
        {
            Initialize();
        }

        /// <summary>
        /// 设置外部文件
        /// </summary>
        [ContextMenu("设置外部文件")]
        private void Set_File()
        {
            string fliePath = SS_File.GetPath(fileLocation) + "/" + FliePath; //文件路径

            JsonData jsonData = new JsonData();

            jsonData["PlayOnAwake"] = PlayOnAwake;
            jsonData["ImageAngle"] = ImageAngle;
            jsonData["Flip"] = Flip.ToString();
            jsonData["CameraSize"] = JsonMapper.ToObject(JsonMapper.ToJson(CameraSize));
            jsonData["CameraFPS"] = CameraFPS;
            jsonData["DeviceIndex"] = DeviceIndex;


            SS_File.SetFile_JsonObject_Format<JsonData>(jsonData, fliePath);
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        private void Get_File()
        {
            string fliePath = SS_File.GetPath(fileLocation) + "/" + FliePath; //文件路径
            if (!SS_File.FilePath_IF(fliePath))
            {
                Set_File();
            }
            else
            {
                UnityEngine.Debug.Log("读取摄像文件");
                JsonData jsonData = SS_File.GetFile_JsonObject(fliePath);
                Flip = (TextureFlip)Enum.Parse(typeof(TextureFlip), (string)jsonData["Flip"]);

                ImageAngle = (float)(double)jsonData["ImageAngle"];

                PlayOnAwake = (bool)jsonData["PlayOnAwake"];
                CameraSize = JsonMapper.ToObject<Vector2>(JsonMapper.ToJson(jsonData["CameraSize"]));

                CameraFPS = (int)jsonData["CameraFPS"];
                DeviceIndex = (int)jsonData["DeviceIndex"];
            }
        }


        /// <summary>
        /// 刷新摄像机
        /// </summary>
        [ContextMenu("刷新摄像机")]
        public void Initialize()
        {
            Application.RequestUserAuthorization(UserAuthorization.WebCam);
            if (Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                //获取设备列表
                devices = WebCamTexture.devices;
                //限制
                DeviceIndex = Mathf.Clamp(DeviceIndex, 0, devices.Length - 1);

                if (webCamera != null) webCamera.Stop();
                webCamera = new WebCamTexture(devices[DeviceIndex].name, (int)CameraSize.x, (int)CameraSize.y, (int)CameraFPS);
                if (PlayOnAwake) webCamera.Play();

                Texture2D texture2D = Get_CamSize();//获取一次图片，拿到摄像机的分辨率
                webCamera.requestedHeight = texture2D.height;//重置拍摄获取的分辨率
                webCamera.requestedWidth = texture2D.width;

                rawImage.texture = webCamera;

                //显示画面自适应计算
                Vector2 RectFormat = SS_Mathf.Rect_ProperFormat_Min(
                    new Vector2(texture2D.width, texture2D.height),
                    new Vector2(((RectTransform)transform).rect.width, ((RectTransform)transform).rect.height)
                );

                ((RectTransform)rawImage.transform).sizeDelta = new Vector2(RectFormat.x, RectFormat.y);

                // 显示翻转
                switch (Flip)
                {
                    case TextureFlip.无翻转: rawImage.transform.localEulerAngles = Vector3.zero; break;
                    case TextureFlip.顺时针90度: rawImage.transform.localEulerAngles = new Vector3(0, 0, -ImageAngle - 90); break;
                    case TextureFlip.逆时针90度: rawImage.transform.localEulerAngles = new Vector3(0, 0, -ImageAngle + 90); break;
                    case TextureFlip.旋转180度: rawImage.transform.localEulerAngles = new Vector3(0, 0, -ImageAngle + 180); break;
                    case TextureFlip.水平翻转: rawImage.transform.localEulerAngles = new Vector3(0, 180, ImageAngle); break;
                    case TextureFlip.顺时针水平翻转: rawImage.transform.localEulerAngles = new Vector3(0, 180, ImageAngle + -90); break;
                    case TextureFlip.逆时针水平翻转: rawImage.transform.localEulerAngles = new Vector3(0, 180, ImageAngle + 90); break;
                    case TextureFlip.垂直翻转: rawImage.transform.localEulerAngles = new Vector3(180, 0, ImageAngle); break;
                }

                Debug.Log(devices[DeviceIndex].name + " [" + texture2D.width + ":" + texture2D.height + "]");
            }
        }

        private Texture2D Get_CamSize()
        {
            Texture2D texture2D = new Texture2D(webCamera.width, webCamera.height, TextureFormat.ARGB32, true);
            texture2D.SetPixels(webCamera.GetPixels());
            texture2D.Apply();
            return texture2D;
        }


        /// <summary>
        /// 切换设备
        /// </summary>
        public void SwitchDevice()
        {
            DeviceIndex++;
            DeviceIndex %= devices.Length;
            Initialize();
        }

        /// <summary>
        /// 获取照片 
        /// </summary>
        /// <returns>获取到的图形Texture2D</returns>
        public Texture2D Get_Texture()
        {
            if (webCamera.isPlaying)
            {
                webCamera.Pause();//暂停 获取不会出现照片错位 
                Texture2D texture2D = SS_Texture.Texture2D_Flip(Get_CamSize(), Flip);//图片翻转 
                webCamera.Play();
                return texture2D;
            }
            else
            {
                return SS_Texture.Texture2D_Flip(Get_CamSize(), Flip);
            }
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