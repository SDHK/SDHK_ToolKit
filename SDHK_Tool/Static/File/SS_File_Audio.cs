using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期：2020.03.02
 *
 * 创建源于语音聊天项目，与讯飞语音 识别/合成 进行连通，需要进行音频转换储存，断断续续开始写
 * 参考了WavUtility进行整理，补齐u3d对Wav音频储存读取的功能
 * 
 * 功能：SS_File的分支部分：对音频文件进行操作的方法，目前只支持16位深度的音频
 * 注：读取出来的AudioClip需要AudioSource播放才能听！！！！
 * 
 * 原音频指PCM
 *
 * 
 */

namespace SDHK_Tool.Static
{

    public static partial class SS_File
    {
        /// <summary>
        /// Wav音频头长度常量
        /// </summary>
        private const int Wav_headerSize = 44;

        #region AudioClip读取

        /// <summary>
        /// 读取文件：读取Wav文件为AudioClip [采样位数16]
        /// </summary>
        /// <param name="path">文件路径</param>
        public static AudioClip GetFile_AudioWav(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);//读取文件所有内容
            return Convert_AudioWavToClip(bytes, Path.GetFileName(path));
        }

        #endregion

        #region AudioClip写入

        /// <summary>
        /// 写入文件：AudioClip存为.wav文件 [采样位数16]
        /// </summary>
        /// <param name="audioClip">音频</param>
        /// <param name="path">文件路径</param>
        public static void SetFile_AudioWav(AudioClip audioClip, string path)
        {
            string Folder = path.Substring(0, path.LastIndexOf('/')); //去除文件名
            Directory.CreateDirectory(Folder); //如果文件夹不存在就创建它
            File.WriteAllBytes(path, Convert_AudioClipToWav(audioClip)); //创建新文件
        }

        #endregion

        #region 转换方法

        /// <summary>
        /// 转换 AudioClip 为 Wav音频
        /// </summary>
        /// <param name="audioClip">AudioClip</param>
        /// <returns>Wav音频</returns>

        public static byte[] Convert_AudioClipToWav(AudioClip audioClip)
        {
            byte[] wav_Header = Get_WavHeader(audioClip); //获取音频头结构数据
            byte[] wav_Data = Get_WavData_In_Clip(audioClip);//获取音频数据

            byte[] audioWav = new byte[wav_Header.Length + wav_Data.Length];//wav文件是音频头长度+音频数据

            wav_Header.CopyTo(audioWav, 0); //写入Wav音频头
            wav_Data.CopyTo(audioWav, wav_Header.Length); //写入音频数据

            return audioWav;
        }


        /// <summary>
        /// 转换 Wav音频 为 AudioClip
        /// </summary>
        /// <param name="audioWav">Wav音频</param>
        /// <param name="Name">音频名字</param>
        /// <returns>AudioClip</returns>
        public static AudioClip Convert_AudioWavToClip(byte[] audioWav, string Name)
        {
            WAV_Header Clip_Header = Get_WavHeader(audioWav);
            float[] Clip_Data = Get_ClipData_In_Wav(audioWav);
            //音频创建（名字，实际长度/通道数，通道数，采样频率 ,false ）;
            AudioClip audioClip = AudioClip.Create(Name, Clip_Data.Length / Clip_Header.FMT_Channels, Clip_Header.FMT_Channels, Clip_Header.FMT_Frequency, false);
            audioClip.SetData(Clip_Data, 0);

            return audioClip;
        }


        #endregion


        #region 数值获取

        /// <summary>
        /// 获取 Clip数据 [wav音频] 
        /// </summary>
        /// <param name="audioWav">Wav音频</param>
        /// <returns>Clip数据</returns>
        private static float[] Get_ClipData_In_Wav(byte[] audioWav)
        {
            byte[] Wav_Data = audioWav.Skip(Wav_headerSize).Take(audioWav.Length).ToArray();//wav文件音频头长度为44字节,从44开始裁剪出原数据
            float[] Clip_Data = new float[Wav_Data.Length / 2];//Clip数据只有Wav数据的一半大小

            for (int i = 0; i < Clip_Data.Length; i++)
            {
                //ClipData = 字节转换.16位数值(Wav字节，提取间隔为2 )/16位数值最大值 : 获得等比float值
                Clip_Data[i] = (float)BitConverter.ToInt16(Wav_Data, i * 2) / Int16.MaxValue;
            }
            return Clip_Data;
        }


        /// <summary>
        /// 获取 Wav数据 [AudioClip音频] 
        /// </summary>
        /// <param name="audioClip">AudioClip</param>
        /// <returns>Wav数据</returns>
        public static byte[] Get_WavData_In_Clip(AudioClip audioClip)
        {
            float[] Clip_Data = new float[audioClip.samples * audioClip.channels]; //Clip音频长度的数组（-1.0到1.0），长度为音频长度*通道数
            byte[] Wav_Data = new byte[Clip_Data.Length * 2];//数据是AudioClip数据的两倍

            audioClip.GetData(Clip_Data, 0);//Clip数据取出

            for (int i = 0; i < Clip_Data.Length; i++)
            {
                //获取字节数组（ 转换.16位数据（ Clip_Data*16位最大值,获得等比值  ） ）.写入(Wav_Data, 间隔2位 )
                BitConverter.GetBytes(Convert.ToInt16(Clip_Data[i] * Int16.MaxValue)).CopyTo(Wav_Data, i * 2);
            }
            return Wav_Data;
        }





        /// <summary>
        /// 获取 Wav音频头 [AudioClip音频]
        /// </summary>
        /// <param name="audioClip">AudioClip</param>
        /// <param name="DATA_Size">深度默认16</param>
        /// <returns> Wav音频头</returns>
        public static byte[] Get_WavHeader(AudioClip audioClip, int DATA_Size = 16)
        {
            return StructToBytes(Set_WavHeader(audioClip.samples * audioClip.channels * 2, audioClip.channels, audioClip.frequency, DATA_Size));
        }

        /// <summary>
        /// 获取 Wav音频头 [wav音频]                       
        /// 注：这个主要是创建audioClip用的
        /// </summary>
        /// <param name="audioWav">Wav格式的音频</param>
        /// <returns> Wav音频头</returns>
        public static WAV_Header Get_WavHeader(byte[] audioWav)
        {
            return Set_WavHeader(
                BitConverter.ToInt32(audioWav, 40),
                BitConverter.ToUInt16(audioWav, 22),
                BitConverter.ToUInt16(audioWav, 24),
                BitConverter.ToUInt16(audioWav, 34)
            );
        }


        /// <summary>
        /// 新建 wav音频
        /// </summary>
        /// <param name="data">原数据</param>
        /// <param name="channels">渠道</param>
        /// <param name="frequency">频率</param>
        /// <param name="bitDepth">深度</param>
        /// <returns>语音音频头字节</returns>
        public static byte[] New_WavByte(byte[] data, int channels, int frequency, int bitDepth)
        {
            byte[] wavHeader = StructToBytes(Set_WavHeader(data.Length, channels, frequency, bitDepth));
            byte[] wav = new byte[wavHeader.Length + data.Length];

            wavHeader.CopyTo(wav, 0);//写入Wav音频头
            data.CopyTo(wav, wavHeader.Length);
            return wav;
        }


        /// <summary>
        /// 设置 音频头结构体
        /// </summary>
        /// <param name="data_Size">原数据长度</param>
        /// <param name="channels">渠道</param>
        /// <param name="frequency">频率</param>
        /// <param name="bitDepth">深度</param>
        /// <returns>语音音频头结构体</returns>
        public static WAV_Header Set_WavHeader(int data_Size, int channels, int frequency, int bitDepth)
        {
            return new WAV_Header
            {
                RIFF_ID = 1179011410, //相当于SDK例子中的"RIFF"，只是转换成了对应的ASCL值
                File_Size = data_Size + 36,//Size是整个文件的长度减去ID和Size的长度（8）,
                RIFF_Type = 1163280727,//固定'WAVE'

                FMT_ID = 544501094,//固定'fmt '
                FMT_Size = 16,//固定
                FMT_Tag = 1,//固定  音频格式
                FMT_Channels = (ushort)channels,//声道数
                FMT_Frequency = frequency,//采样率
                AvgBytesPerSec = frequency * channels * (bitDepth / 8),//每秒数据字节数
                BlockAlign = Convert.ToUInt16(channels * (bitDepth / 8)),//数据块对齐
                BitsPerSample = (ushort)bitDepth,//块深度,采样位数

                DATA_ID = 1635017060,//固定'data' 
                DATA_Size = data_Size //音频数据长度
            };
        }



        #endregion



        //========================================================================



        /// <summary>
        /// 结构体转字节[没见过的序列化]
        /// </summary>
        /// <param name="structure">实例类</param>
        /// <returns>序列化后的字节组</returns>
        public static byte[] StructToBytes(object structure)
        {
            int num = Marshal.SizeOf(structure);
            IntPtr intPtr = Marshal.AllocHGlobal(num);
            byte[] result;
            try
            {
                Marshal.StructureToPtr(structure, intPtr, false);
                byte[] array = new byte[num];
                Marshal.Copy(intPtr, array, 0, num);
                result = array;
            }
            finally
            {
                Marshal.FreeHGlobal(intPtr);
            }
            return result;
        }



        /// <summary>
        /// Wav音频头结构体
        /// </summary>
        public struct WAV_Header
        {
            public int RIFF_ID;//0
            public int File_Size;//4
            public int RIFF_Type;//8
            public int FMT_ID;//12
            public int FMT_Size;//16
            public short FMT_Tag;//20
            public ushort FMT_Channels;//22
            public int FMT_Frequency;//24
            public int AvgBytesPerSec;//28
            public ushort BlockAlign;//32
            public ushort BitsPerSample;//34
            public int DATA_ID;//36
            public int DATA_Size;//40

            //AudioData 44
        }

    }

}