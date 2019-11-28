using System;
using System.Collections;
using System.Collections.Generic;
using SDHK_Tool.Dynamic;
using UnityEngine;
using UnityEngine.Events;

public class SC_Motor_Angle : MonoBehaviour
{


    public Action<float> awt001;
    // [System.Serializable]
    public UnityEvent ActionRegistered;

    public float speed = 10;

    public SD_Motor_Angle motor_Angle;

    // Use this for initialization
    void Start()
    {
        motor_Angle = new SD_Motor_Angle();

        ActionRegistered.Invoke();

    }

    // Update is called once per frame
    void Update()
    {

        motor_Angle.SetTarget_AngleDelta(speed)
        .Run_LerpAngle(Time.deltaTime)
        .Get_MotorSave();


        awt001(motor_Angle.Get_MotorSave());





    }



    public void Run_z()
    {
        awt001 = (float a) => { transform.localEulerAngles = new Vector3(transform.localPosition.x, a, transform.localPosition.z); };
    }
}
