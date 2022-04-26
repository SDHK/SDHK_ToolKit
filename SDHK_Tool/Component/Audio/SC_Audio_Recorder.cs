using System.Collections;
using System;
using System.Collections.Generic;
using SDHK_Tool.Dynamic;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.03.24
 * 
 * 功能：用于监听麦克风 ，录制有限时长的音频
 * 
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// 定时录音器【紧急封装版】
    /// </summary>
    public class SC_Audio_Recorder : MonoBehaviour
    {
        /// <summary>
        /// 音频名字
        /// </summary>
        [Tooltip("音频名字")]
        public string ClipName = "RecorderClip";

        /// <summary>
        /// 录音时长
        /// </summary>
        [Tooltip("录音时长")]
        public int RecorderTime = 10;

        /// <summary>
        /// 录制频率
        /// </summary>
        [Tooltip("录制频率")]
        public int frequency = 16000;

        [Space()]
        [Space()]

        /// <summary>
        /// 麦克风Id
        /// </summary>
        [Tooltip("麦克风Id,0为默认")]
        public int MicId = 0;

        /// <summary>
        /// 麦克风设备列表
        /// </summary>
        [Tooltip("麦克风设备列表")]
        public string[] microphones;

        [Space()]
        [Space()]


        /// <summary>
        /// 是否监测震荡强度：自动停止                      
        /// 开启后：每帧开始检测震荡强度，假如 AutoTime 秒内 震荡强度未超过 AutoVolume 值 则结束录音，否则重新计时
        /// </summary>
        [Tooltip("监测震荡强度：自动停止录制\n\n开启后：每帧开始检测震荡强度\n假如 AutoTime 秒内 震荡强度未超过 AutoVolume 值 \n则结束录音\n否则重新计时")]
        public bool AutoStop = false;

        /// <summary>
        /// 震荡强度监测时长
        /// </summary>
        [Tooltip("震荡强度监测时长")]
        public float AutoTime = 2;

        /// <summary>
        /// 震荡强度监测阀值
        /// </summary>
        [Tooltip("震荡强度监测阀值")]
        public float AutoVolume = 0.1f;

        [Space()]
        [Space()]

        /// <summary>
        /// 声音振幅值：正负值
        /// </summary>
        [Tooltip("声音振幅值")]
        public float AudioVolume = 0;

        /// <summary>
        /// 录音音频片段                    
        /// 注：读取出来的AudioClip需要AudioSource播放才能听！！！                   
        /// </summary>
        [Tooltip("录音音频片段\n注：读取出来的AudioClip需要AudioSource播放才能听见!!!")]
        public AudioClip ClipSave;

        /// <summary>
        /// 录音回调
        /// </summary>
        public Action<AudioClip> CallBack;

        [Space()]
        [Space()]
        /// <summary>
        /// 录制判断
        /// </summary>
        // [System.NonSerialized]
        public bool IsRecording = false;



        private AudioClip SourceClip;//录制缓存片段

        private float[] ClipData;//数据提取器

        private int ClipPointer = 0;//录音指针
        private int LastClipPointer = 0;//上次录音指针

        private SD_TaskActuator TA_TimerEvent;//事件延时任务

        private SD_TaskActuator TA_TimeOutStop;//录制超时停止任务


        // Use this for initialization
        void Start()
        {
            TA_TimerEvent = new SD_TaskActuator();//新建任务
            TA_TimeOutStop = new SD_TaskActuator();

            TA_TimerEvent
            .WaitTime(() => AutoTime, () => AutoVolume < Mathf.Abs(AudioVolume))
            ;

            TA_TimeOutStop//任务注入
            .IF_Event(() => AutoStop)//判断是否为自动停止

                .Event(TA_TimerEvent.Run)//任务启动
                .WaitEvent(() => RecorderTime, () => !TA_TimerEvent.isRun)//超时事件
                .Event(TA_TimerEvent.Stop)

            .IF_Else()
                .WaitTime(() => RecorderTime)//延时事件
            .IF_End()

            .Event(Record_Stop);
            ;

        }

        void FixedUpdate()
        {
            //  Debug.Log(TA_AutoStop.isRun);

            if (microphones.Length != 0 && Microphone.IsRecording(microphones[MicId]))
            {
                ClipPointer = Microphone.GetPosition(microphones[MicId]);//刷新录音指针
                AudioVolume = GetMaxVolume(SourceClip, ClipPointer, ClipPointer - LastClipPointer);//提取每帧振幅强度
                LastClipPointer = ClipPointer;
            }


            TA_TimerEvent.Update();
            TA_TimeOutStop.Update();


        }


        /// <summary>
        /// 录音开始
        /// </summary>
        [ContextMenu("录音开始")]
        public void Record_Play()
        {
            if (!IsRecording)
            {
                microphones = Microphone.devices;//获取设备列表

                if (microphones.Length > 0)
                {
                    Debug.Log("[定时录音器]:开始录音");

                    Microphone.End(microphones[MicId]);//清除麦克风
                    SourceClip = Microphone.Start(microphones[MicId], false, RecorderTime + 10, frequency);//开始录音：录音时间+1秒
                    ClipPointer = Microphone.GetPosition(microphones[MicId]);//重置录音指针
                    IsRecording = true;
                    TA_TimeOutStop.Run();//任务启动
                }
                else
                {
                    Debug.Log("[定时录音器]:找不到麦克风设备！");
                }
            }
            else
            {
                Debug.Log("[定时录音器]:录音未结束");
            }

        }

        /// <summary>
        /// 录音结束
        /// </summary>
        [ContextMenu("录音结束")]
        public void Record_Stop()
        {
            if (IsRecording)
            {
                if (microphones.Length != 0 && Microphone.IsRecording(microphones[MicId]))
                {
                    Debug.Log("[定时录音器]:结束录音");

                    int Pointer = Microphone.GetPosition(microphones[MicId]);//刷新录音指针
                    ClipData = new float[Pointer];//获取录制长度的数组
                    Microphone.End(microphones[MicId]);//停止录音
                    IsRecording = false;
                    SourceClip.GetData(ClipData, 0);//提取录音
                    ClipSave = AudioClip.Create(ClipName, ClipData.Length, 1, frequency, false);//创建保存音频
                    ClipSave.SetData(ClipData, 0);//写入音频数据
                    if (CallBack != null) CallBack(ClipSave);
                }
                else
                {
                    Debug.Log("[定时录音器]:找不到麦克风设备！");
                }

            }
            else
            {
                Debug.Log("[定时录音器]:未开始录音");
            }

        }

        /// <summary>
        /// 获取最大震荡值
        /// </summary>
        /// <param name="clip">音频片段</param>
        /// <param name="Pointer">读取位置</param>
        /// <param name="Part">片段长度</param>
        /// <returns>片段长度中震荡值：正负最大值</returns>
        private float GetMaxVolume(AudioClip clip, int Pointer, int Part)
        {
            int offset = (Pointer - Part) + 1;//获取当前往clip里面写的位置，(若为当前音量，指针当前位置还未写入)   
            if (offset < 0 || Part <= 0) return 0; //小于0时退出检测

            float maxVolume = 0f;//最大震荡值
            float minVolume = 0f;//最小震荡值

            float[] volumeData = new float[Part];//Part长度的音频

            clip.GetData(volumeData, offset);//获取Part长度的音频

            for (int i = 0; i < volumeData.Length; i++)//遍历
            {
                if (volumeData[i] > 0)
                {
                    if (maxVolume < volumeData[i]) maxVolume = volumeData[i];//最大值
                }
                else if (volumeData[i] < 0)
                {
                    if (minVolume > volumeData[i]) minVolume = volumeData[i];//最小值
                }
            }
            return maxVolume > Mathf.Abs(minVolume) ? maxVolume : minVolume;//返回Part长度音频中最大震荡值
        }

    }

}