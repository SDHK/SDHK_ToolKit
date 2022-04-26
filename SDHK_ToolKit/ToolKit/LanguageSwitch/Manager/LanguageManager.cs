
/******************************

 * 作者: 闪电黑客

 * 日期: 2021/06/3 07:02:29

 * 最后日期: 。。。

 * 描述: 
    语言切换管理器

    简易的语言键值替换功能。

    自动全局单例化，需要先载入文件数据才可以使用。
    

******************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using LitJson;
using System;
using UnityEngine.Networking;
using Singleton;
using System.Text.RegularExpressions;
using SDHK_Extension;
using AsyncAwaitEvent;
using System.Threading.Tasks;
using PathAssets;

namespace LanguageSwitch
{
    /// <summary>
    /// 多语言切换管理器
    /// </summary>
    public class LanguageManager : SingletonMonoBase<LanguageManager>
    {
        public Dictionary<string, object> Data = new Dictionary<string, object>();
        private List<ILanguageRefresh> Boxs = new List<ILanguageRefresh>();

        /// <summary>
        /// 注册当前文本盒子：激活或语言切换时将调用刷新功能
        /// </summary>
        public void AddBox(ILanguageRefresh boxBase)
        {
            Boxs.Add(boxBase);
        }

        /// <summary>
        /// 移除当前文本盒子
        /// </summary>
        public void RemoveBox(ILanguageRefresh boxBase)
        {
            Boxs.Remove(boxBase);
        }

        /// <summary>
        /// 用于切换语言后刷新切换当前激活的文本
        /// </summary>
        public void RefreshBoxs()
        {
            foreach (var Box in Boxs)
            {
                Box?.LanguageRefresh();
            }
        }


        // /// <summary>
        // /// 获取值
        // /// </summary>
        // public object GetValue(string path)
        // {
        //     var value = Data.Get(path);
        //     return (value != null) ? value : "[" + path + "]";
        // }

        // /// <summary>
        // /// 获取值
        // /// </summary>
        // public void SetValue(string path, object value)
        // {
        //     Data.Set(value, path);
        // }
    }

    // /// <summary>
    // /// 多语言切换管理器
    // /// </summary>
    // public class LanguageManager : SingletonMonoBase<LanguageManager>
    // {
    //     public ApplicationPathEnum RootPath = ApplicationPathEnum.Path;
    //     [Space()]
    //     public string path = "";
    //     public string languageFolderName = "";
    //     private string extension = ".LanguageSwitch";


    //     [NonSerialized]
    //     public List<string> languageNames = new List<string>();



    //     public Dictionary<string, Dictionary<string, string>> languageData = new Dictionary<string, Dictionary<string, string>>();

    //     private List<ILanguageRefresh> Boxs = new List<ILanguageRefresh>();

    //     ///<summary>
    //     /// 事件：加载全部
    //     /// </summary>
    //     public event Action EventLoadAll;

    //     ///<summary>
    //     /// 事件：加载
    //     /// </summary>
    //     public event Action<UnityWebRequestAsyncOperation> EventLoad;


    //     /// <summary>
    //     /// 事件：加载完成
    //     /// </summary>
    //     public event Action<UnityWebRequest> EventLoadDone;

    //     /// <summary>
    //     /// 事件：加载全部完成
    //     /// </summary>
    //     public event Action<List<UnityWebRequest>> EventLoadAllDone;


    //     /// <summary>
    //     /// 事件：数据更新
    //     /// </summary>
    //     public event Action<LanguageManager> EventDataUpdate;


    //     private string FullPath()
    //     {
    //         return ApplicationExtension.GetPath(RootPath) + path;
    //     }


    //     /// <summary>
    //     /// 获取同目录中的 所有文件夹名称
    //     /// </summary>
    //     public List<string> GetLanguageNames()
    //     {
    //         languageNames = new DirectoryInfo(FullPath()).GetDirectories()
    //        .Select((directories) => directories.Name).ToList();
    //         return languageNames;
    //     }


    //     /// <summary>
    //     /// 加载路径文件夹内所有文件
    //     /// </summary>
    //     public async Task<List<UnityWebRequest>> LoadAll()
    //     {
    //         GetLanguageNames();
    //         languageData.Clear();
    //         var task = LoadAll(new DirectoryInfo(FullPath() + "/" + languageFolderName).GetFiles("*" + extension).Select((fileInfo) => fileInfo.FullName).ToList());
    //         await task;
    //         return task.Result;
    //     }

    //     /// <summary>
    //     /// 加载uri集合
    //     /// </summary>
    //     public async Task<List<UnityWebRequest>> LoadAll(List<string> uris)
    //     {
    //         List<UnityWebRequest> AllResults = new List<UnityWebRequest>();
    //         EventLoadAll?.Invoke();

    //         foreach (var uri in uris)
    //         {
    //             Task<UnityWebRequest> TaskLoad = Load(uri);
    //             await TaskLoad;
    //             AllResults.Add(TaskLoad.Result);
    //         }

    //         EventLoadAllDone?.Invoke(AllResults);

    //         return AllResults;
    //     }

    //     /// <summary>
    //     /// 加载
    //     /// </summary>
    //     public async Task<UnityWebRequest> Load(string uri)
    //     {
    //         var asyncOperation = UnityWebRequest.Get(uri).SendWebRequest();
    //         EventLoad?.Invoke(asyncOperation);

    //         await asyncOperation;
    //         UnityWebRequest request = asyncOperation.webRequest;

    //         string groupName = Path.GetFileNameWithoutExtension(request.url);

    //         if (request.isHttpError || request.isNetworkError)
    //         {
    //             Debug.Log("LanguageSwitch Load Error:" + groupName);
    //         }
    //         else
    //         {
    //             Dictionary<string, string> GroupData = JsonMapper.ToObject<Dictionary<string, string>>(request.downloadHandler.text);

    //             if (languageData.ContainsKey(groupName))
    //             {
    //                 languageData[groupName] = GroupData;
    //             }
    //             else
    //             {
    //                 languageData.Add(groupName, GroupData);
    //             }
    //             EventDataUpdate?.Invoke(this);
    //             RefreshBoxs();
    //         }
    //         EventLoadDone?.Invoke(request);

    //         return request;
    //     }


    //     /// <summary>
    //     /// 保存到文件夹
    //     /// </summary>
    //     public void Save()
    //     {
    //         if (Path.IsPathRooted(FullPath()))
    //         {
    //             Directory.CreateDirectory(FullPath());//如果文件夹不存在就创建它
    //             foreach (var data in languageData)
    //             {
    //                 File.WriteAllText(FullPath() + "/" + data.Key + extension, Convert_String(JsonMapper.ToJson(data.Value)));
    //             }
    //         }
    //     }


    //     /// <summary>
    //     /// 注册当前文本盒子：激活或语言切换时将调用刷新功能
    //     /// </summary>
    //     public void AddBox(ILanguageRefresh boxBase)
    //     {
    //         Boxs.Add(boxBase);
    //     }

    //     /// <summary>
    //     /// 移除当前文本盒子
    //     /// </summary>
    //     public void RemoveBox(ILanguageRefresh boxBase)
    //     {
    //         Boxs.Remove(boxBase);
    //     }



    //     /// <summary>
    //     /// 用于切换语言后刷新切换当前激活的文本
    //     /// </summary>
    //     public void RefreshBoxs()
    //     {
    //         foreach (var Box in Boxs)
    //         {
    //             Box?.LanguageRefresh();
    //         }
    //     }


    //     /// <summary>
    //     /// 获取值
    //     /// </summary>
    //     public string GetValue(string group, string key)
    //     {
    //         try
    //         {
    //             return languageData[group][key];
    //         }
    //         catch
    //         {
    //             return "[" + group + "," + key + "]";
    //         }
    //     }

    //     /// <summary>
    //     /// 设置值
    //     /// </summary>
    //     public void SetValue(string group, string key, string value)
    //     {
    //         if (languageData.ContainsKey(group))
    //         {
    //             if (languageData[group].ContainsKey(key))
    //             {
    //                 languageData[group][key] = value;
    //             }
    //             else
    //             {
    //                 languageData[group].Add(key, value);
    //             }
    //         }
    //         else
    //         {
    //             languageData.Add(group, new Dictionary<string, string>() { [key] = value });
    //         }
    //         EventDataUpdate?.Invoke(this);
    //     }


    //     /// <summary>
    //     /// 乱码转换：用于解决LitJson把类转换成string时出现的乱码
    //     /// </summary>
    //     /// <param name="source">乱码字符串</param>
    //     /// <returns>正常字符串</returns>
    //     private string Convert_String(string source)
    //     {
    //         return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase)
    //         .Replace(source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
    //     }





    // }



}