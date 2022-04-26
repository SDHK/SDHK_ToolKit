using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

namespace ScreenResolution
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ScreenRect
    {
        public int Left; //最左坐标
        public int Top; //最上坐标
        public int Right; //最右坐标
        public int Bottom; //最下坐标
    }

    public class ScreenDLL
    {
        public const uint SWP_SHOWWINDOW = 0x0040;
        public const int GWL_STYLE = -16;//设定一个新的窗口风格。
        public const int WS_CAPTION = 0x00C00000;//标题
        public const int WS_T1 = 0x14CF0000;//有标题窗口
        public const int WS_T0 = 0x140F0000;//无标题窗口

        public delegate bool WNDENUMPROC(IntPtr hwnd, uint lParam); //枚举委托

        /// <summary>
        /// 枚举窗口
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, uint lParam);

        /// <summary>
        /// 获取指定窗口的父窗口句柄
        /// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        /// <summary>
        /// 获取窗口线程进程
        /// </summary>
        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);

        /// <summary>
        /// 设置上次错误
        /// </summary>
        /// <param name="dwErrCode"></param>
        [DllImport("kernel32.dll")]
        public static extern void SetLastError(uint dwErrCode);

        /// <summary>
        /// 获取窗口样式
        /// </summary>
        [DllImport("user32", EntryPoint = "GetWindowLong")]
        public static extern uint GetWindowLong(IntPtr hwnd, int nIndex);

        /// <summary>
        /// 设置窗口样式
        /// </summary>
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, long dwNewLong);


        //hWndInsertAfter 参数可选值:
        public const int HWND_TOP = 0;// 在前面
        public const int HWND_BOTTOM = 1;// 在后面
        public const int HWND_TOPMOST = -1;// 在前面, 位于任何顶部窗口的前面
        public const int HWND_NOTOPMOST = -2; //在前面, 位于其他顶部窗口的后面
        /// <summary>
        /// 设置窗口位置和大小
        /// </summary>
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref ScreenRect lpRect);


        public const int SM_CXSCREEN = 0x00000000;//屏幕宽度
        public const int SM_CYSCREEN = 0x00000001;//屏幕高度
        /// <summary>
        /// 获取屏幕分辨率
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern IntPtr GetSystemMetrics(int nIndex);

        //用于隐藏任务栏？
        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        public static extern Int32 ShowWindow(Int32 hwnd, Int32 nCmdShow);
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern Int32 FindWindow(string lpClassName, string lpWindowName);
        // ShowWindow(FindWindow("Shell_TrayWnd", null), 5);//显示任务栏
        // ShowWindow(FindWindow("Shell_TrayWnd", null), 0);//隐藏任务栏

        /// <summary>
        /// 获取当前主窗口句柄
        /// </summary>
        /// <returns>获取失败则返回IntPtr.Zero</returns>
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



    }



}