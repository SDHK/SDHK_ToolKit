
/******************************

 * Author: 闪电黑客

 * 日期: 2022/01/15 15:54:28

 * 最后日期: 2022/01/15 15:56:41

 * 最后修改: 闪电黑客

 * 描述:  
    
    加载AB包到路径资源结构

******************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using AsyncAwaitEvent;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace PathAssets
{
    /// <summary>
    /// 全局路径数据：一个可以全局调用的 PathData
    /// </summary>
    public static class PathAssetGlobal
    {
        /// <summary>
        /// 节点
        /// </summary>
        public static Dictionary<string, object> Assets = new Dictionary<string, object>();
    }

    public class PathAssetLoader
    {

        private Dictionary<string, object> assetBundles = new Dictionary<string, object>();
        public Dictionary<string, object> pathAsset = new Dictionary<string, object>();


        ///<summary>
        /// 事件：加载清单全部内容
        /// </summary>
        public event Action<PathAssetLoader> EventLoadAll;

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
        public event Action<PathAssetLoader> EventLoadAllDone;

        /// <summary>
        /// 事件：数据更新
        /// </summary>
        public event Action<PathAssetLoader> EventDataUpdate;


        public string path;
        public string manifestName;
        public string DataExtension = "PathData";

        public PathAssetLoader() { }
        public PathAssetLoader(string path, string manifestName)
        {
            this.path = path;
            this.manifestName = manifestName;
        }

        public async Task LoadAll()
        {
#if UNITY_EDITOR //编辑器模式
            await EditorLoadAll();

#else //非编辑器模式

            EventLoadAll?.Invoke(this);

            await LoadRootData();
            await Load(manifestName);

            AssetBundleManifest manifest = pathAsset.NodeGet<AssetBundleManifest>(manifestName, "AssetBundleManifest");
            foreach (var assetBundleName in manifest.GetAllAssetBundles())
            {
                await LoadAllDependencies(manifest, assetBundleName);
            }

            assetBundles.Clear();

            EventLoadAllDone?.Invoke(this);
           
#endif


        }


        private async Task EditorLoadAll()
        {
#if UNITY_EDITOR //编辑器模式

            EventLoadAll?.Invoke(this);

            string rootDataABname = "_rootdata";
            var rootDataPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(rootDataABname, "RootData.PathData");
            if (rootDataPaths.Length > 0)
            {
                pathAsset = Convert_BytesToObject<Dictionary<string, object>>(AssetDatabase.LoadAssetAtPath<TextAsset>(rootDataPaths[0]).bytes);
            }


            foreach (var abName in AssetDatabase.GetAllAssetBundleNames())
            {
                if (abName != rootDataABname)
                {
                    foreach (var assetPath in AssetDatabase.GetAssetPathsFromAssetBundle(abName))
                    {
                        string assetName = Path.GetFileNameWithoutExtension(assetPath);
                        string[] names = assetName.Split('.');

                        var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                        if (names[names.Length - 1] == DataExtension)
                        {
                            pathAsset.Node(abName).Set(Convert_BytesToObject<Dictionary<string, object>>((asset as TextAsset).bytes), names[0]);
                        }
                        else
                        {
                            pathAsset.Node(abName).Set(asset, assetName, asset.GetType().ToString());
                        }

                        EventDataUpdate?.Invoke(this);

                        await Task.Yield();
                    }
                }
            }
            EventLoadAllDone?.Invoke(this);
#endif

        }


        public async Task LoadRootData()
        {

            string ABname = "_rootdata";

            var asyncOperation = UnityWebRequestAssetBundle.GetAssetBundle(path + "/" + ABname).SendWebRequest();
            EventLoad?.Invoke(asyncOperation);
            await asyncOperation;
            UnityWebRequest request = asyncOperation.webRequest;
            if (request.isHttpError || request.isNetworkError || !request.isDone)
            {
                Debug.Log("PathAssetLoader NoRoot :" + path + "/" + ABname);
            }
            else
            {
                AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(request);
                assetBundles.Set(assetBundle, ABname);
                var assetsAsync = assetBundle.LoadAssetAsync<TextAsset>("RootData.PathData");
                await assetsAsync;
                pathAsset = Convert_BytesToObject<Dictionary<string, object>>((assetsAsync.asset as TextAsset).bytes);
                EventDataUpdate?.Invoke(this);
            }
            EventLoadDone?.Invoke(request);
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
        public async Task Load(string ABname)
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
                assetBundles.Set(assetBundle, ABname);

                await LoadObjectsAsync(ABname, assetBundle);
                EventDataUpdate?.Invoke(this);
            }
            EventLoadDone?.Invoke(request);
        }

        //异步获取全部对象塞入字典
        public async Task LoadObjectsAsync(string ABname, AssetBundle assetBundle)
        {
            AssetBundleRequest assetsAsync = assetBundle.LoadAllAssetsAsync();
            await assetsAsync;
            foreach (var asset in assetsAsync.allAssets)
            {
                // Debug.Log("AB包：[" + ABname + "]  名称：[" + asset.name + "]  类型：[" + asset.GetType() + "]");

                string[] names = asset.name.Split('.');
                if (names[names.Length - 1] == DataExtension)
                {
                    pathAsset.Node(ABname).Set(Convert_BytesToObject<Dictionary<string, object>>((asset as TextAsset).bytes), names[0]);
                }
                else
                {
                    pathAsset.Node(ABname).Set(asset, asset.name, asset.GetType().ToString());
                }
            }

            // assetBundle.Unload(false);//!..............
        }


        /// <summary>
        /// 将对象序列化为二进制数据:对象定义时需[Serializable]序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>byte数组</returns>
        public static byte[] Convert_ObjectToBytes(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, obj);
            byte[] data = stream.ToArray();
            stream.Close();
            return data;
        }

        /// <summary>
        /// 将二进制数据反序列化
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>对象</returns>
        public static object Convert_BytesToObject(byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(stream);
            stream.Close();
            return obj;
        }

        /// <summary>
        /// 将二进制数据反序列化
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>对象</returns>
        public static T Convert_BytesToObject<T>(byte[] data)
        {
            return (T)Convert_BytesToObject(data);
        }



    }

}