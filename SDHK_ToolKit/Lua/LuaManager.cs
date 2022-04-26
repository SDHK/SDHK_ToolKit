
/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/18 15:24:46

 * 最后日期: 2021/12/18 15:26:50

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SDHK_Extension;
using UnityEngine;
using XLua;
using AssetBandleTool;
using UnityEditor;
using Singleton;
using UnityEngine.UI;
using PathAssets;
using AsyncAwaitEvent;
using System.Threading.Tasks;

public class LuaManager : SingletonMonoBase<LuaManager>
{
    public LuaEnv luaEnv = new LuaEnv();

    public void Run(string luaCode)
    {
        luaEnv.AddLoader(LoadLuaToolInPathData);
        luaEnv.DoString(luaCode);//启动Lua工具加载
    }

    private byte[] LoadLuaToolInPathData(ref string fileName)
    {
        TextAsset textAsset = PathAssetGlobal.Assets.NodeGet<TextAsset>(fileName);
        if (textAsset != null)
        {
            // Debug.Log("[Lua加载] : " + fileName);
            return textAsset.bytes;
        }
        else
        {
            return null;
        }
    }

    private void Update()
    {
        luaEnv.Tick();
    }


}
