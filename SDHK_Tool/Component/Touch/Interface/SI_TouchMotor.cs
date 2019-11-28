
using UnityEngine;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.7.23
 * 
 * 功能：拖拽电机功能的约定
 *
 */


/// <summary>
/// 触摸电机接口：约定需要的方法
/// </summary>
public interface SI_TouchMotor
{
    /// <summary>
    /// 最后一个触摸点抬起
    /// </summary>
    void TouchOnEndUp(Vector2 TargetPosition, float TargetAngle, Vector3 TargetScale);

    /// <summary>
    /// 触摸按下事件
    /// </summary>
    void TouchOnDown(Vector2 TargetPosition, float TargetAngle, Vector3 TargetScale);

    /// <summary>
    /// 触摸拖拽事件
    /// </summary>
    /// <param name="TargetPosition">移动位置</param>
    /// <param name="TargetAngle">旋转角度</param>
    /// <param name="TargetScale">缩放值</param>
    void TouchOnDrag(Vector2 TargetPosition, float TargetAngle, Vector3 TargetScale);


    /// <summary>
    /// 电机移动方法
    /// </summary>
    /// <param name="Position">移动位置</param>
    /// <returns>处理后的移动位置</returns>
    Vector2 TouchMortor_Mobile(Vector2 TargetPosition);


    /// <summary>
    /// 电机旋转方法
    /// </summary>
    /// <param name="Angle">旋转角度</param>
    /// <returns>处理后的旋转角度</returns>
    float TouchMortor_Rotation(float TargetAngle);

    /// <summary>
    /// 电机缩放方法
    /// </summary>
    /// <param name="Scale">缩放值</param>
    /// <returns>处理后的缩放值</returns>
    Vector3 TouchMortor_Zoom(Vector3 TargetScale);


}
