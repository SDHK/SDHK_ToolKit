using UnityEngine;
using SDHK_Tool.Dynamic;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.7.23
 *
 * 2019.10.12 添加触摸惯性
 * 
 * 功能：平滑和限制数值
 *
 * 注：可以自己写一个自己适用的，只要接上 I_TouchMotor 接口就能赋值给 TouchTransform
 *
 */
namespace SDHK_Tool.Component
{
    /// <summary>
    /// 触摸电机
    /// </summary>
    public class SC_TouchMotor : SB_TouchMotor
    {
        [Space()]

        /// <summary>
        /// 移动平滑速度
        /// </summary>
        [Tooltip("移动平滑速度：越小越快")]
        public float Mobile_SmoothSpeed = 0.1f;



        /// <summary>
        /// 移动提前量偏差
        /// </summary>
        [Tooltip("移动惯性")]
        public float Mobile_Inertial = 0;


        [Space()]
        /// <summary>
        /// 旋转平滑速度
        /// </summary>
        [Tooltip("旋转平滑速度：越小越快")]
        public float Rotation_SmoothSpeed = 0.1f;


        /// <summary>
        /// 旋转提前量偏差
        /// </summary>
        [Tooltip("旋转惯性")]
        public float Rotation_Inertial = 0;

        [Space()]

        /// <summary>
        /// 缩放平滑速度
        /// </summary>
        [Tooltip("缩放平滑速度：越小越快")]
        public float Zoom_SmoothSpeed = 0.1f;

        /// <summary>
        /// 缩放提前量偏差
        /// </summary>
        [Tooltip("缩放惯性")]
        public float Zoom_Inertial = 0;


        [Space()]
        [Space()]



        /// <summary>
        /// 移动限制激活
        /// </summary>
        [Tooltip("移动限制激活")]
        public bool LimitMobile = false;


        /// <summary>
        /// 限制为当前位置
        /// </summary>
        [Tooltip("限制为当前位置")]
        public bool Limit_Current = false;

        /// <summary>
        /// X轴约束
        /// </summary>
        [Tooltip("X轴约束")]
        public bool Limit_X = true;

        /// <summary>
        /// Y轴约束
        /// </summary>
        [Tooltip("Y轴约束")]
        public bool Limit_Y = true;


        /// <summary>
        /// 移动限制最小值
        /// </summary>
        [Tooltip("移动位置限制最小值")]
        public Vector2 Limit_Size_MobileMin = new Vector2(-100, -100);
        /// <summary>
        /// 移动限制最大值
        /// </summary>
        [Tooltip("移动位置限制最大值")]
        public Vector2 Limit_Size_MobileMax = new Vector2(100, 100);

        [Space()]
        [Space()]

        /// <summary>
        /// 旋转限制激活
        /// </summary>
        [Tooltip("旋转限制激活")]
        public bool LimitRotation = false;

        /// <summary>
        /// 旋转角度限制
        /// </summary>
        [Tooltip("旋转角度限制值")]
        public Vector2 Limit_Size_Rotation = new Vector2(-90, 90);

        [Space()]
        [Space()]


        /// <summary>
        /// 缩放限制激活
        /// </summary>
        [Tooltip("缩放限制激活")]
        public bool LimitZoom = false;


        /// <summary>
        /// 缩放尺寸限制
        /// </summary>
        [Tooltip("缩放尺寸限制值")]
        public Vector2 Limit_Size_Zoom = new Vector2(0.5f, 5);


        private Transform TouchObject;
        private Vector3 SaveScale;//保存初始化尺寸

        private Vector2 TargetPosition_Last;
        private float TargetAngle_Last;
        private Vector3 TargetScale_Last;

        private Vector2 Inertia_Position;

        private float Inertia_Angle;
        private Vector3 Inertia_Scale;

        private Vector2 Inertia_Position_Save;
        private float Inertia_Angle_Save;
        private Vector3 Inertia_Scale_Save;

        public SD_Motor_Vector2 MotorMobile;
        public SD_Motor_Angle MotorRotation;
        public SD_Motor_Vector3 MotorZoom;



        public override void Initialize(Transform touchObject)
        {
            TouchObject = touchObject;

            SaveScale = TouchObject.localScale;       //保存物体初始尺寸

            MotorMobile = new SD_Motor_Vector2()	//初始化移动电机值
            .Set_MotorValue(TouchObject.localPosition);


            MotorRotation = new SD_Motor_Angle()	//初始化旋转电机值
            .Set_MotorValue_Angle(TouchObject.localEulerAngles.z);

            MotorZoom = new SD_Motor_Vector3()      //初始化缩放电机值
            .Set_MotorValue(TouchObject.localScale);


        }


        public override void Refresh()
        {
            MotorMobile.Set_MotorValue(TouchObject.localPosition);
            MotorRotation.Set_MotorValue_Angle(TouchObject.localEulerAngles.z);
            MotorZoom.Set_MotorValue(TouchObject.localScale);

            Inertia_Position_Save = Inertia_Position = Vector2.zero;
            Inertia_Angle_Save = Inertia_Angle = 0;
            Inertia_Scale_Save = Inertia_Scale = Vector3.zero;
        }

        private void Start()
        {
            if (TouchObject == null) TouchObject = this.transform;
            
            if (Limit_Current)
            {
                Limit_Size_MobileMin += (Vector2)TouchObject.localPosition;
                Limit_Size_MobileMax += (Vector2)TouchObject.localPosition;
            }
        }

        public override void TouchOnDown(Vector2 TargetPosition, float TargetAngle, Vector3 TargetScale)
        {
            Inertia_Position_Save = Inertia_Position = Vector2.zero;
            Inertia_Angle_Save = Inertia_Angle = 0;
            Inertia_Scale_Save = Inertia_Scale = Vector3.zero;
        }

        public override void TouchOnEndUp(Vector2 TargetPosition, float TargetAngle, Vector3 TargetScale)
        {
            Inertia_Position = Inertia_Position_Save * Mobile_Inertial;
            Inertia_Angle = Inertia_Angle_Save * Rotation_Inertial;
            Inertia_Scale = Inertia_Scale_Save * Zoom_Inertial;
        }

        public override void TouchOnDrag(Vector2 TargetPosition, float TargetAngle, Vector3 TargetScale)
        {
            Inertia_Position_Save = TargetPosition - TargetPosition_Last;
            Inertia_Angle_Save = TargetAngle - TargetAngle_Last;
            Inertia_Scale_Save = TargetScale - TargetScale_Last;

            TargetPosition_Last = TargetPosition;
            TargetAngle_Last = TargetAngle;
            TargetScale_Last = TargetScale;

        }



        public override Vector2 TouchMortor_Mobile(Vector2 TargetPosition)
        {
            return MotorMobile
            .Set_MotorSave(TouchObject.localPosition)
            .SetTarget_Vector(TargetPosition + Inertia_Position)
            .Set_MotorConstraint(LimitMobile)
            .Set_MotorConstraint_Shaft(Limit_X, Limit_Y)
            .Set_MotorConstraint_Limit(Limit_Size_MobileMin, Limit_Size_MobileMax)
            .Set_MotorSpeed(Mobile_SmoothSpeed)
            .Run_SmoothDamp(Time.fixedDeltaTime)
            .Constraint_Vector_Local()
            .Get_MotorSave()
            ;


        }

        public override float TouchMortor_Rotation(float TargetAngle)
        {
            return MotorRotation
            .Set_MotorSave(TouchObject.localEulerAngles.z)
            .SetTarget_Angle(TargetAngle + Inertia_Angle)							//电机目标设置
            .Set_MotorConstraint(LimitRotation)										//电机限制器激活
            .Set_MotorConstraint_Limit(Limit_Size_Rotation.x, Limit_Size_Rotation.y)//电机限制范围设置
            .Set_MotorSpeed(Rotation_SmoothSpeed)									//电机速度设置
            .Run_SmoothDampAngle()													//电机运行平滑移动
            .Constraint_Angle_Complete_Local()										//电机进行本地全面角度限制
            .Get_MotorSave()														//获取电机的角度
            ;
        }

        public override Vector3 TouchMortor_Zoom(Vector3 TargetScale)
        {
            return MotorZoom
            .Set_MotorSave(TouchObject.localScale)
            .SetTarget_Vector(TargetScale + Inertia_Scale)
            .Set_MotorConstraint(LimitZoom)
            .Set_MotorConstraint_Limit(Limit_Size_Zoom.x * SaveScale, Limit_Size_Zoom.y * SaveScale)
            .Set_MotorSpeed(Zoom_SmoothSpeed)
            .Run_SmoothDamp()
            .Constraint_Vector_Local()
            .Get_MotorSave()
            ;
        }

    }
}