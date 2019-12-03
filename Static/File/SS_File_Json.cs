using System;
using System.IO;
using System.Text.RegularExpressions;
using LitJson;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.12.3
 * 
 * 功能：SS_Flie的分支部分：对Json文件进行操作的方法
 */

namespace SDHK_Tool.Static
{

    public static partial class SS_File
    {
        #region Json读取

        /// <summary>
        /// 读取文件： 把Json文件变成类
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
        /// 读取文件： 把Json文件变成json数据类
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns>return : 返回类数据</returns>
        public static JsonData GetFile_JsonObject(string Path)
        {
            string str = File.ReadAllText(Path);//读取文件所有内容        
            return Convert_JsonToObject(str);//json变成类   
        }

        /// <summary>
        /// 读取文件：把Json文件变成json数据类
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
        ///  读取文件： 把Json文件变成类
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


    }
}