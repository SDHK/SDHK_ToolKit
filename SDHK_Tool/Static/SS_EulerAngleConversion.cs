using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.6.11
 * 
 * 功能: 角度与向量的各种转换计算
 */

namespace SDHK_Tool.Static
{

    /// <summary>
    /// 欧拉角,角度转换计算
    /// </summary>
    public static class SS_EulerAngleConversion
    {

        /// <summary>
        /// 角度转换：正负无限 转换成 正负0~180 的角度
        /// </summary>
        /// <param name="Angle">要转换角度</param>
        /// <returns>return ： 转换成 正负0~180 的角度</returns>
        public static float Angle_PN_To_PN180(float Angle)
        {
            Angle = Angle_PN_To_P360(Angle);
            return Angle_P360_To_PN180(Angle);
        }

        /// <summary>
        /// 角度转换：正负无限 转换成 正0~360 的角度
        /// </summary>
        /// <param name="Angle">要转换角度</param>
        /// <returns>return ： 转换成 正0~360 的角度</returns>
        public static float Angle_PN_To_P360(float Angle)
        {
            return ((Angle %= 360) < 0) ? Angle + 360 : Angle;
        }

        /// <summary>
        /// 角度转换：正0~360 转换成 正负0~180 的角度
        /// </summary>
        /// <param name="Angle">要转换角度</param>
        /// <returns>return ： 转换成 正负0~180 的角度</returns>
        public static float Angle_P360_To_PN180(float Angle)
        {
            return (Angle >= 180) ? Angle - 360 : Angle;
        }

        /// <summary>
        /// 角度转换：正0~180 转换成 正负0~90 的角度
        /// </summary>
        /// <param name="Angle">要转换角度</param>
        /// <returns>return ： 转换成 正负0~180 的角度</returns>
        public static float Angle_PN180_To_PN90(float Angle)
        {
            Angle = Mathf.Abs(Angle);
            return (Angle >= 90) ? Angle - 180 : Angle;
        }


        /// <summary>
        /// 欧拉角转换：正负无限 转换成 正负0~180 的欧拉角
        /// </summary>
        /// <param name="EulerAngles">要转换的欧拉角</param>
        /// <returns>return ： 转换成 正负0~180 的欧拉角</returns>
        public static Vector3 EulerAngle_PN_To_PN180(Vector3 EulerAngles)
        {
            EulerAngles.x = Angle_PN_To_PN180(EulerAngles.x);
            EulerAngles.y = Angle_PN_To_PN180(EulerAngles.y);
            EulerAngles.z = Angle_PN_To_PN180(EulerAngles.z);
            return EulerAngles;
        }

        /// <summary>
        /// 欧拉角转换：正负无限 转换成 正0~360 的欧拉角
        /// </summary>
        /// <param name="EulerAngles">要转换的欧拉角</param>
        /// <returns>return ： 转换成 正0~360 的欧拉角</returns>
        public static Vector3 EulerAngle_PN_To_P360(Vector3 EulerAngles)
        {
            EulerAngles.x = Angle_PN_To_P360(EulerAngles.x);
            EulerAngles.y = Angle_PN_To_P360(EulerAngles.y);
            EulerAngles.z = Angle_PN_To_P360(EulerAngles.z);
            return EulerAngles;
        }

        /// <summary>
        /// 欧拉角转换：正0~360 转换成 正负0~180 的欧拉角
        /// </summary>
        /// <param name="EulerAngles">要转换的欧拉角</param>
        /// <returns>return ： 转换成 正负0~180 的欧拉角</returns>
        public static Vector3 EulerAngle_P360_To_PN180(Vector3 EulerAngles)
        {
            EulerAngles.x = Angle_P360_To_PN180(EulerAngles.x);
            EulerAngles.y = Angle_P360_To_PN180(EulerAngles.y);
            EulerAngles.z = Angle_P360_To_PN180(EulerAngles.z);
            return EulerAngles;
        }


        /// <summary>
        /// 二维向量转换为角度：向量不能为0
        /// </summary>
        /// <param name="vector2">要转换的向量</param>
        /// <returns>return ： 角度(360度)</returns>
        public static float Get_Angle_In_Vector2(Vector2 vector2)
        {
            return Quaternion.LookRotation(new Vector3(vector2.x, 0, vector2.y)).eulerAngles.y;  //返回最终角度
        }

        /// <summary>
        /// 三维向量转换为欧拉角：向量不能为0
        /// </summary>
        /// <param name="AngleVector3">指向向量</param>
        /// <returns>return : 欧拉角(360度)</returns>
        public static Vector3 Get_EulerAngle_In_Vector3(Vector3 AngleVector3)
        {
            return Quaternion.LookRotation(AngleVector3).eulerAngles;
        }


        /// <summary>
        /// 角度转换为二维向量（x为横，Y为竖）
        /// </summary>
        /// <param name="Angle_Z">要转换的角度</param>
        /// <returns>return ： 转换的二维向量</returns>
        public static Vector2 Get_Vector2_In_Angle(float Angle_Z)
        {

            Vector3 vector = new Vector3(0, 1, 0);
            vector = Quaternion.AngleAxis(Angle_Z, -Vector3.forward) * vector; //算出旋转后的向量
            return vector;
        }

        /// <summary>
        /// 欧拉角转换为三维向量
        /// </summary>
        /// <param name="EulerAngles">欧拉角</param>
        /// <returns>return : 转换的三维向量</returns>
        public static Vector3 Get_Vector3_In_EulerAngle(Vector3 EulerAngles)
        {
            Vector3 vector = new Vector3(0, 0, 1);
            vector = Quaternion.AngleAxis(EulerAngles.x, -Vector3.left) * vector; //算出旋转后的向量
            vector = Quaternion.AngleAxis(EulerAngles.y, Vector3.up) * vector; //算出旋转后的向量

            return vector;
        }


        /// <summary>
        /// 获取点绕某轴旋转x度后的位置（三维）
        /// </summary>
        /// <param name="position">要旋转的点</param>
        /// <param name="center">旋转的中心点</param>
        /// <param name="axis">旋转的轴</param>
        /// <param name="angle">要旋转角度（顺时针为正）</param>
        /// <returns>旋转后的位置</returns>
        public static Vector3 Get_Vector3_RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)
        {
            Vector3 point = Quaternion.AngleAxis(angle, axis) * (position - center); //算出旋转后的向量
            Vector3 resultVec3 = center + point;        //加上旋转中心位置得到旋转后的位置
            return resultVec3;
        }

        /// <summary>
        /// 获取点绕某轴旋转x度后的位置（二维）
        /// </summary>
        /// <param name="position">要旋转的点</param>
        /// <param name="center">旋转的中心点</param>
        /// <param name="axis">旋转的轴</param>
        /// <param name="angle">一次旋转多少角度（顺时针为正）</param>
        /// <returns>旋转后的位置</returns>
        public static Vector2 Get_Vector2_RotateRound(Vector2 position, Vector2 center, int axis, float angle)
        {
            Vector3 point = Quaternion.AngleAxis(angle, Vector3.forward * axis) * (new Vector3(position.x, 0, position.y) - new Vector3(center.x, 0, center.y)); //算出旋转后的向量
            Vector2 resultVec3 = center + new Vector2(point.x, point.z);        //加上旋转中心位置得到旋转后的位置
            return resultVec3;
        }




        /// <summary>
        /// 获得两个二维向量的角度差，顺时针为正
        /// </summary>
        /// <param name="firstVector2">第一个向量</param>
        /// <param name="secondVector2">第二个向量</param>
        /// <param name="Direction">轴方向：true 为轴向自己（从前往后看）2d一般为false</param>
        /// <returns>return : 返回正负180度角</returns>
        public static float Get_Angle_In_Vector2Deviation(Vector2 firstVector2, Vector2 secondVector2, bool Direction)
        {
            float direction = Vector3.Cross(firstVector2, secondVector2).z;   //根据两个向量判断左右方向

            float angle = Vector3.Angle(firstVector2, secondVector2);         //根据两个向量算出角度

            if ((direction < 0 && !Direction) || (direction > 0 && Direction))
            {
                angle = -angle;
            }
            return angle;
        }

        /// <summary>
        /// 获得两个三维向量的角度差，顺时针为正
        /// </summary>
        /// <param name="firstVector3">第一个向量</param>
        /// <param name="secondVector3">第二个向量</param>
        /// <returns>第一位为左右角度，第二位为俯仰角，第三位为实际偏差角(+-180度)</returns>
        public static Vector3 GetAngle_In_Vector3Deviation(Vector3 firstVector3, Vector3 secondVector3)
        {
            Vector3 firstEulerAngles = Get_EulerAngle_In_Vector3(firstVector3);
            Vector3 secondEulerAngles = Get_EulerAngle_In_Vector3(secondVector3);

            float Angle_Y = Mathf.DeltaAngle(firstEulerAngles.y, secondEulerAngles.y);//获得+-180差值

            float Angle_X = Mathf.DeltaAngle(firstEulerAngles.x, secondEulerAngles.x);//获得+-180差值

            float Angle = Vector3.Angle(firstVector3, secondVector3);

            return new Vector3(Angle_Y, Angle_X, Angle);
        }

        /// <summary>
        /// 获得目标位置位于Transform的方位角度
        /// </summary>
        /// <param name="isTransform">Transform</param>
        /// <param name="TargetVector3">目标点世界坐标位置</param>
        /// <returns>return : 方位角度(+-180度) </returns>
        public static Vector3 Get_Angle_In_Transform(Transform isTransform, Vector3 TargetVector3)
        {
            return EulerAngle_P360_To_PN180(Get_EulerAngle_In_Vector3(isTransform.InverseTransformPoint(TargetVector3)));
        }

    }
}
