using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using SDHK_Tool.Dynamic;
using System;
using SDHK_Tool.Static;
using System.Threading;


/*
 * 作者：闪电Y黑客
 *
 * 日期：2019.6.20
 * 
 * 功能：Udp消息发送端
 *
 * 注：暂时做为组件
 * 
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// UDP发送端
    /// </summary>
    public class SC_UdpSet : MonoBehaviour
    {
        /// <summary>
        /// 目标IP
        /// </summary>
        public string SetIP;
        /// <summary>
        /// 目标端口
        /// </summary>
        public int SetPORT;
        /// <summary>
        /// 发送间隔（秒）
        /// </summary>
        public float SetTimeDelay = 1;
        /// <summary>
        /// 是否启动心跳
        /// </summary>
        public bool StartClock = false;
        /// <summary>
        /// 心跳发送间隔（秒）
        /// </summary>
        public float ClockTimeDelay = 1;



        private Queue SetQueue;//用于发送的数据队列

        private UdpClient udpSet;//udp

        private IPEndPoint SetServerPoint;//客户端地址

        private SD_Thread SetThread;//客户端接收线程

        private DateTime LateTime;//用于心跳计时

        /// <summary>
        /// 添加发送数据队列
        /// </summary>
        /// <param name="data">要发送的数据</param>
        public void SetDataQueue(byte[] data)
        {
            SetQueue.Enqueue(data);
        }

        private void Awake()
        {

            Refresh();
        }


        public void Refresh()
        {

            if (SetThread != null) OnDestroy();

            SetQueue = new Queue();
            LateTime = DateTime.Now;//刷新心跳时间

            udpSet = new UdpClient();
            SetServerPoint = new IPEndPoint(IPAddress.Parse(SetIP), SetPORT);//发送服务器端口IP

            //=[线程启动]======
            SetThread = new SD_Thread(DataQueueProcessor);
            SetThread.Start();
        }

        private void DataQueueProcessor(SD_Thread Bit)//数据队列处理器
        {
            while (Bit.isStart)
            {
                if (SS_TriggerMarker.Clock_System(LateTime, ClockTimeDelay) && StartClock)//心跳时间判断
                {
                    SetQueue.Enqueue(new byte[] { 1 });//心跳消息暂时为Byte 1
                    LateTime = DateTime.Now;//刷新心跳时间
                }

                if (SetQueue.Count > 0)//发送数据
                {
                    byte[] data = (byte[])SetQueue.Dequeue();
                    udpSet.Send(data, data.Length, SetServerPoint);//针对发送数据 
                    LateTime = DateTime.Now;//刷新心跳时间
                }

                Thread.Sleep((int)SetTimeDelay * 1000);//处理延时
            }
        }


        void OnApplicationQuit()//释放资源
        {
            SetThread.End();
            udpSet.Close();
        }
        void OnDestroy()//释放资源
        {
            SetThread.End();
            udpSet.Close();
        }

    }

}