
/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/28 09:59:19

 * 最后日期: 2021/12/28 10:00:06

 * 最后修改: 闪电黑客

 * 描述:  
    
    用于编辑PathData和AssetBandle的窗口

******************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SDHK_Extension;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using AsyncAwaitEvent;
using System.Threading.Tasks;
using System.Threading;
using UnityEditor.IMGUI.Controls;


//!规范化宽度
//!?尝试线程

namespace PathAssets
{

    [Serializable]
    public class PathAssetFileEditorWindow : EditorWindow
    {
        public static PathAssetFileEditorWindow windows;

        public EditorPathAssetTree RootData = new EditorPathAssetTree() { assetTypeEnum = PathAssetTypeEnum.Node };
        public EditorPathAssetTree focusNode = new EditorPathAssetTree() { assetTypeEnum = PathAssetTypeEnum.Node };
        public string fliePath = "";
        private Color bak;
        private Vector2 dataScroll = new Vector2(0, 0);

        private void OnEnable()
        {
            if (windows == null)
            {
                windows = this;
            }
            fliePath = EditorPrefs.GetString("PathAssetFilePath");
            LoadFile();
        }

        private void GUILine()
        {
            GUI.color = Color.black;
            GUILayout.Box(new GUIContent(""), new[] { GUILayout.Height(2), GUILayout.ExpandWidth(true) });
            GUI.color = bak;
        }


        [MenuItem("SDHK_ToolKit/PathAsset")]
        static void ShowWindow()
        {
            if (windows == null)
            {
                windows = EditorWindow.GetWindow<PathAssetFileEditorWindow>(false, "路径资源编辑器");
            }
            windows.Show();//显示窗口
        }
        public void OnGUI()
        {
            bak = GUI.color;

            ShowLoadPath();

            GUILayout.Space(20);

            AssetBandleButton();
            GUILine();
            EditorGUILayout.BeginHorizontal();
            PathNodeButton(focusNode);
            EditorGUILayout.EndHorizontal();
            GUILine();

            dataScroll = GUILayout.BeginScrollView(dataScroll);
            ForeachShowData(focusNode);

            if (AddButton())
            {
                focusNode.subs.Add(new EditorPathAssetTree() { key = focusNode.subs.Count.ToString(), parent = focusNode });
            }

            GUILayout.EndScrollView();
        }

        public void ShowLoadPath()
        {

            Rect rectTextField = EditorGUILayout.GetControlRect();
            fliePath = EditorGUI.TextField(rectTextField, fliePath);
            string selectfliePath = MouseSelect(rectTextField);
            if (selectfliePath != "")
            {
                fliePath = Path.GetDirectoryName(Application.dataPath) + "/" + selectfliePath;
                LoadFile();
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("载入"))
            {
                if (fliePath != "")
                {
                    LoadFile();
                }
            }
            if (GUILayout.Button("预览..."))
            {
                string path = EditorUtility.OpenFilePanel("载入", Path.GetDirectoryName(fliePath), "");
                if (path != "")
                {
                    fliePath = path;
                    LoadFile();
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("保存"))
            {
                SaveFile();
            }
            if (GUILayout.Button("另存为"))
            {
                selectfliePath = EditorUtility.SaveFilePanel("另存为", Path.GetDirectoryName(fliePath), Path.GetFileName(fliePath), "");
                fliePath = selectfliePath;
                SaveFile();
            }
            EditorGUILayout.EndHorizontal();
        }


        public void AssetBandleButton()
        {
            if (GUILayout.Button("AssetBandles", GUILayout.ExpandWidth(false)))
            {
                foreach (var AssetBundleName in AssetDatabase.GetAllAssetBundleNames())
                {
                    foreach (var AssetPath in AssetDatabase.GetAssetPathsFromAssetBundle(AssetBundleName))
                    {
                        string path = AssetBundleName + "/" + Path.GetFileNameWithoutExtension(AssetPath);
                        RootData.AddAsset(path, AssetPath);
                    }
                }
            }


        }

        public void PathNodeButton(EditorPathAssetTree node)
        {
            if (node != null)
            {
                PathNodeButton(node.parent);
                GUI.color = Color.yellow;
                if (GUILayout.Button(node.key, GUILayout.ExpandWidth(false)))
                {
                    focusNode = node;
                    focusNode.AllFoldout(false);
                }
                GUI.color = bak;
            }
        }

        public void ForeachShowData(EditorPathAssetTree data)
        {
            EditorGUILayout.BeginVertical();

            for (int i = 0; i < data.subs.Count; i++)
            {
                SwitchShowItem(data, i);
            }

            EditorGUILayout.EndHorizontal();
        }


        public void SwitchShowItem(EditorPathAssetTree parentData, int index)
        {
            EditorGUILayout.BeginHorizontal();
            switch (parentData.subs[index].assetTypeEnum)
            {
                case PathAssetTypeEnum.String: ShowString(parentData, index); break;
                case PathAssetTypeEnum.Int: ShowInt(parentData, index); break;
                case PathAssetTypeEnum.Float: ShowFloat(parentData, index); break;
                case PathAssetTypeEnum.Node: ShowNode(parentData, index); break;
                case PathAssetTypeEnum.Object: ShowObject(parentData, index); break;
            }
            EditorGUILayout.EndHorizontal();
        }


        public void ShowString(EditorPathAssetTree parentData, int index)
        {
            EditorPathAssetTree ItemData = parentData.subs[index];

            ItemData.assetTypeEnum = EnumPopup(ItemData.assetTypeEnum);

            ItemData.key = DelayedTextField(ItemData.key);
            ItemData.foldout = FoldoutButton(ItemData.foldout);

            if (ItemData.foldout)
            {
                ItemData.value = EditorGUILayout.TextArea(ItemData.value.To<string>(""), GUILayout.MinWidth(227), GUILayout.ExpandWidth(false));
            }
            else
            {
                ItemData.value = EditorGUILayout.DelayedTextField(ItemData.value.To<string>(""), GUILayout.Width(227));
            }

            if (SelectButton(ItemData))
            {
                Menu();
            }
        }


        public void ShowInt(EditorPathAssetTree parentData, int index)
        {
            EditorPathAssetTree ItemData = parentData.subs[index];

            ItemData.assetTypeEnum = EnumPopup(ItemData.assetTypeEnum);

            ItemData.key = DelayedTextField(ItemData.key);
            ItemData.value = EditorGUILayout.IntField(ItemData.value.To<int>(0), GUILayout.Width(250));


            if (SelectButton(ItemData))
            {
                Menu();
            }
        }

        public void ShowFloat(EditorPathAssetTree parentData, int index)
        {
            EditorPathAssetTree ItemData = parentData.subs[index];

            ItemData.assetTypeEnum = EnumPopup(ItemData.assetTypeEnum);

            ItemData.key = DelayedTextField(ItemData.key);
            ItemData.value = EditorGUILayout.FloatField(ItemData.value.To<float>(0), GUILayout.Width(250));


            if (SelectButton(ItemData))
            {
                Menu();
            }

        }
        public void ShowPathAsset(EditorPathAssetTree parentData, int index)
        {
            EditorPathAssetTree ItemData = parentData.subs[index];
            GUI.color = Color.cyan;
            var typeEnum = EnumPopup(ItemData.assetTypeEnum);
            GUI.color = bak;

            if (ItemData.assetTypeEnum != typeEnum)//假如类型变更则删除临时储存
            {
                ItemData.ClearAllAssetBandle();
                ItemData.assetTypeEnum = typeEnum;
            }



        }

        public void ShowNode(EditorPathAssetTree parentData, int index)
        {
            EditorGUILayout.BeginVertical();

            #region Horizontal
            EditorGUILayout.BeginHorizontal();

            EditorPathAssetTree ItemData = parentData.subs[index];

            GUI.color = Color.yellow;
            var typeEnum = EnumPopup(ItemData.assetTypeEnum);
            GUI.color = bak;

            if (ItemData.assetTypeEnum != typeEnum)//假如类型变更则删除临时储存
            {
                ItemData.ClearAllAssetBandle();
                ItemData.assetTypeEnum = typeEnum;
                ItemData.subs.Clear();

            }

            string NewKey = DelayedTextField(ItemData.key);
            if (NewKey != ItemData.key)
            {
                ItemData.key = NewKey;
                ItemData.RefreshAllAssetBandle();
            }

            ItemData.foldout = FoldoutButton(ItemData.foldout);

            EditorGUILayout.LabelField(ItemData.subs.Count.ToString(), GUILayout.Width(164));


            if (GUILayout.Button("节点聚焦", GUILayout.ExpandWidth(false)))
            {
                focusNode = ItemData;
                focusNode.AllFoldout(false);
            }

            if (SelectButton(ItemData))
            {
                NodeMenu();
            }

            EditorGUILayout.EndHorizontal();
            #endregion


            #region Horizontal
            EditorGUILayout.BeginHorizontal();
            if (ItemData.foldout)
            {
                GUILayout.Space(80);
                EditorGUILayout.BeginVertical();
                ForeachShowData(ItemData);
                if (AddButton())
                {
                    ItemData.subs.Add(new EditorPathAssetTree() { key = ItemData.subs.Count.ToString(), parent = ItemData });
                }

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            #endregion


            EditorGUILayout.EndVertical();
        }

        public void ShowObject(EditorPathAssetTree parentData, int index)
        {
            EditorPathAssetTree ItemData = parentData.subs[index];

            GUI.color = Color.cyan;
            var typeEnum = EnumPopup(ItemData.assetTypeEnum);
            GUI.color = bak;

            if (ItemData.assetTypeEnum != typeEnum)//假如类型变更则删除临时储存
            {
                ItemData.ClearAllAssetBandle();
                ItemData.assetTypeEnum = typeEnum;
            }

            Rect mouseSelectRect;

            string objFileName = Path.GetFileNameWithoutExtension(ItemData.value.To<string>(""));
            if (objFileName == "")//键值为空
            {
                ItemData.key = "";

                mouseSelectRect = EditorGUILayout.GetControlRect(GUILayout.Width(453));
                GUI.color = Color.yellow;
                EditorGUI.LabelField(mouseSelectRect, "请拖入资源对象");
                GUI.color = bak;

                if (SelectButton(ItemData))
                {
                    Menu();
                }


            }
            else
            {
                if (ItemData.showAsset == null) //物体为空时显示键值
                {
                    GUI.color = Color.green;
                    if (GUILayout.Button(ItemData.key, GUILayout.Width(453)))
                    {
                        ItemData.RefreshAllAssetBandle();
                        ItemData.LoadShowAsset();
                    }
                    GUI.color = bak;
                }
                else
                {

                    EditorGUILayout.ObjectField(ItemData.showAsset, typeof(UnityEngine.Object), false, GUILayout.Width(414));
                    if (GUILayout.Button("重载", GUILayout.ExpandWidth(false)))
                    {
                        ItemData.RefreshAllAssetBandle();
                        ItemData.LoadShowAsset();
                    }
                }



                if (SelectButton(ItemData))
                {
                    Menu();
                }


                mouseSelectRect = EditorGUILayout.GetControlRect();

                EditorGUI.SelectableLabel(mouseSelectRect, ItemData.value.To<string>(""));
            }



            string objFliePath = MouseSelect(mouseSelectRect);
            if (objFliePath != "")
            {
                ItemData.ClearAllAssetBandle();//删除原来的
                ItemData.key = Path.GetFileNameWithoutExtension(objFliePath);//获取新资源名称
                ItemData.value = objFliePath;
                ItemData.RefreshAllAssetBandle();//设置新资源
                ItemData.LoadShowAsset();
            }

        }

        private GUILayoutOption optionDelayedTextField = GUILayout.Width(200);
        private GUILayoutOption optionFoldoutButton = GUILayout.Width(20);

        private GUILayoutOption optionAddButton = GUILayout.Width(80);
        private GUILayoutOption optionSelectButton = GUILayout.Width(25);
        private GUILayoutOption optionEnumPopup = GUILayout.Width(80);

        public string DelayedTextField(string str) => EditorGUILayout.DelayedTextField(str, optionDelayedTextField);

        public bool FoldoutButton(bool foldout)
        {
            GUI.color = (foldout) ? Color.cyan : bak;
            foldout = GUILayout.Button(foldout ? "▼" : "▶", optionFoldoutButton) ? !foldout : foldout;
            GUI.color = bak;
            return foldout;
        }


        public bool AddButton()
        {
            GUI.color = Color.green;
            bool bit = GUILayout.Button("十", optionAddButton);
            GUI.color = bak;
            return bit;
        }

        public T EnumPopup<T>(T typeEnum) where T : Enum => (T)EditorGUILayout.EnumPopup(typeEnum, optionEnumPopup);
        public string MouseSelect(Rect TextRect)
        {
            if (TextRect.Contains(Event.current.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0 && Event.current.type == EventType.DragPerform)
                {
                    return DragAndDrop.paths[0];
                }
            }
            return "";
        }

        //!鼠标右键
        // var evt = Event.current;
        // var contextRect = EditorGUILayout.GetControlRect(GUILayout.Width(100));
        // if (evt.type == EventType.ContextClick)
        // {
        //     var mousePos = evt.mousePosition;
        //     if (contextRect.Contains(mousePos))
        //     {
        //         evt.Use();
        //     }
        // }

        private bool isCopy = true;
        private EditorPathAssetTree MenuSelectData;
        private EditorPathAssetTree MenuSaveData;

        public bool SelectButton(EditorPathAssetTree pathData)
        {
            pathData.select = MenuSelectData == pathData;
            GUI.color = (pathData.select) ? Color.cyan : bak;
            bool bit = GUILayout.Button(pathData.select ? "◆" : "◇", optionSelectButton);
            if (bit) MenuSelectData = pathData;
            GUI.color = bak;
            return bit;
        }

        public void Menu()
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("复制结构路径"), false, MenuCopyPath, MenuSelectData);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("删除"), false, MenuDelete, MenuSelectData);
            menu.AddItem(new GUIContent("新建"), false, MenuInsertNew, MenuSelectData);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("复制"), false, MenuCopyData, MenuSelectData);
            menu.AddItem(new GUIContent("剪切"), false, MenuCut, MenuSelectData);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("粘贴"), false, MenuInsertPaste, MenuSelectData);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("上移"), false, MenuMoveUp, MenuSelectData);
            menu.AddItem(new GUIContent("下移"), false, MenuMoveDown, MenuSelectData);

            menu.ShowAsContext();
        }

        public void NodeMenu()
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("复制结构路径"), false, MenuCopyPath, MenuSelectData);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("删除"), false, MenuDelete, MenuSelectData);
            menu.AddItem(new GUIContent("新建"), false, MenuInsertNew, MenuSelectData);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("复制"), false, MenuCopyData, MenuSelectData);
            menu.AddItem(new GUIContent("剪切"), false, MenuCut, MenuSelectData);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("粘贴"), false, MenuInsertPaste, MenuSelectData);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("上移"), false, MenuMoveUp, MenuSelectData);
            menu.AddItem(new GUIContent("下移"), false, MenuMoveDown, MenuSelectData);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("全展开"), false, NodeMenuAllFoldoutOpen, MenuSelectData);
            menu.AddItem(new GUIContent("全折叠"), false, NodeMenuAllFoldoutClose, MenuSelectData);


            menu.ShowAsContext();
        }

        public void MenuCopyPath(object obj)
        {
            var ItemData = obj.To<EditorPathAssetTree>(null);
            GUIUtility.systemCopyBuffer = ItemData.GetAssetPath();
        }

        public void NodeMenuAllFoldoutOpen(object obj)
        {
            var ItemData = obj.To<EditorPathAssetTree>(null);
            ItemData.AllFoldout(true);
        }
        public void NodeMenuAllFoldoutClose(object obj)
        {
            var ItemData = obj.To<EditorPathAssetTree>(null);
            ItemData.AllFoldout(false);
        }

        public void MenuDelete(object obj)
        {
            var ItemData = obj.To<EditorPathAssetTree>(null);
            ItemData.parent.subs.Remove(ItemData);
            ItemData.ClearAllAssetBandle();
        }
        public void MenuInsertNew(object obj)
        {
            var ItemData = obj.To<EditorPathAssetTree>(null);
            int index = ItemData.parent.subs.FindIndex((kV) => kV == ItemData);
            ItemData.parent.subs.Insert(index, new EditorPathAssetTree() { key = "new", parent = ItemData.parent });
        }
        public void MenuCopyData(object obj)
        {
            isCopy = true;
            MenuSaveData = obj.To<EditorPathAssetTree>(null);
        }

        public void MenuCut(object obj)
        {
            isCopy = false;
            MenuSaveData = obj.To<EditorPathAssetTree>(null);
        }

        public void MenuInsertPaste(object obj)
        {
            if (MenuSaveData == null) return;

            var ItemData = obj.To<EditorPathAssetTree>(null);
            int index = ItemData.parent.subs.FindIndex((kV) => kV == ItemData);

            EditorPathAssetTree PastePathData = null;

            if (isCopy)
            {
                if (MenuSaveData.assetTypeEnum == PathAssetTypeEnum.Node)
                {
                    PastePathData = new EditorPathAssetTree()
                    {
                        assetTypeEnum = PathAssetTypeEnum.Node,
                        key = MenuSaveData.key
                    }.SetData(MenuSaveData.ToData());
                }
                else if (MenuSaveData.assetTypeEnum != PathAssetTypeEnum.Object)
                {
                    PastePathData = new EditorPathAssetTree()
                    {
                        assetTypeEnum = MenuSaveData.assetTypeEnum,
                        key = MenuSaveData.key,
                        value = MenuSaveData.value
                    };
                }
                index += 1;
            }
            else
            {
                PastePathData = MenuSaveData;
                PastePathData.parent.subs.Remove(PastePathData);
            }

            if (PastePathData != null)
            {
                ItemData.parent.subs.Insert(index, PastePathData);
                PastePathData.parent = ItemData.parent;
                PastePathData.RefreshAllAssetBandle();
            }
        }


        public void MenuMoveUp(object obj)
        {
            var ItemData = obj.To<EditorPathAssetTree>(null);
            int index = ItemData.parent.subs.FindIndex((kV) => kV == ItemData);
            if (index > 0)
            {
                ItemData.parent.subs[index] = ItemData.parent.subs[index - 1];
                ItemData.parent.subs[index - 1] = ItemData;
            }
        }
        public void MenuMoveDown(object obj)
        {
            var ItemData = obj.To<EditorPathAssetTree>(null);
            int index = ItemData.parent.subs.FindIndex((kV) => kV == ItemData);
            if (index < ItemData.parent.subs.Count - 1)
            {
                ItemData.parent.subs[index] = ItemData.parent.subs[index + 1];
                ItemData.parent.subs[index + 1] = ItemData;
            }
        }

        public void SaveFile()
        {
            if (fliePath != "")
            {
                Debug.Log("保存");
                Directory.CreateDirectory(Path.GetDirectoryName(fliePath));//如果文件夹不存在就创建它
                EditorPathAssetTree SaveData = new EditorPathAssetTree().SetData(RootData.ToData());
                SaveData.RemoveAllAsset();
                File.WriteAllBytes(fliePath, Convert_ObjectToBytes(SaveData.ToData()));

                // FileStream fs = new FileStream(fliePath, FileMode.Create);
                // //开始写入
                // var data = Convert_ObjectToBytes(SaveData.ToData());
                // fs.Write(data, 0, data.Length);
                // //清空缓冲区、关闭流
                // fs.Flush();
                // fs.Close();
            }
        }
        public void LoadFile()
        {
            if (File.Exists(fliePath))
            {
                Debug.Log("载入");
                var data = (Convert_BytesToObject<Dictionary<string, object>>(File.ReadAllBytes(fliePath)));
                RootData = new EditorPathAssetTree() { assetTypeEnum = PathAssetTypeEnum.Node, key = "Root" }.SetData(data);
                focusNode = RootData;

                EditorPrefs.SetString("PathAssetFilePath", fliePath);//保存到编辑器全局变量
            }
        }

        /// <summary>
        /// 将对象序列化为二进制数据:对象定义时需[Serializable]序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>byte数组</returns>
        private static byte[] Convert_ObjectToBytes<T>(T obj)
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
        private static T Convert_BytesToObject<T>(byte[] data)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            T obj = (T)bf.Deserialize(stream);
            stream.Close();
            return obj;
        }


        /// <summary>
        /// 通过二进制序列化深拷贝
        /// </summary>
        /// <param name="obj">拷贝对象</param>
        /// <returns>返回对象</returns>
        private static T DeepCopy<T>(T obj)
        {
            object NewObj;
            MemoryStream stream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            //序列化成流
            bf.Serialize(stream, obj);
            stream.Seek(0, SeekOrigin.Begin);//设置当前流正在读取的位置 为开始位置即从0开始

            //反序列化成对象
            NewObj = bf.Deserialize(stream);
            stream.Close();//关闭流

            return (T)NewObj;
        }
    }
}