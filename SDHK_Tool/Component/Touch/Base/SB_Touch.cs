using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.10.17
 * 
 * 功能：触摸抽象基类：共同属性：忽略鼠标事件
 *
 */

/// <summary>
/// 触摸抽象基类：共同属性：忽略鼠标事件
/// </summary>
public abstract class SB_Touch : MonoBehaviour
{
 
    /// <summary>
    /// 触摸忽略鼠标事件
    /// </summary>
    [Tooltip("忽略鼠标")]
    public bool IgnoreMouse = false;
}
