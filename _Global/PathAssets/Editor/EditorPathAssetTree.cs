
/******************************

 * Author: 闪电黑客

 * 日期: 2022/01/15 15:05:30

 * 最后日期: 2022/01/15 15:05:43

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SDHK_Extension;
using UnityEditor;
using UnityEngine;
namespace PathAssets
{

    public enum PathAssetTypeEnum
    {
        String,
        Node,
        Int,
        Float,
        Vector2,
        Vector3,
        Color,
        Object,
        PathData

    }



    [Serializable]
    public class EditorPathAssetTree
    {
        public bool select = false;
        public bool foldout = false;
        public PathAssetTypeEnum assetTypeEnum = PathAssetTypeEnum.String;
        public EditorPathAssetTree parent = null;
        public List<EditorPathAssetTree> subs = new List<EditorPathAssetTree>();
        public string key = "";
        public object value = null;

        public AssetImporter assetImporter = null;
        public UnityEngine.Object showAsset = null;


        //转为字典数据
        public Dictionary<string, object> ToData()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            foreach (var sub in subs)
            {
                if (!data.ContainsKey(sub.key))
                {
                    switch (sub.assetTypeEnum)
                    {
                        case PathAssetTypeEnum.String: data.Add(sub.key, sub.value); break;
                        case PathAssetTypeEnum.Int: data.Add(sub.key, sub.value); break;
                        case PathAssetTypeEnum.Float: data.Add(sub.key, sub.value); break;
                        case PathAssetTypeEnum.Node: data.Add(sub.key, sub.ToData()); break;
                    }
                }
            }
            return data;
        }

        //遍历字典数据建立结构
        public EditorPathAssetTree SetData(Dictionary<string, object> pathData)
        {
            foreach (var data in pathData)
            {
                if (data.Value is string) subs.Add(new EditorPathAssetTree() { parent = this, assetTypeEnum = PathAssetTypeEnum.String, key = data.Key, value = data.Value });
                else if (data.Value is int) subs.Add(new EditorPathAssetTree() { parent = this, assetTypeEnum = PathAssetTypeEnum.Int, key = data.Key, value = data.Value });
                else if (data.Value is float) subs.Add(new EditorPathAssetTree() { parent = this, assetTypeEnum = PathAssetTypeEnum.Float, key = data.Key, value = data.Value });
                else if (data.Value is Dictionary<string, object>) subs.Add(new EditorPathAssetTree() { parent = this, assetTypeEnum = PathAssetTypeEnum.Node, key = data.Key }.SetData(data.Value.To<Dictionary<string, object>>(null)));
            }
            return this;
        }

        //遍历折叠控制
        public void AllFoldout(bool isOpen)
        {
            foldout = isOpen;
            foreach (var item in subs)
            {
                item.AllFoldout(isOpen);
            }
        }

        //移除所有空节点.
        public void RemoveAllBlankNodes()
        {
            for (int i = subs.Count - 1; i >= 0; i--)
            {
                if (subs[i].assetTypeEnum == PathAssetTypeEnum.Node)
                {
                    subs[i].RemoveAllBlankNodes();
                    if (subs[i].subs.Count == 0)
                    {
                        subs.RemoveAt(i);
                    }
                }
            }
        }
        //移除节点.
        public void RemovePath(string path)
        {
            RemovePath(0, path.Split('/', '\\'));
        }
        //移除节点.
        private void RemovePath(int index, params string[] keys)
        {
            string key = keys[index];
            EditorPathAssetTree sub = subs.Find((s) => s.key == key);

            if (index == keys.Length - 1)
            {
                subs.Remove(sub);
            }
            else
            {
                if (sub != null)
                {
                    sub.RemovePath(index + 1, keys);
                }
            }
        }

        #region Asset

        //添加ab包资源的路径
        public void AddAsset(string path, string value)
        {
            AddAsset(0, value, path.Split('/', '\\'));
        }
        //添加ab包资源的ab包路径
        private void AddAsset(int index, string value, params string[] keys)
        {
            string key = keys[index];
            EditorPathAssetTree sub = subs.Find((s) => s.key == key);

            if (index == keys.Length - 1)
            {
                if (sub != null)
                {
                    sub.value = value;
                    sub.LoadShowAsset();
                }
                else
                {
                    sub = new EditorPathAssetTree() { parent = this, assetTypeEnum = PathAssetTypeEnum.Object, key = key, value = value };
                    subs.Add(sub);
                    sub.LoadShowAsset();
                }
            }
            else
            {
                if (sub != null)
                {
                    sub.AddAsset(index + 1, value, keys);
                }
                else
                {
                    EditorPathAssetTree dataNode = new EditorPathAssetTree() { parent = this, assetTypeEnum = PathAssetTypeEnum.Node, key = key };
                    subs.Add(dataNode);
                    dataNode.AddAsset(index + 1, value, keys);
                }
            }
        }

        //移除所有ab包资源节点和空节点
        public void RemoveAllAsset()
        {
            for (int i = subs.Count - 1; i >= 0; i--)
            {
                switch (subs[i].assetTypeEnum)
                {
                    case PathAssetTypeEnum.Object:
                        subs.RemoveAt(i);
                        break;
                    case PathAssetTypeEnum.Node:
                        subs[i].RemoveAllAsset();
                        if (subs[i].subs.Count == 0)
                        {
                            subs.RemoveAt(i);
                        }
                        break;
                }
            }
        }

        //获取这个节点的路径
        public string GetAssetPath()
        {
            return (parent.parent is null ? "" : parent.GetAssetPath() + "/") + key;
        }

        //加载子节点所有显示资源
        public async void LoadSubShowAssets()
        {
            if (assetTypeEnum == PathAssetTypeEnum.Node)
            {
                foreach (var item in subs)
                {
                    item.LoadShowAsset();
                    await Task.Yield();
                }
            }
        }
        //加载这个节点的显示资源
        public void LoadShowAsset()
        {
            if (assetTypeEnum == PathAssetTypeEnum.Object)
            {
                string objFliePath = value.To<string>("");//获取文件路径
                if (File.Exists(objFliePath))//判断路径存在
                {
                    Debug.Log(objFliePath);
                    showAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(objFliePath);//获取临时展示的资源对象
                }
            }
        }

        public async void RefreshAllAssetBandle()
        {
            await AsyncRefreshAllAssetBandle();
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }
        public async void ClearAllAssetBandle()
        {
            await AsyncClearAllAssetBandle();
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }


        private async Task AsyncRefreshAllAssetBandle()
        {
            switch (assetTypeEnum)
            {
                case PathAssetTypeEnum.Object:
                    string objFliePath = value.To<string>("");//获取文件路径
                    if (File.Exists(objFliePath))//判断路径存在
                    {
                        AssetImporter asset = AssetImporter.GetAtPath(objFliePath);//获取资源导入器
                        asset.assetBundleName = parent.GetAssetPath();//设置Ab包路径
                                                                            //后缀：asset.assetBundleVariant       
                        asset.SaveAndReimport();//保存
                    }
                    break;

                case PathAssetTypeEnum.Node:
                    foreach (var item in subs)
                    {
                        await item.AsyncRefreshAllAssetBandle();
                    }
                    break;
            }
        }

        private async Task AsyncClearAllAssetBandle()
        {
            switch (assetTypeEnum)
            {
                case PathAssetTypeEnum.Object:
                    string objFliePath = value.To<string>("");//获取文件路径
                    if (File.Exists(objFliePath))//判断路径存在
                    {
                        showAsset = null;
                        AssetImporter asset = AssetImporter.GetAtPath(objFliePath);//获取资源导入器
                        asset.assetBundleName = null;//设置Ab包路径
                                                     //后缀：asset.assetBundleVariant       
                        asset.SaveAndReimport();//保存
                    }
                    break;
                case PathAssetTypeEnum.Node:
                    foreach (var item in subs)
                    {
                        await item.AsyncClearAllAssetBandle();
                    }
                    break;
            }
        }

        #endregion

    }

    //参考
    //   /// <summary>
    //         /// 查找指定文件夹下指定后缀名的作有文件
    //         /// </summary>
    //         /// <param name="directory">文件夹</param>
    //         /// <param name="searchPattern">后缀名</param>
    //         /// <returns>文件路径集合</returns>
    //         private static void GetAllFilePaths(DirectoryInfo directory, string searchPattern, List<string> fileList)
    //         {
    //             if (directory.Exists || searchPattern.Trim() != string.Empty)
    //             {
    //                 foreach (FileInfo info in directory.GetFiles(searchPattern))
    //                 {
    //                     fileList.Add(info.FullName.ToString());
    //                 }

    //                 foreach (DirectoryInfo info in directory.GetDirectories())//获取文件夹下的子文件夹
    //                 {
    //                     GetAllFilePaths(info, searchPattern, fileList);//递归调用该函数，获取子文件夹下的文件
    //                 }
    //             }
    //         }

}