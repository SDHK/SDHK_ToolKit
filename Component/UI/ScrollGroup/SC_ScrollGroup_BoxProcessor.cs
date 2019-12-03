using System;
using System.Collections;
using System.Collections.Generic;
using SDHK_Tool.Static;
using UnityEngine;
using UnityEngine.UI;
using SDHK_Tool.Dynamic;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.11.04
 * 
 * 功能：动态列表的内容物体处理器
 * 继承 SB_ScrollGroup_BoxProcessor
 * 可作为参考
 *
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// 动态列表简易内容处理器
    /// </summary>
    public class SC_ScrollGroup_BoxProcessor : SB_ScrollGroup_BoxProcessor
    {

        public Dictionary<GameObject, BoxNum> ComponentPool = new Dictionary<GameObject, BoxNum>();


        public override void RefreshGroup()
        {
            ComponentPool.Clear();
        }

        public override void GroupBox_New(GameObject GroupBox, int Index)
        {
            ComponentPool.Add(GroupBox, GroupBox.GetComponent<BoxNum>());
        }

        public override void GroupBox_Del(GameObject GroupBox, int Index)
        {
            ComponentPool.Remove(GroupBox);
        }

        public override void GroupBox_Work(GameObject GroupBox, int Index)
        {
            ComponentPool[GroupBox].Num[0] = Index;
        }

        public override void GroupBox_Idle(GameObject GroupBox, int Index)
        {

        }




    }
}
