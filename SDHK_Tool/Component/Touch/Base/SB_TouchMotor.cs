using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.10.17
 * 
 * 功能：触摸电机抽象基类：共同功能
 *
 */

/// <summary>
/// 功能：触摸电机抽象基类：共同功能
/// </summary>
public abstract class SB_TouchMotor : MonoBehaviour
{

    /// <summary>
    /// 初始化电机
    /// </summary>
    public abstract void Initialize(Transform TouchObject);

    /// <summary>
    /// 刷新电机
    /// </summary>
    public abstract void Refresh();

    /// <summary>
    /// 最后一个触摸点抬起
    /// </summary>
    public abstract void TouchOnEndUp(Vector2 TargetPosition, float TargetAngle, Vector3 TargetScale);

    /// <summary>
    /// 触摸按下事件
    /// </summary>
    public abstract void TouchOnDown(Vector2 TargetPosition, float TargetAngle, Vector3 TargetScale);

    /// <summary>
    /// 触摸拖拽事件
    /// </summary>
    /// <param name="TargetPosition">移动位置</param>
    /// <param name="TargetAngle">旋转角度</param>
    /// <param name="TargetScale">缩放值</param>
    public abstract void TouchOnDrag(Vector2 TargetPosition, float TargetAngle, Vector3 TargetScale);


    /// <summary>
    /// 电机移动方法
    /// </summary>
    /// <param name="Position">移动位置</param>
    /// <returns>处理后的移动位置</returns>
    public abstract Vector2 TouchMortor_Mobile(Vector2 TargetPosition);


    /// <summary>
    /// 电机旋转方法
    /// </summary>
    /// <param name="Angle">旋转角度</param>
    /// <returns>处理后的旋转角度</returns>
    public abstract float TouchMortor_Rotation(float TargetAngle);

    /// <summary>
    /// 电机缩放方法
    /// </summary>
    /// <param name="Scale">缩放值</param>
    /// <returns>处理后的缩放值</returns>
    public abstract Vector3 TouchMortor_Zoom(Vector3 TargetScale);

}
