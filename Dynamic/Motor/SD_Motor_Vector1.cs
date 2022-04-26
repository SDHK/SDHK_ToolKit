using UnityEngine;
using SDHK_Tool.Static;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.6.16
 * 
 * 功能：集合移动方法
 */

namespace SDHK_Tool.Dynamic
{

    /// <summary>
    /// 一维移动电机
    /// </summary>
    public class SD_Motor_Vector1
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
        /// 目标位置
        /// </summary>
        public float MotorTarget;
        /// <summary>
        /// 电机运行后的位置
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
        public SD_Motor_Vector1 Set_MotorActivation(bool Activation)
        {
            MotorActivation = Activation;
            return this;
        }

        /// <summary>
        /// 设置限制器激活
        /// </summary>
        /// <param name="Constraint">激活</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector1 Set_MotorConstraint(bool Constraint)
        {
            MotorConstraint = Constraint;
            return this;
        }

        /// <summary>
        /// 设置电机限制半径
        /// </summary>
        /// <param name="Radius">半径值</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector1 Set_MotorConstraint_Radius(float Radius)
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
        public SD_Motor_Vector1 Set_MotorConstraint_Limit(float Limit_Min, float Limit_Max)
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
        public SD_Motor_Vector1 Set_MotorSpeed(float Speed)
        {
            MotorSpeed = Speed;
            return this;
        }


        /// <summary>
        /// 设置电机目标和位置
        /// </summary>
        /// <param name="Value">值</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector1 Set_MotorValue(float Value)
        {
            MotorTarget = Value;
            MotorSave = Value;
            return this;
        }



        #endregion



        #region 电机操控



        /// <summary>
        /// 设置电机移动差值
        /// </summary>
        /// <param name="VectorDelta">移动差值</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector1 SetTarget_VectorDelta(float VectorDelta)
        {
            if (MotorActivation) MotorTarget += VectorDelta;
            return this;
        }

        /// <summary>
        /// 设置电机移动目标
        /// </summary>
        /// <param name="Vector">移动目标</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector1 SetTarget_Vector(float Vector)
        {
            if (MotorActivation) MotorTarget = Vector;
            return this;
        }

        /// <summary>
        /// 设置电机位置
        /// </summary>
        /// <param name="Vector">位置值</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector1 Set_MotorSave(float Value)
        {
            MotorSave = Value;
            return this;
        }



        #endregion



        #region 电机运行



        /// <summary>
        /// 电机运行 ： 匀速移动
        /// </summary>
        /// <param name="Time_deltaTime">时间参数</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector1 Run_MoveTowards(float Time_deltaTime)
        {
            MotorSave = Mathf.MoveTowards(MotorSave, MotorTarget, MotorSpeed * Time_deltaTime);
            return this;
        }

        /// <summary>
        /// 电机运行 ： 差值移动
        /// </summary>
        /// <param name="Time_deltaTime">时间参数</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector1 Run_Lerp(float Time_deltaTime)
        {
            MotorSave = Mathf.Lerp(MotorSave, MotorTarget, MotorSpeed * Time_deltaTime);
            return this;
        }

        /// <summary>
        /// 电机运行 ： 平滑移动
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Vector1 Run_SmoothDamp()
        {
            MotorSave = Mathf.SmoothDamp(MotorSave, MotorTarget, ref MotorVelocity, MotorSpeed);
            return this;
        }



        #endregion 



        #region 限制器



        /// <summary>
        /// 移动限制器 ： 本地限制【注：需要限制数值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Vector1 Constraint_Vector_Local()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_Vector1(MotorSave, Limit_Min, Limit_Max);
            return this;
        }

        /// <summary>
        /// 移动限制器 ： 本地范围限制【注：需要半径值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Vector1 Constraint_Vector_Local_Scope()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_Vector1_Scope(MotorSave, 0, Limit_Radius);
            return this;
        }

        /// <summary>
        /// 移动限制器 ： 目标限制【注：需要限制数值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Vector1 Constraint_Vector_Target()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_Vector1(MotorSave, MotorTarget + Limit_Min, MotorTarget + Limit_Max);
            return this;
        }

        /// <summary>
        /// 移动限制器 ： 目标范围限制【注：需要半径值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Vector1 Constraint_Vector_Target_Scope()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_Vector1_Scope(MotorSave, MotorTarget, Limit_Radius);
            return this;
        }



        #endregion



        #region 数值获取



        /// <summary>
        /// 获取电机当前位置
        /// </summary>
        /// <returns>return : 电机位置</returns>
        public float Get_MotorSave()
        {
            return MotorSave;
        }

        /// <summary>
        /// 判断电机运行完成
        /// </summary>
        /// <param name="Deviation">偏差值</param>
        /// <returns>bool</returns>
        public bool If_MotorRunFinish(float Deviation)
        {
            return Mathf.Abs(MotorTarget - MotorSave) < Deviation;
        }



        #endregion



    }

}

