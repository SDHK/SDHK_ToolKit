using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.01.08
 * 
 * 参考价值：wavUtility的功能整理，8，24，32深度音频研究
 */

namespace SDHK_Tool.Static
{

    /// <summary>
    /// 音频处理
    /// </summary>
    public class SS_File_Audio
    {

        /// <summary>
        /// 加载PCM格式*.wav音频文件(使用Unity的应用程序数据路径)并转换为AudioClip。
        /// </summary>
        /// <param name="filePath">wav文件路径</param>
        /// <returns>AudioClip</returns>
        public static AudioClip GetFile_AudioClip(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);
            return Convert_WavBytesToAudioClip(fileBytes);
        }


        /// <summary>
        /// 写入文件：把AudioClip储存为.wav格式的文件
        /// </summary>
        /// <param name="audioClip">音频</param>
        /// <param name="filepath">文件路径</param>
        public static void SetFile_AudioClip(AudioClip audioClip, string filepath)
        {
            string Folder = filepath.Substring(0, filepath.LastIndexOf('/'));//去除文件名
            Directory.CreateDirectory(Folder);//如果文件夹不存在就创建它
            SS_File.SetFile_byte(Convert_AudioClipToWav16BitBytes(audioClip), filepath);
        }


        #region wav文件字节与Unity音频文件的转换方法


        /// <summary>
        /// 将Byte转换成AudioClip（wav文件格式）
        /// </summary>
        /// <param name="wav_Bytes">Byte数组（wav文件格式）</param>
        /// <param name="name">AudioClip的名字</param>
        /// <returns>音频</returns>
        public static AudioClip Convert_WavBytesToAudioClip(byte[] wav_Bytes, string name = "wav")
        {
            int subchunk1 = BitConverter.ToInt32(wav_Bytes, 16);//子块
            UInt16 audioFormat = BitConverter.ToUInt16(wav_Bytes, 20);//音频格式

            // 注意:只支持未压缩的PCM wav文件。
            string formatCode = FormatCode(audioFormat);

            Debug.AssertFormat(audioFormat == 1 || audioFormat == 65534, "检测代码格式 '{0}' {1}, 但是目前只支持PCM和WaveFormatExtensable未压缩格式。", audioFormat, formatCode);
            //检测代码格式 但是目前只支持PCM和WaveFormatExtensable未压缩格式;

            UInt16 channels = BitConverter.ToUInt16(wav_Bytes, 22);//渠道
            int sampleRate = BitConverter.ToInt32(wav_Bytes, 24);//采样率

            UInt16 bitDepth = BitConverter.ToUInt16(wav_Bytes, 34);//位深度

            int headerOffset = 16 + 4 + subchunk1 + 4;
            int subchunk2 = BitConverter.ToInt32(wav_Bytes, headerOffset);

            float[] data;
            switch (bitDepth)
            {
                case 8:
                    data = Convert_Wav8BitBytesToAudioClipData(wav_Bytes, headerOffset, subchunk2);
                    break;
                case 16:
                    data = Convert_Wav16BitBytesToAudioClipData(wav_Bytes, headerOffset, subchunk2);
                    break;
                case 24:
                    data = Convert_Wav24BitBytesToAudioClipData(wav_Bytes, headerOffset, subchunk2);
                    break;
                case 32:
                    data = Convert_Wav32BitBytesToAudioClipData(wav_Bytes, headerOffset, subchunk2);
                    break;
                default:
                    throw new Exception(bitDepth + " bit depth is not supported.");//不支持位深度。
            }

            AudioClip audioClip = AudioClip.Create(name, data.Length, (int)channels, sampleRate, false);
            audioClip.SetData(data, 0);
            return audioClip;
        }

        /// <summary>
        /// 将AudioClip转换为Byte数组（wav文件格式）
        /// </summary>
        /// <param name="audioClip">音频</param>
        /// <returns>Byte数组（wav文件格式）</returns>
        public static byte[] Convert_AudioClipToWav16BitBytes(AudioClip audioClip)
        {
            const int headerSize = 44;
            const int BlockSize_16Bit = 2;

            MemoryStream stream = new MemoryStream();

            // 得到位深度
            UInt16 bitDepth = 16; //BitDepth (audioClip);//速度挂钩

            // 注:只支持16位
            int fileSize = audioClip.samples * BlockSize_16Bit + headerSize; // BlockSize (bitDepth)

            WriteFile_Header(ref stream, fileSize);//文件头
            
            WriteFile_Format(ref stream, audioClip.channels, audioClip.frequency, bitDepth);//写文件格式
            WriteFile_Data(ref stream, audioClip, bitDepth);//数据块

            byte[] bytes = stream.ToArray();
            stream.Dispose();

            //验证总字节数
            Debug.AssertFormat(bytes.Length == fileSize, "意外的AudioClip到wav格式字节数: {0} == {1}", bytes.Length, fileSize);
            Debug.Log (bytes.Length);

            return bytes;
            
        }


        #endregion



        #region wav文件字节与Unity音频文件的私有转换方法

        //将8位字节数组转换为AudioClip数据
        private static float[] Convert_Wav8BitBytesToAudioClipData(byte[] source, int headerOffset, int dataSize)
        {
            int wavSize = BitConverter.ToInt32(source, headerOffset);
            Debug.AssertFormat(wavSize > 0 && wavSize == dataSize, "未能从数据字节获得有效的8位wav  大小: {0} 数据字节: {1} 偏移: {2}", wavSize, dataSize, headerOffset);


            float[] data = new float[wavSize];
            sbyte maxValue = sbyte.MaxValue;



            headerOffset += sizeof(int);
            int i = 0;
            while (i < wavSize)
            {
                data[i] = (float)source[i] / maxValue;
                ++i;
            }

            return data;
        }
        //将16位字节数组转换为AudioClip数据
        private static float[] Convert_Wav16BitBytesToAudioClipData(byte[] source, int headerOffset, int dataSize)
        {
            int wavSize = BitConverter.ToInt32(source, headerOffset);
            Debug.AssertFormat(wavSize > 0 && wavSize == dataSize, "未能获得有效的16位wav  大小: {0} 数据字节: {1} 偏移: {2}", wavSize, dataSize, headerOffset);


            int x = sizeof(Int16); // block size = 2
            int convertedSize = wavSize / x;
            Int16 maxValue = Int16.MaxValue;


            headerOffset += sizeof(int);
            float[] data = new float[convertedSize];
            int offset = 0;
            int i = 0;
            while (i < convertedSize)
            {
                offset = i * x + headerOffset;

                data[i] = (float)BitConverter.ToInt16(source, offset) / maxValue;
                ++i;
            }

            Debug.AssertFormat(data.Length == convertedSize, "AudioClip .wav 数据错误  大小: {0} == {1}", data.Length, convertedSize);

            return data;
        }
        //将24位字节数组转换为AudioClip数据
        private static float[] Convert_Wav24BitBytesToAudioClipData(byte[] source, int headerOffset, int dataSize)
        {
            int wavSize = BitConverter.ToInt32(source, headerOffset);
            Debug.AssertFormat(wavSize > 0 && wavSize == dataSize, "未能获得有效的24位wav  大小: {0} 数据字节: {1} 偏移: {2}", wavSize, dataSize, headerOffset);


            int x = 3; // block size = 3
            int convertedSize = wavSize / x;
            int maxValue = Int32.MaxValue;
            byte[] block = new byte[sizeof(int)]; // 使用4字节块复制3个字节，然后用1个偏移量复制字节


            headerOffset += sizeof(int);
            float[] data = new float[convertedSize];
            int offset = 0;
            int i = 0;
            while (i < convertedSize)
            {
                offset = i * x + headerOffset;
                Buffer.BlockCopy(source, offset, block, 1, x);
                data[i] = (float)BitConverter.ToInt32(block, 0) / maxValue;
                ++i;
            }

            Debug.AssertFormat(data.Length == convertedSize, "AudioClip .wav 数据错误  大小: {0} == {1}", data.Length, convertedSize);

            return data;
        }
        //将32位字节数组转换为AudioClip数据
        private static float[] Convert_Wav32BitBytesToAudioClipData(byte[] source, int headerOffset, int dataSize)
        {
            int wavSize = BitConverter.ToInt32(source, headerOffset);
            Debug.AssertFormat(wavSize > 0 && wavSize == dataSize, "未能获得有效的32位wav 大小: {0} 数据字节: {1} 偏移: {2}", wavSize, dataSize, headerOffset);


            int x = sizeof(float); //  block size = 4
            int convertedSize = wavSize / x;
            Int32 maxValue = Int32.MaxValue;


            headerOffset += sizeof(int);
            float[] data = new float[convertedSize];
            int offset = 0;
            int i = 0;
            while (i < convertedSize)
            {
                offset = i * x + headerOffset;

                data[i] = (float)BitConverter.ToInt32(source, offset) / maxValue;

                ++i;
            }

            Debug.AssertFormat(data.Length == convertedSize, "AudioClip .wav 数据错误 大小: {0} == {1}", data.Length, convertedSize);

            return data;
        }

        //====================================================================================

        //将AudioClip数据转换为16字节数组
        private static byte[] Convert_AudioClipDataToInt16Bytes(float[] data)
        {
            MemoryStream dataStream = new MemoryStream();

            int x = sizeof(Int16);

            Int16 maxValue = Int16.MaxValue;

            int i = 0;
            while (i < data.Length)
            {
                dataStream.Write(BitConverter.GetBytes(Convert.ToInt16(data[i] * maxValue)), 0, x);
                ++i;
            }
            byte[] bytes = dataStream.ToArray();

            // 验证转换字节
            Debug.AssertFormat(data.Length * x == bytes.Length, "Unexpected float[] to Int16 to byte[] size: {0} == {1}", data.Length * x, bytes.Length);
            //意外的浮点数[]到Int16到byte[]

            dataStream.Dispose();

            return bytes;
        }

        #endregion



        #region 编写.wav文件的私有方法

        //文件头
        private static void WriteFile_Header(ref MemoryStream stream, int fileSize)
        {
            // riff chunk id小段块id
            byte[] riff = Encoding.ASCII.GetBytes("RIFF");
            BytesToMemoryStream(stream, riff);// "ID"

            // riff chunk size小段块大小
            int chunkSize = fileSize - 8; // total size - 8 for the other two fields in the header 标题中其他两个字段的大小为8
            BytesToMemoryStream(stream, BitConverter.GetBytes(chunkSize));// "CHUNK_SIZE"

            byte[] wave = Encoding.ASCII.GetBytes("WAVE");
            BytesToMemoryStream(stream, wave);// "FORMAT"
        }

        //写文件格式
        private static void WriteFile_Format(ref MemoryStream stream, int channels, int sampleRate, UInt16 bitDepth)
        {
            byte[] id = Encoding.ASCII.GetBytes("fmt ");
            BytesToMemoryStream(stream, id);// "FMT_ID"

            int subchunk1Size = 16; // 24 - 8
            BytesToMemoryStream(stream, BitConverter.GetBytes(subchunk1Size));//"SUBCHUNK_SIZE"

            UInt16 audioFormat = 1;
            BytesToMemoryStream(stream, BitConverter.GetBytes(audioFormat));// "AUDIO_FORMAT"

            UInt16 numChannels = Convert.ToUInt16(channels);
            BytesToMemoryStream(stream, BitConverter.GetBytes(numChannels));// "CHANNELS"

            BytesToMemoryStream(stream, BitConverter.GetBytes(sampleRate));// "SAMPLE_RATE"

            int byteRate = sampleRate * channels * (bitDepth / 8);
            BytesToMemoryStream(stream, BitConverter.GetBytes(byteRate));// "BYTE_RATE"

            UInt16 blockAlign = Convert.ToUInt16(channels * (bitDepth / 8));
            BytesToMemoryStream(stream, BitConverter.GetBytes(blockAlign));// "BLOCK_ALIGN"

            BytesToMemoryStream(stream, BitConverter.GetBytes(bitDepth));// "BITS_PER_SAMPLE"

        }

        //写数据块
        private static void WriteFile_Data(ref MemoryStream stream, AudioClip audioClip, UInt16 bitDepth)
        {
            int BlockSize_16Bit = 2;

            float[] data = new float[audioClip.samples * audioClip.channels];
            audioClip.GetData(data, 0);

            byte[] bytes = Convert_AudioClipDataToInt16Bytes(data);

            byte[] id = Encoding.ASCII.GetBytes("data");

            BytesToMemoryStream(stream, id);// "DATA_ID"

            int subchunk2Size = Convert.ToInt32(audioClip.samples * BlockSize_16Bit); // BlockSize (bitDepth)

            BytesToMemoryStream(stream, BitConverter.GetBytes(subchunk2Size));// "SAMPLES"

            BytesToMemoryStream(stream, bytes);// "DATA"音频数据
        }


        // 将字节写入内存流
        private static void BytesToMemoryStream(MemoryStream stream, byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }

        #endregion


        //格式的代码：DeBug用的
        private static string FormatCode(UInt16 code)
        {
            switch (code)
            {
                case 1:
                    return "PCM";
                case 2:
                    return "ADPCM";
                case 3:
                    return "IEEE";
                case 7:
                    return "μ-law";
                case 65534:
                    return "WaveFormatExtensable";
                default:
                    Debug.LogWarning("Unknown wav code format:" + code);
                    return "";
            }
        }
    }

}
