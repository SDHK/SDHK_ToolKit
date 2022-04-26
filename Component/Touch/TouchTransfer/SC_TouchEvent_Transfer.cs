using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SDHK_Tool.Static;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.10.16
 * 
 * 功能：把自身点击事件传给另一个物体
 *
 */


namespace SDHK_Tool.Component
{
    /// <summary>
    /// 触摸事件转接器：将触摸事件传给另一个物体
    /// </summary>
    public class SC_TouchEvent_Transfer : MonoBehaviour
    , IPointerDownHandler
    , IPointerUpHandler
    {
        /// <summary>
        /// 穿透触摸事件转移延迟时间：实例化的物体需要时间生成0.02应该为最小值了
        /// </summary>
        [Tooltip("穿透触摸的转移延迟时间：实例化的物体需要时间生成")]
        public float TransferTime = 0.02f;  //触摸时间

        /// <summary>
        /// 点击触摸id顺序链表[顺序列表]，通过列表id顺序去字典提取触摸
        /// </summary>
        [Tooltip("触摸点Id顺序表")]
        public List<int> TouchDownIds = new List<int>();
        /// <summary>
        /// 点击触摸字典[无序]
        /// </summary>
        public Dictionary<int, PointerEventData> TouchDownPool = new Dictionary<int, PointerEventData>();
        private GameObject TargetGameObject;//目标游戏物

        /// <summary>
        /// 点击触摸事件转移：触摸事件将由自身转移到目标物体上
        /// </summary>
        /// <param name="targetGameObject">目标物体</param>
        public void Touch_Transfer(GameObject targetGameObject)
        {
            TargetGameObject = targetGameObject;

            if (SC_TouchEvent_RayCast.instance == null)
            {
                Transfer();//普通触摸事件转移为直接转移触摸事件
            }
            else
            {
                Invoke("RayTransfer", TransferTime);//穿透触摸的触摸事件转移，原理为抬起后按下。
            }
        }

        private void RayTransfer()//穿透转移：有穿透器的情况下用普通的触摸穿透器会拿不到这个物体
        {
            //触摸点刷新延迟时间：0.02是因为，实例化的物体需要时间生成

            List<int> TouchDownIds = new List<int>(this.TouchDownIds);

            Dictionary<int, PointerEventData> TouchDownPool = new Dictionary<int, PointerEventData>(this.TouchDownPool);

            foreach (var TouchDownId in TouchDownIds)
            {
                SC_TouchEvent_RayCast.instance.TouchRefresh(TouchDownPool[TouchDownId]);
            }
        }

        private void Transfer()//普通转移
        {
            List<int> TouchDownIds = new List<int>(this.TouchDownIds);

            Dictionary<int, PointerEventData> TouchDownPool = new Dictionary<int, PointerEventData>(this.TouchDownPool);

            foreach (var TouchDownId in TouchDownIds)
            {
                SS_Touch.Transfer_Down(gameObject, TargetGameObject, TouchDownPool[TouchDownId]);
                TouchDownPool[TouchDownId].pointerDrag = TargetGameObject;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (TouchDownPool.ContainsKey(eventData.pointerId)) return;
            TouchDownIds.Add(eventData.pointerId);
            TouchDownPool.Add(eventData.pointerId, eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!TouchDownPool.ContainsKey(eventData.pointerId)) return;
            TouchDownIds.Remove(eventData.pointerId);
            TouchDownPool.Remove(eventData.pointerId);
        }
    }
}