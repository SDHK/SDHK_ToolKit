using System.Collections.Generic;
using TUIOsharp;
using TUIOsharp.DataProcessors;
using UnityEngine;
using SDHK_Tool.Static;
using UnityEngine.EventSystems;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.12.16
 *
 * 2019.12.19 与触摸穿透器联动 
 *
 * 功能：TUIO事件监听器
 *
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// TUIO事件监听器
    /// </summary>
    public class SC_TUIO_Event : MonoBehaviour
    {
        /// <summary>
        /// 监听端口[默认3333]
        /// </summary>
		[Tooltip("监听端口[默认3333]")]
        public int TuioPort = 3333;

        [Space()]

        /// <summary>
        /// 触摸穿透器
        /// </summary>
        [Tooltip("触摸穿透器：挂上则可触发触摸事件")]
        public SC_TouchEvent_RayCast TouchRayCast;

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
        /// 触摸字典[无序]
        /// </summary>
        public Dictionary<int, PointerEventData> Tuio_TouchPool = new Dictionary<int, PointerEventData>();

        private List<int> TuioOnDownId = new List<int>();//按下事件Id池
        private List<int> TuioOnDragId = new List<int>();//拖拽事件Id池
        private List<int> TuioOnUpId = new List<int>();//抬起事件Id池

        private TuioServer server;//TUIO服务
        private CursorProcessor cursorProcessor;//处理器

        // private ObjectProcessor objectProcessor;
        // private BlobProcessor blobProcessor;

        private Vector2 screen;//屏幕长宽值

        private void OnEnable()//启动
        {
            screen.x = Screen.width;
            screen.y = Screen.height;

            cursorProcessor = new CursorProcessor();//委托挂载
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

        private void OnCursorAdded(object sender, TuioCursorEventArgs e)//tuio点添加
        {
            var entity = e.Cursor;
            lock (this)
            {
                if (!Tuio_CursorPool.ContainsKey(e.Cursor.Id))
                {
                    TuioCursorIds.Add(e.Cursor.Id);
                    Tuio_CursorPool.Add(e.Cursor.Id, e);
                    Tuio_PointPool.Add(e.Cursor.Id, new Vector2(e.Cursor.X * screen.x, (1 - e.Cursor.Y) * screen.y));

                    if (TouchRayCast != null)//假如触摸穿透器不为空则启动触摸
                    {
                        if (!Tuio_TouchPool.ContainsKey(e.Cursor.Id))//触摸池没有这个Id则
                        {
                            //新建触摸点
                            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                            pointerEventData.pointerId = e.Cursor.Id;
                            pointerEventData.position = Tuio_PointPool[e.Cursor.Id];//坐标换算

                            Tuio_TouchPool.Add(e.Cursor.Id, pointerEventData);//添加到触摸池
                            TuioOnDownId.Add(e.Cursor.Id);//添加按下事件Id池
                        }
                    }
                }
            }
        }

        private void OnCursorUpdated(object sender, TuioCursorEventArgs e)//tuio点刷新
        {
            var entity = e.Cursor;
            lock (this)
            {
                if (Tuio_CursorPool.ContainsKey(e.Cursor.Id))
                {
                    Tuio_PointPool[e.Cursor.Id] = new Vector2(e.Cursor.X * screen.x, (1 - e.Cursor.Y) * screen.y);

                    if (TouchRayCast != null)
                    {
                        if (Tuio_TouchPool.ContainsKey(e.Cursor.Id))
                        {
                            Tuio_TouchPool[e.Cursor.Id].position = Tuio_PointPool[e.Cursor.Id];//坐标换算更新
                            TuioOnDragId.Add(e.Cursor.Id);//添加拖拽事件Id池
                        }
                    }
                }
            }
        }

        private void OnCursorRemoved(object sender, TuioCursorEventArgs e)//tuio点移除
        {
            var entity = e.Cursor;
            lock (this)
            {
                if (Tuio_CursorPool.ContainsKey(e.Cursor.Id))
                {
                    Tuio_PointPool[e.Cursor.Id] = new Vector2(e.Cursor.X * screen.x, (1 - e.Cursor.Y) * screen.y);

                    if (TouchRayCast != null)
                    {
                        if (Tuio_TouchPool.ContainsKey(e.Cursor.Id))
                        {
                            Tuio_TouchPool[e.Cursor.Id].position = Tuio_PointPool[e.Cursor.Id];//坐标换算更新
                            TuioOnUpId.Add(e.Cursor.Id);//添加抬起事件Id池
                        }
                    }

                    Tuio_CursorPool.Remove(e.Cursor.Id);//Tuio点池去除id
                    Tuio_PointPool.Remove(e.Cursor.Id);//Tuio点坐标池去除id
                    TuioCursorIds.Remove(e.Cursor.Id);//tuioId池去除Id
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (TouchRayCast != null)
            {
                foreach (var Id in new List<int>(TuioOnDownId)) { Tuio_TouchOnDown(Tuio_TouchPool[Id]); }//触摸穿透事件：按下
                foreach (var Id in new List<int>(TuioOnDragId)) { Tuio_TouchOnDrag(Tuio_TouchPool[Id]); }//触摸穿透事件：拖拽
                foreach (var Id in new List<int>(TuioOnUpId)) { Tuio_TouchOnUp(Tuio_TouchPool[Id]); Tuio_TouchPool.Remove(Id); }//触摸穿透事件：抬起

                TuioOnDownId.Clear();   //按下事件Id池清空
                TuioOnDragId.Clear();   //拖拽事件Id池清空
                TuioOnUpId.Clear();     //抬起事件Id池清空
            }
        }

        /// <summary>
        /// Tuio触摸按下事件
        /// </summary>
        /// <param name="pointerEventData">触摸点</param>
        public void Tuio_TouchOnDown(PointerEventData pointerEventData)
        {
            SS_Touch.OnEnter(TouchRayCast.gameObject, pointerEventData);
            SS_Touch.OnDown(TouchRayCast.gameObject, pointerEventData);
            SS_Touch.OnBeginDrag(TouchRayCast.gameObject, pointerEventData);
        }

        /// <summary>
        /// Tuio触摸拖拽事件
        /// </summary>
        /// <param name="pointerEventData">触摸点</param>
        public void Tuio_TouchOnDrag(PointerEventData pointerEventData)
        {
            SS_Touch.OnDrag(TouchRayCast.gameObject, pointerEventData);
        }

        /// <summary>
        /// Tuio触摸抬起事件
        /// </summary>
        /// <param name="pointerEventData">触摸点</param>
        public void Tuio_TouchOnUp(PointerEventData pointerEventData)
        {
            SS_Touch.OnUp(TouchRayCast.gameObject, pointerEventData);
            SS_Touch.OnExit(TouchRayCast.gameObject, pointerEventData);
            SS_Touch.OnEndDrag(TouchRayCast.gameObject, pointerEventData);
        }

    }
}