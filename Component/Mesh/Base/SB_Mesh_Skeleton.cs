using System.Collections.Generic;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.10.17
 * 
 * 功能：网格骨架抽象基类：共同属性：节点集合
 *
 */


/// <summary>
/// 网格骨架抽象基类：共同属性：节点集合
/// </summary>
public abstract class SB_Mesh_Skeleton : MonoBehaviour
{
    /// <summary>
    /// 骨架节点集合
    /// </summary>
	[Tooltip("骨架节点集合列表")]
    public List<Transform> Pionts;

}
