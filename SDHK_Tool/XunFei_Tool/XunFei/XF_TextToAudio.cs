using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using XunFei_Tool;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using SDHK_Tool.Static;
using SDHK_Tool.Dynamic;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.03.04
 * 
 * 功能：讯飞文字合成语音
 *
 * 官方网址：//https://console.xfyun.cn/services/tts
 */


namespace XunFei_Tool
{

    /// <summary>
    /// 讯飞文字合成语音：紧急封装版
    /// </summary>
    public class XF_TextToAudio : MonoBehaviour
    {
        /// <summary>
        /// 讯飞登录器
        /// </summary>
        [Tooltip("讯飞平台登录器")]
        public XF_Login XF_Login;

        [Space()]
        [Space()]

        /// <summary>
        /// 语音合成频率
        /// </summary>
        [Tooltip("合成语音的频率")]
        public int frequency = 16000;

        [Space()]
        [Space()]

        /// <summary>
        /// 请求参数
        /// </summary>
        [Tooltip("请求参数")]
        public string Request_Params = "engine_type = cloud ,voice_name=xiaoyan, text_encoding = UTF8,sample_rate = 16000";

        /// <summary>
        /// 是否在请求中
        /// </summary>
        [Tooltip("是否在请求中")]
        public bool IsRun = false;


        /// <summary>
        /// 语音合成的数据
        /// </summary>
        [System.NonSerialized]
        public byte[] audioData;
        private static MemoryStream memoryStream;//内存流
        private uint audio_len = 0;//接收长度


        private int TagCode = 0;//错误码
        private IntPtr session_ID;//权柄
        private SynthStatus synth_status;//接收标记

        private SD_Thread TH_GetAudioData;//请求会话线程
        private SD_TaskActuator TA_Callback;//请求回调任务


        private Action<byte[]> callback_wavClip;
        private Action<AudioClip> callback_audioClip;

        private string Text;//请求合成的文本字符串

        private void Awake()
        {

            TH_GetAudioData = new SD_Thread(XF_GetAudioData);
            TA_Callback = new SD_TaskActuator();

            TA_Callback
            .WaitEvent(() => !TH_GetAudioData.isRun)
            .Event(() =>
            {
                if (callback_wavClip != null)
                {
                    byte[] Wav = SS_File.New_WavByte(audioData, 1, frequency, 16);
                    callback_wavClip(Wav);
                }

                if (callback_audioClip != null)
                {
                    byte[] Wav = SS_File.New_WavByte(audioData, 1, frequency, 16);
                    callback_audioClip(SS_File.Convert_AudioWavToClip(Wav, "讯飞合成语音"));
                }

                IsRun = false;//!!!标记结束
            });
        }

        private void OnApplicationQuit()
        {
            TH_GetAudioData.End();//线程退出
        }


        // Update is called once per frame
        void FixedUpdate()
        {
            TA_Callback.Update();
        }

        /// <summary>
        /// 注册回调委托
        /// </summary>
        /// <param name="Callback_AudioData">请求回调：音频字节</param>
        public void XF_SetCallBack_Wav(Action<byte[]> Callback_WavClip)
        {
            callback_wavClip = Callback_WavClip;
        }

        /// <summary>
        /// 注册回调委托
        /// </summary>
        /// <param name="Callback_AudioData">请求回调：音频片段</param>
        public void XF_SetCallBack_Clip(Action<AudioClip> Callback_AudioClip)
        {
            callback_audioClip = Callback_AudioClip;
        }

        /// <summary>
        /// 讯飞音频合成请求
        /// </summary>
        /// <param name="text">合成文本</param>
        public void XF_Request(string text)
        {
            if (!IsRun && XF_Login.isLogin)
            {
                Debug.Log("[语音合成]：开始请求会话");
                Text = text;
                session_ID = XF_Msc_DLL.QTTSSessionBegin(Request_Params, ref TagCode);
                if (TagCode != (int)ErrorCode.MSP_SUCCESS)
                {
                    Debug.Log("[语音合成]：会话请求失败!!!" + TagCode);
                    return;
                }
                Debug.Log("[语音合成]：会话请求成功");


                // callback = Callback_AudioData;
                TH_GetAudioData.Run();
                IsRun = true;
                TA_Callback.Run();
            }

        }


        private void XF_GetAudioData(SD_Thread thread_)//音频接收线程
        {

            TagCode = XF_Msc_DLL.QTTSTextPut(Marshal.PtrToStringAnsi(session_ID), Text, (uint)Encoding.UTF8.GetByteCount(Text), string.Empty);
            if (TagCode != (int)ErrorCode.MSP_SUCCESS)
            {
                Debug.Log("[语音合成]：写入文本失败" + TagCode);
                return;
            }
            Debug.Log("[语音合成]：写入文本成功");


            //内存流可直接在内存进行读写，不需要临时缓冲区或者临时文件
            memoryStream = new MemoryStream();

            Debug.Log("[语音合成]：开始接收合成语音");

            while (thread_.isStart)
            {
                try
                {

                    //IntPtr是指针（引用类型）， Debug.Log打印出来的是地址
                    IntPtr Audio_Data = XF_Msc_DLL.QTTSAudioGet(Marshal.PtrToStringAnsi(session_ID), ref audio_len, ref synth_status, ref TagCode);

                    if (TagCode != (int)ErrorCode.MSP_SUCCESS) { Debug.Log("[语音合成]：接受语音失败:" + TagCode); break; }

                    byte[] array = new byte[(int)audio_len];//创建当前长度的音频

                    if (Audio_Data != (IntPtr)0 && audio_len > 0)//判断地址不为空，数据长度不为0
                    {
                        Marshal.Copy(Audio_Data, array, 0, (int)audio_len);//音频数据写入到数组
                        memoryStream.Write(array, 0, (int)audio_len);//将合成的音频字节数据add到内存流中
                    }

                    if (synth_status == SynthStatus.MSP_TTS_FLAG_DATA_END || TagCode != (int)ErrorCode.MSP_SUCCESS) break;//假如结束则跳出
                    Thread.Sleep(10);//防止CPU频繁占用

                }
                catch
                {
                    Debug.Log("[语音合成]：接收出现异常，已被try跳过");
                    break;
                }
            }

            Debug.Log("[语音合成]：接收完毕");

            TagCode = XF_Msc_DLL.QTTSSessionEnd(Marshal.PtrToStringAnsi(session_ID), "");//结束会话

            if (TagCode != (int)ErrorCode.MSP_SUCCESS)
            {
                Debug.Log("[语音合成]：结束会话失败:" + TagCode);
            }
            else
            {
                Debug.Log("[语音合成]：会话结束成功");
            }

            audioData = memoryStream.ToArray();//转存为数组

            memoryStream.Flush();
            memoryStream.Close();//关闭流
        }


    }

}