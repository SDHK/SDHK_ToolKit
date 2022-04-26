using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.12.3
 * 
 * 功能：SS_File的分支部分：对Xml文件进行操作的方法
 */


namespace SDHK_Tool.Static
{
    public static partial class SS_File
    {

        #region Xml读取

        /// <summary>
        /// 读取文件： 把Xml文件变成类
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <returns>return : 返回类</returns>
        public static T GetFile_XmlObject<T>(string path)
        {
            string str = File.ReadAllText(path);//读取文件所有内容
            return Convert_XmlToObject<T>(str);//Xml变成类   
        }

        /// <summary>
        /// 读取文件： 把Xml文件变成类
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <returns>return : 返回类</returns>
        public static T GetFile_XmlObject_WWW<T>(string path)
        {
            string Name = path.Split(':')[0].ToUpper();
            WWW www = new WWW(("http".ToUpper() == Name || "https".ToUpper() == Name) ? path : "file://" + path);
            while (!www.isDone) { }
            return Convert_XmlToObject<T>(www.text);//Xml变成类   
        }

        #endregion

        #region Xml写入

        /// <summary>
        /// 写入文件：把类变成Xml文件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="Object">传入类</param>
        /// <param name="path">文件路径</param>
        public static void SetFile_XmlObject<T>(T Object, string path)
        {
            string Folder = path.Substring(0, path.LastIndexOf('/'));//去除文件名
            Directory.CreateDirectory(Folder);//如果文件夹不存在就创建它
            File.WriteAllText(path, Convert_ObjectToXml(Object));//创建新文件
        }

        #endregion


        #region 序列化转换Xml

        /// <summary>
        /// 将对象序列化为XML数据(转String写入文件)
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>XML数据byte数组</returns>
        public static byte[] Convert_ObjectToXml_Byte<T>(T obj)
        {
            MemoryStream stream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            xs.Serialize(stream, obj);
            byte[] data = stream.ToArray();
            stream.Close();
            return data;
        }

        /// <summary>
        /// 将对象序列化为XML数据
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>XML数据字符串</returns>
        public static string Convert_ObjectToXml<T>(T obj)
        {
            MemoryStream stream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            XmlTextWriter textWriter = new XmlTextWriter(stream,Encoding.GetEncoding("UTF-8"));//定义输出的编码格式
            xs.Serialize(textWriter, obj);
            byte[] data = stream.ToArray();
            stream.Close();
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        /// <summary>
        /// 将XML数据反序列化为指定类型对象(转bytes为对象)
        /// </summary>
        /// <param name="data">XML数据byte数组</param>
        /// <returns>对象</returns>
        public static T Convert_XmlToObject<T>(byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            XmlSerializer xs = new XmlSerializer(typeof(T));
            object obj = xs.Deserialize(stream);
            stream.Close();
            return (T)obj;
        }

        /// <summary>
        /// 将XML数据反序列化为指定类型对象(转String为对象)
        /// </summary>
        /// <param name="str">XML数据字符串</param>
        /// <returns>对象</returns>
        public static T Convert_XmlToObject<T>(string str)
        {
            MemoryStream stream = new MemoryStream();
            byte[] data = Convert_StringToBytes(str);
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            XmlSerializer xs = new XmlSerializer(typeof(T));
            object obj = xs.Deserialize(stream);
            stream.Close();
            return (T)obj;
        }

        #endregion


    }
}