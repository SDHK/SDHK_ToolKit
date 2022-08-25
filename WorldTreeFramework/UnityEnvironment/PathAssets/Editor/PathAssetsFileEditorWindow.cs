using WorldTree;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WorldTree
{


    public class PathAssetsFileEditorWindow : EditorWindow
    {
        public static PathAssetsFileEditorWindow windows;

        public EntityManager root;

        public UpdateManager update;


        [MenuItem("SDHK/PathAssets")]
        static void ShowWindow()
        {
            if (windows == null)
            {
                windows = EditorWindow.GetWindow<PathAssetsFileEditorWindow>(false, "路径资源编辑器");
            }
            windows.Show();//显示窗口
        }

        public PathAssetsFileEditorWindow()
        {
            root = new EntityManager();

            update = root.AddComponent<UpdateManager>();
            //root.AddComponent<MainDomain>();

            Debug.Log(root.ToStringDrawTree());

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
            root.Dispose();
            update = null;
            Debug.Log(root.ToStringDrawTree());

        }

    }

}
