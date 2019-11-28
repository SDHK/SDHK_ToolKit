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
 * 功能：Udp消息接收端
 *
 * 注：暂时做为组件
 * 
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// Udp接收端
    /// </summary>
    public class SC_UdpGet : MonoBehaviour
    {
        /// <summary>
        /// 本地IP
        /// </summary>
        public string Local_IP;
        /// <summary>
        /// 本地端口
        /// </summary>
        public int Local_PORT;

        /// <summary>
        /// 检测心跳超时时间（秒）
        /// </summary>
        public float GetOutTime = 3;
        /// <summary>
        /// 心跳检测时间间隔（秒）
        /// </summary>
        public float ClockTimeDelay = 1;
        /// <summary>
        /// 超时标记,是否超时
        /// </summary>
        public bool TimeOut = false;


        private Queue GetQueue;//用于接收的数据队列

        private UdpClient udpGet;//udp

        private IPEndPoint GetServerPoint;//监听地址

        private SD_Thread GetThread;//数据队列收发线程

        private SD_Thread ClockThread;//心跳线程

        private DateTime LateTime;//心跳计时

        /// <summary>
        /// 获取接收的消息队列
        /// </summary>
        /// <returns>消息队列</returns>
        public Queue GetDataQueue()
        {
            return GetQueue;
        }


        private void Awake()
        {
            Refresh();
        }

        public void Refresh()
        {
           if(GetThread!=null)  OnDestroy();

            GetQueue = new Queue();
            LateTime = DateTime.Now;//刷新心跳时间

            IPAddress ServerIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1];
            Local_IP = ServerIp.ToString(); //获取本地IP


            udpGet = new UdpClient(new IPEndPoint(IPAddress.Parse(Local_IP), Local_PORT)); //绑定IP和端口
            GetServerPoint = new IPEndPoint(IPAddress.Any, 0);//监听接收的所有消息

            //=[线程启动]======
            GetThread = new SD_Thread(DataQueueProcessor);
            ClockThread = new SD_Thread(ClockCheck);
            GetThread.Start();
            ClockThread.Start();
        }

        private void DataQueueProcessor(SD_Thread Bit)//数据队列处理器
        {
            while (Bit.isStart)
            {
                try
                {
                    byte[] data = udpGet.Receive(ref GetServerPoint);//阻塞式接受数据
                    GetQueue.Enqueue(data);//添加到队列
                    LateTime = DateTime.Now;//刷新心跳时间
                }
                catch
                {
                }
            }

        }

        private void ClockCheck(SD_Thread Bit)//心跳检测线程
        {
            while (Bit.isStart)
            {
                TimeOut = SS_TriggerMarker.Clock_System( LateTime, GetOutTime);//心跳超时检测
                Thread.Sleep((int)ClockTimeDelay * 1000);//处理延时
            }
        }


        void OnApplicationQuit()//释放资源
        {
            GetThread.End();
            ClockThread.End();
            udpGet.Close();
        }
        void OnDestroy()//释放资源
        {
            GetThread.End();
            ClockThread.End();
            udpGet.Close();
        }


    }

}