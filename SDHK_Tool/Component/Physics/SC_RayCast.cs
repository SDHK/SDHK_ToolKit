using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SDHK_Tool.Static;
using UnityEngine;
// using SDHK_Tool.Extension;


/*
 * 作者：闪电Y黑客
 * 
 * 日期：2019.12.05
 * 
 * 功能：用于游戏物体碰撞箱的射线检测判断
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// 碰撞箱检测器：射线
    /// </summary>
    public class SC_RayCast : MonoBehaviour
    {
        /// <summary>
        /// 坐标系
        /// </summary>
		[Tooltip("坐标系")]
        public Transform Origin;

        [Space()]

        /// <summary>
        /// 是否应触发触发器
        /// </summary>
        [Tooltip("是否应触发触发器")]
        public QueryTriggerInteraction TriggerInteraction;

        [Space()]

        /// <summary>
        /// 忽略层
        /// </summary>
        [Tooltip("忽略层")]
        public string[] IgnoreLayer;

        [Space()]
        [Space()]

        /// <summary>
        /// 射线节点
        /// </summary>
		[Tooltip("射线节点")]
        public List<Vector3> LinePoints = new List<Vector3>() { Vector3.zero, new Vector3(0, 0, 5) };

        [Space()]
        [Space()]

        /// <summary>
        /// 调试画线
        /// </summary>
        [Tooltip("调试画线")]
        public bool DebugLine = true;

        /// <summary>
        /// 线条颜色
        /// </summary>
        [Tooltip("线条颜色")]
        public Color color = Color.green;


        /// <summary>
        /// 触发事件：进入
        /// </summary>
        public Action<List<RaycastHit>> OnRayCastEnter;

        /// <summary>
        /// 触发事件：离开
        /// </summary>
        public Action<List<RaycastHit>> OnRayCastExit;

        /// <summary>
        /// 触发事件：停留
        /// </summary>
        public Action<List<RaycastHit>> OnRayCastStay;


        private List<RaycastHit> EnterColliders = new List<RaycastHit>();
        private List<RaycastHit> ExitColliders = new List<RaycastHit>();
        private List<RaycastHit> StayColliders = new List<RaycastHit>();

        private List<Vector3> points = new List<Vector3>() { Vector3.zero, new Vector3(0, 0, 5) };
        private Func<RaycastHit, RaycastHit, bool> IF_Func = (RaycastHit a, RaycastHit b) => { return object.ReferenceEquals(a.collider.gameObject, b.collider.gameObject); };
        private List<RaycastHit> LastColliders = new List<RaycastHit>();

        private Vector3 point1_To_point2 = Vector3.zero;

        private bool isCheckBox = false;

        // Use this for initialization
        void Start()
        {
            // OnRayCastEnter = (a) => Debug.Log("Enter");
            // OnRayCastExit = (a) => Debug.Log("Exit");
            // OnRayCastStay = (a) => Debug.Log("Stay");
        }

        // Update is called once per frame
        void Update()//Fixed
        {
            if (points.Count < 2) return;

            List<RaycastHit> NowColliders = new List<RaycastHit>();

            points = (Origin != null) ? LinePoints.ConvertAll<Vector3>(a => Origin.SE_Local_To_World(a)) : LinePoints;

            for (int i = 1; i < points.Count; i++)
            {
                point1_To_point2 = points[i] - points[i - 1];
                isCheckBox = Physics.Raycast(points[i - 1], point1_To_point2, point1_To_point2.magnitude, ~LayerMask.GetMask(IgnoreLayer), TriggerInteraction);

                if (isCheckBox)
                {
                    NowColliders = NowColliders.Union(Physics.RaycastAll(points[i - 1], point1_To_point2, point1_To_point2.magnitude, ~LayerMask.GetMask(IgnoreLayer), TriggerInteraction).ToList()).ToList();
                }
            }

            if (NowColliders.Count > 0 || LastColliders.Count > 0)
            {
                LastColliders.RemoveAll(v => v.collider == null);//去除可能出现的空物体

                EnterColliders = SS_GameObject.List_Except(NowColliders, LastColliders, IF_Func);
                ExitColliders = SS_GameObject.List_Except(LastColliders, NowColliders, IF_Func);
                StayColliders = SS_GameObject.List_Intersect(NowColliders, LastColliders, IF_Func);



                if (OnRayCastEnter != null && EnterColliders.Count > 0) { OnRayCastEnter(EnterColliders); }
                if (OnRayCastExit != null && ExitColliders.Count > 0) { OnRayCastExit(ExitColliders); }
                if (OnRayCastStay != null && StayColliders.Count > 0) { OnRayCastStay(StayColliders); }
            }

            LastColliders = NowColliders;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (DebugLine)
            {
                // Gizmos.matrix
                Gizmos.color = color;
                for (int i = 1; i < points.Count; i++) Gizmos.DrawLine(points[i - 1], points[i]);
            }
        }
#endif


    }

}