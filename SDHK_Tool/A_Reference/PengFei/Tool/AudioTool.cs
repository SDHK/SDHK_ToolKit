using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public static partial class AudioTool
{
    public static AudioClip Combine(List<AudioClip>clips)
    {
        if (clips == null || clips.Count == 0)
            return null;

        int channels = clips[0].channels;
        int frequency = clips[0].frequency;
        for (int i = 1; i < clips.Count; i++)
        {
            if (clips[i].channels != channels || clips[i].frequency != frequency)
            {
                Debug.Log("channels or frequency is different!");
                return null;
            }
        }

        using (MemoryStream memoryStream = new MemoryStream())
        {
            for (int i = 0; i < clips.Count; i++)
            {
                if (clips[i] == null)
                    continue;

                clips[i].LoadAudioData();

                var buffer = clips[i].GetData();

                Debug.Log("bufferLength" + buffer.Length);

                //会有一部分卡顿的片段,需要试一下,
                memoryStream.Write(buffer, 0, buffer.Length - (1764 * 3));
            }

            var bytes = memoryStream.ToArray();
            var result = AudioClip.Create("Combine", bytes.Length / 4 / channels, channels, frequency, false);
            result.SetData(bytes);
            return result;
        }
    }

    public static AudioClip Wav2Clip(string path)
    {
        byte[] bs;
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        bs = new Byte[fs.Length];
        fs.Read(bs, 0, (int)fs.Length);
        fs.Close();
        return WavUtility.ToAudioClip(bs);
    }

    public static byte[] Wav2Byte(string path)
    {
        byte[] bs;
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        bs = new Byte[fs.Length];
        fs.Read(bs, 0, (int)fs.Length);
        fs.Close();
        return bs;
    }

    public static byte[] Clip2Byte(AudioClip clip)
    {
        return clip.GetData();
    }


    public static void Clip2Wav(AudioClip clip,string path)
    {
        using (FileStream fileStream = CreateEmpty(path))
        {
            ConvertAndWrite(fileStream, clip);

            WriteHeader(fileStream, clip);
        }
    }
}


public static partial class AudioTool
{
    const int HEADER_SIZE = 44;

    public static byte[] ClipConvert2Byte(AudioClip clip)
    {
        float[] samples = new float[clip.samples];

        clip.GetData(samples, 0);

        Int16[] intData = new Int16[samples.Length];
        //converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]

        Byte[] bytesData = new Byte[samples.Length * 2];
        //bytesData array is twice the size of
        //dataSource array because a float converted in Int16 is 2 bytes.

        int rescaleFactor = 32767; //to convert float to Int16

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            Byte[] byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        return bytesData;
    }

    public static byte[] AudioClipToByte(AudioClip clip)
    {
        float[] data = new float[clip.samples];
        clip.GetData(data, 0);
        int rescaleFactor = 32767; //to convert float to Int16
        byte[] outData = new byte[data.Length * 2];
        for (int i = 0; i < data.Length; i++)
        {
            short temshort = (short)(data[i] * rescaleFactor);
            byte[] temdata = BitConverter.GetBytes(temshort);
            outData[i * 2] = temdata[0];
            outData[i * 2 + 1] = temdata[1];
        }
        return outData;
    }


    public static void ConvertAndWrite(FileStream fileStream, AudioClip clip)
    {
        Byte[] bytesData = ClipConvert2Byte(clip);

        fileStream.Write(bytesData, 0, bytesData.Length);
    }
    public static void WriteHeader(FileStream fileStream, AudioClip clip)
    {

        int hz = clip.frequency;
        int channels = clip.channels;
        int samples = clip.samples;

        fileStream.Seek(0, SeekOrigin.Begin);

        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        fileStream.Write(riff, 0, 4);

        Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
        fileStream.Write(chunkSize, 0, 4);

        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        fileStream.Write(wave, 0, 4);

        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        fileStream.Write(fmt, 0, 4);

        Byte[] subChunk1 = BitConverter.GetBytes(16);
        fileStream.Write(subChunk1, 0, 4);

        UInt16 two = 2;
        UInt16 one = 1;

        Byte[] audioFormat = BitConverter.GetBytes(one);
        fileStream.Write(audioFormat, 0, 2);

        Byte[] numChannels = BitConverter.GetBytes(channels);
        fileStream.Write(numChannels, 0, 2);

        Byte[] sampleRate = BitConverter.GetBytes(hz);
        fileStream.Write(sampleRate, 0, 4);

        Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2
        fileStream.Write(byteRate, 0, 4);

        UInt16 blockAlign = (ushort)(channels * 2);
        fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

        UInt16 bps = 16;
        Byte[] bitsPerSample = BitConverter.GetBytes(bps);
        fileStream.Write(bitsPerSample, 0, 2);

        Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
        fileStream.Write(datastring, 0, 4);

        Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
        fileStream.Write(subChunk2, 0, 4);

        //		fileStream.Close();
    }

    public static FileStream CreateEmpty(string filepath)
    {
        FileStream fileStream = new FileStream(filepath, FileMode.Create);
        byte emptyByte = new byte();

        for (int i = 0; i < HEADER_SIZE; i++) //preparing the header
        {
            fileStream.WriteByte(emptyByte);
        }

        return fileStream;
    }



}

public static partial class AudioTool
{
    /// <summary>
    /// 有关讯飞的可能要用到这个
    /// </summary>
    /// <param name="memoryStream"></param>
    /// <param name="path"></param>
    /// <param name=""></param>
    /// <param name="write2Stream"></param>
    public static void IncreateHead(ref MemoryStream memoryStream, string path, Action write2Stream)
    {
        //内存流可直接在内存进行读写，不需要临时缓冲区或者临时文件
        //MemoryStream memoryStream = new MemoryStream();

        memoryStream.Write(new byte[44], 0, 44);//为结构体开辟空间，后面用来存储音频文件结构体

        write2Stream();

        WAVE_Header wave_Header = getWave_Header((int)memoryStream.Length);

        byte[] array2 = StructToBytes(wave_Header);

        memoryStream.Position = 0L;//将指针定位到开头

        memoryStream.Write(array2, 0, array2.Length);//存储结构体的字节数组

        memoryStream.Position = 0L;//将指针定位到开头 

        if (path != null)
        {
            FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);

            memoryStream.WriteTo(fileStream);//将内存流中的数据写入到文件流，文件流写入到音频文件中I        

            memoryStream.Close();//关闭流

            fileStream.Close();
        }
    }


    private struct WAVE_Header
    {
        public int RIFF_ID;
        public int File_Size;
        public int RIFF_Type;
        public int FMT_ID;
        public int FMT_Size;
        public short FMT_Tag;
        public ushort FMT_Channel;
        public int FMT_SamplesPerSec;
        public int AvgBytesPerSec;
        public ushort BlockAlign;
        public ushort BitsPerSample;
        public int DATA_ID;
        public int DATA_Size;
    }

    /// <summary>
    /// 结构体初始化赋值
    /// </summary>
    /// <param name="data_len"></param>
    /// <returns></returns>
    private static WAVE_Header getWave_Header(int data_len)
    {
        return new WAVE_Header
        {
            RIFF_ID = 1179011410,//相当于SDK例子中的"RIFF"，只是转换成了对应的ASCL值
            File_Size = data_len - 8,
            RIFF_Type = 1163280727,
            FMT_ID = 544501094,
            FMT_Size = 16,
            FMT_Tag = 1,
            FMT_Channel = 1,
            FMT_SamplesPerSec = 16000,
            AvgBytesPerSec = 32000,
            BlockAlign = 2,
            BitsPerSample = 16,
            DATA_ID = 1635017060,
            DATA_Size = data_len - 44
        };
    }

    /// <summary>
    /// 结构体转字符串
    /// </summary>
    /// <param name="structure"></param>
    /// <returns></returns>
    private static byte[] StructToBytes(object structure)
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
}
public static class AudioClipExtension
{
    public static byte[] GetData(this AudioClip clip)
    {
        float[] data = new float[clip.samples * clip.channels];

        clip.GetData(data, 0);

        byte[] bytes = new byte[data.Length * 4];

        Buffer.BlockCopy(data, 0, bytes, 0, bytes.Length);

        return bytes;
    }

    public static void SetData(this AudioClip clip, byte[] bytes)
    {
        float[] data = new float[bytes.Length / 4];

        Buffer.BlockCopy(bytes, 0, data, 0, bytes.Length);

        clip.SetData(data, 0);
    }
}