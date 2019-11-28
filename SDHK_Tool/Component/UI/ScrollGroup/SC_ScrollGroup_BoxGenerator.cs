using System;
using System.Collections;
using System.Collections.Generic;
using SDHK_Tool.Static;
using UnityEngine;
using UnityEngine.UI;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.11.04
 * 
 * 功能：动态列表的内容物体生成器 
 * 继承 SB_ScrollGroup_BoxGenerator
 * 可作为参考
 *
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// 动态列表简易内容生成器
    /// </summary>
    public class SC_ScrollGroup_BoxGenerator : SB_ScrollGroup_BoxGenerator
    {
        /// <summary>
        /// 内容数量
        /// </summary>
        [Tooltip("要生成的内容数")]
        public int BoxCount = 10;

        public bool Loop = false;

        /// <summary>
        /// 预制体
        /// </summary>
        [Tooltip("要生成的预制体")]
        public GameObject Box_Pfb;

        /// <summary>
        /// 对象池
        /// </summary>
        /// <typeparam name="int">编号</typeparam>
        /// <typeparam name="GameObject">储存物体</typeparam>
        public Dictionary<int, GameObject> ObjectPool = new Dictionary<int, GameObject>();


        public override void RefreshGroup()
        {
            foreach (var item in ObjectPool) Destroy(item.Value);
            ObjectPool.Clear();
        }

        public override GameObject EnterGroupBox(int ObjectId, int Index)
        {

            if (BoxCount < 1) return null; //数量不能为0
            int Num = (Loop) ? SS_Mathf.Int_Loop(Index, BoxCount) : Index; //换算成整数循环
            if (!(Loop || SS_Mathf.If_IntervalValue(Num, 0, BoxCount - 1))) return null;
            

            if (!ObjectPool.ContainsKey(ObjectId)) ObjectPool.Add(ObjectId, Instantiate(Box_Pfb, transform));
            GameObject gameObject_new = ObjectPool[ObjectId];

            (
                (gameObject_new.GetComponent<BoxNum>() == null)
              ? gameObject_new.AddComponent<BoxNum>()
              : gameObject_new.GetComponent<BoxNum>()
            )
            .Num = new List<int>() { Index };

            return gameObject_new;
        }

        public override void ExitGroupBox(int ObjectId, int Index)
        {

        }


    }
}
