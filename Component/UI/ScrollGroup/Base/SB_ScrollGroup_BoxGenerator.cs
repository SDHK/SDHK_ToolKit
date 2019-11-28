using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.11.04
 * 
 * 功能：滚动列表生成器的抽象基类：共同属性：内容物体的添加删除
 *
 */


/// <summary>
/// 滚动列表生成器的抽象基类：共同属性：内容物体的添加删除
/// </summary>
public abstract class SB_ScrollGroup_BoxGenerator : MonoBehaviour
{
    /// <summary>
    /// 生成器刷新
    /// </summary>
    public abstract void RefreshGroup();

    /// <summary>
    /// 生成提取方法
    /// </summary>
    /// <param name="Index">生成编号</param>
    /// <returns>返回生成的游戏物体</returns>
    public abstract GameObject EnterGroupBox(int ObjectId, int Index);

    /// <summary>
    /// 删除的方法
    /// </summary>
    /// <param name="Index">要删除的编号</param>
    public abstract void ExitGroupBox(int ObjectId, int Index);

}
