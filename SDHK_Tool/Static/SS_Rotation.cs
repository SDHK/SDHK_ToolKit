using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.6.11
 * 
 * 功能：旋转方法的集合类
 */

namespace SDHK_Tool.Static
{

    /// <summary>
    /// 旋转集合
    /// </summary>
    public static class SS_Rotation
    {
        /// <summary>
        /// 欧拉角匀速旋转
        /// </summary>
        /// <param name="CurrentEulerAngle">当前欧拉角</param>
        /// <param name="TargetEulerAngle">目标欧拉角</param>
        /// <param name="RotationSpeed">旋转速度</param>
        /// <returns>return : 旋转后的欧拉角</returns>
        public static Vector3 EulerAngles_MoveTowards(Vector3 CurrentEulerAngle, Vector3 TargetEulerAngle, float RotationSpeed)
        {
            CurrentEulerAngle.x = Mathf.MoveTowardsAngle(CurrentEulerAngle.x, TargetEulerAngle.x, RotationSpeed);
            CurrentEulerAngle.y = Mathf.MoveTowardsAngle(CurrentEulerAngle.y, TargetEulerAngle.y, RotationSpeed);
            CurrentEulerAngle.z = Mathf.MoveTowardsAngle(CurrentEulerAngle.z, TargetEulerAngle.z, RotationSpeed);
            return CurrentEulerAngle;
        }

        /// <summary>
        /// 欧拉角插值旋转
        /// </summary>
        /// <param name="CurrentEulerAngle">当前欧拉角</param>
        /// <param name="TargetEulerAngle">目标欧拉角</param>
        /// <param name="RotationSpeed">旋转速度（差值比例）</param>
        /// <returns>return : 旋转后的欧拉角</returns>
        public static Vector3 EulerAngles_Lerp(Vector3 CurrentEulerAngle, Vector3 TargetEulerAngle, float RotationSpeed)
        {
            CurrentEulerAngle.x = Mathf.LerpAngle(CurrentEulerAngle.x, TargetEulerAngle.x, RotationSpeed);
            CurrentEulerAngle.y = Mathf.LerpAngle(CurrentEulerAngle.y, TargetEulerAngle.y, RotationSpeed);
            CurrentEulerAngle.z = Mathf.LerpAngle(CurrentEulerAngle.z, TargetEulerAngle.z, RotationSpeed);
            return CurrentEulerAngle;
        }

        /// <summary>
        /// 欧拉角平滑旋转
        /// </summary>
        /// <param name="CurrentEulerAngle">当前欧拉角</param>
        /// <param name="TargetEulerAngle">目标欧拉角</param>
        /// <param name="yVelocity">ref 当前速度</param>
        /// <param name="MoveTime">到达目标的近似时间</param>
        /// <returns>return : 旋转后的欧拉角</returns>
        public static Vector3 EulerAngles_SmoothDamp(Vector3 CurrentEulerAngle, Vector3 TargetEulerAngle, ref Vector3 yVelocity, float MoveTime)
        {
            CurrentEulerAngle.x = Mathf.SmoothDampAngle(CurrentEulerAngle.x, TargetEulerAngle.x, ref yVelocity.x, MoveTime);
            CurrentEulerAngle.y = Mathf.SmoothDampAngle(CurrentEulerAngle.y, TargetEulerAngle.y, ref yVelocity.y, MoveTime);
            CurrentEulerAngle.z = Mathf.SmoothDampAngle(CurrentEulerAngle.z, TargetEulerAngle.z, ref yVelocity.z, MoveTime);
            return CurrentEulerAngle;
        }

        /// <summary>
        /// 欧拉角自轴旋转
        /// </summary>
        /// <param name="CurrentEulerAngle">当前物体欧拉角(transform.eulerAngles)</param>
        /// <param name="direction">旋转轴向量及角度大小（Vector3.up）</param>
        /// <returns>return : 自转后的欧拉角</returns>
        public static Vector3 EulerAngles_AxisRotation(Vector3 CurrentEulerAngle, Vector3 direction)
        {
            return (Quaternion.Euler(CurrentEulerAngle) * Quaternion.Euler(direction)).eulerAngles;
        }


        /// <summary>
        /// 匀速旋转
        /// </summary>
        /// <param name="CurrentAngle">当前角度</param>
        /// <param name="TargetAngle">目标角度</param>
        /// <param name="RotationSpeed">旋转速度</param>
        /// <returns>return : 旋转后的角度</returns>
        public static float Angle_MoveTowards(float CurrentAngle, float TargetAngle, float RotationSpeed)
        {
            return Mathf.MoveTowardsAngle(CurrentAngle, TargetAngle, RotationSpeed);
        }

        /// <summary>
        /// 差值旋转
        /// </summary>
        /// <param name="CurrentAngle">当前角度</param>
        /// <param name="TargetAngle">目标角度</param>
        /// <param name="RotationSpeed">旋转速度</param>
        /// <returns>return : 旋转后的角度</returns>
        public static float Angle_Lerp(float CurrentAngle, float TargetAngle, float RotationSpeed)
        {
            return Mathf.LerpAngle(CurrentAngle, TargetAngle, RotationSpeed);
        }

        /// <summary>
        /// 平滑旋转
        /// </summary>
        /// <param name="CurrentAngle">当前角度</param>
        /// <param name="TargetAngle">目标角度</param>
        /// <param name="yVelocity">ref 当前速度</param>
        /// <param name="MoveTime">到达目标的近似时间</param>
        /// <returns>return : 旋转后的角度</returns>
        public static float Angle_SmoothDamp(float CurrentAngle, float TargetAngle, ref float yVelocity, float MoveTime)
        {
            return Mathf.SmoothDampAngle(CurrentAngle, TargetAngle, ref yVelocity, MoveTime);
        }

        /// <summary>
        /// 自轴旋转
        /// </summary>
        /// <param name="CurrentAngle">当前角度</param>
        /// <param name="direction">方向值</param>
        /// <returns>return : 自转后的角度</returns>
        public static float Angle_AxisRotation(float CurrentAngle, float direction)
        {
            return SS_EulerAngleConversion.Angle_PN_To_P360(CurrentAngle + direction);
        }

    }
}
