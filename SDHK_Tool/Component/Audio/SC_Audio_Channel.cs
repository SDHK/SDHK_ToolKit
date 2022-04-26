using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.07.06
 * 
 * 功能：用于多声道音频播放,这个组件需要将 AudioSettings 设置为 7.1声道模式，电脑的音频设置也要 设置为 7.1声道模式
 *
 * 注：因Unity3d只有一个AudioListener,没法做的真正的7.1声道输出
 * 	这个组件的功能是通过 不同的空间位置 使得u3d的混音分离，而达到 7 声道的效果。
 * 	测试发现并不能完全分离混音，但影响已经调到最小了。
 *
 *	(重低音声道是 无法分离的全部混音 ， 前中喇叭 和 重低音 是同一个接口，最好别用 前中喇叭)
 * 
 */

/// <summary>
/// 多声道音频定位器
/// </summary>
namespace SDHK_Tool.Component
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]//强制添加一个AudioSource
    public class SC_Audio_Channel : MonoBehaviour
    {
        /// <summary>
        /// 音频声道枚举
        /// </summary>
        public AudioChannel audioChannel;

        private AudioListener audioListener;


        // Use this for initialization
        void Start()
        {
            audioListener = FindObjectOfType(typeof(AudioListener)) as AudioListener;//全局寻找组件
            Set_AudioChannel(audioChannel);
        }

        /// <summary>
        /// 设置音频声道
        /// </summary>
        /// <param name="audioChannel">音频声道枚举</param>
        public void Set_AudioChannel(AudioChannel audioChannel)
        {
            transform.parent = audioListener.transform;
            transform.position = AudioPosition(audioChannel);
        }

        private Vector3 AudioPosition(AudioChannel audioChannel)
        {
            Vector3 Position = Vector3.zero;
            switch (audioChannel)
            {
                case AudioChannel.LeftFront: Position = new Vector3(-1f, 0, 1.74f); break;
                case AudioChannel.RightFront: Position = new Vector3(1f, 0, 1.74f); break;
                case AudioChannel.Left: Position = new Vector3(-1, 0, 0); break;
                case AudioChannel.Right: Position = new Vector3(1, 0, 0); break;
                case AudioChannel.LeftRear: Position = new Vector3(-1f, 0, -1f); break;
                case AudioChannel.RightRear: Position = new Vector3(1f, 0, -1f); break;
                case AudioChannel.CentralFront: Position = new Vector3(0, 0, 1.74f); break;
            }
            return Position;
        }

        [ContextMenu("设置AudioSource")]
        private void SetAudioSource()
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();

            foreach (var audioSource in audioSources)
            {
                audioSource.spatialBlend = 1;
                audioSource.dopplerLevel = 0;
            }

        }

        [ContextMenu("设置声道7.1模式")]
        private void SetSpeakerMode()
        {
            AudioConfiguration config;
            config = AudioSettings.GetConfiguration();
            if (config.speakerMode != AudioSpeakerMode.Mode7point1)
            {
                config.speakerMode = AudioSpeakerMode.Mode7point1;
                AudioSettings.Reset(config);
            }
            Debug.Log("声道设置完毕！");
        }
    }

    /// <summary>
    /// 音频声道枚举
    /// </summary>
    public enum AudioChannel
    {
        LeftFront,
        RightFront,
        Left,
        Right,
        LeftRear,
        RightRear,
        CentralFront
    }

}