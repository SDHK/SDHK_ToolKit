using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.10.17
 * 
 * 功能：触摸事件抽象基类：共同属性：触摸池
 *
 */

/// <summary>
/// 触摸事件抽象基类：共同属性：触摸池
/// </summary>
public abstract class SB_TouchEvent : SB_Touch
{
    /// <summary>
    /// 触摸id顺序链表[顺序列表]，通过列表id顺序去字典提取触摸
    /// </summary>
    [Tooltip("触摸点Id顺序表")]
    public List<int> TouchIds = new List<int>();

    /// <summary>
    /// 触摸字典[无序]
    /// </summary>
    public Dictionary<int, PointerEventData> TouchPool = new Dictionary<int, PointerEventData>();
}
