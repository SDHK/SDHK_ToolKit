using System;
using System.Collections.Generic;
using System.Linq;
using SDHK_Tool.Static;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期：2019.10.22
 * 
 * 功能：用于游戏物体碰撞箱的检测判断
 */

namespace SDHK_Tool.Component
{
    /// <summary>
    /// 碰撞箱检测器：球形
    /// </summary>
    public class SC_Overlap_Sphere : MonoBehaviour
    {
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
        /// 中心偏移
        /// </summary>
        [Tooltip("中心点偏移")]
        public Vector3 Center = Vector3.zero;

        [Space()]

        /// <summary>
        /// 半径大小
        /// </summary>
        [Tooltip("半径大小")]
        public float Radius = 0.5f;

        [Space()]
        [Space()]

        /// <summary>
        /// 调试画线
        /// </summary>
        [Tooltip("调试画线")]
        public bool DebugLine = true;

        [Space()]

        /// <summary>
        /// 线条颜色
        /// </summary>
        [Tooltip("线条颜色")]
        public Color color = Color.green;


        /// <summary>
        /// 触发事件：进入
        /// </summary>
        public Action<List<Collider>> OnOverlapEnter;

        /// <summary>
        /// 触发事件：离开
        /// </summary>
        public Action<List<Collider>> OnOverlapExit;

        /// <summary>
        /// 触发事件：停留
        /// </summary>
        public Action<List<Collider>> OnOverlapStay;


        private List<Collider> EnterColliders;
        private List<Collider> ExitColliders;
        private List<Collider> StayColliders;

        private Func<Collider, Collider, bool> IF_Func = (Collider a, Collider b) => { return object.ReferenceEquals(a.gameObject, b.gameObject); };

        private List<Collider> LastColliders = new List<Collider>();
        private Vector3 OverlapCenter;
        private Vector3 OverlapSize;


        private bool isCheckSphere = false;
        private bool isLastCheckSphere = false;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            OverlapCenter = transform.TransformPoint(Center);

            isCheckSphere = Physics.CheckSphere(OverlapCenter, Radius, ~LayerMask.GetMask(IgnoreLayer), TriggerInteraction);

            if (isCheckSphere || isCheckSphere != isLastCheckSphere)
            {
                List<Collider> NowColliders = Physics.OverlapSphere(OverlapCenter, Radius, ~LayerMask.GetMask(IgnoreLayer), TriggerInteraction).ToList();

                LastColliders.RemoveAll(v => v == null);//去除可能出现的空物体

                EnterColliders = SS_GameObject.List_Except(NowColliders, LastColliders, IF_Func);
                ExitColliders = SS_GameObject.List_Except(LastColliders, NowColliders, IF_Func);
                StayColliders = SS_GameObject.List_Intersect(NowColliders, LastColliders, IF_Func);

                if (OnOverlapEnter != null && EnterColliders.Count > 0) OnOverlapEnter(EnterColliders);
                if (OnOverlapExit != null && ExitColliders.Count > 0) OnOverlapExit(ExitColliders);
                if (OnOverlapStay != null && StayColliders.Count > 0) OnOverlapStay(StayColliders);

                LastColliders = NowColliders;
            }
            isLastCheckSphere = isCheckSphere;

        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {

            if (DebugLine)
            {
                Gizmos.matrix = transform.localToWorldMatrix;

                Gizmos.color = color;
                Gizmos.DrawWireSphere(Center, Radius);

                Gizmos.color = new Color(color.r, color.g, color.b, color.a * 0.3f);
                Gizmos.DrawSphere(Center, Radius);
            }

        }
#endif
    }
}