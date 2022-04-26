using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.6.11
 * 
 * 功能：解三角的静态方法
 */

namespace SDHK_Tool.Static
{

    /// <summary>
    /// 用于unity3d解三角形
    /// </summary>
    public static class SS_TriangleSolutions
    {


        //=[三角形求值]=================================================
        /// <summary>
        /// 提供 : 两个角,获得第三角 : 角A=180-(角B+角C) 同理
        /// </summary>
        /// <param name="B_Angle">角B</param>
        /// <param name="C_Angle">角C</param>
        /// <returns>return : 角A</returns>
        public static float Get_A_Angle_AAA(float B_Angle, float C_Angle)//获取角
        {
            return 180 - (B_Angle + C_Angle);
        }
        
        /// <summary>
        /// 提供 : 角和对边 , 获得外接圆直径
        /// </summary>
        /// <param name="Angle">角</param>
        /// <param name="Edge">角的对边</param>
        /// <returns>return : 外接圆直径</returns>
        public static float GetDiameter_Edge_Angle(float Angle, float Edge)//获取外接圆直径
        {
            return Edge / Mathf.Sin(Angle * Mathf.Deg2Rad);//获取外接直径
        }
        
        /// <summary>
        /// 提供 : 角和外接圆直径 , 获得角的对边
        /// </summary>
        /// <param name="Angle">角</param>
        /// <param name="diameter">外接圆直径</param>
        /// <returns>return : 角的对边</returns>
        public static float GetEdge_Angle_Diameter(float Angle, float diameter)//通过外接圆直径获得边
        {
            return Mathf.Sin(Angle * Mathf.Deg2Rad) * diameter;//通过直径获得边
        }
        
        /// <summary>
        /// 提供 : 角的对边和外接圆直径 , 获得<=90的角
        /// </summary>
        /// <param name="Edge">角的对边</param>
        /// <param name="diameter">外接圆直径</param>
        /// <returns>return : <=90的角</returns>
        public static float GetAngle_Edge_Diameter(float Edge, float diameter)//通过外接圆直径获得<=90的角
        {
            return Mathf.Asin(Edge / diameter) * Mathf.Rad2Deg;//获得<=90的角
        }


        /// <summary>
        /// 提供 : 三个边 , 获得一个角
        /// </summary>
        /// <param name="A_Edge">角A的对边</param>
        /// <param name="B_Edge">角B的对边</param>
        /// <param name="C_Edge">角C的对边</param>
        /// <returns>return : 角A</returns>
        public static float Get_A_Angle_EEE(float A_Edge, float B_Edge, float C_Edge)//从三边获得一角
        {
            return Mathf.Acos((B_Edge * B_Edge + C_Edge * C_Edge - A_Edge * A_Edge) / (2 * (B_Edge * C_Edge))) * Mathf.Rad2Deg;
        }
        
        /// <summary>
        /// 提供 : 两个边和一个夹角 , 获得夹角的对边
        /// </summary>
        /// <param name="A_Angle">夹角A</param>
        /// <param name="B_Edge">角B的对边</param>
        /// <param name="C_Edge">角C的对边</param>
        /// <returns>return : 角A的对边</returns>
        public static float Get_A_Edge_EAE(float A_Angle, float B_Edge, float C_Edge)//夹角获取对边
        {
            return Mathf.Sqrt(B_Edge * B_Edge + C_Edge * C_Edge - 2 * B_Edge * C_Edge * Mathf.Cos(A_Angle * Mathf.Deg2Rad));
        }
    }


}