using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using SDHK_Extension;

namespace ScreenResolution
{


    [CustomEditor(typeof(ScreenManager))] //指定要编辑的脚本对象
    public class ScreenManagerEditor : Editor
    {

        private ScreenManager script;

        public override void OnInspectorGUI()
        {
            script = target as ScreenManager;
            Color bak = GUI.color;

            GUI.color = Color.green;

            if (GUILayout.Button("预览...", GUILayout.Height(40)))
            {
                string path = EditorUtility.OpenFilePanel("载入", (Directory.Exists(script.FullPath())) && script.path != "" ? Path.GetDirectoryName(script.FullPath()) : script.FullPath(), "");

                if (path != "")
                {
                    path = path.Replace(ApplicationExtension.GetPath(script.RootPath), "");
                    script.path = path;
                    script.Load();
                }
            }
            GUI.color = bak;
            
            GUILayout.Space(20);


            base.OnInspectorGUI();



            GUILayout.Space(20);

            GUI.color = Color.green;

            if (GUILayout.Button("保存", GUILayout.Height(40)))
            {
                script.Save();
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("载入", GUILayout.Height(40)))
            {
                script.Load();
            }


            GUI.color = bak;

        }


    }
}