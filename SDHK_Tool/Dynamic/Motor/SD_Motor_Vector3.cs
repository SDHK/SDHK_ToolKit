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
    /// 三维移动电机
    /// </summary>
    public class SD_Motor_Vector3
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
        /// 限制X轴激活
        /// </summary>
        public bool Constraint_X = true;
        /// <summary>
        /// 限制Y轴激活
        /// </summary>
        public bool Constraint_Y = true;
        /// <summary>
        /// 限制Z轴激活
        /// </summary>
        public bool Constraint_Z = true;

        /// <summary>
        /// 限制数：最小限制
        /// </summary>
        public Vector3 Limit_Min = Vector3.zero;
        /// <summary>
        /// 限制数：最大限制
        /// </summary>
        public Vector3 Limit_Max = Vector3.zero;
        /// <summary>
        /// 范围半径限制【用于范围限制】
        /// </summary>
        public float Limit_Radius = 0;
        /// <summary>
        /// 范围自由半径限制【用于自定义范围限制】
        /// </summary>
        public Vector3 Limit_RadiusFree = Vector3.zero;
        /// <summary>
        /// 目标位置
        /// </summary>
        public Vector3 MotorTarget;
        /// <summary>
        /// 电机运行后的位置
        /// </summary>
        public Vector3 MotorSave;
        /// <summary>
        /// 平滑运行返回的速度
        /// </summary>
        public Vector3 MotorVelocity;



        #region 数值赋值



        /// <summary>
        /// 设置电机激活
        /// </summary>
        /// <param name="Activation">激活</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Set_MotorActivation(bool Activation)
        {
            MotorActivation = Activation;
            return this;
        }

        /// <summary>
        /// 设置限制器激活
        /// </summary>
        /// <param name="Constraint">激活</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Set_MotorConstraint(bool Constraint)
        {
            MotorConstraint = Constraint;
            return this;
        }

        /// <summary>
        /// 设置限制轴激活
        /// </summary>
        /// <param name="Constraint_Shaft">轴激活</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Set_MotorConstraint_Shaft(bool X = true, bool Y = true, bool Z = true)
        {

            Constraint_X = X;
            Constraint_Y = Y;
            Constraint_Z = Z;

            return this;
        }

        /// <summary>
        /// 设置电机限制半径
        /// </summary>
        /// <param name="Radius">半径值</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Set_MotorConstraint_Radius(float Radius)
        {
            Limit_Radius = Radius;
            return this;
        }

        /// <summary>
        /// 设置电机限制半径（自由半径）
        /// </summary>
        /// <param name="Radius">半径值</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Set_MotorConstraint_Radius(Vector3 RadiusFree)
        {
            Limit_RadiusFree = RadiusFree;
            return this;
        }

        /// <summary>
        /// 设置电机限制值
        /// </summary>
        /// <param name="Limit_Min">最小值</param>
        /// <param name="Limit_Max">最大值</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Set_MotorConstraint_Limit(Vector3 Limit_Min, Vector3 Limit_Max)
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
        public SD_Motor_Vector3 Set_MotorSpeed(float Speed)
        {
            MotorSpeed = Speed;
            return this;
        }

        /// <summary>
        /// 设置电机目标
        /// </summary>
        /// <param name="Value">目标值</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Set_MotorTarget(Vector3 Value)
        {
            MotorTarget = Value;
            return this;
        }

        /// <summary>
        /// 设置电机位置
        /// </summary>
        /// <param name="Value">位置值</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Set_MotorSave(Vector3 Value)
        {
            MotorSave = Value;
            return this;
        }

        /// <summary>
        /// 设置电机目标和位置
        /// </summary>
        /// <param name="Value">值</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Set_MotorValue(Vector3 Value)
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
        public SD_Motor_Vector3 SetTarget_VectorDelta(Vector3 VectorDelta)
        {
            if (MotorActivation) MotorTarget += VectorDelta;
            return this;
        }

        /// <summary>
        /// 设置电机移动目标
        /// </summary>
        /// <param name="Vector">移动目标</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 SetTarget_Vector(Vector3 Vector)
        {
            if (MotorActivation) MotorTarget = Vector;
            return this;
        }



        #endregion




        #region 电机运行

        /// <summary>
        /// 电机运行 ： 匀速移动（速度越大越快，需要较大的值）
        /// </summary>
        /// <param name="Time_deltaTime">时间参数</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Run_MoveTowards(float Time_deltaTime)
        {
            MotorSave = Vector3.MoveTowards(MotorSave, MotorTarget, MotorSpeed * Time_deltaTime);
            return this;
        }

        /// <summary>
        /// 电机运行 ： 差值移动（速度越大越快）
        /// </summary>
        /// <param name="Time_deltaTime">时间参数</param>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Run_Lerp(float Time_deltaTime)
        {
            MotorSave = Vector3.Lerp(MotorSave, MotorTarget, MotorSpeed * Time_deltaTime);
            return this;
        }

        /// <summary>
        /// 电机运行 ： 平滑移动（速度越小越快）
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Run_SmoothDamp()
        {
            MotorSave = Vector3.SmoothDamp(MotorSave, MotorTarget, ref MotorVelocity, MotorSpeed);
            return this;
        }

        //===[自轴移动]======
        /// <summary>
        /// 电机运行：自轴移动操控式（相对于自身坐标系的轴移动）
        /// </summary>
        /// <param name="Origin">物体位置（transform.position）</param>
        /// <param name="direction">方向（transform.forward为正前方）</param>
        /// <param name="Time_deltaTime">时间参数</param>
        /// <returns>return : 电机组件</returns>
        public SD_Motor_Vector3 Run_ShaftDelta(Vector3 Origin, Vector3 direction, float Time_deltaTime)
        {
            if (MotorActivation)
            {
                MotorSave = Origin + direction * MotorSpeed * Time_deltaTime;
            }
            return this;
        }



        #endregion 



        #region 限制器



        /// <summary>
        /// 移动限制器 ： 本地限制【注：需要限制数值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Constraint_Vector_Local()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_Vector3(MotorSave, Limit_Min, Limit_Max, Constraint_X, Constraint_Y, Constraint_Z);
            return this;
        }

        /// <summary>
        /// 移动限制器 ： 本地范围限制(球形范围限制)【注：需要半径值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Constraint_Vector_Local_Scope()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_Vector3_Scope(MotorSave, Vector2.zero, Limit_Radius);
            return this;
        }

        /// <summary>
        /// 移动限制器 ： 本地范围限制（自由范围限制）【注：需要自由半径值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Constraint_Vector_Local_Scope_Free()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_Vector3(MotorSave, -Limit_RadiusFree, Limit_RadiusFree, Constraint_X, Constraint_Y, Constraint_Z);
            return this;
        }

        /// <summary>
        /// 移动限制器 ： 目标限制 【注：需要限制数值】
        /// 注：需要限制范围值
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Constraint_Vector_Target()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_Vector3(MotorSave, MotorTarget + Limit_Min, MotorTarget + Limit_Max, Constraint_X, Constraint_Y, Constraint_Z);
            return this;
        }

        /// <summary>
        /// 移动限制器 ： 目标范围限制（球形半径范围限制）【注：需要半径值】
        /// 注：需要半径值
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Constraint_Vector_Target_Scope()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_Vector3_Scope(MotorSave, MotorTarget, Limit_Radius);
            return this;
        }

        /// <summary>
        /// 移动限制器 ： 目标范围限制（自由半径范围限制）【注：需要自由半径值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_Vector3 Constraint_Vector_Target_Scope_Free()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_Vector3(MotorSave, MotorTarget - Limit_RadiusFree, MotorTarget + Limit_RadiusFree, Constraint_X, Constraint_Y, Constraint_Z);
            return this;
        }



        #endregion



        #region 数值获取



        /// <summary>
        /// 获取电机当前位置
        /// </summary>
        /// <returns>return : 电机位置</returns>
        public Vector3 Get_MotorSave()
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
            return (MotorTarget - MotorSave).magnitude < Deviation;
        }



        #endregion



    }

}