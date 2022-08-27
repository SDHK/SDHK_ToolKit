using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WorldTree
{

    class InitialDomainAddSystem : AddSystem<InitialDomain>
    {
        public override void OnAdd(InitialDomain self)
        {
            self.Root.AddChildren<ConsoleTreeView>();
        }
    }
    class ConsoleTreeViewAddSystem : AddSystem<ConsoleTreeView>
    {
        public override void OnAdd(ConsoleTreeView self)
        {
            self.systems = self.RootGetSystemGroup<IConsoleTreeViewItemSystem>();

            self.ViewShows = self.Root.ObjectPoolManager.Get<UnitDictionary<long, bool>>();
            self.ViewShows.TryAdd(self.Root.id, true);
            foreach (var item in self.Root.allEntities)
            {
                self.ViewShows.TryAdd(item.Value.id, true);
            }
            self.currentNode = self.Root.id;

            self.BtnStyle = new GUIStyle("Box");
            //self.BtnStyle.normal.background = Texture2D.blackTexture;

        }
    }

    class ConsoleTreeViewRemoveSystem : RemoveSystem<ConsoleTreeView>
    {
        public override void OnRemove(ConsoleTreeView self)
        {
            self.ViewShows.Recycle();
        }
    }
    class ConsoleTreeViewEntitySystem : EntitySystem<ConsoleTreeView>
    {
        public override void OnAddEntity(ConsoleTreeView self, Entity entity)
        {
            self.ViewShows.TryAdd(entity.id, true);
        }

        public override void OnRemoveEntity(ConsoleTreeView self, Entity entity)
        {
            self.ViewShows.Remove(entity.id);
        }
    }

    class ConsoleTreeViewOnGUISystem : OnGUISystem<ConsoleTreeView>
    {
        public override void OnGUI(ConsoleTreeView self, float deltaTime)
        {
            self.rect = GUI.Window(self.GetHashCode(), self.rect, self.GUIWindowMax, "世界树控制台");
        }

    }
    public class ConsoleTreeView : Entity
    {
        public UnitDictionary<long, bool> ViewShows;
        public Rect rect = new Rect(0, 0, 1000, 1000);

        public SystemGroup systems;
        public long currentNode;
        private Vector2 scrollLogView = Vector2.zero;
        private bool AreaView = false;


        public string box = "Button";
        public string box2 = "Box";
        public GUIStyle BtnStyle;

        public void GUIWindowMax(int windowId)
        {
           
            AreaView = EditorGUILayout.Foldout(AreaView, "测试!!!!!");
            box2 = GUILayout.TextField(box2);
            if (GUILayout.Button("测试"))
            {
                box = box2;
            }


            scrollLogView = GUILayout.BeginScrollView(scrollLogView, GUILayout.Height(900));

            TraversalRecursive(Root);

            GUILayout.EndScrollView();
            GUI.DragWindow();

            //子物体绘制
        }
        public void TraversalRecursive(Entity node)
        {
            GUILayout.BeginVertical();
            if (systems.TryGetValue(node.Type, out List<ISystem> systemList))
            {
                foreach (IConsoleTreeViewItemSystem system in systemList)
                {
                    system.Draw(node, this);
                }
            }
            else if (systems.TryGetValue(typeof(Entity), out systemList))
            {
                foreach (IConsoleTreeViewItemSystem system in systemList)
                {
                    system.Draw(node, this);
                }
            }
            GUILayout.BeginHorizontal();
            GUILayout.Space(100);
            GUILayout.BeginVertical();

            ViewShows.TryGetValue(node.id, out bool value);
            if (value)
            {
                GUILayout.BeginVertical(BtnStyle);

                if (node.components == null && node.children == null) ViewShows[node.id] = false;

                if (node.components != null)
                {
                    GUILayout.Label("Components:");

                    foreach (var item in node.components)
                    {
                        TraversalRecursive(item.Value);
                    }

                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical(BtnStyle);

                if (node.children != null)
                {
                    GUILayout.Label("Children:");

                    foreach (var item in node.children)
                    {
                        TraversalRecursive(item.Value);
                    }
                }
                GUILayout.EndVertical();

            }
            GUILayout.EndVertical();


            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

    }




    public class ConsoleTreeViewItem : ConsoleTreeViewItemSystem<Entity>
    {
        public override void OnDraw(Entity self, ConsoleTreeView consoleTreeView)
        {
            GUILayout.BeginHorizontal("Button", GUILayout.Width(600));
            GUILayout.Label(self.ToString(), consoleTreeView.BtnStyle, GUILayout.Width(400));

            if (consoleTreeView.ViewShows.ContainsKey(self.id))
            {
                bool bit = consoleTreeView.ViewShows[self.id];
                if (GUILayout.Button(bit ? "收起" : "展开", GUILayout.Width(200)))
                {
                    consoleTreeView.ViewShows[self.id] = !bit;
                }
            }
            GUILayout.EndHorizontal();
        }
    }


    #region ConsoleManager

    public class ConsoleManager : Entity
    {
        public UnitDictionary<long, bool> ViewShows;
        public SystemGroup systems;

    }

    class ConsoleManagerAddSystem : AddSystem<ConsoleManager>
    {
        public override void OnAdd(ConsoleManager self)
        {
            self.ViewShows = self.Root.ObjectPoolManager.Get<UnitDictionary<long, bool>>();
            foreach (var item in self.Root.allEntities)
            {
                self.ViewShows.TryAdd(item.Key, true);
            }
        }
    }

    class ConsoleManagerRemoveSystem : RemoveSystem<ConsoleManager>
    {
        public override void OnRemove(ConsoleManager self)
        {
            self.ViewShows.Recycle();
        }
    }



    class ConsoleManagerEntitySystem : EntitySystem<ConsoleManager>
    {
        public override void OnAddEntity(ConsoleManager self, Entity entity)
        {
            self.ViewShows.TryAdd(entity.id, false);
        }

        public override void OnRemoveEntity(ConsoleManager self, Entity entity)
        {
            self.ViewShows.Remove(entity.id);
        }
    }

    //主控窗口Log消息：大小窗，FPS显示

    //全局式树状图：多开，焦点

    //跟踪挂载式信息面板


    //窗口属于组件绘制父物体UI代码
    //每个代码独立窗口
    public class ConsoleWindow : Entity
    {
        public SystemGroup systems;

        public Rect rect = new Rect(0, 0, 1000, 1000);
        public void GUIWindowMax(int windowId)
        {
            GUI.DragWindow();

            //子物体绘制
        }
    }

    //class ConsoleWindowOnGUISystem : OnGUISystem<ConsoleWindow>
    //{
    //    public override void OnGUI(ConsoleWindow self, float deltaTime)
    //    {
    //        if (Event.current.type != EventType.Repaint)
    //        {
    //            self.rect = GUI.Window((int)self.id, self.rect, self.GUIWindowMax, "世界树控制台");
    //        }

    //    }

    //}

    class ConsoleWindowEvent : EventDelegate { }

    public class WorldTreeConsole : Entity
    {
        public void GUITreeView()
        {
            GUILayout.BeginVertical();
            GUILayout.Label(this.Parent.ToString());

            GUILayout.BeginHorizontal();


            GUILayout.Space(40);


            GUILayout.BeginVertical();

            UnitList<Type> TypeKeys = this.Root.ObjectPoolManager.Get<UnitList<Type>>();
            foreach (var item in this.Parent.Components)
            {
                TypeKeys.Add(item.Key);
            }

            foreach (Type item in TypeKeys)
            {
                if (this.Parent.TryGetComponent(item, out Entity component))
                {
                    if (component.Type != this.Type)
                    {
                        component.AddComponent<WorldTreeConsole>().GUITreeView();
                    }
                }
            }
            TypeKeys.Recycle();

            UnitList<long> Keys = this.Root.ObjectPoolManager.Get<UnitList<long>>();

            foreach (var item in this.Parent.Children)
            {
                Keys.Add(item.Key);
            }

            foreach (long item in Keys)
            {
                if (this.Parent.TryGetChildren(item, out Entity entity))
                {
                    entity.AddComponent<WorldTreeConsole>().GUITreeView();
                }
            }
            Keys.Recycle();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
    }
    #endregion
}
