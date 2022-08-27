/***********************************
 
 * 作者: 闪电黑客

 * 日期: 2021/10/29 18:53:29

 * 最后日期: 。。。

 * 描述: 

    Debug工具

    组合键[ctrl + alt + ~] 控制界面开关

***********************************/
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Profiling;
using System.IO;

public class DebuggerGG :MonoBehaviour
{

    // private static DebuggerGG instance;//实例
    // private static readonly object _lock = new object();//对象锁

    // /// <summary>
    // /// 单例实例化
    // /// </summary>
    // public static DebuggerGG Instance()
    // {
    //     if (instance == null)
    //     {
    //         lock (_lock)
    //         {
    //             if (instance == null)
    //             {
    //                 var gameObj = new GameObject(typeof(DebuggerGG).Name);
    //                 instance = gameObj.AddComponent<DebuggerGG>();
    //                 UnityEngine.Object.DontDestroyOnLoad(gameObj);
    //                 Debug.Log("[单例启动][Mono] : " + gameObj.name);
    //             }
    //         }
    //     }
    //     return instance;
    // }

    public bool AllowShow = false;//允许界面显示
    public bool Logtxt = false;//打印到txt
    public int LineCount = 500;//滚动列表可显示行数
    private bool scrollLogViewBit = true;

    private float size = 1;
    private int fontSize = 14;
    private int SpaceSize = 4;


    private GUILayoutOption BtnWidth = GUILayout.Width(100);
    private GUILayoutOption BtnHeight = GUILayout.Height(30);

    private GUILayoutOption SizeBtnWidth = GUILayout.Width(30);
    private GUILayoutOption SizeBtnHeight = GUILayout.Height(14);

    private GUILayoutOption LogBtnWidth = GUILayout.Width(20);
    private GUILayoutOption LogBtnHeight = GUILayout.Width(20);

    private GUIStyle BtnStyle = new GUIStyle();
    private GUIStyle textStyle = new GUIStyle();



    private DebugType debugType = DebugType.Console;
    private List<LogData> logDatas = new List<LogData>();
    private int currentLogIndex = -1;

    private Dictionary<LogType, int> logsCount = new Dictionary<LogType, int>();
    private Dictionary<LogType, bool> logsShow = new Dictionary<LogType, bool>();
    private Vector2 scrollLogView = Vector2.zero;
    private Vector2 scrollCurrentLogView = Vector2.zero;
    private Vector2 scrollSystemView = Vector2.zero;
    private bool ShowMax = false;
    private Rect windowRect = new Rect(0, 0, 100, 60);

    private int fps = 0;
    private Color fpsColor = Color.white;
    private int frameNumber = 0;
    private float lastShowFPSTime = 0f;

    private bool DebugStop = false;

    private void Start()
    {

        foreach (LogType item in Enum.GetValues(typeof(LogType)))
        {
            logsCount.Add(item, 0);
            logsShow.Add(item, true);
        }

        Application.logMessageReceived += LogHandler;
        Application.logMessageReceived += LogText;

        BtnStyle.alignment = TextAnchor.MiddleCenter;
        BtnStyle.normal.textColor = Color.white;
        BtnStyle.normal.background = Texture2D.grayTexture;
        textStyle.normal.textColor = Color.white;


    }


    private void OnDestory()
    {
        Application.logMessageReceived -= LogHandler;
        Application.logMessageReceived -= LogText;
    }

    public void LogText(string condition, string stackTrace, LogType type)//?打印到文件
    {
        if (Logtxt)
        {
            LogData log = new LogData();
            log.time = DateTime.Now.ToString("HH:mm:ss");
            log.message = condition;
            log.stackTrace = stackTrace;

            StreamWriter fs = new StreamWriter(Application.streamingAssetsPath + "/DebugLog.txt", true);
            fs.WriteLine("[" + type.ToString() + "][" + log.time + "] " + log.message + "\r\n\r\n" + log.stackTrace);
            fs.Close();
        }
    }



    private void Update()
    {
        //开关快捷键
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.BackQuote))
        {
            AllowShow = !AllowShow;
        }

        if (AllowShow)
        {
            frameNumber += 1;//每秒帧数累计
            float time = Time.realtimeSinceStartup - lastShowFPSTime;
            if (time >= 1)//大于一秒后
            {
                fps = (int)(frameNumber / time);//计算帧数
                frameNumber = 0;//归零
                lastShowFPSTime = Time.realtimeSinceStartup;
            }
        }
    }

    private void LogHandler(string condition, string stackTrace, LogType type)
    {
        LogData log = new LogData();
        log.time = DateTime.Now.ToString("HH:mm:ss");
        log.message = condition;
        log.stackTrace = stackTrace;

        log.type = type;

        if (logsCount[type] < 9999) logsCount[type] = logsCount[type] + 1;


        if (!DebugStop) logDatas.Add(log);

        if (logDatas.Count > LineCount) logDatas.RemoveAt(0);

        if (logsCount[LogType.Warning] > 0)
        {
            fpsColor = Color.yellow;
        }
        if (logsCount[LogType.Warning] > 0 || logsCount[LogType.Error] > 0)
        {
            fpsColor = Color.red;
        }
    }

    private void OnGUI()
    {
        if (AllowShow)
        {

            if (ShowMax)
            {
                windowRect = GUI.Window(this.GetHashCode(), windowRect, GUIWindowMax, "DEBUGGER" + windowRect);
            }
            else
            {
                windowRect = GUI.Window(this.GetHashCode(), windowRect, GUIWindowMin, "DEBUGGER");
            }
        }
    }
    //小窗口
    private void GUIWindowMin(int windowId)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 20));

        windowRect.width = 100 * size;
        windowRect.height = 60 * size;

        BtnWidth = GUILayout.Width(100 * size);
        BtnHeight = GUILayout.Height(30 * size);


        GUI.contentColor = fpsColor;
        if (GUILayout.Button("FPS:" + fps, BtnStyle, BtnHeight))
        {
            ShowMax = true;
        }
        GUI.contentColor = Color.white;
    }

    //大窗口
    private void GUIWindowMax(int windowId)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 20));

        BtnWidth = GUILayout.Width(80 * size);
        BtnHeight = GUILayout.Height(30 * size);

        SizeBtnWidth = GUILayout.Width(30 * size);
        SizeBtnHeight = GUILayout.Height(30 * size);

        LogBtnWidth = GUILayout.Width(12 * size);
        LogBtnHeight = GUILayout.Height(12 * size);


        windowRect.width = 600 * size;
        windowRect.height = 450 * size;

        textStyle.fontSize = (int)(fontSize * size);
        BtnStyle.fontSize = (int)(fontSize * size);

        #region title
        GUILayout.BeginHorizontal();
        GUI.contentColor = fpsColor;
        if (GUILayout.Button("FPS:" + fps, BtnStyle, BtnWidth, BtnHeight))
        {
            ShowMax = false;
        }

        GUILayout.Space(SpaceSize);


        GUI.contentColor = Color.white;

        if (GUILayout.Button("+", BtnStyle, SizeBtnWidth, SizeBtnHeight))
        {
            size += 0.5f;
        }

        GUILayout.Space(SpaceSize);

        if (GUILayout.Button("-", BtnStyle, SizeBtnWidth, SizeBtnHeight))
        {
            if (size > 1) size -= 0.5f;
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(SpaceSize);

        GUILayout.BeginHorizontal();


        foreach (DebugType debugType in Enum.GetValues(typeof(DebugType)))
        {
            GUI.contentColor = (this.debugType == debugType ? Color.white : Color.gray);
            if (GUILayout.Button(debugType.ToString(), BtnStyle, BtnWidth, BtnHeight))
            {
                this.debugType = debugType;
            }
            GUILayout.Space(SpaceSize);
        }

        GUILayout.EndHorizontal();

        #endregion

        GUILayout.Space(SpaceSize);
        GUILayout.Space(SpaceSize);
        GUILayout.Space(SpaceSize);

        GUI.contentColor = Color.white;

        #region console
        if (debugType == DebugType.Console)
        {

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear", BtnStyle, BtnWidth, BtnHeight))
            {
                logDatas.Clear();
                foreach (LogType item in Enum.GetValues(typeof(LogType)))
                {
                    logsCount[item] = 0;
                }
                currentLogIndex = -1;
                fpsColor = Color.white;
            }

            GUILayout.Space(SpaceSize);

            GUI.contentColor = (DebugStop ? Color.red : Color.white);
            if (GUILayout.Button("Stop", BtnStyle, BtnWidth, BtnHeight))
            {
                DebugStop = !DebugStop;
            }

            GUILayout.Space(SpaceSize);

            GUI.contentColor = (scrollLogViewBit ? Color.green : Color.white);
            if (GUILayout.Button("ScrollLog", BtnStyle, BtnWidth, BtnHeight))
            {
                scrollLogViewBit = !scrollLogViewBit;
            }
            GUI.contentColor = Color.white;

            GUILayout.Space(SpaceSize);

            GUI.contentColor = (Logtxt ? Color.green : Color.white);
            if (GUILayout.Button("Logtxt", BtnStyle, BtnWidth, BtnHeight))
            {
                Logtxt = !Logtxt;
            }
            GUI.contentColor = Color.white;

            GUILayout.EndHorizontal();

            GUILayout.Space(SpaceSize);
            GUILayout.Space(SpaceSize);

            GUILayout.BeginHorizontal();


            foreach (LogType item in Enum.GetValues(typeof(LogType)))
            {
                GUI.contentColor = (logsShow[item] ? Color.cyan : Color.gray);
                if (GUILayout.Button(item.ToString() + " [" + logsCount[item] + "]", BtnStyle, BtnWidth, BtnHeight))
                {
                    logsShow[item] = !logsShow[item];
                }
                GUI.contentColor = Color.white;
                GUILayout.Space(SpaceSize);
            }


            GUILayout.Space(SpaceSize);

            GUILayout.EndHorizontal();

            GUI.contentColor = Color.white;

            scrollLogView = GUILayout.BeginScrollView(scrollLogView, "Box", GUILayout.Height(165 * size));
            for (int i = 0; i < logDatas.Count; i++)
            {
                Color color = Color.white;
                bool show = show = logsShow[logDatas[i].type];

                switch (logDatas[i].type)
                {
                    case LogType.Error:
                        color = Color.red;
                        break;
                    case LogType.Assert:
                        color = Color.cyan;
                        break;
                    case LogType.Warning:
                        color = Color.yellow;
                        break;
                    case LogType.Log:
                        color = Color.white;
                        break;
                    case LogType.Exception:
                        color = Color.red;
                        break;
                }

                if (show)
                {
                    GUILayout.BeginHorizontal();

                    GUI.contentColor = (currentLogIndex == i ? Color.green : Color.gray);
                    if (GUILayout.Button("x", BtnStyle, LogBtnWidth, LogBtnHeight))
                    {
                        currentLogIndex = i;
                    }
                    GUI.contentColor = color;
                    GUILayout.Label(" [" + logDatas[i].type + "] [" + logDatas[i].time + "] " + logDatas[i].message, textStyle);
                    GUI.contentColor = Color.white;
                    GUILayout.FlexibleSpace();

                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();


            if (scrollLogViewBit) scrollLogView.Set(0, scrollLogView.y + size);

            scrollCurrentLogView = GUILayout.BeginScrollView(scrollCurrentLogView, "Box");
            if (currentLogIndex != -1)
            {
                GUILayout.Label(logDatas[currentLogIndex].message + "\r\n\r\n" + logDatas[currentLogIndex].stackTrace, textStyle);
            }
            GUILayout.EndScrollView();
        }
        #endregion

        #region memory
        else if (debugType == DebugType.Memory)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Memory Information", textStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box");


#if UNITY_5
            GUILayout.Label("总内存：" + Profiler.GetTotalReservedMemory() / 1000000 + "MB",textStyle);
            GUILayout.Label("已占用内存：" + Profiler.GetTotalAllocatedMemory() / 1000000 + "MB",textStyle);
            GUILayout.Label("空闲中内存：" + Profiler.GetTotalUnusedReservedMemory() / 1000000 + "MB",textStyle);
            GUILayout.Label("总Mono堆内存：" + Profiler.GetMonoHeapSize() / 1000000 + "MB",textStyle);
            GUILayout.Label("已占用Mono堆内存：" + Profiler.GetMonoUsedSize() / 1000000 + "MB",textStyle);
#endif
            // #if UNITY_7
            GUILayout.Label("总内存：" + Profiler.GetTotalReservedMemoryLong() / 1000000 + "MB", textStyle);
            GUILayout.Label("已占用内存：" + Profiler.GetTotalAllocatedMemoryLong() / 1000000 + "MB", textStyle);
            GUILayout.Label("空闲中内存：" + Profiler.GetTotalUnusedReservedMemoryLong() / 1000000 + "MB", textStyle);
            GUILayout.Label("总Mono堆内存：" + Profiler.GetMonoHeapSizeLong() / 1000000 + "MB", textStyle);
            GUILayout.Label("已占用Mono堆内存：" + Profiler.GetMonoUsedSizeLong() / 1000000 + "MB", textStyle);
            // #endif
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("卸载未使用的资源", BtnStyle))
            {
                Resources.UnloadUnusedAssets();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(SpaceSize);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("使用GC垃圾回收", BtnStyle))
            {
                GC.Collect();
            }
            GUILayout.EndHorizontal();
        }
        #endregion

        #region system
        else if (debugType == DebugType.System)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("System Information", textStyle);
            GUILayout.EndHorizontal();

            scrollSystemView = GUILayout.BeginScrollView(scrollSystemView, "Box");
            GUILayout.Label("操作系统：" + SystemInfo.operatingSystem, textStyle);
            GUILayout.Label("系统内存：" + SystemInfo.systemMemorySize + "MB", textStyle);
            GUILayout.Label("处理器：" + SystemInfo.processorType, textStyle);
            GUILayout.Label("处理器数量：" + SystemInfo.processorCount, textStyle);
            GUILayout.Label("显卡：" + SystemInfo.graphicsDeviceName, textStyle);
            GUILayout.Label("显卡类型：" + SystemInfo.graphicsDeviceType, textStyle);
            GUILayout.Label("显存：" + SystemInfo.graphicsMemorySize + "MB", textStyle);
            GUILayout.Label("显卡标识：" + SystemInfo.graphicsDeviceID, textStyle);
            GUILayout.Label("显卡供应商：" + SystemInfo.graphicsDeviceVendor, textStyle);
            GUILayout.Label("显卡供应商标识码：" + SystemInfo.graphicsDeviceVendorID, textStyle);
            GUILayout.Label("设备模式：" + SystemInfo.deviceModel, textStyle);
            GUILayout.Label("设备名称：" + SystemInfo.deviceName, textStyle);
            GUILayout.Label("设备类型：" + SystemInfo.deviceType, textStyle);
            GUILayout.Label("设备标识：" + SystemInfo.deviceUniqueIdentifier, textStyle);
            GUILayout.EndScrollView();
        }
        #endregion

        #region screen
        else if (debugType == DebugType.Screen)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Screen Information", textStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box");


            GUILayout.Label("DPI：" + Screen.dpi, textStyle);

            GUILayout.Space(SpaceSize);

            GUILayout.Label("分辨率：" + Screen.currentResolution.ToString(), textStyle);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("全屏", BtnStyle))
            {
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, !Screen.fullScreen);
            }
            GUILayout.EndHorizontal();
        }
        #endregion

        #region Quality
        else if (debugType == DebugType.Quality)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Quality Information", textStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box");
            string value = "";
            if (QualitySettings.GetQualityLevel() == 0)
            {
                value = " [最低]";
            }
            else if (QualitySettings.GetQualityLevel() == QualitySettings.names.Length - 1)
            {
                value = " [最高]";
            }

            GUILayout.Label("图形质量：" + QualitySettings.names[QualitySettings.GetQualityLevel()] + value, textStyle);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("降低一级图形质量", BtnStyle))
            {
                QualitySettings.DecreaseLevel();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(SpaceSize);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("提升一级图形质量", BtnStyle))
            {
                QualitySettings.IncreaseLevel();
            }
            GUILayout.EndHorizontal();
        }
        #endregion

        #region Environment
        else if (debugType == DebugType.Environment)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Environment Information", textStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box");
            GUILayout.Label("项目名称：" + Application.productName, textStyle);
#if UNITY_5
            GUILayout.Label("项目ID：" + Application.bundleIdentifier,textStyle);
#endif
#if UNITY_7
            GUILayout.Label("项目ID：" + Application.identifier,textStyle);
#endif
            GUILayout.Label("项目版本：" + Application.version, textStyle);
            GUILayout.Label("Unity版本：" + Application.unityVersion, textStyle);
            GUILayout.Label("公司名称：" + Application.companyName, textStyle);
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("退出程序", BtnStyle))
            {
                Application.Quit();
            }
            GUILayout.EndHorizontal();
        }
        #endregion
    }


}
public struct LogData
{
    public string time;
    public LogType type;
    public string message;
    public string stackTrace;
}
public enum DebugType
{
    Console,
    Memory,
    System,
    Screen,
    Quality,
    Environment
}
