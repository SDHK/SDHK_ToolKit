using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.01.17 
 *
 * 创建源于炮塔旋转限制，用于限制旋转角度，而后扩展为数值限制工具
 *
 * 2019.6.11 从 MyTool 转到 SDHK_Tool 
 *
 * 2020.02.28 合并到 SS_Mathf 里
 * 
 * 功能：对向量和欧拉角进行约束 
 */


namespace SDHK_Tool.Static
{
    /// <summary>
    /// 向量约束器                                      
    /// </summary>
    public static partial class SS_Mathf
    {

        #region 一维

        /// <summary>
        /// 数值约束
        /// </summary>
        /// <param name="Current">当前数值</param>
        /// <param name="Limit_Min">最小值</param>
        /// <param name="Limit_Max">最大值</param>
        /// <returns>return ：约束后的数值</returns>
        public static float Clamp_Vector1(float Current, float Limit_Min, float Limit_Max)
        {
            return Mathf.Clamp(Current, Limit_Min, Limit_Max);
        }

        /// <summary>
        /// 数值范围约束
        /// </summary>
        /// <param name="Current">当前数值</param>
        /// <param name="TargetPoint">约束点</param>
        /// <param name="Limit_Radius">限制半径</param>
        /// <returns>return ：约束后的数值</returns>
        public static float Clamp_Vector1_Scope(float Current, float TargetPoint, float Limit_Radius)
        {
            return Mathf.Clamp(Current, TargetPoint - Limit_Radius, TargetPoint + Limit_Radius);
        }


        #endregion


        #region 二维

        //tranframe 范围


        /// <summary>
        /// 二维向量约束
        /// </summary>
        /// <param name="Current">当前向量</param>
        /// <param name="Limit_Min">最小值</param>
        /// <param name="Limit_Max">最大值</param>
        /// <returns>return ：约束后的向量</returns>
        public static Vector2 Clamp_Vector2(Vector2 Current, Vector2 Limit_Min, Vector2 Limit_Max)
        {
            Current.x = Mathf.Clamp(Current.x, Limit_Min.x, Limit_Max.x);
            Current.y = Mathf.Clamp(Current.y, Limit_Min.y, Limit_Max.y);
            return Current;
        }

        /// <summary>
        /// 二维向量约束[轴可控式]
        /// </summary>
        /// <param name="Current">当前向量</param>
        /// <param name="Limit_Min">最小值</param>
        /// <param name="Limit_Max">最大值</param>
        /// <param name="Limit_X">X轴限制激活</param>
        /// <param name="Limit_Y">Y轴限制激活</param>
        /// <returns>return ：约束后的向量</returns>
        /// 
        public static Vector2 Clamp_Vector2(Vector2 Current, Vector2 Limit_Min, Vector2 Limit_Max, bool Limit_X = true, bool Limit_Y = true)
        {
            if (Limit_X) Current.x = Mathf.Clamp(Current.x, Limit_Min.x, Limit_Max.x);
            if (Limit_Y) Current.y = Mathf.Clamp(Current.y, Limit_Min.y, Limit_Max.y);
            return Current;
        }



        /// <summary>
        /// 二维向量圆形范围约束
        /// </summary>
        /// <param name="Current">当前向量</param>
        /// <param name="TargetPoint">约束点</param>
        /// <param name="Limit_Radius">限制半径</param>
        /// <returns>return ：约束后的向量</returns>
        public static Vector2 Clamp_Vector2_Scope(Vector2 Current, Vector2 TargetPoint, float Limit_Radius)
        {
            Vector2 PointsToCurrent = Current - TargetPoint;
            return (PointsToCurrent.magnitude > Limit_Radius) ? (PointsToCurrent.normalized * Limit_Radius) + TargetPoint : Current;
        }

        #endregion


        #region 三维

        /// <summary>
        /// 三维向量约束
        /// </summary>
        /// <param name="Current">当前向量</param>
        /// <param name="Limit_Min">最小值</param>
        /// <param name="Limit_Max">最大值</param>
        /// <returns>return ：约束后的向量</returns>
        public static Vector3 Clamp_Vector3(Vector3 Current, Vector3 Limit_Min, Vector3 Limit_Max)
        {
            Current.x = Mathf.Clamp(Current.x, Limit_Min.x, Limit_Max.x);
            Current.y = Mathf.Clamp(Current.y, Limit_Min.y, Limit_Max.y);
            Current.z = Mathf.Clamp(Current.z, Limit_Min.z, Limit_Max.z);
            return Current;
        }

        /// <summary>
        /// 三维向量约束[轴可控式]
        /// </summary>
        /// <param name="Current">当前向量</param>
        /// <param name="Limit_Min">最小值</param>
        /// <param name="Limit_Max">最大值</param>
        /// <param name="Limit_X">X轴限制激活</param>
        /// <param name="Limit_Y">Y轴限制激活</param>
        /// <param name="Limit_Z">Z轴限制激活</param>
        /// <returns>return ：约束后的向量</returns>
        public static Vector3 Clamp_Vector3(Vector3 Current, Vector3 Limit_Min, Vector3 Limit_Max, bool Limit_X = true, bool Limit_Y = true, bool Limit_Z = true)
        {
            if (Limit_X) Current.x = Mathf.Clamp(Current.x, Limit_Min.x, Limit_Max.x);
            if (Limit_Y) Current.y = Mathf.Clamp(Current.y, Limit_Min.y, Limit_Max.y);
            if (Limit_Z) Current.z = Mathf.Clamp(Current.z, Limit_Min.z, Limit_Max.z);
            return Current;
        }

        /// <summary>
        /// 三维向量球形范围约束
        /// </summary>
        /// <param name="Current">当前向量</param>
        /// <param name="TargetPoint">约束点</param>
        /// <param name="Limit_Radius">限制半径</param>
        /// <returns>return ：约束后的向量</returns>
        public static Vector3 Clamp_Vector3_Scope(Vector3 Current, Vector3 TargetPoint, float Limit_Radius)
        {
            Vector3 PointsToCurrent = Current - TargetPoint;
            return (PointsToCurrent.magnitude > Limit_Radius) ? (PointsToCurrent.normalized * Limit_Radius) + TargetPoint : Current;
        }


        #endregion


        #region 角度

        /// <summary>
        /// 角度约束（以0为起点的约束）                                  
        ///【注意：组件上的欧拉角 X 轴是经过计算的，最大值为+-90度，谨慎使用】
        /// </summary>
        /// <param name="CurrentAngle">当前角度</param>
        /// <param name="Limit_Min">最小值</param>
        /// <param name="Limit_Max">最大值</param>
        /// <returns>return ：约束后的角度（正负180度）</returns>
        public static float Clamp_Angle(float CurrentAngle, float Limit_Min, float Limit_Max)
        {
            CurrentAngle = SS_EulerAngleConversion.Angle_PN_To_PN180(CurrentAngle);

            return Mathf.Clamp(CurrentAngle, Limit_Min, Limit_Max);
        }

        /// <summary>
        /// 角度约束(全面角度约束)                                    
        ///【注意：组件上的欧拉角 X 轴是经过计算的，最大值为+-90度，谨慎使用】
        /// </summary>
        /// <param name="CurrentAngle">当前角度</param>
        /// <param name="Limit_Min">最小值</param>
        /// <param name="Limit_Max">最大值</param>
        /// <returns>return ：约束后的角度</returns>
        public static float Clamp_Angle_Complete(float CurrentAngle, float Limit_Min, float Limit_Max)
        {

            float MinToMax = Mathf.DeltaAngle(Limit_Min, Limit_Max);//获得+-180差值

            MinToMax = SS_EulerAngleConversion.Angle_PN_To_P360(MinToMax);//转换为360度差

            float Center = Limit_Min + MinToMax / 2;//中心角度

            return Clamp_Angle_Complete_Scope(CurrentAngle, Center, MinToMax / 2);
        }

        /// <summary>
        /// 角度范围约束(全面角度约束)                               
        ///【注意：组件上的欧拉角 X 轴是经过计算的，最大值为+-90度，谨慎使用】
        /// </summary>
        /// <param name="CurrentAngle">当前角度</param>
        /// <param name="TargetAngle">约束角度</param>
        /// <param name="Limit_Radius">限制半径</param>
        /// <returns>return ：约束后的角度</returns>
        public static float Clamp_Angle_Complete_Scope(float CurrentAngle, float TargetAngle, float Limit_Radius)
        {
            float CurToCen = Mathf.DeltaAngle(CurrentAngle, TargetAngle);//获得+-180差值

            if (CurToCen > Limit_Radius) CurrentAngle = TargetAngle - Limit_Radius;
            else
            if (CurToCen < -Limit_Radius) CurrentAngle = TargetAngle + Limit_Radius;

            return CurrentAngle;
        }

        #endregion


        #region 欧拉角


        /// <summary>
        /// 欧拉角约束                                            
        ///【注意：组件上的欧拉角 X 轴是经过计算的，最大值为+-90度，谨慎使用】
        /// </summary>
        /// <param name="CurrentAngle">当前欧拉角</param>
        /// <param name="Limit_Min">最小值</param>
        /// <param name="Limit_Max">最大值</param>
        /// <returns>return ：约束后的角度（正负0~180）</returns>
        public static Vector3 Clamp_EulerAngle(Vector3 CurrentAngle, Vector3 Limit_Min, Vector3 Limit_Max)
        {
            CurrentAngle.x = Clamp_Angle(CurrentAngle.x, Limit_Min.x, Limit_Max.x);
            CurrentAngle.y = Clamp_Angle(CurrentAngle.y, Limit_Min.y, Limit_Max.y);
            CurrentAngle.z = Clamp_Angle(CurrentAngle.z, Limit_Min.z, Limit_Max.z);
            return CurrentAngle;
        }

        /// <summary>
        /// 欧拉角约束 [轴可控式]                                  
        ///【注意：组件上的欧拉角 X 轴是经过计算的，最大值为+-90度，谨慎使用】
        /// </summary>
        /// <param name="CurrentAngle">当前欧拉角</param>
        /// <param name="Limit_Min">最小值</param>
        /// <param name="Limit_Max">最大值</param>
        /// <param name="Limit_X">X轴限制激活</param>
        /// <param name="Limit_Y">Y轴限制激活</param>
        /// <param name="Limit_Z">Z轴限制激活</param>
        /// <returns>return ：约束后的角度（正负0~180）</returns>
        public static Vector3 Clamp_EulerAngle(Vector3 CurrentAngle, Vector3 Limit_Min, Vector3 Limit_Max, bool Limit_X = true, bool Limit_Y = true, bool Limit_Z = true)
        {
            if (Limit_X) CurrentAngle.x = Clamp_Angle(CurrentAngle.x, Limit_Min.x, Limit_Max.x);
            if (Limit_Y) CurrentAngle.y = Clamp_Angle(CurrentAngle.y, Limit_Min.y, Limit_Max.y);
            if (Limit_Z) CurrentAngle.z = Clamp_Angle(CurrentAngle.z, Limit_Min.z, Limit_Max.z);
            return CurrentAngle;
        }

        /// <summary>
        /// 欧拉角约束 (全面角度约束)                               
        ///【注意：组件上的欧拉角 X 轴是经过计算的，最大值为+-90度，谨慎使用】
        /// </summary>
        /// <param name="CurrentEulerAngle">当前欧拉角</param>
        /// <param name="Limit_Min">最小值</param>
        /// <param name="Limit_Max">最大值</param>
        /// <returns>return ：约束后的角度</returns>
        public static Vector3 Clamp_EulerAngle_Complete(Vector3 CurrentEulerAngle, Vector3 Limit_Min, Vector3 Limit_Max)
        {
            CurrentEulerAngle.x = Clamp_Angle_Complete_Scope(CurrentEulerAngle.x, Limit_Min.x, Limit_Max.x);
            CurrentEulerAngle.y = Clamp_Angle_Complete_Scope(CurrentEulerAngle.y, Limit_Min.x, Limit_Max.y);
            CurrentEulerAngle.z = Clamp_Angle_Complete_Scope(CurrentEulerAngle.z, Limit_Min.x, Limit_Max.z);
            return CurrentEulerAngle;
        }

        /// <summary>
        /// 欧拉角约束 [轴可控式] (全面角度约束)                        
        ///【注意：组件上的欧拉角 X 轴是经过计算的，最大值为+-90度，谨慎使用】
        /// </summary>
        /// <param name="CurrentEulerAngle">当前欧拉角</param>
        /// <param name="Limit_Min">最小值</param>
        /// <param name="Limit_Max">最大值</param>
        /// <param name="Limit_X">X轴限制激活</param>
        /// <param name="Limit_Y">Y轴限制激活</param>
        /// <param name="Limit_Z">Z轴限制激活</param>
        /// <returns>return ：约束后的角度</returns>
        public static Vector3 Clamp_EulerAngle_Complete(Vector3 CurrentEulerAngle, Vector3 Limit_Min, Vector3 Limit_Max, bool Limit_X = true, bool Limit_Y = true, bool Limit_Z = true)
        {
            if (Limit_X) CurrentEulerAngle.x = Clamp_Angle_Complete_Scope(CurrentEulerAngle.x, Limit_Min.x, Limit_Max.x);
            if (Limit_Y) CurrentEulerAngle.y = Clamp_Angle_Complete_Scope(CurrentEulerAngle.y, Limit_Min.x, Limit_Max.y);
            if (Limit_Z) CurrentEulerAngle.z = Clamp_Angle_Complete_Scope(CurrentEulerAngle.z, Limit_Min.x, Limit_Max.z);
            return CurrentEulerAngle;
        }

        /// <summary>
        /// 欧拉角范围约束 (全面角度约束)                                  
        ///【注意：组件上的欧拉角 X 轴是经过计算的，最大值为+-90度，谨慎使用】
        /// </summary>
        /// <param name="CurrentEulerAngle">当前角度</param>
        /// <param name="TargetEulerAngle">约束角度</param>
        /// <param name="Limit_Radius">限制半径</param>
        /// <returns>return ：约束后的角度</returns>
        public static Vector3 Clamp_EulerAngle_Complete_Scope(Vector3 CurrentEulerAngle, Vector3 TargetEulerAngle, Vector3 Limit_Radius)
        {
            CurrentEulerAngle.x = Clamp_Angle_Complete_Scope(CurrentEulerAngle.x, TargetEulerAngle.x, Limit_Radius.x);
            CurrentEulerAngle.y = Clamp_Angle_Complete_Scope(CurrentEulerAngle.y, TargetEulerAngle.y, Limit_Radius.y);
            CurrentEulerAngle.z = Clamp_Angle_Complete_Scope(CurrentEulerAngle.z, TargetEulerAngle.z, Limit_Radius.z);
            return CurrentEulerAngle;
        }


        /// <summary>
        /// 欧拉角范围约束 [轴可控式] (全面角度约束)                                   
        ///【注意：组件上的欧拉角 X 轴是经过计算的，最大值为+-90度，谨慎使用】
        /// </summary>
        /// <param name="CurrentEulerAngle">当前角度</param>
        /// <param name="TargetEulerAngle">约束角度</param>
        /// <param name="Limit_Radius">限制半径</param>
        /// <param name="Limit_X">X轴限制激活</param>
        /// <param name="Limit_Y">Y轴限制激活</param>
        /// <param name="Limit_Z">Z轴限制激活</param>
        /// <returns>return ：约束后的角度</returns>
        public static Vector3 Clamp_EulerAngle_Complete_Scope(Vector3 CurrentEulerAngle, Vector3 TargetEulerAngle, Vector3 Limit_Radius, bool Limit_X = true, bool Limit_Y = true, bool Limit_Z = true)
        {
            if (Limit_X) CurrentEulerAngle.x = Clamp_Angle_Complete_Scope(CurrentEulerAngle.x, TargetEulerAngle.x, Limit_Radius.x);
            if (Limit_Y) CurrentEulerAngle.y = Clamp_Angle_Complete_Scope(CurrentEulerAngle.y, TargetEulerAngle.y, Limit_Radius.y);
            if (Limit_Z) CurrentEulerAngle.z = Clamp_Angle_Complete_Scope(CurrentEulerAngle.z, TargetEulerAngle.z, Limit_Radius.z);
            return CurrentEulerAngle;
        }

        #endregion


    }
}
