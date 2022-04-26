using System.Collections.Generic;
using TUIOsharp;
using TUIOsharp.DataProcessors;
using UnityEngine;
using System;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.12.16
 *
 * 功能：TUIO事件监听器
 *
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// TUIO事件监听器
    /// </summary>
    public class SC_TUIO_Listener : MonoBehaviour
    {
        /// <summary>
        /// 监听端口[默认3333]
        /// </summary>
		[Tooltip("监听端口[默认3333]")]
        public int TuioPort = 3333;

        [Space()]

        /// <summary>
        /// Tuio点Id顺序链表[顺序列表]，通过列表id顺序去字典提取Tuio触摸点
        /// </summary>
        [Tooltip("Tuio点Id顺序表")]
        public List<int> TuioCursorIds = new List<int>();

        /// <summary>
        /// Tuio字典[无序]
        /// </summary>
        public Dictionary<int, TuioCursorEventArgs> Tuio_CursorPool = new Dictionary<int, TuioCursorEventArgs>();

        /// <summary>
        /// Tuio点坐标字典[无序]
        /// </summary>
        public Dictionary<int, Vector2> Tuio_PointPool = new Dictionary<int, Vector2>();


        /// <summary>
        /// 触发事件：进入
        /// </summary>
        public Action<List<int>> TuioEnter;

        /// <summary>
        /// 触发事件：离开
        /// </summary>
        public Action<List<int>> TuioExit;

        /// <summary>
        /// 触发事件：停留刷新
        /// </summary>
        public Action<List<int>> TuioStay;

        private List<int> TuioEnterId = new List<int>();//进入事件Id池
        private List<int> TuioStayId = new List<int>();//更新事件Id池
        private List<int> TuioExitId = new List<int>();//退出事件Id池

        private TuioServer server;//TUIO服务
        private CursorProcessor cursorProcessor;//处理器

        // private ObjectProcessor objectProcessor;
        // private BlobProcessor blobProcessor;

        private Vector2 screen;//屏幕长宽值

        private void OnEnable()//启动
        {
            screen.x = Screen.width;
            screen.y = Screen.height;

            cursorProcessor = new CursorProcessor();//委托挂载到线程里
            cursorProcessor.CursorAdded += OnCursorAdded;
            cursorProcessor.CursorUpdated += OnCursorUpdated;
            cursorProcessor.CursorRemoved += OnCursorRemoved;

            server = new TuioServer(TuioPort);//启动服务
            server.Connect();//连接

            server.AddDataProcessor(cursorProcessor);//处理器添加
        }

        private void OnDisable()//关闭
        {
            if (server != null)
            {
                server.RemoveDataProcessor(cursorProcessor);
                server.RemoveAllDataProcessors();
                server.Disconnect();
                server = null;
            }
        }

        private void OnCursorAdded(object sender, TuioCursorEventArgs e)//tuio点添加（线程）
        {
            var entity = e.Cursor;
            lock (this)
            {
                if (!Tuio_CursorPool.ContainsKey(e.Cursor.Id))
                {
                    TuioCursorIds.Add(e.Cursor.Id);
                    Tuio_CursorPool.Add(e.Cursor.Id, e);
                    Tuio_PointPool.Add(e.Cursor.Id, new Vector2(e.Cursor.X * screen.x, (1 - e.Cursor.Y) * screen.y));//坐标换算

                    TuioEnterId.Add(e.Cursor.Id);//添加进入事件Id池
                }
            }
        }

        private void OnCursorUpdated(object sender, TuioCursorEventArgs e)//tuio点刷新（线程）
        {
            var entity = e.Cursor;
            lock (this)
            {
                if (Tuio_CursorPool.ContainsKey(e.Cursor.Id))
                {
                    Tuio_PointPool[e.Cursor.Id] = new Vector2(e.Cursor.X * screen.x, (1 - e.Cursor.Y) * screen.y);//坐标换算

                    TuioStayId.Add(e.Cursor.Id);//添加更新事件Id池
                }
            }
        }

        private void OnCursorRemoved(object sender, TuioCursorEventArgs e)//tuio点移除（线程）
        {
            var entity = e.Cursor;
            lock (this)
            {
                if (Tuio_CursorPool.ContainsKey(e.Cursor.Id))
                {
                    Tuio_PointPool[e.Cursor.Id] = new Vector2(e.Cursor.X * screen.x, (1 - e.Cursor.Y) * screen.y);//坐标换算

                    TuioExitId.Add(e.Cursor.Id);//添加退出事件Id池

                    Tuio_CursorPool.Remove(e.Cursor.Id);//Tuio点池去除id
                    TuioCursorIds.Remove(e.Cursor.Id);//TuioId池去除Id
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

            if (TuioEnter != null && TuioEnterId.Count > 0) { TuioEnter(new List<int>(TuioEnterId)); }  //委托事件：进入

            if (TuioStay != null && TuioStayId.Count > 0) { TuioStay(new List<int>(TuioStayId)); }      //委托事件：刷新

            if (TuioExit != null && TuioExitId.Count > 0) { TuioExit(new List<int>(TuioExitId)); }      //委托事件：退出

            foreach (var Id in TuioExitId) Tuio_PointPool.Remove(Id);//Tuio点坐标池去除id
            TuioEnterId.Clear();   //进入事件Id池清空
            TuioStayId.Clear();   //更新事件Id池清空
            TuioExitId.Clear();     //退出事件Id池清空
        }

    }
}