using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class SS_Flie1
{

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




}
