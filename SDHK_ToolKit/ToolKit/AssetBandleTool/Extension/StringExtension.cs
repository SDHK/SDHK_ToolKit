

/******************************

 * 作者: 闪电黑客

 * 日期: 2021/07/09 07:24:29

 * 最后日期: 2021/07/09 07:30:29

 * 描述: 
    AssetBandle管理器的字符串扩展：
    
    简化书写


******************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AssetBandleTool
{

    public static class StringExtension
    {


        /// <summary>
        /// 从一个AB包内获取指定类型的对象名称列表
        /// </summary>
        /// <param name="ABname">包名</param>
        /// <typeparam name="T">类型名</typeparam>
        public static string[] ABAssetNames<T>(this string ABname)
        {
            return AssetBandleManager.Instance().GetAssetNames(ABname, typeof(T));
        }

        /// <summary>
        /// 加载资源对象 ：需要先加载好AB包
        /// </summary>
        /// <param name="ABname">AB包名字</param>
        /// <param name="assetName">资源对象名字</param>
        /// <typeparam name="T">资源类型</typeparam>
        public static T ABLoadAsset<T>(this string ABname, string assetName)
        where T : UnityEngine.Object
        {
            return AssetBandleManager.Instance().LoadAsset<T>(ABname, assetName);
        }

        /// <summary>
        /// 卸载AB包(true/false) 释放那个AssetBundle文件内存镜像和并销毁所有用Load创建的Asset内存对象。/释放AssetBundle文件的内存镜像，不包含Load创建的Asset内存对象。
        /// </summary>
        /// <param name="ABname">AB包名字</param>
        /// <param name="unloadAllLoadedObjects">卸载模式</param>
        public static void ABUnload(this string ABname, bool unloadAllLoadedObjects = false)
        {
            AssetBandleManager.Instance().Unload(ABname, unloadAllLoadedObjects);
        }



    }

}