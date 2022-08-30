/****************************************

* 作者： 闪电黑客
* 日期： 2022/8/29 21:46

* 描述： 

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


//思考self.Log方法

namespace WorldTree
{
    public class GUIWindow : GUIBase
    {
        public Rect rect = new Rect(0,0,500,500);
      
        public void Window(int windowId)
        {
            GUILayout.Button("!!!!");
            GUILayout.Button("!!!!????");
            GUI.DragWindow();
        }
    }


    class GUIWindowAddSystem : AddSystem<GUIWindow>
    {
        public override void OnAdd(GUIWindow self)
        {
            self.Texture = self.GetColorTexture(Color.white);
        }
    }

    class GUIWindowOnGUISystem : OnGUISystem<GUIWindow>
    {
        public override void OnGUI(GUIWindow self, float deltaTime)
        {
            self.rect = GUILayout.Window(self.GetHashCode(), self.rect, self.Window, default(string), self.Style);
        }
    }
}
