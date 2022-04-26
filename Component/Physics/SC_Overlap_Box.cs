using System;
using System.Collections.Generic;
using UnityEngine;
using SDHK_Tool.Static;
using System.Linq;

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
    /// 碰撞箱检测器：箱形
    /// </summary>
    public class SC_Overlap_Box : MonoBehaviour
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
        /// 尺寸大小
        /// </summary>
        [Tooltip("尺寸大小")]
        public Vector3 Size = Vector3.one;

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

        private bool isCheckBox = false;
        private bool isLastCheckBox = false;


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            OverlapCenter = transform.TransformPoint(Center);

            OverlapSize = new Vector3(
                transform.lossyScale.x * Size.x,
                 transform.lossyScale.y * Size.y,
                  transform.lossyScale.z * Size.z
            ) * 0.5f;

            isCheckBox = Physics.CheckBox(OverlapCenter, OverlapSize, transform.rotation, ~LayerMask.GetMask(IgnoreLayer));

            if (isCheckBox || isCheckBox != isLastCheckBox)
            {
                List<Collider> NowColliders = Physics.OverlapBox(OverlapCenter, OverlapSize, transform.rotation, ~LayerMask.GetMask(IgnoreLayer), TriggerInteraction).ToList();

                LastColliders.RemoveAll(v => v == null);//去除可能出现的空物体

                EnterColliders = SS_GameObject.List_Except(NowColliders, LastColliders, IF_Func);
                ExitColliders = SS_GameObject.List_Except(LastColliders, NowColliders, IF_Func);
                StayColliders = SS_GameObject.List_Intersect(NowColliders, LastColliders, IF_Func);

                if (OnOverlapEnter != null && EnterColliders.Count > 0) { OnOverlapEnter(EnterColliders); }
                if (OnOverlapExit != null && ExitColliders.Count > 0) { OnOverlapExit(ExitColliders); }
                if (OnOverlapStay != null && StayColliders.Count > 0) { OnOverlapStay(StayColliders); }

                LastColliders = NowColliders;
            }
            isLastCheckBox = isCheckBox;

        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (DebugLine)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.color = color;
                Gizmos.DrawWireCube(Center, Size);
            }
        }
#endif

    }


}