using SDHK;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathAssetsFileEditorWindow : EditorWindow
{
    public static PathAssetsFileEditorWindow windows;

    public SoloistFramework soloist;

    public UpdateManager update;


    [MenuItem("SDHK/PathAssets")]
    static void ShowWindow()
    {
        if (windows == null)
        {
            windows = EditorWindow.GetWindow<PathAssetsFileEditorWindow>(false, "路径资源编辑器");

            windows.soloist = SoloistFramework.GetInstance();//实体管理器单例
            windows.update = UpdateManager.GetInstance();
            MainEntity.GetInstance();

            Debug.Log(windows.soloist.AllEntityString(RootEntity.Root, "\t"));
        }
        windows.Show();//显示窗口


    }

    private void OnEnable()
    {

    }

    public void OnGUI()
    {


    }
    private void OnInspectorUpdate()
    {
        update.Update();
    }


    private void OnDestroy()
    {
        soloist.Dispose();
        update = null;

    }


}
