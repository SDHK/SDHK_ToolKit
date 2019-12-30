using System.Collections.Generic;
using System.Linq;
using SDHK_Tool.Static;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.7.24
 *
 * 2019.10.12 添加父物体事件渗透
 *
 * 2019.12.16 修复穿透抬起Bug
 * 
 * 功能：将触摸事件变的可穿透
 *
 * 注：该组件要保持在所有ui的最上层
 *
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// 触摸事件穿透器：将触摸事件穿透所有UI
    /// </summary>
    public class SC_TouchEvent_RayCast : MonoBehaviour
    , IBeginDragHandler
    , IEndDragHandler
    , IDragHandler
    , IPointerDownHandler
    , IPointerUpHandler
    , IScrollHandler
    , IPointerEnterHandler
    , IPointerExitHandler
    {
        public static SC_TouchEvent_RayCast instance;//单例

        /// <summary>
        /// 应用画布
        /// </summary>
        [Tooltip("应用画布")]
        public GraphicRaycaster canvas;//作用画布

        /// <summary>
        /// 拦截标签
        /// </summary>
        [Tooltip("拦截标签")]
        public string InterceptTag = "";//拦截标签

        /// <summary>
        /// 忽略层
        /// </summary>
        [Tooltip("忽略层")]
        public string[] IgnoreLayer;//忽略层

        /// <summary>
        /// 点击获取的UI集合字典
        /// </summary>
        public Dictionary<int, List<RaycastResult>> TouchRayDownLists = new Dictionary<int, List<RaycastResult>>();//点击集合

        /// <summary>
        /// 拖拽获取的UI集合字典
        /// </summary>
        public Dictionary<int, List<RaycastResult>> TouchRayDragLists = new Dictionary<int, List<RaycastResult>>();//拖拽集合

        /// <summary>
        /// 停留获取的UI集合字典
        /// </summary>
        public Dictionary<int, List<RaycastResult>> TouchRayEnterLists = new Dictionary<int, List<RaycastResult>>();//停留集合

        /// <summary>
        /// 触摸字典[无序]
        /// </summary>
        public Dictionary<int, PointerEventData> TouchEnterPool = new Dictionary<int, PointerEventData>();


        private Image Image;

        private void Awake()
        {
            instance = this;//单例
        }

        // Use this for initialization
        void Start()
        {

            Image //必须要有图片
            = (GetComponent<Image>() == null)
            ? gameObject.AddComponent<Image>()
            : GetComponent<Image>()
            ;

            Image.color = new Color(1, 1, 1, 0);//颜色透明化

        }

        public void OnPointerDown(PointerEventData eventData)//按下事件
        {
            if (TouchRayDownLists.ContainsKey(eventData.pointerId)) return;

            List<RaycastResult> UI_List = SS_Ray.UIRayCast(canvas, eventData.position, InterceptTag, IgnoreLayer);//提取物体列表

            if (UI_List.Count < 1) return;
            UI_List.RemoveAt(0);//顶层为自己，剔除。
            if (!TouchRayDownLists.ContainsKey(eventData.pointerId)) TouchRayDownLists.Add(eventData.pointerId, UI_List);//添加物体列表到字典

            //触摸事件通知：按下，开始拖拽
            SS_Ray.UISendEvent(UI_List, eventData, ExecuteEvents.pointerDownHandler);

        }

        public void OnPointerUp(PointerEventData eventData)//抬起事件
        {

            if (!TouchRayDownLists.ContainsKey(eventData.pointerId)) return;
            //抬起事件

            //触摸事件通知：停止拖拽，抬起
            SS_Ray.UISendEvent(TouchRayDownLists[eventData.pointerId], eventData, ExecuteEvents.pointerUpHandler);

            //点击事件

            List<RaycastResult> UI_List = SS_Ray.UIRayCast(canvas, eventData.position, InterceptTag, IgnoreLayer);//提取物体列表

            if (UI_List.Count > 0) UI_List.RemoveAt(0);//顶层为自己，剔除。

            List<RaycastResult> Click_List = SS_GameObject.List_Intersect(TouchRayDownLists[eventData.pointerId], UI_List, (RaycastResult a, RaycastResult b) => { return object.ReferenceEquals(a.gameObject, b.gameObject); });

            SS_Ray.UISendEvent(Click_List, eventData, ExecuteEvents.pointerClickHandler);

            if (TouchRayDownLists.ContainsKey(eventData.pointerId)) TouchRayDownLists.Remove(eventData.pointerId);  //去除物体列表

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (TouchRayDragLists.ContainsKey(eventData.pointerId)) return;
            List<RaycastResult> UI_List = SS_Ray.UIRayCast(canvas, eventData.position, InterceptTag, IgnoreLayer);//提取物体列表
            if (UI_List.Count < 1) return;
            UI_List.RemoveAt(0);//顶层为自己，剔除。
            if (!TouchRayDragLists.ContainsKey(eventData.pointerId)) TouchRayDragLists.Add(eventData.pointerId, UI_List);//添加物体列表到字典
            //触摸事件通知：拖拽开始
            SS_Ray.UISendEvent(TouchRayDragLists[eventData.pointerId], eventData, ExecuteEvents.beginDragHandler);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!TouchRayDragLists.ContainsKey(eventData.pointerId)) return;
            //触摸事件通知：拖拽结束
            SS_Ray.UISendEvent(TouchRayDragLists[eventData.pointerId], eventData, ExecuteEvents.endDragHandler);
            if (TouchRayDragLists.ContainsKey(eventData.pointerId)) TouchRayDragLists.Remove(eventData.pointerId);  //去除物体列表
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (TouchRayEnterLists.ContainsKey(eventData.pointerId)) return;
            List<RaycastResult> UI_List = SS_Ray.UIRayCast(canvas, eventData.position, InterceptTag, IgnoreLayer);//提取物体列表
            if (UI_List.Count < 1) return;
            UI_List.RemoveAt(0);//顶层为自己，剔除。

            if (!TouchRayEnterLists.ContainsKey(eventData.pointerId)) TouchRayEnterLists.Add(eventData.pointerId, UI_List);//停留UI集合池添加

            //触摸事件通知：停留
            SS_Ray.UISendEvent(UI_List, eventData, ExecuteEvents.pointerEnterHandler);

            if (!TouchEnterPool.ContainsKey(eventData.pointerId)) TouchEnterPool.Add(eventData.pointerId, eventData);//停留池添加
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!TouchRayEnterLists.ContainsKey(eventData.pointerId)) return;
            if (TouchEnterPool.ContainsKey(eventData.pointerId)) TouchEnterPool.Remove(eventData.pointerId);//停留池删除

            //触摸事件通知：离开
            SS_Ray.UISendEvent(TouchRayEnterLists[eventData.pointerId], eventData, ExecuteEvents.pointerExitHandler);

            if (TouchRayEnterLists.ContainsKey(eventData.pointerId)) TouchRayEnterLists.Remove(eventData.pointerId);  //停留UI集合池剔除
        }

        public void OnDrag(PointerEventData eventData)//拖拽事件
        {
            if (!TouchRayDragLists.ContainsKey(eventData.pointerId)) return;
            //触摸事件通知：拖拽
            SS_Ray.UISendEvent(TouchRayDownLists[eventData.pointerId], eventData, ExecuteEvents.dragHandler);
        }

        public void OnScroll(PointerEventData eventData)
        {
            List<RaycastResult> UI_List = SS_Ray.UIRayCast(canvas, eventData.position, InterceptTag, IgnoreLayer);//提取物体列表
            if (UI_List.Count < 1) return;
            UI_List.RemoveAt(0);//顶层为自己，剔除。

            //触摸事件通知：滚轮
            SS_Ray.UISendEvent(UI_List, eventData, ExecuteEvents.scrollHandler);
        }

        public void OnStay(PointerEventData eventData)
        {
            List<RaycastResult> UI_List = SS_Ray.UIRayCast(canvas, eventData.position, InterceptTag, IgnoreLayer);//提取物体列表
            if (UI_List.Count < 1) return;
            UI_List.RemoveAt(0);//顶层为自己，剔除。

            List<RaycastResult> Enter_List = SS_GameObject.List_Except(UI_List, TouchRayEnterLists[eventData.pointerId], (RaycastResult a, RaycastResult b) => { return object.ReferenceEquals(a.gameObject, b.gameObject); });
            List<RaycastResult> Exit_List = SS_GameObject.List_Except(TouchRayEnterLists[eventData.pointerId], UI_List, (RaycastResult a, RaycastResult b) => { return object.ReferenceEquals(a.gameObject, b.gameObject); });

            //触摸事件通知：停留
            SS_Ray.UISendEvent(Enter_List, eventData, ExecuteEvents.pointerEnterHandler);

            //触摸事件通知：离开
            SS_Ray.UISendEvent(Exit_List, eventData, ExecuteEvents.pointerExitHandler);

            TouchRayEnterLists[eventData.pointerId] = new List<RaycastResult>(UI_List);
        }

        /// <summary>
        /// 刷新触摸点
        /// </summary>
        /// <param name="EventData">触摸点</param>
        public void TouchRefresh(PointerEventData EventData)
        {
            OnPointerUp(EventData);
            OnPointerDown(EventData);
        }


        // Update is called once per frame
        private void LateUpdate()
        {
            foreach (var item in TouchEnterPool)
            {
                OnStay(item.Value);
            }
        }

    }

}