using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.8.29
 *
 * 2019.10.25 继承 SB_Mesh_Skeleton 抽象类的骨架脚本
 * 
 * 功能：用于Mesh的骨架线条
 */

namespace SDHK_Tool.Component
{
    /// <summary>
    /// 用于Mesh的骨架线条
    /// </summary>
    public class SC_Mesh_SkeletonLine : SB_Mesh_Skeleton
    {

        /// <summary>
        /// 动态刷新激活
        /// </summary>
        [Tooltip("动态刷新激活")]
        public bool Dynamic = true;

        /// <summary>
        /// 调试画线
        /// </summary>
        [Tooltip("调试画线")]
        public bool Debug_Line = true;

        /// <summary>
        /// 不计算根节点
        /// </summary>
        [Tooltip("不计算根节点")]
        public bool Ignore_RootNode = false;

        /// <summary>
        /// 线条节点数量
        /// </summary>
        [Tooltip("线条节点数量")]
        public int LineCount = 10;

        /// <summary>
        /// 每段长度
        /// </summary>
        [Tooltip("每段长度")]
        public float LineDistance = 1;

        /// <summary>
        /// 弯曲角度：w为受影响段数
        /// </summary>
        [Tooltip("弯曲角度：w为受影响段数")]
        public Vector4[] BendAngles = new Vector4[1];


        private Vector3 Angle = Vector3.zero;

        private int Bend_i = 0;

        private int Bend_Count = 0;


        private void Awake()
        {

        }


        [ContextMenu("生成新的骨架线")]
        private void Initialize()
        {
            if (Pionts.Count != 0)
            {
                foreach (var Line in Pionts)
                {
                    DestroyImmediate(Line.gameObject);
                }
                Pionts.Clear();
            }

            if (Pionts.Count == 0)
            {
                for (int i = 0; i < LineCount; i++)
                {
                    Pionts.Add(new GameObject().transform);
                    Pionts[i].parent = transform;
                }
                Line_draw();
            }
        }

        private void Line_draw()
        {
            Bend_i = 0;
            Bend_Count = 0;

            Angle = (Ignore_RootNode) ? Vector3.zero : (Vector3)BendAngles[Bend_i];

            Pionts[0].transform.localPosition = Vector3.zero;
            Pionts[0].transform.localEulerAngles = Angle;

            for (int i = 1; i < LineCount; i++)
            {

                Bend_Count++;

                if (BendAngles[Bend_i].w - 1 < Bend_Count && Bend_i < BendAngles.Length - 1)
                {
                    Bend_i++;
                    Bend_Count = 0;
                }

                Pionts[i].transform.position = Pionts[i - 1].forward * LineDistance + Pionts[i - 1].position;

                Angle += (Vector3)BendAngles[Bend_i];

                Pionts[i].transform.localEulerAngles = Angle;

            }
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (Pionts == null || Pionts.Count == 0) return;
            if (Dynamic) Line_draw();
            if (Debug_Line) DeBugDraw();
        }
#endif


        private void DeBugDraw()
        {
            for (int i = 0; i < Pionts.Count - 1; i++)
            {
                Debug.DrawLine(Pionts[i].position, Pionts[i + 1].position, (i % 2 == 0) ? Color.yellow : Color.red);
            }

            foreach (var item in Pionts)
            {
                Debug.DrawLine(item.position, item.up + item.position, Color.green);
            }
        }

    }
}