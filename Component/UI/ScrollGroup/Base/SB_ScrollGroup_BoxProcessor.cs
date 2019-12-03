using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.11.04
 * 
 * 功能：滚动列表对象处理器的抽象基类：共同属性：内容物体的添加删除等共同方法
 *
 */


/// <summary>
/// 滚动列表处理器的抽象基类：共同属性：内容物体的添加删除
/// </summary>
public abstract class SB_ScrollGroup_BoxProcessor : MonoBehaviour
{
    /// <summary>
    /// 处理器刷新
    /// </summary>
    public abstract void RefreshGroup();

    /// <summary>
    /// 新建对象
    /// </summary>
    /// <param name="GroupBox">对象</param>
    /// <param name="Index">数据编号</param>
    public abstract void GroupBox_New(GameObject GroupBox, int Index);

    /// <summary>
    /// 删除对象
    /// </summary>
    /// <param name="GroupBox">对象</param>
    /// <param name="Index">数据编号</param>
    public abstract void GroupBox_Del(GameObject GroupBox, int Index);


    /// <summary>
    /// 工作对象
    /// </summary>
    /// <param name="GroupBox">对象</param>
    /// <param name="Index">数据编号</param>
    public abstract void GroupBox_Work(GameObject GroupBox, int Index);

    /// <summary>
    /// 闲置对象
    /// </summary>
    /// <param name="GroupBox">对象</param>
    /// <param name="Index">数据编号</param>
    public abstract void GroupBox_Idle(GameObject GroupBox, int Index);



}
