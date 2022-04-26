using UnityEngine;
using System;
// using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using SDHK_Tool.Dynamic;
using SDHK_Tool.Static;
using LitJson;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.03.25
 * 
 * 功能：讯飞语音识别为文字
 * 
 * 官方网址：//https://console.xfyun.cn/services/iat
 */

namespace XunFei_Tool
{

    /// <summary>
    /// 讯飞语音识别为文字：紧急封装版
    /// </summary>
    public class XF_AudioToText : MonoBehaviour
    {

        /// <summary>
        /// 讯飞登录器
        /// </summary>
        [Tooltip("讯飞平台登录器")]
        public XF_Login XF_Login;

        [Space()]
        [Space()]

        /// <summary>
        /// 请求参数
        /// </summary>
        [Tooltip("请求参数")]
        public string Request_Params = "engine_type = cloud,sub = iat,language = zh_cn,domain = iat,accent = mandarin,sample_rate = 16000";

        /// <summary>
        /// 是否在请求中
        /// </summary>
        [Tooltip("是否在请求中")]
        public bool IsRun = false;


        /// <summary>
        /// 语音识别出来的字符串
        /// </summary>
        [System.NonSerialized]
        public string Text;


        private int TagCode = 0;//错误码
        private IntPtr session_ID;//权柄
        private SynthStatus synth_status;//接收标记

        private EpStatus epStatus;//请求状态
        private RsltStatus rsltStatus;//识别状态


        private SD_Thread TH_GetTextData;//请求会话线程
        private SD_TaskActuator TA_Callback;//请求回调任务


        private Action<string> callback_text;//回调委托

        private byte[] audio_Data;//请求识别的音频数据


        // string text = str;//text是待合成文本

        private void Awake()
        {
            TH_GetTextData = new SD_Thread(XF_GetTextData);
            TA_Callback = new SD_TaskActuator();

            TA_Callback
            .WaitEvent(() => !TH_GetTextData.isRun)
            .Event(() =>
            {

                if (callback_text != null)
                {
                    callback_text(Text);
                }

                IsRun = false;//!!!标记结束
            })
            ;
        }


        private void OnApplicationQuit()
        {
            TH_GetTextData.End();//线程退出
        }


        // Update is called once per frame
        void FixedUpdate()
        {
            TA_Callback.Update();
        }


        /// <summary>
        /// 注册回调委托
        /// </summary>
        /// <param name="Callback_Text">请求回调：识别文字</param>
        public void XF_SetCallBack_Text(Action<string> Callback_Text)
        {
            callback_text = Callback_Text;
        }

        /// <summary>
        /// 讯飞语音识别请求
        /// </summary>
        /// <param name="clip">音频</param>
        public void XF_Request(AudioClip clip)
        {
            if (!IsRun && XF_Login.isLogin)
            {
                Debug.Log("[语音识别]：开始请求会话");
                session_ID = XF_Msc_DLL.QISRSessionBegin(null, Request_Params, ref TagCode);
                if (TagCode != (int)ErrorCode.MSP_SUCCESS)
                {
                    Debug.Log("[语音识别]：会话请求失败!!!" + TagCode);
                    return;
                }
                Debug.Log("[语音识别]：会话请求成功");
                audio_Data = SS_File.Get_WavData_In_Clip(clip);


                TH_GetTextData.Run();
                IsRun = true;
                TA_Callback.Run();

            }

        }

        private void XF_GetTextData(SD_Thread thread_)//音频接收线程
        {
            TagCode = XF_Msc_DLL.QISRAudioWrite(Marshal.PtrToStringAnsi(session_ID), audio_Data, (uint)audio_Data.Length
                   , AudioStatus.MSP_AUDIO_SAMPLE_LAST
                   , ref epStatus
                   , ref rsltStatus
               );
            if (TagCode != (int)ErrorCode.MSP_SUCCESS)
            {
                Debug.Log("[语音识别]：写入音频失败" + TagCode);
                return;
            }
            Debug.Log("[语音识别]：写入音频成功");

            Text = "";

            Debug.Log("[语音识别]：开始接收识别文字");

            while (thread_.isStart)
            {

                try
                {

                    IntPtr Text_Data = XF_Msc_DLL.QISRGetResult(Marshal.PtrToStringAnsi(session_ID), ref rsltStatus, 1000, ref TagCode);
                    if (TagCode != (int)ErrorCode.MSP_SUCCESS) { Debug.Log("[语音识别]：接收文字失败:" + TagCode); break; }

                    string text = Marshal.PtrToStringAnsi(Text_Data);
                    if (Text_Data != (IntPtr)0 && text != "")//判断地址不为空
                    {
                        JsonData textData = SS_File.Convert_JsonToObject(text);

                        for (int i = 0; i < textData["ws"].Count; i++)
                        {
                            Text += textData["ws"][i]["cw"][0]["w"].ToString();
                        }
                        // Debug.Log(textData["ws"][0]["cw"][0]["w"].ToString()); 
                        // Text += text;
                    }


                    if (rsltStatus == RsltStatus.MSP_REC_STATUS_COMPLETE || TagCode != (int)ErrorCode.MSP_SUCCESS) break;//假如结束则跳出


                    Thread.Sleep(100);//防止CPU频繁占用
                }
                catch
                {
                    Debug.Log("[语音识别]：接收出现异常，已被try跳过");
                    break;
                }
            }

            Debug.Log("[语音识别]：接收完毕");

            TagCode = XF_Msc_DLL.QISRSessionEnd(Marshal.PtrToStringAnsi(session_ID), "");

            if (TagCode != (int)ErrorCode.MSP_SUCCESS)
            {
                Debug.Log("[语音识别]：结束会话失败:" + TagCode);
            }
            else
            {
                Debug.Log("[语音识别]：会话结束成功");
            }


        }



    }

}