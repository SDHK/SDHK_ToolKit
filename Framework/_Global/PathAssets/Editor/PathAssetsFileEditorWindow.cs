using SDHK;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorDomain : EntityRoot { }

public class PathAssetsFileEditorWindow : EditorWindow
{
    public static PathAssetsFileEditorWindow windows;

    public EntityDomain root;
    public SoloistFramework soloist;

    public UpdateManager update;


    [MenuItem("SDHK/PathAssets")]
    static void ShowWindow()
    {
        if (windows == null)
        {
            windows = EditorWindow.GetWindow<PathAssetsFileEditorWindow>(false, "路径资源编辑器");


            //MainEntity.GetInstance();

            //Debug.Log(windows.soloist.AllEntityString(RootEntity.Root, "\t"));
        }
        windows.Show();//显示窗口
    }

    public PathAssetsFileEditorWindow()
    {
        soloist= SoloistFramework.GetInstance();
        root = soloist.root;

        //root = new EditorDomain();

        update = root.GetComponent<UpdateManager>();
        root.GetComponent<MainEntity>();

        Debug.Log(SoloistFramework.AllEntityString(root, "\t"));

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
        Debug.Log(SoloistFramework.AllEntityString(root, "\t"));

    }


}
