using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.12.3
 * 
 * 功能：SS_File的分支部分：对文件路径进行处理的方法
 */

namespace SDHK_Tool.Static
{
    public enum FileLocation //根节点枚举类
    {
        StreamingAssetsFolder,
        DataFolder,
        PeristentDataFolder
    }

    public static partial class SS_File
    {

        #region 文件路径读取

        /// <summary>
        /// 读取文件名字
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns>return : 文件名集合</returns>
        public static List<string> GetNames_File(string path)
        {
            List<string> FileNames = new List<string>();
            DirectoryInfo info = new DirectoryInfo(path);//读取路径文件夹文件
            FileInfo[] infos = info.GetFiles();//读取所有文件名

            foreach (FileInfo file in infos)//遍历每个文件
            {
                FileNames.Add(file.Name);//存入链表
            }
            return FileNames;
        }

        /// <summary>
        /// 读取文件名字(后缀过滤)
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="type">后缀名</param>
        /// <returns>return : 文件名集合</returns>
        public static List<string> GetNames_File(string path, string type)
        {
            List<string> FileNames = new List<string>();
            DirectoryInfo info = new DirectoryInfo(path);//读取路径文件夹文件
            FileInfo[] infos = info.GetFiles();//读取所有文件名
            type = type.ToUpper();
            foreach (FileInfo file in infos)//遍历每个文件
            {
                string[] fileName = file.Name.Split('.');
                if (type == fileName[fileName.Length - 1].ToUpper()) FileNames.Add(file.Name);//存入链表        
            }
            return FileNames;
        }

        /// <summary>
        /// 读取文件名字(后缀集合过滤)
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="types">后缀集合</param>
        /// <returns>return : 文件名集合</returns>
        public static List<string> GetNames_File(string path, string[] types)
        {
            List<string> FileNames = new List<string>();
            DirectoryInfo info = new DirectoryInfo(path);//读取路径文件夹文件
            FileInfo[] infos = info.GetFiles();//读取所有文件名

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
        /// 读取文件路径
        /// </summary>
        /// <param name="path">文件夹</param>
        /// <returns>return : 文件路径集合</returns>
        public static List<string> GetPaths_File(string path)
        {
            return new List<string>(System.IO.Directory.GetFiles(path));
        }

        /// <summary>
        /// 读取文件路径（后缀过滤）
        /// </summary>
        /// <param name="path">文件夹</param>
        /// <param name="type">后戳名</param>
        /// <returns>return : 文件路径集合</returns>
        public static List<string> GetPaths_File(string path, string type)
        {
            List<string> FileNames = new List<string>();
            foreach (var filePath in System.IO.Directory.GetFiles(path))//读取所有文件
            {
                string[] fileName = filePath.Split('.');//分隔
                if (type.ToUpper() == fileName[fileName.Length - 1].ToUpper())//判断后缀相等
                    FileNames.Add(filePath);//存入链表        
            }
            return FileNames;
        }

        /// <summary>
        /// 读取文件路径（后缀集合过滤）
        /// </summary>
        /// <param name="path">文件夹</param>
        /// <param name="type">后戳集合</param>
        /// <returns>return : 文件路径集合</returns>
        public static List<string> GetPaths_File(string path, string[] types)
        {
            List<string> FileNames = new List<string>();
            foreach (var filePath in System.IO.Directory.GetFiles(path))//读取所有文件
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


        /// <summary>
        /// 获取枚举路径
        /// </summary>
        /// <param name="location">路径枚举</param>
        /// <returns>路径</returns>
        public static string GetPath(FileLocation location)
        {
            string result = string.Empty;
            switch (location)
            {
                case FileLocation.DataFolder:
                    result = Application.dataPath;
                    break;
                case FileLocation.StreamingAssetsFolder:
                    result = Application.streamingAssetsPath;
                    break;
                case FileLocation.PeristentDataFolder:
                    result = Application.persistentDataPath;
                    break;
            }
            return result;
        }

        #endregion



        #region 文件夹路径读取
        /// <summary>
        /// 读取文件夹名字
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns>return : 文件夹名集合</returns>
        public static List<string> GetNames_Folder(string path)
        {
            List<string> FolderNames = new List<string>();
            DirectoryInfo[] dirInfo = new DirectoryInfo(path).GetDirectories();
            foreach (var Folder in dirInfo)
            {
                FolderNames.Add(Folder.Name);
            }
            return FolderNames;
        }

        /// <summary>
        /// 读取文件夹路径
        /// </summary>
        /// <param name="path">文件夹</param>
        /// <returns>return : 文件路径集合</returns>
        public static List<string> GetPaths_Folder(string path)
        {
            List<string> FolderNames = new List<string>();
            DirectoryInfo[] dirInfo = new DirectoryInfo(path).GetDirectories();
            foreach (var Folder in dirInfo)
            {
                FolderNames.Add(path + "/" + Folder.Name);
            }
            return FolderNames;
        }

        #endregion

        #region 路径新建

        /// <summary>
        /// 文件夹路径创建
        /// </summary>
        /// <param name="path">要创建的路径</param>
        public static void Path_New(string path)
        {
            Directory.CreateDirectory(path);//如果文件夹不存在就创建它
        }

        #endregion

        #region 路径删除

        /// <summary>
        /// 文件夹路径删除
        /// </summary>
        /// <param name="path">要删除的路径</param>
        /// <param name="Mode">（true/false）删除子路径/删除文件夹</param>
        public static void Path_Delete(string path, bool Mode = true)
        {
            if (Mode)
            {
                Directory.GetFiles(path).ToList().ForEach(File.Delete);
                Directory.GetDirectories(path).ToList().ForEach(Directory.Delete);
            }
            else
            {
                Directory.Delete(path, true);
            }
        }

        #endregion

        /// <summary>
        /// 判断文件路径是否存在
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>bool</returns>
        public static bool FilePath_IF(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// 判断文件夹路径是否存在
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>bool</returns>
        public static bool Path_IF(string path)
        {
            return Directory.Exists(path);
        }

    }
}