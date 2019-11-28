using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using LitJson;
using UnityEngine;
using ZXing;
using ZXing.QrCode;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.6.11
 * 
 * 功能：对外部文件的读取
 */

namespace SDHK_Tool.Static
{
    /// <summary>
    /// 用于Unity3d的资源文件读写类
    /// </summary>
    public static class SS_File
    {
        #region 路径读取


        #region 文件路径读取

        /// <summary>
        /// 获取文件名字
        /// </summary>
        /// <param name="Path">文件夹路径</param>
        /// <returns>return : 文件名集合</returns>
        public static List<string> GetNames_File(string Path)
        {
            List<string> FileNames = new List<string>();
            DirectoryInfo info = new DirectoryInfo(Path);//获取路径文件夹文件
            FileInfo[] infos = info.GetFiles();//获取所有文件名

            foreach (FileInfo file in infos)//遍历每个文件
            {
                FileNames.Add(file.Name);//存入链表
            }
            return FileNames;
        }

        /// <summary>
        /// 获取文件名字(后缀过滤)
        /// </summary>
        /// <param name="Path">文件夹路径</param>
        /// <param name="type">后缀名</param>
        /// <returns>return : 文件名集合</returns>
        public static List<string> GetNames_File(string Path, string type)
        {
            List<string> FileNames = new List<string>();
            DirectoryInfo info = new DirectoryInfo(Path);//获取路径文件夹文件
            FileInfo[] infos = info.GetFiles();//获取所有文件名
            type = type.ToUpper();
            foreach (FileInfo file in infos)//遍历每个文件
            {
                string[] fileName = file.Name.Split('.');
                if (type == fileName[fileName.Length - 1].ToUpper()) FileNames.Add(file.Name);//存入链表        
            }
            return FileNames;
        }

        /// <summary>
        /// 获取文件名字(后缀集合过滤)
        /// </summary>
        /// <param name="Path">文件夹路径</param>
        /// <param name="types">后缀集合</param>
        /// <returns>return : 文件名集合</returns>
        public static List<string> GetNames_File(string Path, string[] types)
        {
            List<string> FileNames = new List<string>();
            DirectoryInfo info = new DirectoryInfo(Path);//获取路径文件夹文件
            FileInfo[] infos = info.GetFiles();//获取所有文件名

            foreach (FileInfo file in infos)//遍历每个文件
            {
                string[] fileName = file.Name.Split('.');
                foreach (var type in types)
                {
                    if (type.ToUpper() == fileName[fileName.Length - 1].ToUpper()) FileNames.Add(file.Name);//存入链表
                }

            }
            return FileNames;
        }



        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="Path">文件夹</param>
        /// <returns>return : 文件路径集合</returns>
        public static List<string> GetPaths_File(string Path)
        {
            return new List<string>(System.IO.Directory.GetFiles(Path));
        }


        /// <summary>
        /// 获取文件路径（后缀过滤）
        /// </summary>
        /// <param name="Path">文件夹</param>
        /// <param name="type">后戳名</param>
        /// <returns>return : 文件路径集合</returns>
        public static List<string> GetPaths_File(string Path, string type)
        {
            List<string> FileNames = new List<string>();
            foreach (var filePath in System.IO.Directory.GetFiles(Path))//获取所有文件
            {
                string[] fileName = filePath.Split('.');//分隔
                if (type.ToUpper() == fileName[fileName.Length - 1].ToUpper())//判断后缀相等
                    FileNames.Add(filePath);//存入链表        
            }
            return FileNames;
        }

        /// <summary>
        /// 获取文件路径（后缀集合过滤）
        /// </summary>
        /// <param name="Path">文件夹</param>
        /// <param name="type">后戳集合</param>
        /// <returns>return : 文件路径集合</returns>
        public static List<string> GetPaths_File(string Path, string[] types)
        {
            List<string> FileNames = new List<string>();
            foreach (var filePath in System.IO.Directory.GetFiles(Path))//获取所有文件
            {
                string[] fileName = filePath.Split('.');//分隔
                foreach (var type in types)
                {
                    if (type.ToUpper() == fileName[fileName.Length - 1].ToUpper())//判断后缀相等
                        FileNames.Add(filePath);//存入链表        
                }
            }
            return FileNames;
        }

        #endregion

        #region 文件夹路径读取
        /// <summary>
        /// 获取文件夹名字
        /// </summary>
        /// <param name="Path">文件夹路径</param>
        /// <returns>return : 文件夹名集合</returns>
        public static List<string> GetNames_Folder(string Path)
        {
            List<string> FolderNames = new List<string>();
            DirectoryInfo[] dirInfo = new DirectoryInfo(Path).GetDirectories();
            foreach (var Folder in dirInfo)
            {
                FolderNames.Add(Folder.Name);
            }
            return FolderNames;
        }

        /// <summary>
        /// 获取文件夹路径
        /// </summary>
        /// <param name="Path">文件夹</param>
        /// <returns>return : 文件路径集合</returns>
        public static List<string> GetPaths_Folder(string Path)
        {
            List<string> FolderNames = new List<string>();
            DirectoryInfo[] dirInfo = new DirectoryInfo(Path).GetDirectories();
            foreach (var Folder in dirInfo)
            {
                FolderNames.Add(Path + "/" + Folder.Name);
            }
            return FolderNames;
        }
        #endregion


        #endregion


        #region 路径新建

        /// <summary>
        /// 文件夹路径创建
        /// </summary>
        /// <param name="Path">要创建的路径</param>
        public static void Path_New(string Path)
        {
            Directory.CreateDirectory(Path);//如果文件夹不存在就创建它
        }

        #endregion

        #region 路径删除

        /// <summary>
        /// 文件夹路径删除
        /// </summary>
        /// <param name="Path">要删除的路径</param>
        /// <param name="Mode">（true/false）删除子路径/删除文件夹</param>
        public static void Path_Delete(string Path, bool Mode = true)
        {
            if (Mode)
            {
                Directory.GetFiles(Path).ToList().ForEach(File.Delete);
                Directory.GetDirectories(Path).ToList().ForEach(Directory.Delete);
            }
            else
            {
                Directory.Delete(Path, true);
            }
        }

        #endregion


        #region 文件读取


        #region 字符串读取

        /// <summary>
        /// 获取文件：字符串读取
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>return : 字符串</returns>
        public static string GetFile_String(string Path)
        {
            return File.ReadAllText(Path);
        }


        /// <summary>
        /// 获取文件：字符串读取
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>return : 字符串</returns>
        public static string GetFile_String_WWW(string Path)
        {
            string Name = Path.Split(':')[0].ToUpper();
            WWW www = new WWW(("http".ToUpper() == Name || "https".ToUpper() == Name) ? Path : "file://" + Path);
            while (!www.isDone) { }
            return www.text;
        }

        #endregion

        #region 二进制读取

        /// <summary>
        /// 获取文件：二进制文件读取
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>return : Byte[]</returns>
        public static byte[] GetFile_Bytes(string Path)
        {
            return File.ReadAllBytes(Path);
        }

        /// <summary>
        /// 获取文件：二进制文件读取
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>return : 字符串</returns>
        public static byte[] GetFile_Bytes_WWW(string Path)
        {
            string Name = Path.Split(':')[0].ToUpper();
            WWW www = new WWW(("http".ToUpper() == Name || "https".ToUpper() == Name) ? Path : "file://" + Path);
            while (!www.isDone) { }
            return www.bytes;
        }

        /// <summary>
        /// 获取文件： 把二进制文件变成类
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="Path">文件路径</param>
        /// <returns>return : 返回类</returns>
        public static T GetFile_ByteObject<T>(string Path)
        {
            byte[] bytes = File.ReadAllBytes(Path);//读取文件所有内容
            return Convert_BytesToObject<T>(bytes);//二进制变成类   
        }

        /// <summary>
        /// 获取文件： 把二进制文件变成类
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="Path">文件路径</param>
        /// <returns>return : 返回类</returns>
        public static T GetFile_ByteObject_WWW<T>(string Path)
        {
            string Name = Path.Split(':')[0].ToUpper();
            WWW www = new WWW(("http".ToUpper() == Name || "https".ToUpper() == Name) ? Path : "file://" + Path);
            while (!www.isDone) { }
            return Convert_BytesToObject<T>(www.bytes);//二进制变成类   
        }

        #endregion

        #region Xml读取

        /// <summary>
        /// 获取文件： 把Xml文件变成类
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="Path">文件路径</param>
        /// <returns>return : 返回类</returns>
        public static T GetFile_XmlObject<T>(string Path)
        {
            string str = File.ReadAllText(Path);//读取文件所有内容
            return Convert_XmlToObject<T>(str);//Xml变成类   
        }

        /// <summary>
        /// 获取文件： 把Xml文件变成类
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="Path">文件路径</param>
        /// <returns>return : 返回类</returns>
        public static T GetFile_XmlObject_WWW<T>(string Path)
        {
            string Name = Path.Split(':')[0].ToUpper();
            WWW www = new WWW(("http".ToUpper() == Name || "https".ToUpper() == Name) ? Path : "file://" + Path);
            while (!www.isDone) { }
            return Convert_XmlToObject<T>(www.text);//Xml变成类   
        }

        #endregion

        #region Json获取

        /// <summary>
        /// 获取文件： 把Json文件变成类
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="Path">文件路径</param>
        /// <returns>return : 返回类</returns>
        public static T GetFile_JsonObject<T>(string Path)
        {
            string str = File.ReadAllText(Path);//读取文件所有内容
            return Convert_JsonToObject<T>(str);//json变成类   
        }

        /// <summary>
        /// 获取文件： 把Json文件变成json数据类
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns>return : 返回类数据</returns>
        public static JsonData GetFile_JsonObject(string Path)
        {
            string str = File.ReadAllText(Path);//读取文件所有内容        
            return Convert_JsonToObject(str);//json变成类   
        }

        /// <summary>
        /// 获取文件：把Json文件变成json数据类
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>return : 返回类数据</returns>
        public static JsonData GetFile_JsonObject_WWW(string Path)
        {
            string Name = Path.Split(':')[0].ToUpper();
            WWW www = new WWW(("http".ToUpper() == Name || "https".ToUpper() == Name) ? Path : "file://" + Path);
            while (!www.isDone) { }
            return Convert_JsonToObject(www.text);
        }

        /// <summary>
        ///  获取文件： 把Json文件变成类
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>return : 返回类</returns>
        public static T GetFile_JsonObject_WWW<T>(string Path)
        {
            string Name = Path.Split(':')[0].ToUpper();
            WWW www = new WWW(("http".ToUpper() == Name || "https".ToUpper() == Name) ? Path : "file://" + Path);
            while (!www.isDone) { }
            return Convert_JsonToObject<T>(www.text);
        }
        #endregion

        #region 图片获取
        /// <summary>
        /// 获取文件：图片读取
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>return : 图片</returns>
        public static Texture2D GetFile_Texture2D_WWW(string Path)
        {
            string Name = Path.Split(':')[0].ToUpper();
            WWW www = new WWW(("http".ToUpper() == Name || "https".ToUpper() == Name) ? Path : "file://" + Path);
            while (!www.isDone) { }
            return www.texture;
        }

        /// <summary>
        /// 获取文件：精灵图片读取
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>return : 精灵图片</returns>
        public static Sprite GetFile_Sprite_WWW(string Path)
        {
            string Name = Path.Split(':')[0].ToUpper();
            WWW www = new WWW(("http".ToUpper() == Name || "https".ToUpper() == Name) ? Path : "file://" + Path);
            while (!www.isDone) { }
            return Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero);
        }


        /// <summary>
        /// 获取文件：图片读取
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>return : 图片</returns>
        public static Texture2D GetFile_Texture2D(string Path)
        {
            //创建文件读取流
            FileStream fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
            //创建文件长度缓冲区
            byte[] bytes = new byte[fileStream.Length];
            //读取文件
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            //释放文件读取流
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;
            //创建Texture
            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(bytes);
            return texture;
        }

        /// <summary>
        /// 获取文件：图片批量读取
        /// </summary>
        /// <param name="Paths">路径集合</param>
        /// <returns>return : 图片集合</returns>
        public static List<Texture2D> GetFile_Texture2D(List<string> Paths)
        {
            List<Texture2D> Texture2Ds = new List<Texture2D>();
            foreach (string Path in Paths)
            {
                Texture2Ds.Add(GetFile_Texture2D(Path));
            }
            return Texture2Ds;
        }

        /// <summary>
        /// 获取文件：图片批量读取(后缀过滤)
        /// </summary>
        /// <param name="Paths">路径集合</param>
        /// <param name="type">后戳名</param>
        /// <returns>return : 图片集合</returns>
        public static List<Texture2D> GetFile_Texture2D(string Path, string type)
        {
            return GetFile_Texture2D(GetPaths_File(Path, type));
        }

        /// <summary>
        /// 获取文件：图片批量读取(后缀集合过滤)
        /// </summary>
        /// <param name="Paths">路径集合</param>
        /// <param name="types">后戳集合</param>
        /// <returns>return : 图片集合</returns>
        public static List<Texture2D> GetFile_Texture2D(string Path, string[] types)
        {
            return GetFile_Texture2D(GetPaths_File(Path, types));
        }




        /// <summary>
        /// 获取文件：精灵图片读取
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>return : 精灵图片</returns>
        public static Sprite GetFile_Sprite(string Path)
        {
            Texture2D texture = GetFile_Texture2D(Path);
            //创建Sprite
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            return sprite;
        }

        /// <summary>
        /// 获取文件：精灵图片批量读取
        /// </summary>
        /// <param name="Paths">路径集合</param>
        /// <returns>return : 精灵图片集合</returns>
        public static List<Sprite> GetFile_Sprite(List<string> Paths)
        {
            List<Sprite> Sprites = new List<Sprite>();
            foreach (string Path in Paths)
            {
                Sprites.Add(GetFile_Sprite(Path));
            }
            return Sprites;
        }

        /// <summary>
        /// 获取文件：精灵图片批量读取（后缀过滤）
        /// </summary>
        /// <param name="Paths">路径集合</param>
        /// <param name="type">后戳名</param>
        /// <returns>return : 精灵图片集合</returns>
        public static List<Sprite> GetFile_Sprite(string Path, string type)
        {
            return GetFile_Sprite(GetPaths_File(Path, type));
        }

        /// <summary>
        /// 获取文件：精灵图片批量读取（后缀集合过滤）
        /// </summary>
        /// <param name="Paths">路径集合</param>
        /// <param name="types">后戳集合</param>
        /// <returns>return : 精灵图片集合</returns>
        public static List<Sprite> GetFile_Sprite(string Path, string[] types)
        {
            return GetFile_Sprite(GetPaths_File(Path, types));
        }
        #endregion


        #endregion


        #region 文件写入


        #region 字符串写入

        /// <summary>
        /// 写入文件：字符串写入文件
        /// </summary>
        /// <param name="str">传入字符串</param>
        /// <param name="Path">文件路径</param>
        public static void SetFile_String(string str, string Path)
        {
            string Folder = Path.Substring(0, Path.LastIndexOf('/'));//去除文件名
            Directory.CreateDirectory(Folder);//如果文件夹不存在就创建它
            File.WriteAllText(Path, str);//创建新文件
        }

        #endregion

        #region 二进制写入

        /// <summary>
        /// 写入文件：二进制写入文件
        /// </summary>
        /// <param name="bytes">传入二进制数组</param>
        /// <param name="Path">文件路径</param>
        public static void SetFile_byte(byte[] bytes, string Path)
        {
            string Folder = Path.Substring(0, Path.LastIndexOf('/'));//去除文件名
            Directory.CreateDirectory(Folder);//如果文件夹不存在就创建它
            File.WriteAllBytes(Path, bytes);//创建新文件
        }

        /// <summary>
        /// 写入文件：把类变成二进制文件
        /// </summary>
        /// <param name="bytes">传入二进制数组</param>
        /// <param name="Path">文件路径</param>
        public static void SetFile_byteObject<T>(T Object, string Path)
        {
            string Folder = Path.Substring(0, Path.LastIndexOf('/'));//去除文件名
            Directory.CreateDirectory(Folder);//如果文件夹不存在就创建它
            File.WriteAllBytes(Path, Convert_ObjectToBytes(Object));//创建新文件
        }


        #endregion

        #region Xml写入

        /// <summary>
        /// 写入文件：把类变成json文件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="Object">传入类</param>
        /// <param name="Path">文件路径</param>
        public static void SetFile_XmlObject<T>(T Object, string Path)
        {
            string Folder = Path.Substring(0, Path.LastIndexOf('/'));//去除文件名
            Directory.CreateDirectory(Folder);//如果文件夹不存在就创建它
            File.WriteAllText(Path, Convert_ObjectToXml(Object));//创建新文件
        }

        #endregion

        #region Json写入

        /// <summary>
        /// 写入文件：把类变成json文件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="Object">传入类</param>
        /// <param name="Path">文件路径</param>
        public static void SetFile_JsonObject<T>(T Object, string Path)
        {
            string Folder = Path.Substring(0, Path.LastIndexOf('/'));//去除文件名
            Directory.CreateDirectory(Folder);//如果文件夹不存在就创建它
            File.WriteAllText(Path, Convert_ObjectToJson(Object));//创建新文件
        }

        /// <summary>
        /// 写入文件：把类变成json文件(缩进格式)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="Object">传入类</param>
        /// <param name="Path">文件路径</param>
        public static void SetFile_JsonObject_Format<T>(T Object, string Path)
        {
            string Folder = Path.Substring(0, Path.LastIndexOf('/'));//去除文件名
            Directory.CreateDirectory(Folder);//如果文件夹不存在就创建它
            File.WriteAllText(Path, Convert_JsonFormat(Convert_ObjectToJson(Object)));//创建新文件
        }
        #endregion

        #region 图片写入
        /// <summary>
        /// 写入文件：图片保存为JPG
        /// </summary>
        /// <param name="texture2D">Texture2D图片纹理</param>
        /// <param name="Path">保存路径</param>
        public static void SetFile_Texture2D_JPG(Texture2D texture2D, string Path)
        {
            string Folder = Path.Substring(0, Path.LastIndexOf('/'));//去除文件名
            Directory.CreateDirectory(Folder);//如果文件夹不存在就创建它
            byte[] imageTytes = texture2D.EncodeToJPG();
            File.WriteAllBytes(Path, imageTytes);
        }

        /// <summary>
        /// 写入文件：图片保存为PNG
        /// </summary>
        /// <param name="texture2D">Texture2D图片纹理</param>
        /// <param name="Path">保存路径</param>
        public static void SetFile_Texture2D_PNG(Texture2D texture2D, string Path)
        {
            string Folder = Path.Substring(0, Path.LastIndexOf('/'));//去除文件名
            Directory.CreateDirectory(Folder);//如果文件夹不存在就创建它
            byte[] imageTytes = texture2D.EncodeToPNG();
            File.WriteAllBytes(Path, imageTytes);
        }
        #endregion


        #endregion



        #region 文件转换

        #region json转换


        /// <summary>
        /// 乱码转换：用于解决LitJson把类转换成string时出现的乱码
        /// </summary>
        /// <param name="source">乱码字符串</param>
        /// <returns>正常字符串</returns>
        public static string Convert_String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase)
            .Replace(source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        /// <summary>
        /// 将对象转换成json（封装LitJson）
        /// </summary>
        /// <param name="obj">实例化的对象</param>
        /// <returns>return : Json字符串</returns>
        public static string Convert_ObjectToJson<T>(T obj)
        {
            return Convert_String(JsonMapper.ToJson(obj));
        }

        /// <summary>
        /// 将json转换成对象（封装LitJson）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">Json字符串</param>
        /// <returns>return : 实例化的对象</returns>
        public static T Convert_JsonToObject<T>(string json)
        {
            return JsonMapper.ToObject<T>(json);
        }

        /// <summary>
        /// 将json转换成对象（封装LitJson）
        /// </summary>
        /// <param name="json">Json字符串</param>
        /// <returns>return : 实例化的对象数据</returns>
        public static JsonData Convert_JsonToObject(string json)
        {
            return JsonMapper.ToObject(json);
        }

        /// <summary>
        /// 判断字符串是否为json格式
        /// </summary>
        /// <param name="json">要判断的json字符串</param>
        /// <returns>return : bool</returns>
        public static bool If_JsonString(string json)
        {
            try
            {
                JsonMapper.ToObject<object>(json);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 格式转换：将Json缩进整理，用于查看
        /// </summary>
        /// <param name="sourceJson">源Json字符串</param>
        /// <returns>return : 整理后的Json字符串</returns>
        public static string Convert_JsonFormat(string sourceJson)
        {
            sourceJson += " ";
            int itap = 0;
            string newjson = "";

            for (int i = 0; i < sourceJson.Length - 1; i++)
            {
                if (sourceJson[i] == '{' || sourceJson[i] == '[')
                {
                    itap++;
                    newjson += sourceJson[i] + "\n";
                    for (int a = 0; a < itap; a++) { newjson += "\t"; }
                }
                else if ((sourceJson[i] == '}' || sourceJson[i] == ']'))
                {
                    itap--;
                    newjson += "\n";
                    for (int a = 0; a < itap; a++) { newjson += "\t"; }
                    newjson += sourceJson[i] + "" + ((sourceJson[i + 1] == ',') ? "," : "") + "\n";
                    if (sourceJson[i + 1] == ',') i++;
                    for (int a = 0; a < itap; a++) { newjson += "\t"; }
                }
                else if (sourceJson[i] != '}' && sourceJson[i] != ']' && sourceJson[i + 1] == ',')
                {
                    newjson += sourceJson[i] + "" + sourceJson[i + 1] + "\n";
                    i++;
                    for (int a = 0; a < itap; a++) { newjson += "\t"; }
                }
                else
                {
                    newjson += sourceJson[i];
                }
            }
            return newjson;
        }

        #endregion

        #region 二进制转换

        /// <summary>
        /// 使用UTF8编码将byte数组转成字符串
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>字符串</returns>
        public static string Convert_bytesToString(byte[] data)
        {
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        /// <summary>
        /// 使用指定字符编码将byte数组转成字符串
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <param name="encoding">指定字符编码</param>
        /// <returns>字符串</returns>
        public static string Convert_bytesToString(byte[] data, Encoding encoding)
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
            xs.Serialize(stream, obj);
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

        #region 二维码转换

        /// <summary>
        /// 将字符串转换为二维码图片
        /// </summary>
        /// <param name="Str">字符串（网址）</param>
        /// <returns>return : 二维码图片</returns>
        public static Texture2D Convert_StrToQrcodeTexture2D(string Str)
        {
            Texture2D encoded = new Texture2D(256, 256);

            QrCodeEncodingOptions qrEncodeOption = new ZXing.QrCode.QrCodeEncodingOptions();
            qrEncodeOption.CharacterSet = "UTF-8"; //设置编码格式，否则读取'中文'乱码
            qrEncodeOption.Height = encoded.height;
            qrEncodeOption.Width = encoded.width;
            qrEncodeOption.Margin = 0; //设置周围空白边距

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = qrEncodeOption
            };
            encoded.SetPixels32(writer.Write(Str));
            encoded.Apply();
            return encoded;
        }

        /// <summary>
        /// 将字符串转换为二维码精灵图片
        /// </summary>
        /// <param name="Str">字符串（网址）</param>
        /// <returns>return : 二维码精灵图片</returns>
        public static Sprite Convert_StrToQrcodeSprite(string Str)
        {
            Texture2D encoded = Convert_StrToQrcodeTexture2D(Str);
            return Sprite.Create(encoded, new Rect(0, 0, encoded.width, encoded.height), Vector2.zero);
        }


        /// <summary>
        /// 将二维码转为字符串
        /// </summary>
        /// <param name="texture">二维码图片</param>
        /// <returns>return : 字符串</returns>
        public static string Convert_QrcodeTexture2DToStr(Texture2D texture)
        {
            BarcodeReader mReader = new BarcodeReader();
            var result = mReader.Decode(texture.GetPixels32(), texture.width, texture.height);
            return (result != null) ? result.Text : null;
        }



        #endregion


        #endregion

    }


    /// <summary>
    /// 用于序列化的Vector3结构
    /// </summary>
    [Serializable]
    public struct SF_Vector3
    {
        public float x;
        public float y;
        public float z;

        public SF_Vector3(float rX, float rY, float rZ)
        {
            x = rX;
            y = rY;
            z = rZ;
        }

        // 以字符串形式返回,方便调试查看
        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2}]", x, y, z);
        }

        // 隐式转换：将SF_Vector3 转换成 Vector3
        public static implicit operator Vector3(SF_Vector3 rValue)
        {
            return new Vector3(rValue.x, rValue.y, rValue.z);
        }

        // 隐式转换：将Vector3 转成 SF_Vector3
        public static implicit operator SF_Vector3(Vector3 rValue)
        {
            return new SF_Vector3(rValue.x, rValue.y, rValue.z);
        }
    }





}
