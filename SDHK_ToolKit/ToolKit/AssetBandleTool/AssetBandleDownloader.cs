
/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/22 15:02:28

 * 最后日期: 2021/12/22 15:02:52

 * 最后修改: 闪电黑客

 * 描述:   //!暂时无用用于参考

******************************/
using System;
using UnityEngine;
using UnityEngine.Networking;
using AsyncAwaitEvent;
using System.Threading.Tasks;
using PathAssetsTest;

namespace AssetBandleTool
{

   

    /// <summary>
    /// AssetBandle加载器
    /// </summary>
    public class AssetBandleDownloader
    {
        /// <summary>
        /// AssetBundle集合
        /// </summary>
        public PathData assetBundles = new PathData();

        /// <summary>
        /// 资源对象
        /// </summary>
        public PathData assetDatas = new PathData();

        ///<summary>
        /// 事件：加载清单全部内容
        /// </summary>
        public event Action<AssetBandleDownloader> EventLoadAll;

        ///<summary>
        /// 事件：单个ab包加载
        /// </summary>
        public event Action<UnityWebRequestAsyncOperation> EventLoad;
        /// <summary>
        /// 事件：单个加载完成
        /// </summary>
        public event Action<UnityWebRequest> EventLoadDone;

        /// <summary>
        /// 事件：加载全部完成
        /// </summary>
        public event Action<AssetBandleDownloader> EventLoadAllDone;

        /// <summary>
        /// 事件：数据更新
        /// </summary>
        public event Action<AssetBandleDownloader> EventDataUpdate;

        public string path;
        public string manifestName;


        public AssetBandleDownloader(string path, string manifestName)
        {
            this.path = path;
            this.manifestName = manifestName;
        }

        /// <summary>
        /// 卸载AB包(true/false) 释放那个AssetBundle文件内存镜像和并销毁所有用Load创建的AssetObject内存对象。/释放AssetBundle文件的内存镜像，不包含Load创建的AssetObject内存对象。
        /// </summary>
        /// <param name="ABname">AB包名字</param>
        /// <param name="unloadObjects">卸载模式</param>
        public void Unload(string ABname = "", bool unloadObjects = false)
        {
            if (ABname == "")
            {
                foreach (var assetBundle in assetBundles)
                {
                    ((AssetBundle)assetBundle.Value).Unload(unloadObjects);
                }

                if (unloadObjects)
                {
                    assetDatas.Clear();
                }
                assetBundles.Clear();
            }

            if (assetBundles.ContainsPath(ABname))
            {
                assetBundles.Get<AssetBundle>(ABname).Unload(unloadObjects);

                if (unloadObjects)
                {
                    assetDatas.RemoveNode(ABname);
                }

                assetBundles.RemoveNode(ABname);

                // Resources.UnloadUnusedAssets();
            }
        }


        public async Task LoadAll()
        {
            Task<UnityWebRequest> TaskLoad;

            EventLoadAll?.Invoke(this);

            TaskLoad = Load(manifestName);
            await TaskLoad;

            AssetBundleManifest manifest = assetBundles.Get<AssetBundle>(manifestName).LoadAsset<AssetBundleManifest>("AssetBundleManifest");//获取清单文件列表
            foreach (var assetBundleName in manifest.GetAllAssetBundles())
            {
                var TaskLoadAll = LoadAllDependencies(manifest, assetBundleName);
                await TaskLoadAll;
            }

            EventLoadAllDone?.Invoke(this);
        }
        //递归加载所有依赖
        public async Task LoadAllDependencies(AssetBundleManifest assetBundleManifest, string assetBundleName)
        {
            foreach (var dependencieName in assetBundleManifest.GetAllDependencies(assetBundleName))
            {
                if (!assetBundles.ContainsPath(dependencieName))
                {
                    await LoadAllDependencies(assetBundleManifest, dependencieName); ;
                }
            }

            if (!assetBundles.ContainsPath(assetBundleName))
            {
                await Load(assetBundleName);
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        public async Task<UnityWebRequest> Load(string ABname)
        {

            var asyncOperation = UnityWebRequestAssetBundle.GetAssetBundle(path + "/" + ABname).SendWebRequest();

            EventLoad?.Invoke(asyncOperation);

            await asyncOperation;

            UnityWebRequest request = asyncOperation.webRequest;

            if (request.isHttpError || request.isNetworkError || !request.isDone)
            {
                Debug.Log("AssetBundleData Load Error:" + path + "/" + ABname);
            }
            else
            {
                AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(request);

                assetBundles[ABname] = assetBundle;

                await LoadObjectsAsync(ABname, assetBundle);
                EventDataUpdate?.Invoke(this);
            }
            EventLoadDone?.Invoke(request);

            return request;
        }

        //异步获取全部对象塞入字典
        public async Task LoadObjectsAsync(string ABname, AssetBundle assetBundle)
        {
            AssetBundleRequest assetsAsync = assetBundle.LoadAllAssetsAsync();
            await assetsAsync;
            foreach (var asset in assetsAsync.allAssets)
            {
                //  Debug.Log("AB包：[" + ABname + "]  名称：[" + asset.name + "]  类型：[" + asset.GetType() + "]");

                if (assetDatas.ContainsPath(asset.GetType(), ABname, asset.name))
                {
                    Debug.Log("assetObject重复：[" + ABname + "]  名称：[" + asset.name + "]  类型：[" + asset.GetType() + "]");
                }

                assetDatas.NodeSet(asset, asset.GetType(), ABname, asset.name);
            }
        }

    }


}