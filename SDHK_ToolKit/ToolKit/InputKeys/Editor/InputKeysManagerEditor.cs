using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;



namespace InputKeys
{
    [CustomEditor(typeof(InputKeysManager))] //指定要编辑的脚本对象

    public class InputKeysManagerEditor : Editor
    {
        private List<bool> bits = new List<bool>();
        private List<string> GroupsName = new List<string>();
        private List<string> KeysName = new List<string>();


        private InputKeysManager script;


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying) return;

            script = target as InputKeysManager;

            GUILayout.Space(20);



            Color bak = GUI.color;
            GUI.color = Color.green;

            if (GUILayout.Button("重新加载", GUILayout.Height(40)))
            {
                script.Load();
            }
            GUI.color = bak;

            GUILayout.Space(20);


            GroupsName = script.inputKeyGroups.Keys.ToList();
            for (int i = 0; i < GroupsName.Count; i++)
            {

                if (bits.Count < GroupsName.Count)
                {
                    bits.Add(true);
                }
                else if (bits.Count > GroupsName.Count && bits.Count > 0)
                {
                    bits.RemoveAt(bits.Count - 1);
                }

                GUI.color = Color.green;

                bits[i] = EditorGUILayout.Foldout(bits[i], GroupsName[i]);

                GUI.color = bak;


                if (bits[i])
                {
                    EditorGUILayout.Space();

                    KeysName = script.inputKeyGroups[GroupsName[i]].Keys.ToList();
                    for (int i1 = 0; i1 < KeysName.Count; i1++)
                    {
                        EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.Space();

                        if (script.inputKeyGroups[GroupsName[i]][KeysName[i1]].IsKeys()) GUI.color = Color.yellow;

                        EditorGUILayout.LabelField(KeysName[i1], GUILayout.MinWidth(100));

                        GUI.color = bak;

                        GUILayout.Space(10);
                        EditorGUILayout.LabelField(script.inputKeyGroups[GroupsName[i]][KeysName[i1]].ToString(), GUILayout.MinWidth(150));
                        EditorGUILayout.Space();

                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                }

            }

        }


    }
}