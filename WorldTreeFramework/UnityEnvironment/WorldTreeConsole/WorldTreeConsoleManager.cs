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
            self.Root.AddChildren<GUIWindow>();
        }
    }
    class ConsoleTreeViewAddSystem : AddSystem<ConsoleTreeView>
    {
        public override void OnAdd(ConsoleTreeView self)
        {
            self.systems = self.RootGetSystemGroup<IConsoleTreeViewItemSystem>();

            self.componentShowSwitchs = self.Root.ObjectPoolManager.Get<UnitDictionary<long, bool>>();
            self.childrenShowSwitchs = self.Root.ObjectPoolManager.Get<UnitDictionary<long, bool>>();

            self.componentShowSwitchs.TryAdd(self.Root.id, true);
            self.childrenShowSwitchs.TryAdd(self.Root.id, true);
            foreach (var item in self.Root.allEntities)
            {
                self.componentShowSwitchs.TryAdd(item.Value.id, true);
                self.childrenShowSwitchs.TryAdd(item.Value.id, true);
            }
            self.currentNode = self.Root;

            self.black3 = new GUIStyle("Label");
            self.black3.normal.background = new Texture2D(1, 1, Texture2D.grayTexture.format, true);
            self.black3.normal.background.SetPixel(0, 0, new Color(0.3f, 0.3f, 0.3f));
            self.black3.normal.background.Apply();

            self.black2 = new GUIStyle("Label");
            self.black2.normal.background = new Texture2D(1, 1, Texture2D.grayTexture.format, true);
            self.black2.normal.background.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f));
            self.black2.normal.background.Apply();

            self.black1 = new GUIStyle("Label");
            self.black1.normal.background = new Texture2D(1, 1, Texture2D.grayTexture.format, true);
            self.black1.normal.background.SetPixel(0, 0, new Color(0.1f, 0.1f, 0.1f));
            self.black1.normal.background.Apply();


            self.blue = new GUIStyle("Label");
            self.blue.normal.background = new Texture2D(1, 1, Texture2D.grayTexture.format, true);
            self.blue.normal.background.SetPixel(0, 0, new Color(0.2f, 0.3f, 0.5f));
            self.blue.normal.background.Apply();

            self.Transparent = new GUIStyle("Label");
            self.Transparent.normal.background = new Texture2D(1, 1, Texture2D.grayTexture.format, true);
            self.Transparent.normal.background.SetPixel(0, 0, new Color(0, 0, 0, 0));
            self.Transparent.normal.background.Apply();


            self.BtnStyle = new GUIStyle("Label");
            self.BtnStyle.alignment = TextAnchor.MiddleLeft;
        }
    }

    class ConsoleTreeViewRemoveSystem : RemoveSystem<ConsoleTreeView>
    {
        public override void OnRemove(ConsoleTreeView self)
        {
            self.componentShowSwitchs.Recycle();
            self.childrenShowSwitchs.Recycle();
        }
    }
    class ConsoleTreeViewEntitySystem : EntitySystem<ConsoleTreeView>
    {
        public override void OnAddEntity(ConsoleTreeView self, Entity entity)
        {
            self.componentShowSwitchs.TryAdd(entity.id, true);
            self.childrenShowSwitchs.TryAdd(entity.id, true);
        }

        public override void OnRemoveEntity(ConsoleTreeView self, Entity entity)
        {
            if (self.currentNode == entity)
            {
                self.currentNode = entity.Parent;
            }
            self.componentShowSwitchs.Remove(entity.id);
            self.componentShowSwitchs.Remove(entity.id);
        }
    }

    class ConsoleTreeViewOnGUISystem : OnGUISystem<ConsoleTreeView>
    {
        public override void OnGUI(ConsoleTreeView self, float deltaTime)
        {
<<<<<<< HEAD
            self.rect = GUI.Window(self.GetHashCode(), self.rect, self.GUIWindowMax, default(string), GUIDefault.StyleBlack2);
=======
            Rect rect = new Rect(self.rect.x, self.rect.y, self.rect.width * GUIDefault.size, self.rect.height * GUIDefault.size);
            rect = GUILayout.Window(self.GetHashCode(), rect, self.GUIWindowMax, default(string));
            self.rect.x = rect.x;
            self.rect.y = rect.y;

>>>>>>> 8ee27bd5be6bacc41385365f58caa9acfbb7a436
        }

    }
    public class ConsoleTreeView : Entity
    {
        public UnitDictionary<long, bool> componentShowSwitchs;
        public UnitDictionary<long, bool> childrenShowSwitchs;
        public Rect rect = new Rect(0, 0, 1000, 1000);

        public SystemGroup systems;
        public Entity currentNode;
        public Entity selectNode;
        private Vector2 scrollLogView = Vector2.zero;


        public string box = "Button";
        public string box2 = "Box";
        public GUIStyle BtnStyle;

        public GUIStyle black3;
        public GUIStyle black2;
        public GUIStyle black1;
        public GUIStyle blue;
        public GUIStyle Transparent;

        private Color bak;

        public void GUIWindowMax(int windowId)
        {
<<<<<<< HEAD
=======
            
            beginVertical1.Draw();
>>>>>>> 8ee27bd5be6bacc41385365f58caa9acfbb7a436

            bak = GUI.color;

            //GUI.color = Color.yellow;

            GUILayout.BeginHorizontal(GUIDefault.StyleBlack2, GUIDefault.optionHeight25);

            GUILayout.Label("世界树可视化", GUIDefault.StyleTransparent);
            GUILayout.FlexibleSpace();

            GUIDefault.CloseButton(() => { });

            GUILayout.EndHorizontal();


<<<<<<< HEAD
            GUIDefault.LineHorizontal();
=======

            lineHorizontal.Draw();

            GUIWindowContent();

            GUILayout.EndVertical();
            GUI.DragWindow();

        }
>>>>>>> 8ee27bd5be6bacc41385365f58caa9acfbb7a436



            GUILayout.BeginHorizontal(GUIDefault.StyleBlack2);

            PathNodeView(currentNode);

            GUILayout.EndHorizontal();


            GUIDefault.LineHorizontal();

            //GUI.color = bak;



            GUILayout.BeginVertical();

            scrollLogView = GUILayout.BeginScrollView(scrollLogView, GUIDefault.StyleBlack2);
            //TraversalRecursive(currentNode);
            ForeachShow(currentNode);
            GUILayout.EndScrollView();

            GUILayout.EndVertical();
            GUI.DragWindow();

            //子物体绘制
        }

        public void TraversalRecursive(Entity node)
        {
            GUILayout.BeginVertical();

            ShowItem(node);

            //GUILayout.BeginHorizontal();

            //GUILayout.Space(100);
            ForeachShow(node);

            //GUILayout.EndHorizontal();

            //GUIDefault.LineHorizontal();

            GUILayout.EndVertical();
        }


        public void PathNodeView(Entity entity)
        {
            if (entity != null)
            {
                PathNodeView(entity.Parent);
                if (GUILayout.Button(entity.Type.Name, GUIDefault.StyleBlack3, GUILayout.ExpandWidth(false)))
                {
                    currentNode = entity;
                }
            }
        }


        public void ShowItem(Entity node)
        {
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
        }
        public void ForeachShow(Entity node)
        {
            GUILayout.BeginVertical();



            ForeachComponents(node);
            ForeachChildren(node);

            GUILayout.EndVertical();


        }

        public void ForeachComponents(Entity node)
        {

            componentShowSwitchs.TryGetValue(node.id, out bool value);

            if (node.components != null && value)
            {
                //GUIDefault.LineHorizontal();

                GUILayout.BeginHorizontal(GUIDefault.StyleBlack1);
                GUILayout.BeginHorizontal(GUIDefault.StyleBlack2);
                //GUILayout.Space(100);
                GUILayout.Label("Components:", GUIDefault.StyleTransparent, GUIDefault.optionWidth80);

                GUIDefault.LineVertical();

                GUILayout.BeginVertical();

            
                foreach (var item in node.components)
                {
                    TraversalRecursive(item.Value);
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.EndHorizontal();

            }
        }

        public void ForeachChildren(Entity node)
        {
            childrenShowSwitchs.TryGetValue(node.id, out bool value);
            if (node.children != null && value)
            {
                GUILayout.BeginHorizontal(GUIDefault.StyleBlack1);
                GUILayout.BeginHorizontal(GUIDefault.StyleBlack2);

                GUILayout.Label("Children:", GUIDefault.StyleTransparent, GUIDefault.optionWidth80);

                //GUILayout.Space(100);

                GUIDefault.LineVertical();

                GUILayout.BeginVertical();

        

                foreach (var item in node.children)
                {
                    TraversalRecursive(item.Value);
                }

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.EndHorizontal();

            }

        }


    }




    public class ConsoleTreeViewItem : ConsoleTreeViewItemSystem<Entity>
    {
        public override void OnDraw(Entity self, ConsoleTreeView console)
        {

            bool componentSwitch = console.componentShowSwitchs[self.id];
            bool childrenSwitch = console.childrenShowSwitchs[self.id];

            //GUILayout.BeginHorizontal("Box", GUILayout.Width(600));
            GUILayout.BeginHorizontal(console.selectNode == self ? GUIDefault.StyleBlue : GUIDefault.StyleBlack3, GUILayout.Width(600));
            //EditorGUILayout

            if (self.components != null || self.children != null)
            {
                GUIDefault.FoldoutButton(componentSwitch || childrenSwitch, (bit) =>
                {
                    componentSwitch = !bit;
                    childrenSwitch = componentSwitch;
                });
            }
            else
            {
                GUILayout.Space(25);
            }


            GUILayout.BeginHorizontal();

            GUIDefault.Button(self.Type.Name, () =>
            {
                if (console.selectNode == self)
                {
                    console.currentNode = self;
                }
                console.selectNode = self;
            }, GUILayout.Width(300));


            if (self.components != null)
            {
                if (GUILayout.Button(componentSwitch ? "▼" : "▶", GUIDefault.StyleTransparent, GUILayout.Width(20)))
                {
                    componentSwitch = !componentSwitch;
                }
                GUILayout.Label(self.components.Count.ToString(), GUILayout.Width(80));
            }
            else
            {
                componentSwitch = false;
            }


            if (self.children != null)
            {
                if (GUILayout.Button(childrenSwitch ? "▼" : "▶", GUIDefault.StyleTransparent, GUILayout.Width(20)))
                {
                    childrenSwitch = !childrenSwitch;
                }
                GUILayout.Label(self.children.Count.ToString(), GUILayout.Width(80));
            }
            else
            {
                childrenSwitch = false;
            }

            console.componentShowSwitchs[self.id] = componentSwitch;
            console.childrenShowSwitchs[self.id] = childrenSwitch;

            GUILayout.EndHorizontal();
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
