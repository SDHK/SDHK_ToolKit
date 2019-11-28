using UnityEngine;
using UnityEngine.EventSystems;
using SDHK_Tool.Static;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.10.16
 * 
 * 功能：事件发送到父物体
 *
 */

namespace SDHK_Tool.Component
{
    /// <summary>
    /// 触摸事件渗透器：将触摸事件传给父物体
    /// </summary>
    public class SC_TouchEvent_SendParent : MonoBehaviour
    , IPointerDownHandler
    , IPointerUpHandler
    , IPointerClickHandler
    , IBeginDragHandler
    , IEndDragHandler
    , IDragHandler
    , IDropHandler
    , IScrollHandler
    , IPointerEnterHandler
    , IPointerExitHandler
    {
        /// <summary>
        /// 事件传递给父物体
        /// </summary>
        [Tooltip("事件传给最近组件的父物体")]
        public bool SendNearlyParent = false;

        public void OnBeginDrag(PointerEventData eventData)
        {
            SendParent(SendNearlyParent, gameObject, eventData, ExecuteEvents.beginDragHandler);
        }
        public void OnDrag(PointerEventData eventData)
        {
            SendParent(SendNearlyParent, gameObject, eventData, ExecuteEvents.dragHandler);
        }
        public void OnDrop(PointerEventData eventData)
        {
            SendParent(SendNearlyParent, gameObject, eventData, ExecuteEvents.dropHandler);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            SendParent(SendNearlyParent, gameObject, eventData, ExecuteEvents.endDragHandler);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            SendParent(SendNearlyParent, gameObject, eventData, ExecuteEvents.pointerDownHandler);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            SendParent(SendNearlyParent, gameObject, eventData, ExecuteEvents.pointerClickHandler);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            SendParent(SendNearlyParent, gameObject, eventData, ExecuteEvents.pointerUpHandler);
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            SendParent(SendNearlyParent, gameObject, eventData, ExecuteEvents.pointerEnterHandler);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            SendParent(SendNearlyParent, gameObject, eventData, ExecuteEvents.pointerExitHandler);
        }
        public void OnScroll(PointerEventData eventData)
        {
            SendParent(SendNearlyParent, gameObject, eventData, ExecuteEvents.scrollHandler);
        }

        /// <summary>
        /// 消息发送给父物体
        /// </summary>
        /// <param name="NearlyParent">是否为最近组件父物体</param>
        /// <param name="ThisGameObject">当前游戏物体</param>
        /// <param name="eventData">触摸点信息</param>
        /// <param name="function">触摸事件类型</param>
        /// <typeparam name="T">查找的组件</typeparam>
        public void SendParent<T>(bool NearlyParent, GameObject ThisGameObject, PointerEventData eventData, ExecuteEvents.EventFunction<T> function)
         where T : IEventSystemHandler//泛型约束
        {
            GameObject NewGameObject = (NearlyParent) ? SS_GameObject.GetParent_In_Component<T>(ThisGameObject) : ThisGameObject.transform.parent.gameObject;
            if (NewGameObject != null) ExecuteEvents.Execute(NewGameObject, eventData, function);//触摸事件传递

        }

    }
}