using System.Collections.Generic;
using System.IO;
using System.Linq;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.12.3
 * 
 * 功能：SS_Flie的分支部分：对文件路径进行处理的方法
 */

namespace SDHK_Tool.Static
{

    public static partial class SS_File
    {

        #region 文件路径读取

        /// <summary>
        /// 读取文件名字
        /// </summary>
        /// <param name="Path">文件夹路径</param>
        /// <returns>return : 文件名集合</returns>
        public static List<string> GetNames_File(string Path)
        {
            List<string> FileNames = new List<string>();
            DirectoryInfo info = new DirectoryInfo(Path);//读取路径文件夹文件
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
        /// <param name="Path">文件夹路径</param>
        /// <param name="type">后缀名</param>
        /// <returns>return : 文件名集合</returns>
        public static List<string> GetNames_File(string Path, string type)
        {
            List<string> FileNames = new List<string>();
            DirectoryInfo info = new DirectoryInfo(Path);//读取路径文件夹文件
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
        /// <param name="Path">文件夹路径</param>
        /// <param name="types">后缀集合</param>
        /// <returns>return : 文件名集合</returns>
        public static List<string> GetNames_File(string Path, string[] types)
        {
            List<string> FileNames = new List<string>();
            DirectoryInfo info = new DirectoryInfo(Path);//读取路径文件夹文件
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
        /// <param name="Path">文件夹</param>
        /// <returns>return : 文件路径集合</returns>
        public static List<string> GetPaths_File(string Path)
        {
            return new List<string>(System.IO.Directory.GetFiles(Path));
        }

        /// <summary>
        /// 读取文件路径（后缀过滤）
        /// </summary>
        /// <param name="Path">文件夹</param>
        /// <param name="type">后戳名</param>
        /// <returns>return : 文件路径集合</returns>
        public static List<string> GetPaths_File(string Path, string type)
        {
            List<string> FileNames = new List<string>();
            foreach (var filePath in System.IO.Directory.GetFiles(Path))//读取所有文件
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
        /// <param name="Path">文件夹</param>
        /// <param name="type">后戳集合</param>
        /// <returns>return : 文件路径集合</returns>
        public static List<string> GetPaths_File(string Path, string[] types)
        {
            List<string> FileNames = new List<string>();
            foreach (var filePath in System.IO.Directory.GetFiles(Path))//读取所有文件
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
        /// 读取文件夹名字
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
        /// 读取文件夹路径
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




    }
}