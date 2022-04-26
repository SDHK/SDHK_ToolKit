using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDHK_Tool.Dynamic;
using SDHK_Tool.Component;
using SDHK_Tool.Static;

public class CameraTouch : MonoBehaviour
{

    public Transform Camera;

    public SD_Motor_EulerAngle motor_EulerAngle;

    public SC_TouchEvent_Down touch_Down;

    public Vector2 LastTarget;

    public Vector2 target;

    // Use this for initialization
    void Start()
    {
        motor_EulerAngle = new SD_Motor_EulerAngle();
        motor_EulerAngle.MotorConstraint = true;

        touch_Down.TouchOnDown = () =>
		{
			target = touch_Down.TouchPool[touch_Down.TouchIds[0]].position;//屏坐标
			LastTarget = target;

			motor_EulerAngle.SetTarget_Angle(motor_EulerAngle.Get_MotorSave());
		};


        touch_Down.TouchOnStay = () =>
        {
            target = touch_Down.TouchPool[touch_Down.TouchIds[0]].position;//屏坐标

            Vector3 Vector;
            Vector.x = -(target - LastTarget).y * 0.1f;//屏向量
            Vector.y = (target - LastTarget).x * 0.1f;//屏向量

            Vector.z = 0;

            Debug.Log(Vector);
            motor_EulerAngle.SetTarget_AngleDelta(Vector * 0.5f);

            LastTarget = target;
        };

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        motor_EulerAngle
        .Set_MotorSpeed(0.1f)
        .Set_MotorConstraint_Shaft(Y: false)
        .Set_MotorConstraint_Limit(new Vector3(-80, 0, 0), new Vector3(80, 0, 0))

        .Run_SmoothDampAngle()
        .Constraint_Angle_Local()
        ;

        Camera.localEulerAngles = motor_EulerAngle.Get_MotorSave();

    }
}
