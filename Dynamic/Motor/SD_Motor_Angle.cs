using UnityEngine;
using SDHK_Tool.Static;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.6.16
 * 
 * 功能：集合旋转方法
 */


namespace SDHK_Tool.Dynamic
{

    /// <summary>
    /// 角度旋转电机
    /// </summary>
    public class SD_Motor_Angle
    {

        /// <summary>
        /// 电机激活
        /// </summary>
        public bool MotorActivation = true;
        /// <summary>
        /// 电机速度
        /// </summary>
        public float MotorSpeed = 10;
        /// <summary>
        /// 限制器激活
        /// </summary>
        public bool MotorConstraint = false;
        /// <summary>
        /// 限制数：最小限制
        /// </summary>
        public float Limit_Min = 0;
        /// <summary>
        /// 限制数：最大限制
        /// </summary>
        public float Limit_Max = 0;
        /// <summary>
        /// 范围半径限制【用于范围限制】
        /// </summary>
        public float Limit_Radius = 0;
        /// <summary>
        /// 目标角度
        /// </summary>
        public float MotorTarget;
        /// <summary>
        /// 电机运行后的角度
        /// </summary>
        public float MotorSave;
        /// <summary>
        /// 平滑运行返回的速度
        /// </summary>
        public float MotorVelocity;



        #region 数值赋值



        /// <summary>
        /// 设置电机激活
        /// </summary>
        /// <param name="Activation">激活</param>
        /// <returns>电机</returns>
        public SD_Motor_Angle Set_MotorActivation(bool Activation)
        {
            MotorActivation = Activation;
            return this;
        }

        /// <summary>
        /// 设置限制器激活
        /// </summary>
        /// <param name="Constraint">激活</param>
        /// <returns>电机</returns>
        public SD_Motor_Angle Set_MotorConstraint(bool Constraint)
        {
            MotorConstraint = Constraint;
            return this;
        }

        /// <summary>
        /// 设置电机限制半径
        /// </summary>
        /// <param name="Radius">半径值</param>
        /// <returns>电机</returns>
        public SD_Motor_Angle Set_MotorConstraint_Radius(float Radius)
        {
            Limit_Radius = Radius;
            return this;
        }

        /// <summary>
        /// 设置电机限制值
        /// </summary>
        /// <param name="Limit_Min">最小值</param>
        /// <param name="Limit_Max">最大值</param>
        /// <returns>电机</returns>
        public SD_Motor_Angle Set_MotorConstraint_Limit(float Limit_Min, float Limit_Max)
        {
            this.Limit_Min = Limit_Min;
            this.Limit_Max = Limit_Max;
            return this;
        }

        /// <summary>
        /// 设置电机速度
        /// </summary>
        /// <param name="Speed">速度值</param>
        /// <returns>电机</returns>
        public SD_Motor_Angle Set_MotorSpeed(float Speed)
        {
            MotorSpeed = Speed;
            return this;
        }


        /// <summary>
        /// 设置电机目标角度和当前角度
        /// </summary>
        /// <param name="Value">角度</param>
        /// <returns>电机</returns>
        public SD_Motor_Angle Set_MotorValue_Angle(float angle)
        {
            MotorTarget = angle;
            MotorSave = angle;
            return this;
        }


        /// <summary>
        /// 设置电机目标角度和当前角度
        /// </summary>
        /// <param name="vector2">角度向量</param>
        /// <returns>电机</returns>
        public SD_Motor_Angle Set_MotorValue_Vector(Vector2 vector2)
        {
            MotorTarget = SS_EulerAngleConversion.Get_Angle_In_Vector2(vector2);
            MotorSave = MotorTarget;
            return this;
        }

        /// <summary>
        /// 设置电机位置
        /// </summary>
        /// <param name="angle">角度值</param>
        /// <returns>电机</returns>
        public SD_Motor_Angle Set_MotorSave(float angle)
        {
            MotorSave = angle;
            return this;
        }




        #endregion



        #region 电机操控



        /// <summary>
        /// 设置电机旋转差值
        /// </summary>
        /// <param name="AngleDelta">旋转差值</param>
        /// <returns>电机</returns>
        public SD_Motor_Angle SetTarget_AngleDelta(float AngleDelta)
        {
            if (MotorActivation) MotorTarget += AngleDelta;
            MotorTarget = SS_EulerAngleConversion.Angle_PN_To_P360(MotorTarget);
            return this;
        }

        /// <summary>
        /// 设置电机旋转目标
        /// </summary>
        /// <param name="Angle">旋转目标</param>
        /// <returns>电机</returns>
        public SD_Motor_Angle SetTarget_Angle(float Angle)
        {
            if (MotorActivation) MotorTarget = Angle;
            MotorTarget = SS_EulerAngleConversion.Angle_PN_To_P360(MotorTarget);
            return this;
        }

        /// <summary>
        /// 设置电机旋转目标向量
        /// </summary>
        /// <param name="vector2">旋转目标角度向量</param>
        /// <returns>电机</returns>
        public SD_Motor_Angle SetTarget_Vector(Vector2 vector2)
        {
            if (MotorActivation && vector2 != Vector2.zero) MotorTarget = SS_EulerAngleConversion.Get_Angle_In_Vector2(vector2);
            MotorTarget = SS_EulerAngleConversion.Angle_PN_To_P360(MotorTarget);
            return this;
        }


        #endregion



        #region 电机运行



        /// <summary>
        /// 电机运行 ： 匀速旋转
        /// </summary>
        /// <param name="Time_deltaTime">时间参数</param>
        /// <returns>电机</returns>
        public SD_Motor_Angle Run_MoveTowardsAngle(float Time_deltaTime)
        {
            MotorSave = Mathf.MoveTowardsAngle(MotorSave, MotorTarget, MotorSpeed * Time_deltaTime);
            return this;
        }

        /// <summary>
        /// 电机运行 ： 差值旋转
        /// </summary>
        /// <param name="Time_deltaTime">时间参数</param>
        /// <returns>电机</returns>
        public SD_Motor_Angle Run_LerpAngle(float Time_deltaTime)
        {
            MotorSave = Mathf.LerpAngle(MotorSave, MotorTarget, MotorSpeed * Time_deltaTime);
            return this;
        }

        /// <summary>
        /// 电机运行 ： 平滑旋转
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Angle Run_SmoothDampAngle()
        {
            MotorSave = Mathf.SmoothDampAngle(MotorSave, MotorTarget, ref MotorVelocity, MotorSpeed);
            return this;
        }



        #endregion



        #region 限制器



        /// <summary>
        /// 旋转限制器 ： 本地限制【注：需要限制数值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Angle Constraint_Angle_Local()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_Angle(MotorSave, Limit_Min, Limit_Max);
            return this;
        }

        /// <summary>
        /// 旋转限制器 ： 本地范围限制【注：需要半径值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Angle Constraint_Angle_Local_Scope()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_Angle(MotorSave, -Limit_Radius, Limit_Radius);
            return this;
        }

        /// <summary>
        /// 旋转限制器 ： 本地全面角度限制【注：需要限制数值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Angle Constraint_Angle_Complete_Local()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_Angle_Complete(MotorSave, Limit_Min, Limit_Max);
            return this;
        }

        /// <summary>
        /// 旋转限制器 ： 目标限制【注：需要限制数值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Angle Constraint_Angle_Target()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_Angle_Complete(MotorSave, MotorTarget + Limit_Min, MotorTarget + Limit_Max);
            return this;
        }

        /// <summary>
        /// 旋转限制器 ： 目标范围限制【注：需要半径值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Angle Constraint_Angle_Target_Scope()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_Angle_Complete_Scope(MotorSave, MotorTarget, Limit_Radius);
            return this;
        }

        #endregion



        #region 数值获取



        /// <summary>
        /// 获取电机当前角度
        /// </summary>
        /// <returns>return : 电机角度</returns>
        public float Get_MotorSave()
        {
            return MotorSave;
        }

        /// <summary>
        /// 判断电机旋转完成
        /// </summary>
        /// <param name="Deviation">偏差值</param>
        /// <returns>bool</returns>
        public bool If_MotorSpinFinish(float Deviation)
        {
            return Mathf.Abs(Mathf.DeltaAngle(MotorTarget, MotorSave)) < Deviation;
        }



        #endregion




    }
}

