using System;
using UnityEngine;

namespace WorldTree
{

    //class WorldTreeViewManagerSingletonEagerSystem : SingletonEagerSystem<WorldTreeViewManager> { }

    class InitialDomainAddSystem : AddSystem<InitialDomain>
    {
        public override void OnAdd(InitialDomain self)
        {
            self.Root.AddComponent<WorldTreeViewManager>();
        }
    }

    public class WorldTreeViewManager : Entity
    {

    }



    class WorldTreeViewManagerOnGUISystem : OnGUISystem<WorldTreeViewManager>
    {
        public override void OnGUI(WorldTreeViewManager self, float deltaTime)
        {
            GUILayout.BeginVertical();
            self.Root.AddComponent<WorldTreeView>().GUITreeView();
            GUILayout.EndVertical();

        }
    }

    public class WorldTreeView : Entity
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
                                component.AddComponent<WorldTreeView>().GUITreeView();
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
                            entity.AddComponent<WorldTreeView>().GUITreeView();
                        }
                    }
                    Keys.Recycle();
                    GUILayout.EndVertical();

                GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
    }
}
