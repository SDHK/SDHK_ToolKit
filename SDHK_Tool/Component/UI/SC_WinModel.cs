using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using LitJson;
using SDHK_Tool.Static;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.03.31
 *
 * 功能：无边框窗口，等比缩放
 *
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// 无边框窗口
    /// </summary>
    public class SC_WinModel : MonoBehaviour
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
        public string FliePath = "WinInfo.json";

        [Space()]
        [Space()]

        /// <summary>
        /// 屏幕模式
        /// </summary>
        public WinModel winModel = WinModel.自适应;

        [Space()]
        [Space()]

        [Tooltip("切换快捷键：按住")]
        public KeyCode ModelKey0 = KeyCode.LeftAlt;
        [Tooltip("切换快捷键：按下")]
        public KeyCode ModelKey1 = KeyCode.M;

        [Space()]
        [Space()]

        /// <summary>  
        /// 窗口位置：X轴 ,0点在左上 
        /// </summary>  
        [Tooltip("窗口位置：X轴 ,0点在左上")]
        public int winPosX;
        /// <summary>  
        /// 窗口位置：Y轴 ,0点在左上
        /// </summary>  
        [Tooltip("窗口位置：Y轴 ,0点在左上")]
        public int winPosY;

        /// <summary>  
        /// 窗口宽度  
        /// </summary>  
        [Tooltip("窗口宽度")]
        public int winWidth;
        /// <summary>  
        /// 窗口高度  
        /// </summary>  
        [Tooltip("窗口宽度")]
        public int winHeight;

        private Vector2 WinRect; //窗口大小

        private bool WinBit = true; //窗口模式标记器

        public delegate bool WNDENUMPROC(IntPtr hwnd, uint lParam); //枚举委托



        public enum WinModel //屏幕模式
        {
            自适应, //自适应
            原像素, //原像素
            全屏 //全屏
        }

        /// <summary>
        /// 枚举窗口
        /// </summary>
        /// <param name="lpEnumFunc"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, uint lParam);

        /// <summary>
        /// 获取指定窗口的父窗口句柄
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        /// <summary>
        /// 获取窗口线程进程
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpdwProcessId"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);

        /// <summary>
        /// 设置上次错误
        /// </summary>
        /// <param name="dwErrCode"></param>
        [DllImport("kernel32.dll")]
        public static extern void SetLastError(uint dwErrCode);

        /// <summary>
        /// 获取窗口样式？
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("user32", EntryPoint = "GetWindowLong")]
        private static extern uint GetWindowLong(IntPtr hwnd, int nIndex);

        /// <summary>
        /// 设置窗口样式？
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="_nIndex"></param>
        /// <param name="dwNewLong"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);

        /// <summary>
        /// 设置窗口位置和大小
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hWndInsertAfter"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        //hWndInsertAfter 参数可选值:
        // HWND_TOP = 0; {在前面}
        // HWND_BOTTOM = 1; {在后面}
        // HWND_TOPMOST = HWND(-1); {在前面, 位于任何顶部窗口的前面}
        // HWND_NOTOPMOST = HWND(-2); {在前面, 位于其他顶部窗口的后面}

        /// <summary>
        /// 获取系统最前端的窗口？
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 获取屏幕分辨率
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern IntPtr GetSystemMetrics(int nIndex);

        [DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
        private static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, int bAlpha, int dwFlags);

        public IntPtr CurrentWindowHandle;

        //===[不清楚的常量]========
        private const int SM_CXSCREEN = 0x00000000;//屏幕宽度
        private const int SM_CYSCREEN = 0x00000001;//屏幕高度

        private const uint SWP_SHOWWINDOW = 0x0040;
        
        private const int GWL_STYLE = -16;
        private const int WS_BORDER = 1;
        private const int WS_POPUP = 0x800000;

        private const int WS_EX_LAYERED = 0x80000;
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int GWL_EXSTYLE = -20;
        private const int LWA_ALPHA = 0;

        void Awake()
        {

            CurrentWindowHandle = GetCurrentWindowHandle();

#if UNITY_EDITOR //编辑器模式
            // Get_WinFile (); //运行模式下启动时读取文件设置窗口
            Set_WinFile(); //编辑模式下运行一次刷新一次文件
            Get_WinFile(); //运行模式下启动时读取文件设置窗口

#else //非编辑器模式

            Get_WinFile (); //运行模式下启动时读取文件设置窗口
            Set_WinScreen ();

#endif

        }

        /// <summary>
        /// 设置外部文件
        /// </summary>
        [ContextMenu("设置外部文件")]
        private void Set_WinFile()
        {
            string fliePath = SS_File.GetPath(fileLocation) + "/" + FliePath; //文件路径

            if (winWidth == 0 && winHeight == 0) //假如为0获取当前窗口尺寸
            {
                winWidth = Screen.width;
                winHeight = Screen.height;
            }
            //    winModel = (WinModel)Enum.Parse(typeof(WinModel), "winModel");
            //创建文件
            JsonData jsonData = new JsonData();
            jsonData["winModel"] = winModel.ToString();

            jsonData["winPosX"] = winPosX;
            jsonData["winPosY"] = winPosY;
            jsonData["winWidth"] = winWidth;
            jsonData["winHeight"] = winHeight;

            jsonData["ModelKey0"] = ModelKey0.ToString();
            jsonData["ModelKey1"] = ModelKey1.ToString();

            SS_File.SetFile_JsonObject_Format<JsonData>(jsonData, fliePath);
        }

        /// <summary>
        /// 读取窗口文件
        /// </summary>
        private void Get_WinFile()
        {
            string fliePath = SS_File.GetPath(fileLocation) + "/" + FliePath;
            if (!SS_File.FilePath_IF(fliePath)) //判断文件是否存在
            {
                //创建文件
                Set_WinFile();
            }
            else
            {
                //读取文件
                UnityEngine.Debug.Log("读取无边框文件");

                JsonData jsonData = SS_File.GetFile_JsonObject(fliePath);
                winModel = (WinModel)Enum.Parse(typeof(WinModel), (string)jsonData["winModel"]);

                winPosX = (int)jsonData["winPosX"];
                winPosY = (int)jsonData["winPosY"];
                winWidth = (int)jsonData["winWidth"];
                winHeight = (int)jsonData["winHeight"];

                ModelKey0 = (KeyCode)Enum.Parse(typeof(KeyCode), (string)jsonData["ModelKey0"]);
                ModelKey1 = (KeyCode)Enum.Parse(typeof(KeyCode), (string)jsonData["ModelKey1"]);

            }
        }

        /// <summary>
        /// 自适应计算
        /// </summary>
        public void ProperFormat()
        {

            int resWidth = (int)GetSystemMetrics(SM_CXSCREEN);
            int resHeight = (int)GetSystemMetrics(SM_CYSCREEN);
            Vector2 ScreenRect = new Vector2(winWidth, winHeight);
            Vector2 SystemRect = new Vector2(resWidth, resHeight);
            WinRect = SS_Mathf.Rect_ProperFormat_Max(ScreenRect, SystemRect);
        }

        /// <summary>
        /// 设置窗口
        /// </summary>
        public void Set_WinScreen()
        {

#if UNITY_EDITOR //编辑器模式

#else
            switch (winModel)
            {
                case WinModel.自适应:
                    {
                        ProperFormat();
                        SetWindowLong(GetCurrentWindowHandle(), GWL_STYLE, WS_POPUP & WS_BORDER);
                        SetWindowPos(GetCurrentWindowHandle(), -1, winPosX, winPosY, (int)WinRect.x, (int)WinRect.y, SWP_SHOWWINDOW);
                    }
                    break;
                case WinModel.原像素:
                    {
                        SetWindowLong(GetCurrentWindowHandle(), GWL_STYLE, WS_POPUP & WS_BORDER);
                        SetWindowPos(GetCurrentWindowHandle(), -1, winPosX, winPosY, winWidth, winHeight, SWP_SHOWWINDOW);
                    }
                    break;

                case WinModel.全屏:
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true); //全屏
                    break;
                    // default:
            }

#endif

        }

        //测试
        public void Set_WindowPos(int x, int y, int winWidth1, int winHeight1)
        {
            SetWindowPos(CurrentWindowHandle, 0, x, y, winWidth1, winWidth1, SWP_SHOWWINDOW);
        }

        void Update()
        {

            if (Input.GetKey(ModelKey0) && Input.GetKeyDown(ModelKey1))
            {

                UnityEngine.Debug.Log("Windows：" + winModel);
                switch (winModel)
                {
                    case WinModel.自适应:
                        winModel = WinModel.原像素;
                        break;
                    case WinModel.原像素:
                        winModel = WinModel.自适应;
                        break;
                    case WinModel.全屏:
                        winModel = WinModel.全屏;
                        break;
                }

                Get_WinFile();
                Set_WinScreen();

            }

        }

        /// <summary>
        /// 获取当前主窗口句柄
        /// </summary>
        /// <returns>返回当前进程的主窗口句柄，如果获取失败则返回IntPtr.Zero</returns>
        public static IntPtr GetCurrentWindowHandle()
        {
            IntPtr ptrWindows = IntPtr.Zero; //句柄初始化

            Process CurrentProcess = Process.GetCurrentProcess(); // 获取当前进程

            uint CurrentProcessId = (uint)CurrentProcess.Id; // 获取当前窗口进程 ID  

            bool bResult = EnumWindows( //通过进程枚举窗口查询

                new WNDENUMPROC( //匿名委托
                    delegate (IntPtr hwnd, uint lParam)
                    {
                        uint processId = 0; //进程id
                        if (GetParent(hwnd) == IntPtr.Zero) //判断 窗口句柄 不存在 父窗口
                        {
                            GetWindowThreadProcessId(hwnd, ref processId); //通过 窗口句柄 获取 进程Id

                            if (processId == lParam) // 判断 当前进程Id 与 当前窗口进程Id  
                            {
                                ptrWindows = hwnd; // 把 窗口句柄 缓存起来  
                                SetLastError(0); // 设置无错误  
                                return false; // 返回 false 可以终止枚举窗口  
                            }
                        }
                        return true;
                    }
                ), CurrentProcessId //会传给匿名委托的 lParam
            );

            // 获取最后win32错误（）
            return (!bResult && Marshal.GetLastWin32Error() == 0) ? ptrWindows : IntPtr.Zero; //返回句柄
        }

        /// <summary>
        /// 设置窗体具有鼠标穿透效果:未测试
        /// </summary>
        public void SetPenetrate(IntPtr Handle)
        {
            GetWindowLong(Handle, GWL_EXSTYLE);
            SetWindowLong(Handle, GWL_EXSTYLE, WS_EX_TRANSPARENT | WS_EX_LAYERED);
            SetLayeredWindowAttributes(Handle, 0, 100, LWA_ALPHA);
        }

    }

}