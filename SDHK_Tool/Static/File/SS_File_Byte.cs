using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.12.3
 * 
 * 功能：SS_File的分支部分：对二进制文件进行操作的方法
 */


namespace SDHK_Tool.Static
{

    public static partial class SS_File
    {

        #region 二进制读取

        /// <summary>
        /// 读取文件：二进制文件读取
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>return : Byte[]</returns>
        public static byte[] GetFile_Bytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        /// <summary>
        /// 读取文件：二进制文件读取
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>return : 字符串</returns>
        public static byte[] GetFile_Bytes_WWW(string path)
        {
            string Name = path.Split(':')[0].ToUpper();
            WWW www = new WWW(("http".ToUpper() == Name || "https".ToUpper() == Name) ? path : "file://" + path);
            while (!www.isDone) { }
            return www.bytes;
        }

        /// <summary>
        /// 读取文件： 把二进制文件变成类
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <returns>return : 返回类</returns>
        public static T GetFile_ByteObject<T>(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);//读取文件所有内容
            return Convert_BytesToObject<T>(bytes);//二进制变成类   
        }

        /// <summary>
        /// 读取文件： 把二进制文件变成类
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <returns>return : 返回类</returns>
        public static T GetFile_ByteObject_WWW<T>(string path)
        {
            string Name = path.Split(':')[0].ToUpper();
            WWW www = new WWW(("http".ToUpper() == Name || "https".ToUpper() == Name) ? path : "file://" + path);
            while (!www.isDone) { }
            return Convert_BytesToObject<T>(www.bytes);//二进制变成类   
        }

        #endregion


        #region 二进制写入

        /// <summary>
        /// 写入文件：二进制写入文件
        /// </summary>
        /// <param name="bytes">传入二进制数组</param>
        /// <param name="path">文件路径</param>
        public static void SetFile_byte(byte[] bytes, string path)
        {
            string Folder = path.Substring(0, path.LastIndexOf('/'));//去除文件名
            Directory.CreateDirectory(Folder);//如果文件夹不存在就创建它
            File.WriteAllBytes(path, bytes);//创建新文件
        }

        /// <summary>
        /// 写入文件：把类变成二进制文件
        /// </summary>
        /// <param name="bytes">传入二进制数组</param>
        /// <param name="path">文件路径</param>
        public static void SetFile_byteObject<T>(T Object, string path)
        {
            string Folder = path.Substring(0, path.LastIndexOf('/'));//去除文件名
            Directory.CreateDirectory(Folder);//如果文件夹不存在就创建它
            File.WriteAllBytes(path, Convert_ObjectToBytes(Object));//创建新文件
        }


        #endregion




        #region 二进制转换

        /// <summary>
        /// 使用UTF8编码将byte数组转成字符串
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>字符串</returns>
        public static string Convert_BytesToString(byte[] data)
        {
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        /// <summary>
        /// 使用指定字符编码将byte数组转成字符串
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <param name="encoding">指定字符编码</param>
        /// <returns>字符串</returns>
        public static string Convert_BytesToString(byte[] data, Encoding encoding)
        {
            return encoding.GetString(data, 0, data.Length);
        }

        /// <summary>
        /// 使用UTF8编码将字符串转成byte数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>byte数组</returns>
        public static byte[] Convert_StringToBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// 使用指定字符编码将字符串转成byte数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encoding">指定字符编码</param>
        /// <returns>byte数组</returns>
        public static byte[] Convert_StringToBytes(string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        #endregion


        #region 序列化转换

        /// <summary>
        /// 将对象序列化为二进制数据:对象定义时需[Serializable]序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>byte数组</returns>
        public static byte[] Convert_ObjectToBytes<T>(T obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, obj);
            byte[] data = stream.ToArray();
            stream.Close();
            return data;
        }

        /// <summary>
        /// 将二进制数据反序列化
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>对象</returns>
        public static T Convert_BytesToObject<T>(byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            T obj = (T)bf.Deserialize(stream);
            stream.Close();
            return obj;
        }

        /// <summary>
        /// 通过二进制序列化深拷贝
        /// </summary>
        /// <param name="obj">拷贝对象</param>
        /// <returns>返回对象</returns>
        public static T DeepCopy<T>(T obj)
        {
            object NewObj;
            MemoryStream stream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
           
            //序列化成流
            bf.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);//设置当前流正在读取的位置 为开始位置即从0开始

            //反序列化成对象
            NewObj = bf.Deserialize(stream);
            stream.Close();//关闭流

            return (T)NewObj;
        }

        #endregion



    }

}