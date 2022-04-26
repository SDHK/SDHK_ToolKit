using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;


/*
 * 作者：闪电Y黑客
 *
 * 日期：2020.5.12
 * 
 * 功能：TCP消息接收端
 *
 * 注：暂时做为组件
 * 
 */

namespace SDHK_Tool.Component
{
    /// <summary>
    /// TCP接收端
    /// </summary>
    public class SC_TcpGet : MonoBehaviour
    {

        /// <summary>
        /// 本地端口
        /// </summary>
        public int Local_PORT;

        private TcpListener tcpListener;
        private TcpClient client;
        private IPEndPoint iPEndPoint;

        private NetworkStream networkStream;

        private Thread GetThread;//数据队列收发线程

        private Queue GetQueue;//用于接收的数据队列



        private long DataLength = 0;

        private MemoryStream Data;

        /// <summary>
        /// 获取数据片段
        /// </summary>
        private byte[] dataGet;


        /// <summary>
        /// 包头数据
        /// </summary>
        private byte[] HeadGet;

        // Use this for initialization
        void Awake()
        {
            Initialize();
        }

        [ContextMenu("TCP启动")]
        public void Initialize()
        {
            GetQueue = new Queue();
            iPEndPoint = new IPEndPoint(IPAddress.Any, Local_PORT);
            tcpListener = new TcpListener(iPEndPoint);

            tcpListener.Start();//启动监听
            GetThread = new Thread(DataQueueProcessor);
            GetThread.Start();
        }


        /// <summary>
        /// 获取接收的消息队列
        /// </summary>
        /// <returns>消息队列</returns>
        public Queue GetDataQueue()
        {
            return GetQueue;
        }

        private void DataQueueProcessor()
        {
            client = tcpListener.AcceptTcpClient();
            networkStream = client.GetStream();

            int HeadLength = 0;
            Int32 GetNetLength = 0;//每次接收的数据长度

            int ReadByteLength = client.ReceiveBufferSize;// ReceiveBufferSize是固定大小65536
            int DataCount;//数据段落数量
            int EndDataLength = 0;//数据末尾长度
            int GetDataLength = 0;//数据末尾长度

            HeadGet = new byte[8];//数据头部读取

            Data = new MemoryStream();//新建内存流

            while (true)
            {
                HeadLength = 0;
                while (HeadLength < HeadGet.Length)
                {
                    while (!networkStream.DataAvailable) { Thread.Sleep(1); }//无数据空转等待数据
                    byte[] HeadByte = new byte[HeadGet.Length - HeadLength];//创建接收数组
                    int NetLength = networkStream.Read(HeadByte, 0, HeadByte.Length); //阻塞接收，返回的byteLength是实际长度
                    HeadByte.CopyTo(HeadGet, HeadLength);
                    HeadLength += NetLength;
                }

                DataLength = BitConverter.ToInt64(HeadGet, 0);//读取包头 获得 数据长度

                Data = new MemoryStream();//新建内存流

                GetDataLength = ReadByteLength;//默认接收长度
                EndDataLength = (DataLength > ReadByteLength) ? (int)(DataLength % ReadByteLength) : (int)DataLength;//结尾接收长度
                DataCount = (DataLength > ReadByteLength) ? (int)(DataLength - EndDataLength) / ReadByteLength : 0;//接收段落数量

                while (DataCount >= 0)
                {
                    if (DataCount == 0) GetDataLength = EndDataLength;

                    GetNetLength = 0;//归零
                    while (GetNetLength < GetDataLength)
                    {
                        while (!networkStream.DataAvailable) { Thread.Sleep(1); }//无数据空转等待数据

                        byte[] Bytes = new byte[GetDataLength - GetNetLength];//创建接收数组

                        int NetLength = networkStream.Read(Bytes, 0, Bytes.Length); //阻塞接收，返回的byteLength是实际长度

                        Data.Write(Bytes, 0, NetLength);//追加写入内容

                        GetNetLength += NetLength;//累加
                    }

                    if (DataCount == 0)
                    {
                        GetQueue.Enqueue(Data.ToArray());//添加消息到队列
                        Data.Close();//关闭内存流
                        Debug.Log("添加消息到队列");
                    }
                    DataCount--;
                }
            }
        }


        /// <summary>
        /// 关闭tcpGet
        /// </summary>
        [ContextMenu("TCP关闭")]
        public void Close()
        {
            if (Data != null) Data.Close();
            if (client != null) client.Close();
            if (tcpListener != null) tcpListener.Stop();
            if (networkStream != null) networkStream.Close();
            if (GetThread != null) GetThread.Abort();
        }

        void OnApplicationQuit()//释放资源
        {
            Close();
        }

        void OnDestroy()//释放资源
        {
            Close();
        }





    }
}