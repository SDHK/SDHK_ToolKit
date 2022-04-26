using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Xml;
using System.IO;
using System.Diagnostics;
using SDHK_Tool.Static;



/// <summary>
/// 更改窗口大小脚本
/// </summary>
public class WinModel : MonoBehaviour
{
    #region
    //public Rect screenPosition;

    //[DllImport("user32.dll")]

    //static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);

    //[DllImport("user32.dll")]

    //static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    //[DllImport("user32.dll")]

    //static extern IntPtr GetForegroundWindow();

    //// not used rigth now

    ////const uint SWP_NOMOVE = 0x2;

    ////const uint SWP_NOSIZE = 1;

    ////const uint SWP_NOZORDER = 0x4;

    ////const uint SWP_HIDEWINDOW = 0x0080; 

    //const uint SWP_SHOWWINDOW = 0x0040;

    //const int GWL_STYLE = -16;

    //const int WS_BORDER = 1;

    ////  public bool GGPM = false;
    //// Use this for initialization
    //void Start()
    //{

    //    SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_BORDER);

    //    bool result = SetWindowPos(GetForegroundWindow(), 0, (int)screenPosition.x, (int)screenPosition.y, (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);
    //    // GGPM = true;
    //}
    #endregion

    /// <summary>  
    /// 窗口左上角x  
    /// </summary>  
    public int winPosX;
    /// <summary>  
    /// 窗口左上角y  
    /// </summary>  
    public int winPosY;

    /// <summary>  
    /// 窗口宽度  
    /// </summary>  
    public int winWidth;
    /// <summary>  
    /// 窗口高度  
    /// </summary>  
    public int winHeight;

    public bool isWin;

    public delegate bool WNDENUMPROC(IntPtr hwnd, uint lParam);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, uint lParam);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetParent(IntPtr hWnd);
    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);
    [DllImport("kernel32.dll")]
    public static extern void SetLastError(uint dwErrCode);



    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("User32.dll", EntryPoint = "GetSystemMetrics")]
    public static extern IntPtr GetSystemMetrics(int nIndex);

    public static readonly System.IntPtr HWND_TOPMOST = new System.IntPtr(-1);

    const int SM_CXSCREEN = 0x00000000;
    const int SM_CYSCREEN = 0x00000001;

    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16;
    const int WS_BORDER = 1;
    const int WS_POPUP = 0x800000;

    public static string _WindowTitle;


    public static IntPtr GetProcessWnd()
    {
        IntPtr ptrWnd = IntPtr.Zero;
        uint pid = (uint)Process.GetCurrentProcess().Id;  // 当前进程 ID  
        bool bResult = EnumWindows(new WNDENUMPROC(delegate (IntPtr hwnd, uint lParam)
    {
        uint id = 0;
        if (GetParent(hwnd) == IntPtr.Zero)
        {
            GetWindowThreadProcessId(hwnd, ref id);
            if (id == lParam)    // 找到进程对应的主窗口句柄  
            {
                ptrWnd = hwnd;   // 把句柄缓存起来  
                SetLastError(0);    // 设置无错误  
                return false;   // 返回 false 以终止枚举窗口  
            }
        }
        return true;
    }), pid);
        return (!bResult && Marshal.GetLastWin32Error() == 0) ? ptrWnd : IntPtr.Zero;
    }

    void Awake()
    {
// #if UNITY_EDITOR



// #else

        
        int resWidth = (int)GetSystemMetrics(SM_CXSCREEN);
        int resHeight = (int)GetSystemMetrics(SM_CYSCREEN);
        
        winPosX = 0;
        winPosY = 0;
        //winWidth = 1920;
        //winHeight = 720;
        //当前屏幕分辨率  

        string filepath = Application.streamingAssetsPath + "/XML/WinInfo.xml";
        //Debug.Log(filepath);
        if (File.Exists(filepath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filepath);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Window").ChildNodes;
            foreach (XmlElement xe in nodelist)
            {
                if (xe.GetAttribute("isWindow") == "Yes")
                {
                    UnityEngine.Debug.Log("ISWin");
                    isWin = true;
                    foreach (XmlElement xe1 in xe.ChildNodes)
                    {
                        string X = xe1.GetAttribute("winposX");
                        String Y = xe1.GetAttribute("winposY");
                        string W = xe1.GetAttribute("winWidth");
                        string H = xe1.GetAttribute("winHeight");
                        _WindowTitle = xe1.GetAttribute("windowTielt");
                        winPosX = int.Parse(X);
                        winPosY = int.Parse(Y);
                        winWidth = int.Parse(W);
                        winHeight = int.Parse(H);
                    }
                }
                else
                {
                    isWin = false;
                }
            }
        }

        if (isWin)
        {
            

            SetWindowLong(GetProcessWnd(), GWL_STYLE, WS_POPUP & WS_BORDER);
            //测试发现左下角坐标为（0,1),修改如下  
            bool result = SetWindowPos(GetProcessWnd(), -1, winPosX, winPosY, winWidth, winHeight, SWP_SHOWWINDOW);
            //bool result = SetWindowPos(GetForegroundWindow(), 0, winPosX, winPosY, winWidth, winHeight, SWP_SHOWWINDOW); 
        }

        //winPosX = resWidth / 2 - winWidth / 2;
        // winPosY = resHeight / 2 - winHeight / 2 - 1;

// #endif



    }
    void Update()
    {
        if (isWin)
        {
            if (Input.GetKey(KeyCode.Q) && Input.GetKeyDown(KeyCode.DownArrow))
            {
                bool result = SetWindowPos(GetProcessWnd(), 0, winPosX, winPosY, winWidth, winHeight, SWP_SHOWWINDOW);
            }
        }
    }

    void OnGUI()
    {

    }




    /// <summary>  
    /// 窗口左上角x  
    /// </summary>  
    public void SetWinPosX(int winPosX)
    {

        this.winPosX = winPosX;
    }
    /// <summary>  
    /// 窗口左上角y  
    /// </summary>  
    public void SetWinPosY(int winPosY)
    {

        this.winPosY = winPosY;
    }

    /// <summary>  
    /// 窗口宽度  
    /// </summary>  
    public void SetWinWidth(int winWidth)
    {

        this.winWidth = winWidth;
    }
    /// <summary>  
    /// 窗口高度  
    /// </summary>  
    public void SetWinHeight(int winHeight)
    {

        this.winHeight = winHeight;
    }

    public void SetIsWin(bool isWin)
    {

        this.isWin = isWin;
    }

}
