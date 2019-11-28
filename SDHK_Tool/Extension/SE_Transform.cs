using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDHK_Tool.Static;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.10.23
 * 
 * 功能：Transform类的扩展方法
 */

namespace SDHK_Tool.Extension
{
    /// <summary>
    /// Transform的扩展方法类
    /// </summary>
    public static class SE_Transform
    {
        /// <summary>
        /// 直接输入参数
        /// </summary>
        public static Vector3 SE_Position(this Transform transform, float x, float y, float z)
        {
            return transform.position = new Vector3(x, y, z);
        }

        /// <summary>
        /// 单独修改x值
        /// </summary>
        public static Vector3 SE_Position_X(this Transform transform, float x)
        {
            return transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }

        /// <summary>
        /// 单独修改y值
        /// </summary>
        public static Vector3 SE_Position_Y(this Transform transform, float y)
        {
            return transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }

        /// <summary>
        /// 单独修改z值
        /// </summary>
        public static Vector3 SE_Position_Z(this Transform transform, float z)
        {
            return transform.position = new Vector3(transform.position.x, transform.position.y, z);
        }



        /// <summary>
        /// 直接输入参数
        /// </summary>
        public static Vector3 SE_LocalPosition(this Transform transform, float x, float y, float z)
        {
            return transform.localPosition = new Vector3(x, y, z);
        }

        /// <summary>
        /// 单独修改x值
        /// </summary>
        public static Vector3 SE_LocalPosition_X(this Transform transform, float x)
        {
            return transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        }

        /// <summary>
        /// 单独修改y值
        /// </summary>
        public static Vector3 SE_LocalPosition_Y(this Transform transform, float y)
        {
            return transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        }

        /// <summary>
        /// 单独修改z值
        /// </summary>
        public static Vector3 SE_LocalPosition_Z(this Transform transform, float z)
        {
            return transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
        }



        /// <summary>
        /// 直接输入参数
        /// </summary>
        public static Vector3 SE_EulerAngles(this Transform transform, float x, float y, float z)
        {
            return transform.eulerAngles = new Vector3(x, y, z);
        }

        /// <summary>
        /// 单独修改x值
        /// </summary>
        public static Vector3 SE_EulerAngles_X(this Transform transform, float x)
        {
            return transform.eulerAngles = new Vector3(x, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        /// <summary>
        /// 单独修改y值
        /// </summary>
        public static Vector3 SE_EulerAngles_Y(this Transform transform, float y)
        {
            return transform.eulerAngles = new Vector3(transform.eulerAngles.x, y, transform.eulerAngles.z);
        }

        /// <summary>
        /// 单独修改z值
        /// </summary>
        public static Vector3 SE_EulerAngles_Z(this Transform transform, float z)
        {
            return transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, z);
        }



        /// <summary>
        /// 直接输入参数
        /// </summary>
        public static Vector3 SE_LocalEulerAngles(this Transform transform, float x, float y, float z)
        {
            return transform.localEulerAngles = new Vector3(x, y, z);
        }

        /// <summary>
        /// 单独修改x值
        /// </summary>
        public static Vector3 SE_LocalEulerAngles_X(this Transform transform, float x)
        {
            return transform.localEulerAngles = new Vector3(x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

        /// <summary>
        /// 单独修改y值
        /// </summary>
        public static Vector3 SE_LocalEulerAngles_Y(this Transform transform, float y)
        {
            return transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, y, transform.localEulerAngles.z);
        }

        /// <summary>
        /// 单独修改z值
        /// </summary>
        public static Vector3 SE_LocalEulerAngles_Z(this Transform transform, float z)
        {
            return transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, z);
        }



        /// <summary>
        /// 直接输入参数
        /// </summary>
        public static Vector3 SE_LocalScale(this Transform transform, float x, float y, float z)
        {
            return transform.localScale = new Vector3(x, y, z);
        }

        /// <summary>
        /// 单独修改x值
        /// </summary>
        public static Vector3 SE_LocalScale_X(this Transform transform, float x)
        {
            return transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }

        /// <summary>
        /// 单独修改y值
        /// </summary>
        public static Vector3 SE_LocalScale_Y(this Transform transform, float y)
        {
            return transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
        }

        /// <summary>
        /// 单独修改z值
        /// </summary>
        public static Vector3 SE_LocalScale_Z(this Transform transform, float z)
        {
            return transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
        }


        /// <summary>
        /// 单独修改x值
        /// </summary>
        public static Vector2 SE_AnchoredPosition_X(this RectTransform rectTransform, float x)
        {
            return rectTransform.anchoredPosition = new Vector2(x, rectTransform.anchoredPosition.y);
        }

        /// <summary>
        /// 单独修改y值
        /// </summary>
        public static Vector2 SE_AnchoredPosition_Y(this RectTransform rectTransform, float y)
        {
            return rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
        }

        /// <summary>
        /// 世界坐标转局部坐标
        /// </summary>
        /// <param name="WorldPoint">世界坐标点</param>
        /// <returns>局部坐标</returns>
        public static Vector3 SE_World_To_local(this Transform transform, Vector3 WorldPoint)
        {
            return transform.InverseTransformPoint(WorldPoint);
        }

        /// <summary>
        /// 世界坐标转局部坐标
        /// </summary>
        /// <param name="LocalPoint">局部坐标点</param>
        /// <returns>世界坐标</returns>
        public static Vector3 SE_Local_To_World(this Transform transform, Vector3 LocalPoint)
        {
            return transform.TransformPoint(LocalPoint);
        }


        /// <summary>
        /// 局部坐标转局部坐标
        /// </summary>
        /// <param name="CurrentTransform">当前坐标系</param>
        /// <param name="LocalPoint">当前局部坐标点</param>
        /// <returns>局部坐标点</returns>
        public static Vector3 SE_Local_To_Local(this Transform transform, Transform CurrentTransform, Vector3 LocalPoint)
        {
            return transform.InverseTransformPoint(CurrentTransform.TransformPoint(LocalPoint));
        }





        /// <summary>
        /// 匀速移动
        /// </summary>
        /// <param name="TargetPosition">目标位置</param>
        /// <param name="MoveSpeed">移动速度</param>
        /// <param name="space">坐标系</param>
        public static void SE_Position_MoveTowards(this Transform transform, Vector3 TargetPosition, float MoveSpeed, Space space = Space.World)
        {
            if (space == Space.World)
                transform.position = Vector3.MoveTowards(transform.position, TargetPosition, MoveSpeed);
            else
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, TargetPosition, MoveSpeed);
        }

        /// <summary>
        /// 差值移动
        /// </summary>
        /// <param name="TargetPosition">目标位置</param>
        /// <param name="MoveSpeed">移动速度</param>
        /// <param name="space">坐标系</param>
        public static void SE_Position_Lerp(this Transform transform, Vector3 TargetPosition, float MoveSpeed, Space space = Space.World)
        {
            if (space == Space.World)
                transform.position = Vector3.Lerp(transform.position, TargetPosition, MoveSpeed);
            else
                transform.localPosition = Vector3.Lerp(transform.localPosition, TargetPosition, MoveSpeed);
        }

        /// <summary>
        /// 平滑移动
        /// </summary>
        /// <param name="TargetPosition">目标位置</param>
        /// <param name="MoveTime">移动速度</param>
        /// <param name="space">坐标系</param>
        /// <returns> 移动速度</returns>
        public static Vector3 SE_Position_SmoothDamp(this Transform transform, Vector3 TargetPosition, float MoveTime, Space space = Space.World)
        {
            Vector3 yVelocity = new Vector3();

            if (space == Space.World)
                transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref yVelocity, MoveTime);
            else
                transform.localPosition = Vector3.SmoothDamp(transform.localPosition, TargetPosition, ref yVelocity, MoveTime);

            return yVelocity;
        }



        /// <summary>
        /// 欧拉角匀速旋转
        /// </summary>
        /// <param name="TargetEulerAngle">目标欧拉角</param>
        /// <param name="RotatingMoveSpeed">旋转速度</param>
        /// <param name="space">坐标系</param>
        public static void SE_EulerAngles_MoveTowards(this Transform transform, Vector3 TargetEulerAngle, float RotatingMoveSpeed, Space space = Space.World)
        {
            if (space == Space.World)
                transform.eulerAngles = SS_EulerAngleRotation.EulerAngles_MoveTowards(transform.eulerAngles, TargetEulerAngle, RotatingMoveSpeed);
            else
                transform.localEulerAngles = SS_EulerAngleRotation.EulerAngles_MoveTowards(transform.localEulerAngles, TargetEulerAngle, RotatingMoveSpeed);
        }

        /// <summary>
        /// 欧拉角插值旋转
        /// </summary>
        /// <param name="TargetEulerAngle">目标欧拉角</param>
        /// <param name="RotatingMoveSpeed">旋转速度</param>
        /// <param name="space">坐标系</param>
        public static void SE_EulerAngles_Lerp(this Transform transform, Vector3 TargetEulerAngle, float RotatingMoveSpeed, Space space = Space.World)
        {
            if (space == Space.World)
                transform.eulerAngles = SS_EulerAngleRotation.EulerAngles_Lerp(transform.eulerAngles, TargetEulerAngle, RotatingMoveSpeed);
            else
                transform.localEulerAngles = SS_EulerAngleRotation.EulerAngles_Lerp(transform.localEulerAngles, TargetEulerAngle, RotatingMoveSpeed);
        }

        /// <summary>
        /// 欧拉角平滑旋转
        /// </summary>
        /// <param name="TargetEulerAngle">目标欧拉角</param>
        /// <param name="MoveTime">旋转时间</param>
        /// <param name="space">坐标系</param>
        /// <returns>旋转速度</returns>
        public static Vector3 SE_EulerAngles_SmoothDamp(this Transform transform, Vector3 TargetEulerAngle, float MoveTime, Space space = Space.World)
        {
            Vector3 yVelocity = new Vector3();

            if (space == Space.World)
                transform.eulerAngles = SS_EulerAngleRotation.EulerAngles_SmoothDamp(transform.eulerAngles, TargetEulerAngle, ref yVelocity, MoveTime);
            else
                transform.localEulerAngles = SS_EulerAngleRotation.EulerAngles_SmoothDamp(transform.localEulerAngles, TargetEulerAngle, ref yVelocity, MoveTime);

            return yVelocity;
        }



        /// <summary>
        /// 匀速缩放
        /// </summary>
        /// <param name="TargetPosition">目标尺寸</param>
        /// <param name="MoveSpeed">缩放速度</param>
        public static void SE_Scale_MoveTowards(this Transform transform, Vector3 TargetPosition, float MoveSpeed)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, TargetPosition, MoveSpeed);
        }

        /// <summary>
        /// 差值缩放
        /// </summary>
        /// <param name="TargetPosition">目标尺寸</param>
        /// <param name="MoveSpeed">缩放速度</param>
        public static void SE_Scale_Lerp(this Transform transform, Vector3 TargetPosition, float MoveSpeed)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, TargetPosition, MoveSpeed);
        }

        /// <summary>
        /// 平滑缩放
        /// </summary>
        /// <param name="TargetPosition">目标尺寸</param>
        /// <param name="MoveTime">缩放时间</param>
        public static Vector3 SE_Scale_SmoothDamp(this Transform transform, Vector3 TargetPosition, float MoveTime)
        {
            Vector3 yVelocity = new Vector3();
            transform.localScale = Vector3.SmoothDamp(transform.localScale, TargetPosition, ref yVelocity, MoveTime);
            return yVelocity;
        }



        /// <summary>
        /// 三维向量约束
        /// </summary>
        /// <param name="Limit_Min">最小值</param>
        /// <param name="Limit_Max">最大值</param>
        /// <param name="Limit_X">X轴限制激活</param>
        /// <param name="Limit_Y">Y轴限制激活</param>
        /// <param name="Limit_Z">Z轴限制激活</param>
        /// <param name="space">坐标系</param>
        public static void SE_Position_Constraint(this Transform transform, Vector3 Limit_Min, Vector3 Limit_Max, bool Limit_X = true, bool Limit_Y = true, bool Limit_Z = true, Space space = Space.World)
        {
            if (space == Space.World)
                transform.position = SS_VectorConstraint.Constraint_Vector3(transform.position, Limit_Min, Limit_Max, Limit_X, Limit_Y, Limit_Z);
            else
                transform.localPosition = SS_VectorConstraint.Constraint_Vector3(transform.localPosition, Limit_Min, Limit_Max, Limit_X, Limit_Y, Limit_Z);
        }

        /// <summary>
        /// 三维向量球形范围约束
        /// </summary>
        /// <param name="TargetPoint">约束点</param>
        /// <param name="Limit_Radius">限制半径</param>
        /// <param name="space">坐标系</param>
        public static void SE_Position_Constraint_Scope(this Transform transform, Vector3 TargetPoint, float Limit_Radius, Space space = Space.World)
        {
            if (space == Space.World)
                transform.position = SS_VectorConstraint.Constraint_Vector3_Scope(transform.position, TargetPoint, Limit_Radius);
            else
                transform.localPosition = SS_VectorConstraint.Constraint_Vector3_Scope(transform.localPosition, TargetPoint, Limit_Radius);
        }


        /// <summary>
        /// 欧拉角约束
        /// </summary>
        /// <param name="Limit_Min">最小值</param>
        /// <param name="Limit_Max">最大值</param>
        /// <param name="Limit_X">X轴限制激活</param>
        /// <param name="Limit_Y">Y轴限制激活</param>
        /// <param name="Limit_Z">Z轴限制激活</param>
        /// <param name="space">坐标系</param>
        public static void SE_EulerAngles_Constraint(this Transform transform, Vector3 Limit_Min, Vector3 Limit_Max, bool Limit_X = true, bool Limit_Y = true, bool Limit_Z = true, Space space = Space.World)
        {
            if (space == Space.World)
                transform.eulerAngles = SS_VectorConstraint.Constraint_EulerAngle(transform.eulerAngles, Limit_Min, Limit_Max, Limit_X, Limit_Y, Limit_Z);
            else
                transform.localEulerAngles = SS_VectorConstraint.Constraint_EulerAngle(transform.localEulerAngles, Limit_Min, Limit_Max, Limit_X, Limit_Y, Limit_Z);
        }


        /// <summary>
        /// 尺寸约束
        /// </summary>
        /// <param name="Limit_Min">最小值</param>
        /// <param name="Limit_Max">最大值</param>
        /// <param name="Limit_X">X轴限制激活</param>
        /// <param name="Limit_Y">Y轴限制激活</param>
        /// <param name="Limit_Z">Z轴限制激活</param>
        public static void SE_Scale_Constraint(this Transform transform, Vector3 Limit_Min, Vector3 Limit_Max, bool Limit_X = true, bool Limit_Y = true, bool Limit_Z = true)
        {
            transform.localScale = SS_VectorConstraint.Constraint_Vector3(transform.localScale, Limit_Min, Limit_Max, Limit_X, Limit_Y, Limit_Z);
        }






    }

}