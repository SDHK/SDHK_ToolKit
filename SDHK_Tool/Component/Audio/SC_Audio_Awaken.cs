using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.02.27
 * 
 * 功能：用于监听麦克风语音唤醒
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// 语音唤醒
    /// </summary>
    public class SC_Audio_Awaken : MonoBehaviour
    {

        // 短语识别器
        private PhraseRecognizer phraseRecognizer;

        /// <summary>
        /// 运行时启动
        /// </summary>
		[Tooltip("运行时启动")]
        public bool StartOnAwake = true;

        [Space()]
        /// <summary>
        /// 识别灵敏度
        /// </summary>
		[Tooltip("识别灵敏度")]
        public ConfidenceLevel Sensitivity = ConfidenceLevel.Medium;

        [Space()]
        /// <summary>
        /// 关键词
        /// </summary>
		[Tooltip("关键词")]
        public string[] keywords = { "你好", "唤醒", "开始", "停止" };

        /// <summary>
        /// 识别回调事件
        /// </summary>
        public Action<PhraseRecognizedEventArgs> CallBack;



        // Use this for initialization
        void Start()
        {
            if (StartOnAwake)
            {
                Refresh();
                Run();
            }
        }


        /// <summary>
        /// 识别器刷新创建
        /// </summary>
        public void Refresh()
        {
            if (phraseRecognizer != null)
            {
                phraseRecognizer.Stop();
                phraseRecognizer.Dispose();
            }

            //创建一个识别器
            phraseRecognizer = new KeywordRecognizer(keywords, Sensitivity);

            //通过注册监听的方法
            phraseRecognizer.OnPhraseRecognized += OnPhraseRecognized;
            Debug.Log("[语音唤醒]：识别器创建");
        }

        /// <summary>
        /// 识别器运行
        /// </summary>
        public void Run()
        {
            phraseRecognizer.Start();
            Debug.Log("[语音唤醒]：识别器开启");
        }

        /// <summary>
        /// 识别器停止
        /// </summary>
        public void Stop()
        {
            phraseRecognizer.Stop();
            Debug.Log("[语音唤醒]：识别器停止");
        }

        /// <summary>
        /// 识别器关闭
        /// </summary>
        public void Close()
        {
            phraseRecognizer.Stop();
            phraseRecognizer.Dispose();
            Debug.Log("[语音唤醒]：识别器关闭");
        }

        /// <summary>
        /// 短语识别回调
        /// </summary>
        /// <param name="EventArgs">回调事件参数</param>
        private void OnPhraseRecognized(PhraseRecognizedEventArgs EventArgs)
        {
            Debug.Log("[语音唤醒]：唤醒成功:" + EventArgs.text);
            if (CallBack != null)
            {

                CallBack(EventArgs);
            }

            // EventArgs.confidence 正确识别确定性的度量。
            // EventArgs.phraseDuration 说出该短语所花费的时间。
            // EventArgs.phraseStartTime 话语开始的时刻。
            // EventArgs.semanticMeanings 公认短语的语义含义。
            // EventArgs.text 被识别的关键词。
        }

        private void OnApplicationQuit()
        {
            if (phraseRecognizer != null)
            {
                Close();
            }
        }
    }
}