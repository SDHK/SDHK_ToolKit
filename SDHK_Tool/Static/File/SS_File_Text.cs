using System.IO;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.12.3
 * 
 * 功能：SS_File的分支部分：对Text文本文件进行操作的方法
 */

namespace SDHK_Tool.Static
{

    public static partial class SS_File
    {
        #region 字符串读取

        /// <summary>
        /// 读取文件：字符串读取
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>return : 字符串</returns>
        public static string GetFile_String(string path)
        {
            return File.ReadAllText(path);
        }

        /// <summary>
        /// 读取文件：字符串读取
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>return : 字符串</returns>
        public static string GetFile_String_WWW(string path)
        {
            string Name = path.Split(':')[0].ToUpper();
            WWW www = new WWW(("http".ToUpper() == Name || "https".ToUpper() == Name) ? path : "file://" + path);
            while (!www.isDone) { }
            return www.text;
        }

        #endregion


        #region 字符串写入

        /// <summary>
        /// 写入文件：字符串写入文件
        /// </summary>
        /// <param name="str">传入字符串</param>
        /// <param name="path">文件路径</param>
        public static void SetFile_String(string str, string path)
        {
            string Folder = path.Substring(0, path.LastIndexOf('/'));//去除文件名
            Directory.CreateDirectory(Folder);//如果文件夹不存在就创建它
            File.WriteAllText(path, str);//创建新文件
        }

        #endregion

    }






}