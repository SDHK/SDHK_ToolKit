using UnityEngine;
using SDHK_Tool.Static;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.6.16
 * 
 * 功能：集合欧拉角旋转方法
 */

namespace SDHK_Tool.Dynamic
{

    /// <summary>
    /// 欧拉角旋转电机
    /// </summary>
    public class SD_Motor_EulerAngle
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
        public SD_Motor_EulerAngle Set_MotorActivation(bool Activation)
        {
            MotorActivation = Activation;
            return this;
        }

        /// <summary>
        /// 设置限制器激活
        /// </summary>
        /// <param name="Constraint">激活</param>
        /// <returns>电机</returns>
        public SD_Motor_EulerAngle Set_MotorConstraint(bool Constraint)
        {
            MotorConstraint = Constraint;
            return this;
        }

        /// <summary>
        /// 设置限制轴激活
        /// </summary>
        /// <param name="Constraint_Shaft">轴激活</param>
        /// <returns>电机</returns>
        public SD_Motor_EulerAngle Set_MotorConstraint_Shaft(bool X = true, bool Y = true, bool Z = true)
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
        public SD_Motor_EulerAngle Set_MotorConstraint_Radius(Vector3 Radius)
        {
            Limit_RadiusFree = Radius;
            return this;
        }

        /// <summary>
        /// 设置电机限制值
        /// </summary>
        /// <param name="Limit_Min">最小值</param>
        /// <param name="Limit_Max">最大值</param>
        /// <returns>电机</returns>
        public SD_Motor_EulerAngle Set_MotorConstraint_Limit(Vector3 Limit_Min, Vector3 Limit_Max)
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
        public SD_Motor_EulerAngle Set_MotorSpeed(float Speed)
        {
            MotorSpeed = Speed;
            return this;
        }



        /// <summary>
        /// 设置电机目标角度和当前
        /// </summary>
        /// <param name="EulerAngle">欧拉角</param>
        /// <returns>电机</returns>
        public SD_Motor_EulerAngle Set_MotorValue_Angle(Vector3 EulerAngle)
        {
            MotorTarget = EulerAngle;
            MotorSave = EulerAngle;
            return this;
        }

        /// <summary>
        /// 设置电机目标角度和当前
        /// </summary>
        /// <param name="vector3">角度向量</param>
        /// <returns>电机</returns>
        public SD_Motor_EulerAngle Set_MotorValue_Vector3(Vector3 vector3)
        {
            MotorTarget = vector3;
            MotorSave = vector3;
            return this;
        }

        /// <summary>
        /// 设置电机位置
        /// </summary>
        /// <param name="angle">欧拉角</param>
        /// <returns>电机</returns>
        public SD_Motor_EulerAngle Set_MotorSave(Vector3 EulerAngle)
        {
            MotorSave = EulerAngle;
            return this;
        }



        #endregion



        #region 电机操控



        /// <summary>
        /// 设置电机旋转差值
        /// </summary>
        /// <param name="VectorDelta">旋转差值</param>
        /// <returns>电机</returns>
        public SD_Motor_EulerAngle SetTarget_AngleDelta(Vector3 VectorDelta)
        {
            if (MotorActivation) MotorTarget += VectorDelta;
            MotorTarget = SS_EulerAngleConversion.EulerAngle_PN_To_P360(MotorTarget);
            return this;
        }

        /// <summary>
        /// 设置电机旋转目标
        /// </summary>
        /// <param name="Vector">旋转目标</param>
        /// <returns>电机</returns>
        public SD_Motor_EulerAngle SetTarget_Angle(Vector3 Vector)
        {
            if (MotorActivation) MotorTarget = Vector;
            MotorTarget = SS_EulerAngleConversion.EulerAngle_PN_To_P360(MotorTarget);
            return this;
        }


        /// <summary>
        /// 设置电机旋转目标向量
        /// </summary>
        /// <param name="Vector">旋转目标角度向量</param>
        /// <returns>电机</returns>
        public SD_Motor_EulerAngle SetTarget_Vector(Vector3 Vector)
        {
            if (MotorActivation && Vector != Vector3.zero) MotorTarget = SS_EulerAngleConversion.Get_EulerAngle_In_Vector3(Vector);
            MotorTarget = SS_EulerAngleConversion.EulerAngle_PN_To_P360(MotorTarget);
            return this;
        }


        #endregion



        #region 电机运行



        /// <summary>
        /// 电机运行 ： 匀速旋转
        /// </summary>
        /// <param name="Time_deltaTime">时间参数</param>
        /// <returns>电机</returns>
        public SD_Motor_EulerAngle Run_MoveTowardsAngle(float Time_deltaTime)
        {
            MotorSave = SS_EulerAngleRotation.EulerAngles_MoveTowards(MotorSave, MotorTarget, MotorSpeed * Time_deltaTime);
            return this;
        }

        /// <summary>
        /// 电机运行 ： 差值旋转
        /// </summary>
        /// <param name="Time_deltaTime">时间参数</param>
        /// <returns>电机</returns>
        public SD_Motor_EulerAngle Run_LerpAngle(float Time_deltaTime)
        {
            MotorSave = SS_EulerAngleRotation.EulerAngles_Lerp(MotorSave, MotorTarget, MotorSpeed * Time_deltaTime);
            return this;
        }

        /// <summary>
        /// 电机运行 ： 平滑旋转
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_EulerAngle Run_SmoothDampAngle()
        {
            MotorSave = SS_EulerAngleRotation.EulerAngles_SmoothDamp(MotorSave, MotorTarget, ref MotorVelocity, MotorSpeed);
            return this;
        }

        //===[自轴旋转]======
        /// <summary>
        /// 电机运行：欧拉角操控式自转（相对于自身坐标系的轴旋转）
        /// </summary>
        /// <param name="OriginEulerAngle">物体欧拉角（transform.eulerAngles）</param>
        /// <param name="direction">方向（Vector3.up）</param>
        /// <param name="Time_deltaTime">时间参数</param>
        /// <returns>return : 电机组件</returns>
        public SD_Motor_EulerAngle Run_ShaftControlled(Vector3 OriginEulerAngle, Vector3 direction, float Time_deltaTime)
        {
            if (MotorActivation)
            {
                MotorSave = SS_EulerAngleRotation.EulerAngles_AxisRotation(OriginEulerAngle, direction * MotorSpeed * Time_deltaTime);
            }
            return this;
        }



        #endregion



        #region 限制器



        /// <summary>
        /// 旋转限制器 ： 本地限制【注：需要限制数值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_EulerAngle Constraint_Angle_Local()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_EulerAngle(MotorSave, Limit_Min, Limit_Max, Constraint_X, Constraint_Y, Constraint_Z);
            return this;
        }

        /// <summary>
        /// 旋转限制器 ： 本地范围限制 【注：需要自由半径值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_EulerAngle Constraint_Angle_Local_Scope()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_EulerAngle(MotorSave, -Limit_RadiusFree, Limit_RadiusFree, Constraint_X, Constraint_Y, Constraint_Z);
            return this;
        }

        /// <summary>
        /// 旋转限制器 ： 本地全面角度限制【注：需要限制数值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_EulerAngle Constraint_Angle_Complete_Local()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_EulerAngle_Complete(MotorSave, Limit_Min, Limit_Max, Constraint_X, Constraint_Y, Constraint_Z);
            return this;
        }

        /// <summary>
        /// 旋转限制器 ： 目标限制【注：需要限制数值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_EulerAngle Constraint_Angle_Target()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_EulerAngle_Complete(MotorSave, MotorTarget + Limit_Min, MotorTarget + Limit_Max, Constraint_X, Constraint_Y, Constraint_Z);
            return this;
        }

        /// <summary>
        /// 旋转限制器 ： 目标范围限制【注：需要自由半径值】
        /// </summary>
        /// <returns>电机</returns>
        public SD_Motor_EulerAngle Constraint_Angle_Target_Scope()
        {
            if (MotorConstraint) MotorSave = SS_VectorConstraint.Constraint_EulerAngle_Complete_Scope(MotorSave, MotorTarget, Limit_RadiusFree, Constraint_X, Constraint_Y, Constraint_Z);
            return this;
        }

        #endregion



        #region 数值获取



        /// <summary>
        /// 获取电机当前角度
        /// </summary>
        /// <returns>return : 电机角度</returns>
        public Vector3 Get_MotorSave()
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
            return Mathf.Abs(Vector3.Angle(MotorTarget, MotorSave)) < Deviation;
        }



        #endregion



    }

}
