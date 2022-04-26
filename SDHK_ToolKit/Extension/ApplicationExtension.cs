using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SDHK_Extension
{
    /// <summary>
    /// 根路径枚举
    /// </summary>
    public enum ApplicationPathEnum
    {
        /// <summary>
        /// “”空字符串
        /// </summary>
        Path,
        StreamingAssetsPath,
        DataPath,
        PersistentDataPath
    }

    public static class ApplicationExtension
    {

        /// <summary>
        /// 根据枚举获取路径 ，Path返回空字符串
        /// </summary>
        public static string GetPath(ApplicationPathEnum e)
        {
            switch (e)
            {
                case ApplicationPathEnum.Path: return "";
                case ApplicationPathEnum.StreamingAssetsPath: return Application.streamingAssetsPath;
                case ApplicationPathEnum.DataPath: return Application.dataPath;
                case ApplicationPathEnum.PersistentDataPath: return Application.persistentDataPath;
            }

            return "";
        }
    }
}