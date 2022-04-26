/******************************

 * 作者: 闪电黑客

 * 日期: 2021/07/08 06:30:29

 * 最后日期: 2021/07/08 06:30:29

 * 描述: 
    协程服务扩展：Web请求等待

******************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace CoroutineSystem
{


    /// <summary>
    /// Web请求
    /// </summary>
    public static class WebRequest
    {
        /// <summary>
        /// 协程启动请求等待
        /// </summary>
        /// <param name="request">WebRequest</param>
        /// <param name="callBack">回调</param>
        public static UnityWebRequest CoroutineWeb(this UnityWebRequest request, Action<UnityWebRequest> callBack)
        {

            CoroutineServer.Instance().StartCoroutine(WebCallback(request, callBack));
            return request;
        }

        /// <summary>
        /// 协程启动请求等待
        /// </summary>
        /// <param name="request">WebRequest</param>
        /// <param name="mono">运行的主线程</param>
        /// <param name="callBack">回调</param>
        /// <returns></returns>
        public static UnityWebRequest CoroutineWeb(this UnityWebRequest request, MonoBehaviour mono, Action<UnityWebRequest> callBack)
        {
            mono.StartCoroutine(WebCallback(request, callBack));
            return request;
        }

        private static IEnumerator WebCallback(UnityWebRequest request, Action<UnityWebRequest> callBack)
        {
            yield return request.SendWebRequest();
            callBack(request);
        }

        //!!!
        public static AssetBundle GetAssetBundle(this UnityWebRequest request)
        {
            if (!(request.isHttpError || request.isNetworkError) && request.isDone)
            {
                return DownloadHandlerAssetBundle.GetContent(request);
            }

            return null;
        }
    }



}