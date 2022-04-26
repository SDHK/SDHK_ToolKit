using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using SDHK_Extension;
using Singleton;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using AsyncAwaitEvent;
using System.Threading.Tasks;

namespace ScreenResolution
{

    public class ScreenManager : SingletonMonoBase<ScreenManager>
    {

        #region GUI
        private bool GUIMax = false;
        private float size = 1;
        private int fontSize = 14;
        private int SpaceSize = 4;
        private float timeClock = -1;

        private GUILayoutOption BtnWidth;
        private GUILayoutOption BtnHeight;
        private GUIStyle BtnStyle = new GUIStyle();
        private Rect _windowRect = new Rect(0, 0, 100, 60);


        #endregion


        private IntPtr CurrentWindowHandle;

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                UnityEngine.Debug.Log("WindowModel 启动！");
                Load();
            }
            BtnStyle.alignment = TextAnchor.MiddleCenter;
            BtnStyle.normal.textColor = Color.white;
            BtnStyle.normal.background = Texture2D.grayTexture;
        }


        public bool AllowShow = false;

        public bool isDone = true;

        [Space()]

        public ApplicationPathEnum RootPath = ApplicationPathEnum.Path;
        [Space()]
        public string path = "";



        private ScreenData dataMemento;//备份
        private UnityWebRequest request;

        public ScreenData data;


        /// <summary>
        /// 事件：加载
        /// </summary>
        public event Action<UnityWebRequestAsyncOperation> EventLoad;

        /// <summary>
        /// 事件：加载完成
        /// </summary>
        public event Action<UnityWebRequest> EventLoadDone;

        /// <summary>
        /// 事件：数据更新
        /// </summary>
        public event Action<ScreenManager> EventDataUpdate;



        public string FullPath()
        {
            return ApplicationExtension.GetPath(RootPath) + path;
        }

        /// <summary>
        /// 加载
        /// </summary>
        public UnityWebRequestAsyncOperation Load()
        {
            return Load(FullPath());
        }

        /// <summary>
        /// 加载：完整路径
        /// </summary>
        public UnityWebRequestAsyncOperation Load(string uri)
        {
            if (isDone)
            {
                var asyncOperation = UnityWebRequest.Get(uri).SendWebRequest();
                AsyncLoad(asyncOperation);
                return asyncOperation;
            }
            else
            {
                return null;
            }
        }

        private async void AsyncLoad(UnityWebRequestAsyncOperation asyncOperation)
        {
            isDone = false;
            EventLoad?.Invoke(asyncOperation);

            await asyncOperation;
            var request = asyncOperation.webRequest;
            if (request.isHttpError || request.isNetworkError)
            {
                UnityEngine.Debug.Log("ScreenManager Load Error");
            }
            else
            {
                data = Convert_BytesToObject<ScreenData>(request.downloadHandler.data);
                dataMemento = DeepCopy(data);
                Refresh();
            }

            isDone = true;
            EventLoadDone?.Invoke(request);
        }



        /// <summary>
        /// 恢复
        /// </summary>
        public void RestoreData()
        {
            data = DeepCopy(dataMemento);
            Refresh();
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            if (Path.IsPathRooted(FullPath()))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(FullPath()));//如果文件夹不存在就创建它
                File.WriteAllBytes(FullPath(), Convert_ObjectToBytes(data));
                dataMemento = DeepCopy(data);
            }
        }

        /// <summary>
        /// 刷新画面
        /// </summary>
        public void Refresh()
        {
            EventDataUpdate?.Invoke(this);

#if UNITY_EDITOR //编辑器模式
#else //非编辑器模式
           ScreenSwitch();
#endif
        }

        private async void ScreenSwitch()
        {
            Screen.fullScreen = false;//窗口化用于刷新

            CurrentWindowHandle = ScreenDLL.GetCurrentWindowHandle();

            await Task.Yield();

            switch (data.mode)
            {
                case ScreenMode.全屏:
                    {
                        data.width = Screen.currentResolution.width;
                        data.height = Screen.currentResolution.height;
                        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true); //全屏
                    }
                    break;
                case ScreenMode.窗口:
                    {
                        SetWindow(ScreenDLL.WS_T1);
                    }
                    break;
                case ScreenMode.无边框窗口:
                    {
                        SetWindow(0);
                    }
                    break;
                case ScreenMode.边框窗口:
                    {
                        SetWindow(ScreenDLL.WS_T0);
                    }
                    break;
                case ScreenMode.黑边全屏:
                    {
                        ScreenAutoFit();
                        Screen.SetResolution(data.width, data.height, true); //全屏
                    }
                    break;

            }

        }

        private void SetWindow(int dwNewLong)
        {
            ScreenDLL.SetWindowLong(CurrentWindowHandle, ScreenDLL.GWL_STYLE, dwNewLong);//窗口化
            ScreenDLL.SetWindowPos(CurrentWindowHandle, data.isTop ? ScreenDLL.HWND_TOPMOST : ScreenDLL.HWND_NOTOPMOST, data.x, data.y, data.width, data.height, ScreenDLL.SWP_SHOWWINDOW);
        }
        // ShowWindow(FindWindow("Shell_TrayWnd", null), 5);//显示任务栏
        // ShowWindow(FindWindow("Shell_TrayWnd", null), 0);//隐藏任务栏


        public void ScreenAutoFit()//比例适应
        {
            Vector2 ScreenRect = new Vector2(data.width, data.height);
            Vector2 SystemRect = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
            ScreenRect = RectProperFormatMax(ScreenRect, SystemRect);

            data.width = (int)ScreenRect.x;
            data.height = (int)ScreenRect.y;
        }

        /// <summary>
        /// 矩形计算器：将一个矩形按照比例缩放到合适的尺寸
        /// </summary>
        /// <param name="Origin">要缩放的矩形长宽</param>
        /// <param name="Reference">长宽最大范围</param>
        /// <returns>最大长宽的合适尺寸</returns>
        private static Vector2 RectProperFormatMax(Vector2 Origin, Vector2 Reference)
        {
            float AspectRatio = Origin.x / Origin.y;//获取长宽比

            if (Reference.x / Reference.y <= AspectRatio)
            {
                Reference.y = Reference.x / AspectRatio;
            }
            else
            {
                Reference.x = Reference.y * AspectRatio;
            }
            return Reference;
        }


        /// <summary>
        /// 将对象序列化为二进制数据:对象定义时需[Serializable]序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>byte数组</returns>
        private static byte[] Convert_ObjectToBytes<T>(T obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, obj);
            byte[] data = stream.ToArray();
            stream.Close();
            return data;
        }

        /// <summary>
        /// 将二进制数据反序列化
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>对象</returns>
        private static T Convert_BytesToObject<T>(byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            T obj = (T)bf.Deserialize(stream);
            stream.Close();
            return obj;
        }


        /// <summary>
        /// 通过二进制序列化深拷贝
        /// </summary>
        /// <param name="obj">拷贝对象</param>
        /// <returns>返回对象</returns>
        private static T DeepCopy<T>(T obj)
        {
            object NewObj;
            MemoryStream stream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            //序列化成流
            bf.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);//设置当前流正在读取的位置 为开始位置即从0开始

            //反序列化成对象
            NewObj = bf.Deserialize(stream);
            stream.Close();//关闭流

            return (T)NewObj;
        }


        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.BackQuote))
            {
                AllowShow = !AllowShow;
            }

            if (timeClock != -1)
            {
                if (timeClock > 0)
                {
                    timeClock -= Time.deltaTime;
                }
                else
                {
                    timeClock = -1;
                    RestoreData();
                }
            }
        }

        private void OnGUI()
        {
            if (AllowShow)
            {
                BtnStyle.fontSize = (int)(fontSize * size);
                SpaceSize = (int)(2 * size);

                if (GUIMax)
                {
                    _windowRect = GUI.Window(this.GetHashCode(), _windowRect, GUIWindowMax, "ScreenConsole");
                }
                else
                {
                    _windowRect = GUI.Window(this.GetHashCode(), _windowRect, GUIWindowMin, "ScreenConsole");
                }
            }

        }

        private void GUIWindowMin(int windowId)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            _windowRect.width = 100 * size;
            _windowRect.height = 60 * size;

            BtnWidth = GUILayout.Width(100 * size);
            BtnHeight = GUILayout.Height(30 * size);

            GUI.contentColor = (data.isTop ? Color.green : Color.white);
            if (GUILayout.Button("屏幕控制", BtnStyle, BtnHeight))
            {
                GUIMax = true;
            }
            GUI.contentColor = Color.white;
        }

        private void GUIWindowMax(int windowId)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            _windowRect.width = 150 * size;
            _windowRect.height = 300 * size;

            BtnWidth = GUILayout.Width(80 * size);
            BtnHeight = GUILayout.Height(20 * size);

            GUI.contentColor = (data.isTop ? Color.green : Color.white);
            if (GUILayout.Button("屏幕控制", BtnStyle, BtnHeight))
            {
                GUIMax = false;
            }

            GUILayout.Space(SpaceSize);
            GUILayout.Space(SpaceSize);
            GUILayout.Space(SpaceSize);

            GUI.contentColor = Color.white;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+", BtnStyle, BtnHeight))
            {
                size += 0.5f;
            }

            GUILayout.Space(SpaceSize);

            if (GUILayout.Button("-", BtnStyle, BtnHeight))
            {
                if (size > 1) size -= 0.5f;
            }
            GUILayout.EndHorizontal();


            GUILayout.Space(SpaceSize);

            GUILayout.Space(SpaceSize);

            foreach (ScreenMode screenMode in Enum.GetValues(typeof(ScreenMode)))
            {
                GUI.contentColor = (data.mode == screenMode ? Color.green : Color.white);
                if (GUILayout.Button(screenMode.ToString(), BtnStyle, BtnHeight))
                {
                    data.mode = screenMode;
                }
                GUILayout.Space(SpaceSize);
            }
            GUILayout.Space(SpaceSize);
            GUILayout.Space(SpaceSize);

            GUI.contentColor = (data.isTop ? Color.green : Color.white);
            if (GUILayout.Button("顶置", BtnStyle, BtnHeight))
            {
                data.isTop = !data.isTop;
            }
            GUI.contentColor = Color.white;

            GUILayout.Space(SpaceSize);
            GUILayout.Space(SpaceSize);
            GUILayout.Space(SpaceSize);

            GUILayout.BeginHorizontal();

            data.x = int.Parse(GUILayout.TextField(data.x.ToString(), BtnStyle));

            GUILayout.Space(SpaceSize);

            data.y = int.Parse(GUILayout.TextField(data.y.ToString(), BtnStyle));

            GUILayout.EndHorizontal();

            GUILayout.Space(SpaceSize);

            GUILayout.BeginHorizontal();

            data.width = int.Parse(GUILayout.TextField(data.width.ToString(), BtnStyle));

            GUILayout.Space(SpaceSize);

            data.height = int.Parse(GUILayout.TextField(data.height.ToString(), BtnStyle));

            GUILayout.EndHorizontal();

            GUILayout.Space(SpaceSize);
            GUILayout.Space(SpaceSize);
            GUILayout.Space(SpaceSize);



            if (timeClock == -1)
            {
                if (GUILayout.Button("确定", BtnStyle, BtnHeight))
                {
                    Refresh();
                    timeClock = 9;
                }

                GUILayout.Space(SpaceSize);

                if (GUILayout.Button("重载", BtnStyle, BtnHeight))
                {
                    Load();
                }
            }
            else
            {
                if (GUILayout.Button("取消:" + timeClock.ToString("0"), BtnStyle, BtnHeight))
                {
                    timeClock = -1;
                    RestoreData();
                }

                GUILayout.Space(SpaceSize);

                if (GUILayout.Button("保存", BtnStyle, BtnHeight))
                {
                    timeClock = -1;
                    Save();
                }
            }

        }

    }
}