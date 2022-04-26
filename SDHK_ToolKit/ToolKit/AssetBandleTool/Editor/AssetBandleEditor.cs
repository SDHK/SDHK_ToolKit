
/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/26 19:34:42

 * 最后日期: 2021/12/26 19:35:00

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetBandleEditor : Editor
{
    /// <summary>
    /// 第二种打包方式：选择的多个物体都生成于一个包内
    /// </summary>
    // [MenuItem("SDHK_ToolKit/AB/BuildABSecelt")]
    static void BuildABSecelt()
    {
        // AssetBundleBuild[] abbs = new AssetBundleBuild[1];
        // abbs[0].assetBundleName = "Custom";
        // UnityEngine.Object[] selects = Selection.objects;
        // string[] selectnames = new string[selects.Length];
        // for (int i = 0; i < selects.Length; i++)
        // {
        //     //获取选择到资源的路径
        //     selectnames[i] = AssetDatabase.GetAssetPath(selects[i]);
        // }
        // abbs[0].assetNames = selectnames;
        // //该方法不再依靠Unity编辑器中的AssetBundleName进行打包，而是通过纯代码的方式进行打包
        // BuildPipeline.BuildAssetBundles("Assets/ABS", abbs, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        // //更新Project视图
        // AssetDatabase.Refresh();
    }
    /// <summary>
    /// 选择某个物体或多个物体，把它（它们）的assetBundleName设置为自己预设体的名称
    /// </summary>
    // [MenuItem("SDHK_ToolKit/AB/SetABName")]
    static void SetABName()
    {
        // UnityEngine.Object[] selects = Selection.objects;
        // foreach (var item in selects)
        // {
            // string path = AssetDatabase.GetAssetPath(item);
        //     Debug.Log(path);
            // AssetImporter asset = AssetImporter.GetAtPath(path);
        //     asset.assetBundleName = "ABTest/" + item.name;
        //     //变体：asset.assetBundleVariant       
        //     asset.SaveAndReimport();
        // }
        // AssetDatabase.Refresh();
    }
}
