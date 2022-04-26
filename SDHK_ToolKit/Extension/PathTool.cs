using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


namespace SDHK_Extension
{



    public static class PathTool
    {


        /// <summary>
        /// 查找指定文件夹下指定后缀名的作有文件
        /// </summary>
        /// <param name="directory">文件夹</param>
        /// <param name="searchPattern">后缀名</param>
        /// <returns>文件路径集合</returns>
        public static void GetAllFilePaths(DirectoryInfo directory, string searchPattern, List<string> fileList)
        {
            if (directory.Exists || searchPattern.Trim() != string.Empty)
            {
                foreach (FileInfo info in directory.GetFiles(searchPattern))
                {
                    fileList.Add(info.FullName.ToString());
                }

                foreach (DirectoryInfo info in directory.GetDirectories())//获取文件夹下的子文件夹
                {
                    GetAllFilePaths(info, searchPattern, fileList);//递归调用该函数，获取子文件夹下的文件
                }
            }
        }

        public static string[] GetFilePaths(this string path, string searchPattern)
        {
            if (Directory.Exists(path))
            {
                return new DirectoryInfo(path).GetFiles(searchPattern).Select((file) => file.FullName).ToArray();
            }
            return null;
        }

        public static string[] GetFilePaths(this string path)
        {
            if (Directory.Exists(path))
            {
                return new DirectoryInfo(path).GetFiles().Select((file) => file.FullName).ToArray();
            }
            return null;
        }

        public static string[] GetDirectoryNames(this string path)
        {
            if (Directory.Exists(path))
            {
                return new DirectoryInfo(path).GetDirectories().Select((directorie) => directorie.Name).ToArray();
            }
            return null;
        }

    }
}