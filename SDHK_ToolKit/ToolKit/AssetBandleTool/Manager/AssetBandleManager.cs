/******************************

 * 作者: 闪电黑客

 * 日期: 2021/07/09 07:24:29

 * 最后日期: 2021/07/09 07:30:29

 * 描述: 
    简单的AssetBandle管理器：
    
    主要用于加载本地AB包到Unity


******************************/


using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Singleton;
using SDHK_Extension;
using AsyncAwaitEvent;
using System.Threading.Tasks;
using PathAssets;

namespace AssetBandleTool
{

    /// <summary>
    /// AssetBandle管理器
    /// </summary>
    public class AssetBandleManager : SingletonMonoBase<AssetBandleManager>
    {

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                Debug.Log("AssetBandleManager 启动！");
            }
        }

        /// <summary>
        /// AB包清单
        /// </summary>
        public AssetBundleManifest manifest;

        /// <summary>
        /// AB包列表
        /// </summary>
        public Dictionary<string, AssetBundle> assetBundles = new Dictionary<string, AssetBundle>();


        /// <summary>
        /// AB包的资源对象字典:(AB包名,类型,对象名称，对象资源 ) 游戏对象会储存在这，进行重复取用（例如预制体，图片资源等...）
        /// </summary>
        public Dictionary<string, Dictionary<Type, Dictionary<string, UnityEngine.Object>>> assetObject = new Dictionary<string, Dictionary<Type, Dictionary<string, UnityEngine.Object>>>();


        ///<summary>
        /// 事件：加载清单全部内容
        /// </summary>
        public event Action EventLoadAll;

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
        public event Action<List<UnityWebRequest>> EventLoadAllDone;


        /// <summary>
        /// 事件：数据更新
        /// </summary>
        public event Action<AssetBandleManager> EventDataUpdate;


        public ApplicationPathEnum RootPath = ApplicationPathEnum.Path;
        [Space()]

        public string path = "";

        public string mainName = "";

        private string FullPath()
        {
            return ApplicationExtension.GetPath(RootPath) + path;
        }

        /// <summary>
        /// 获取资源对象的名字列表：需要先加载好AB包，包名为""时返回的是AB包名列表
        /// </summary>
        /// <param name="ABname">包名</param>
        /// <param name="type">类型名</param>
        public string[] GetAssetNames(string ABname = "", Type type = null)
        {
            if (ABname == "")
            {
                return assetObject.Keys.ToArray();
            }
            else
            {
                if (assetObject.ContainsKey(ABname))
                {
                    if (type == null)
                    {
                        return assetObject[ABname].SelectMany((a) => a.Value.Keys.ToArray()).ToArray();
                    }
                    else
                    {
                        if (assetObject[ABname].ContainsKey(type))
                        {
                            return assetObject[ABname][type].Keys.ToArray();
                        }
                        else
                        {
                            return new string[0];
                        }
                    }
                }
                else
                {
                    return new string[0];
                }
            }
        }



        /// <summary>
        /// 加载资源对象 ：需要先加载好AB包
        /// </summary>
        /// <param name="ABname">AB包名字</param>
        /// <param name="assetName">资源对象名字</param>
        /// <typeparam name="T">资源类型</typeparam>
        public T LoadAsset<T>(string ABname, string assetName)
        where T : UnityEngine.Object
        {
            if (assetObject.ContainsKey(ABname))
            {
                if (assetObject[ABname].ContainsKey(typeof(T)))
                {
                    if (assetObject[ABname][typeof(T)].ContainsKey(assetName))
                    {
                        return assetObject[ABname][typeof(T)][assetName] as T;
                    }
                }
            }

            if (assetBundles.ContainsKey(ABname))
            {
                T assetObj = assetBundles[ABname].LoadAsset<T>(assetName);

                if (assetObj != null)
                {
                    if (assetObject.ContainsKey(ABname))
                    {
                        if (assetObject[ABname].ContainsKey(typeof(T)))
                        {
                            if (!assetObject[ABname][typeof(T)].ContainsKey(assetName))
                            {
                                assetObject[ABname][typeof(T)].Add(assetName, assetObj);
                            }
                        }
                        else
                        {
                            assetObject[ABname].Add(typeof(T), new Dictionary<string, UnityEngine.Object>()
                            {
                                [assetName] = assetObj
                            });
                        }
                    }
                    else
                    {
                        assetObject.Add(ABname, new Dictionary<Type, Dictionary<string, UnityEngine.Object>>()
                        {
                            [typeof(T)] = new Dictionary<string, UnityEngine.Object>()
                            {
                                [assetName] = assetObj
                            }
                        }
                        );
                    }
                }

                return assetObj;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 卸载AB包(true/false) 释放那个AssetBundle文件内存镜像和并销毁所有用Load创建的AssetObject内存对象。/释放AssetBundle文件的内存镜像，不包含Load创建的AssetObject内存对象。
        /// </summary>
        /// <param name="ABname">AB包名字</param>
        /// <param name="unloadAllLoadedObjects">卸载模式</param>
        public void Unload(string ABname = "", bool unloadAllLoadedObjects = false)
        {
            if (ABname == "")
            {
                foreach (var assetBundle in assetBundles)
                {
                    assetBundle.Value.Unload(unloadAllLoadedObjects);
                }

                if (unloadAllLoadedObjects)
                {
                    assetObject.Clear();
                }
                assetBundles.Clear();
            }

            if (assetBundles.ContainsKey(ABname))
            {
                assetBundles[ABname].Unload(unloadAllLoadedObjects);

                if (unloadAllLoadedObjects)
                {
                    assetObject.Remove(ABname);
                }

                assetBundles.Remove(ABname);

                // Resources.UnloadUnusedAssets();
            }
        }



        /// <summary>
        /// 加载清单全部内容
        /// </summary>
        public async Task<List<UnityWebRequest>> LoadAll()
        {
            Task<UnityWebRequest> TaskLoad;
            List<UnityWebRequest> AllResults = new List<UnityWebRequest>();

            EventLoadAll?.Invoke();

            TaskLoad = Load(mainName);
            await TaskLoad;
            AllResults.Add(TaskLoad.Result);

            manifest = assetBundles[mainName].LoadAsset<AssetBundleManifest>("AssetBundleManifest");//获取清单文件列表
            foreach (var assetBundleName in manifest.GetAllAssetBundles())
            {
                var TaskLoadAll = LoadAllDependencies(manifest, assetBundleName);
                await TaskLoadAll;
                foreach (var LoadAllResult in TaskLoadAll.Result)
                {
                    AllResults.Add(LoadAllResult);
                }
            }

            EventLoadAllDone?.Invoke(AllResults);

            return AllResults;
        }

        //递归加载所有依赖
        private async Task<List<UnityWebRequest>> LoadAllDependencies(AssetBundleManifest assetBundleManifest, string assetBundleName)
        {
            List<UnityWebRequest> AllResults = new List<UnityWebRequest>();

            foreach (var dependencieName in assetBundleManifest.GetAllDependencies(assetBundleName))
            {
                if (!assetBundles.ContainsKey(dependencieName))
                {
                    var TaskLoadAll = LoadAllDependencies(assetBundleManifest, dependencieName);

                    await TaskLoadAll;

                    foreach (var LoadAllResult in TaskLoadAll.Result)
                    {
                        AllResults.Add(LoadAllResult);
                    }

                }
            }

            if (!assetBundles.ContainsKey(assetBundleName))
            {
                Task<UnityWebRequest> TaskLoad = Load(assetBundleName);
                await TaskLoad;
                AllResults.Add(TaskLoad.Result);
            }

            return AllResults;
        }


        /// <summary>
        /// 加载
        /// </summary>
        public async Task<UnityWebRequest> Load(string ABname)
        {
            var asyncOperation = UnityWebRequestAssetBundle.GetAssetBundle(FullPath() + "/" + ABname).SendWebRequest();

            EventLoad?.Invoke(asyncOperation);

            await asyncOperation;

            UnityWebRequest request = asyncOperation.webRequest;

            if (request.isHttpError || request.isNetworkError || !request.isDone)
            {
                Debug.Log("AssetBundleManager Load Error:" + ABname);
            }
            else
            {
                AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(request);
                if (assetBundles.ContainsKey(ABname))
                {
                    assetBundles[ABname] = assetBundle;
                }
                else
                {
                    assetBundles.Add(ABname, assetBundle);
                }
                await AsyncLoadAllAssetObject(ABname, assetBundle);
                EventDataUpdate?.Invoke(this);
            }
            EventLoadDone?.Invoke(request);

            return request;
        }



        private async Task AsyncLoadAllAssetObject(string ABname, AssetBundle assetBundle)
        {
            AssetBundleRequest assetsAsync = assetBundle.LoadAllAssetsAsync();
            await assetsAsync;

            foreach (var asset in assetsAsync.allAssets)
            {
                // Debug.Log("AB包：[" + ABname + "]  名称：[" + asset.name + "]  类型：[" + asset.GetType().Name + "]");

                if (assetObject.ContainsKey(ABname))
                {
                    if (assetObject[ABname].ContainsKey(asset.GetType()))
                    {
                        if (!assetObject[ABname][asset.GetType()].ContainsKey(asset.name))
                        {
                            assetObject[ABname][asset.GetType()].Add(asset.name, asset);
                        }
                        else
                        {
                            // Debug.Log("assetObject重复：[" + ABname + "]  名称：[" + asset.name + "]  类型：[" + asset.GetType() + "]");
                            assetObject[ABname][asset.GetType()][asset.name] = asset;
                        }
                    }
                    else
                    {
                        assetObject[ABname].Add(asset.GetType(), new Dictionary<string, UnityEngine.Object>()
                        {
                            [asset.name] = asset
                        });
                    }
                }
                else
                {
                    assetObject.Add(ABname, new Dictionary<Type, Dictionary<string, UnityEngine.Object>>()
                    {
                        [asset.GetType()] = new Dictionary<string, UnityEngine.Object>()
                        {
                            [asset.name] = asset
                        }
                    }
                    );
                }
            }
        }


    }
}
