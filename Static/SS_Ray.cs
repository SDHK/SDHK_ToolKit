using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * 作者：闪电Y黑客
 * 
 * 日期：2019.6.11
 * 
 * 功能：用于射线检测
 */

namespace SDHK_Tool.Static
{

    /// <summary>
    /// 射线检测类
    /// </summary>
    public static class SS_Ray
    {
        /// <summary>
        /// 射线检测：障碍穿越
        /// </summary>
        /// <param name="origin">射线发射原点</param>
        /// <param name="direction">射线发射向量</param>
        /// <param name="collision_distance">二次延伸长度</param>
        /// <param name="LayerMask">遮罩层:~LayerMask.GetMask(Tags.player)//忽略player</param>
        /// <returns>return : 穿越后的位置</returns>
        public static Vector3 Obstacle_crossing(Vector3 origin, Vector3 direction, float collision_distance, int LayerMask)
        {
            RaycastHit hit;
            Vector3 direction_Extended = direction + direction.normalized * collision_distance;//向量延伸

            return (Physics.Raycast(origin, direction_Extended, out hit, direction_Extended.magnitude, LayerMask))
            ?
                hit.point - (direction.normalized * collision_distance)//前往射线碰撞位置
            :
                origin + direction//回到原始位置
            ;
        }

        /// <summary>
        /// 画布射线：UI事件穿透
        /// </summary>
        /// <param name="eventData">eventData事件</param>
        /// <param name="function">ExecuteEvents.触摸事件</param>
        /// <typeparam name="T">事件类型</typeparam>
        public static void UICastEvent<T>(PointerEventData eventData, ExecuteEvents.EventFunction<T> function)
            where T : IEventSystemHandler
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            GameObject current = eventData.pointerCurrentRaycast.gameObject;
            for (int i = 0; i < results.Count; i++)
            {
                if (current != results[i].gameObject)
                {
                    ExecuteEvents.Execute(results[i].gameObject, eventData, function);//触摸事件传递!!!!!!
                }
            }
        }

        /// <summary>
        /// 画布射线：穿透获取UI集合列表
        /// </summary>
        /// <param name="Canvas">画布射线组件</param>
        /// <param name="position">屏幕坐标</param>
        /// <returns>穿透的UI列表</returns>
        public static List<RaycastResult> UIRayCast(GraphicRaycaster Canvas, Vector2 position)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            List<RaycastResult> list = new List<RaycastResult>();
            eventData.position = position;
            Canvas.Raycast(eventData, list);
            return list;
        }

        /// <summary>
        /// 画布射线：穿透获取UI集合列表
        /// </summary>
        /// <param name="canvas">画布射线组件</param>
        /// <param name="point">屏幕坐标</param>
        /// <param name="InterceptTag">拦截标签</param>
        /// <param name="ignoreLayer">忽略层</param>
        /// <returns> UI集合列表</returns>
        public static List<RaycastResult> UIRayCast(GraphicRaycaster canvas, Vector2 point, string InterceptTag, string[] ignoreLayer)
        {
            List<RaycastResult> UI_List = new List<RaycastResult>();//提取物体列表
            foreach (var UI in SS_Ray.UIRayCast(canvas, point))
            {
                if (!SS_GameObject.If_IgnoreLayer(UI.gameObject, ignoreLayer))
                {
                    UI_List.Add(UI);
                    if (UI.gameObject.tag == InterceptTag) break;
                }
            }
            return UI_List;
        }

        /// <summary>
        /// UI投射事件传递
        /// </summary>
        /// <param name="UI_Objects">UI投射集合列表</param>
        /// <param name="eventData">eventData事件</param>
        /// <param name="function">ExecuteEvents.触摸事件</param>
        /// <typeparam name="T">类型</typeparam>
        public static void UISendEvent<T>(List<RaycastResult> UI_Objects, PointerEventData eventData, ExecuteEvents.EventFunction<T> function)
        where T : IEventSystemHandler//泛型约束
        {

            foreach (var UI_Object in UI_Objects)
            {
                if (UI_Object.gameObject != null)
                {
                    ExecuteEvents.Execute(UI_Object.gameObject, eventData, function);//触摸事件传递
                }
            }
        }

    }
}

