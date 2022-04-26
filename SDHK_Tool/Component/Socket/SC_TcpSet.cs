using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;



/*
 * 作者：闪电Y黑客
 *
 * 日期：2020.5.12
 * 
 * 功能：TCP消息发送端
 *
 * 注：暂时做为组件
 * 
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// TCP发送端:水蜜桃的提示：死循环线程需要休息,死循环添加sleep（1）后解决了无法退出线程的问题
    /// </summary>
    public class SC_TcpSet : MonoBehaviour
    {

        /// <summary>
        /// 目标IP
        /// </summary>
        public string SetIP;

        /// <summary>
        /// 目标端口
        /// </summary>
        public int SetPORT;

        private Queue SetQueue;//用于发送的数据队列

        private Thread SetThread;//消息发送线程

        private TcpClient tcpClient;
        private NetworkStream networkStream;


        /// <summary>
        /// 添加发送数据队列
        /// </summary>
        /// <param name="data">要发送的数据</param>
        public void SetDataQueue(byte[] data)
        {
            SetQueue.Enqueue(data);
        }

        // Use this for initialization
        void Start()
        {
            Initialize();
        }

        [ContextMenu("TCP启动")]
        public void Initialize()
        {
            SetQueue = new Queue();
            tcpClient = new TcpClient();

            Debug.Log("TCP启动");

            // tcpClient.Connect(IPAddress.Parse(SetIP), SetPORT);
            // networkStream = tcpClient.GetStream();

            SetThread = new Thread(DataQueueProcessor);
            SetThread.Start();
        }




        private void DataQueueProcessor()
        {
            do
            {
                try
                {
                    tcpClient.Connect(IPAddress.Parse(SetIP), SetPORT);
                    networkStream = tcpClient.GetStream();
                }
                catch
                {
                    Debug.Log("未连接上！");
                    Thread.Sleep(1000);
                }
            } while (networkStream == null);

            Debug.Log("已连接上！");

            while (true)
            {

                if (SetQueue.Count > 0)//发送数据
                {
                    if (networkStream.CanWrite)
                    {
                        Debug.Log("发送数据");

                        byte[] data = (byte[])SetQueue.Dequeue();//获取数据

                        byte[] dataHead = BitConverter.GetBytes(data.LongLength);//Long类型转为Byte[],为8位
                        byte[] SetData = new byte[dataHead.Length + data.Length];//新建发送数组

                        dataHead.CopyTo(SetData, 0);//包头写入，数据长度
                        data.CopyTo(SetData, dataHead.Length);//数据写入

                        networkStream.Write(SetData, 0, SetData.Length);//发送数据

                    }
                    else
                    {
                        Debug.Log("不能写入数据流！！！");
                        tcpClient.Close();
                        networkStream.Close();
                        return;
                    }
                }

                Thread.Sleep(1);

            }

        }

        /// <summary>
        /// 关闭tcpSet
        /// </summary>
        [ContextMenu("TCP关闭")]
        public void Close()
        {
            if (tcpClient != null) tcpClient.Close();
            if (networkStream != null) networkStream.Close();
            if (SetThread != null)
            {
                // SetThread.Join();
                SetThread.Abort();
                SetThread.Join();

            }
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