using System;
using UnityEngine;

namespace WorldTree
{

    //class WorldTreeConsoleManagerSingletonEagerSystem : SingletonEagerSystem<WorldTreeConsoleManager> { }

    class InitialDomainAddSystem : AddSystem<InitialDomain>
    {
        public override void OnAdd(InitialDomain self)
        {
            self.Root.AddComponent<WorldTreeConsoleManager>();
        }
    }

    public class WorldTreeConsoleManager : Entity
    {

        public Rect rect = new Rect(0, 0, 1000, 1000);
        public void GUIWindowMax(int windowId)
        {
            GUI.DragWindow(new Rect(0, 0, 1000, 1000));
            GUILayout.BeginHorizontal();
            this.Root.AddComponent<WorldTreeConsole>().GUITreeView();
            GUILayout.EndHorizontal();

        }

        public static void DrawTree(Entity entity)//给实体添加遍历方法，对象池合并成一个
        {
            GUILayout.Label(entity.ToString());

            if (entity.components != null)
            {
                if (entity.components.Count > 0)
                {
                    UnitList<Type> TypeKeys = entity.Root.UnitPoolManager.Get<UnitList<Type>>();
                    foreach (var item in entity.components)
                    {
                        TypeKeys.Add(item.Key);
                    }

                    foreach (Type item in TypeKeys)
                    {
                        if (entity.TryGetComponent(item, out Entity component))
                        {
                            DrawTree(entity);
                        }
                    }
                    TypeKeys.Recycle();
                }
            }




        }

    }

    class WorldTreeConsoleManagerAddSystem : AddSystem<WorldTreeConsoleManager>
    {
        public override void OnAdd(WorldTreeConsoleManager self)
        {

        }
    }


    class WorldTreeConsoleManagerOnGUISystem : OnGUISystem<WorldTreeConsoleManager>
    {
        public override void OnGUI(WorldTreeConsoleManager self, float deltaTime)
        {
            if (Event.current.type != EventType.Repaint)
            {
                self.rect = GUI.Window((int)self.id, self.rect, self.GUIWindowMax, "世界树控制台");
            }

        }

    }

    public class WorldTreeConsole : Entity
    {
        public void GUITreeView()
        {
            GUILayout.BeginVertical();
            GUILayout.Label(this.Parent.ToString());

            GUILayout.BeginHorizontal();


            GUILayout.Space(40);


            GUILayout.BeginVertical();

            UnitList<Type> TypeKeys = this.Root.UnitPoolManager.Get<UnitList<Type>>();
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

            UnitList<long> Keys = this.Root.UnitPoolManager.Get<UnitList<long>>();

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
}
