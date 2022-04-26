using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.01.17 
 * 
 * 创建源于炮塔旋转功能，用于计算炮塔与目标不在统一角度时的炮塔旋转角度
 *
 * 2019.6.11 从 MyTool 转到 SDHK_Tool 
 *
 * 2020.02.28  SS_TriangleSolutions 合并到 SS_Mathf 
 * 
 * 功能：解三角的静态方法
 */

namespace SDHK_Tool.Static
{

    /// <summary>
    /// 解三角形                                              
    /// </summary>
    public static partial class SS_Mathf
    {


        //=[三角形求值]=================================================
        /// <summary>
        /// 解三角形 : 获得角A : 角A=180-(角B+角C) 同理
        /// </summary>
        /// <param name="B_Angle">角B</param>
        /// <param name="C_Angle">角C</param>
        /// <returns>return : 角A</returns>
        public static float Triangle_Get_A_Angle_AAA(float B_Angle, float C_Angle)//获取角
        {
            return 180 - (B_Angle + C_Angle);
        }

        /// <summary>
        /// 解三角形 : 获得外接圆直径
        /// </summary>
        /// <param name="Angle">角</param>
        /// <param name="Edge">角的对边</param>
        /// <returns>return : 外接圆直径</returns>
        public static float Triangle_GetDiameter(float Angle, float Edge)//获取外接圆直径
        {
            return Edge / Mathf.Sin(Angle * Mathf.Deg2Rad);//获取外接直径
        }

        /// <summary>
        /// 解三角形 : 获得角的对边
        /// </summary>
        /// <param name="Angle">角</param>
        /// <param name="diameter">外接圆直径</param>
        /// <returns>return : 角的对边</returns>
        public static float Triangle_GetEdge(float Angle, float diameter)//通过外接圆直径获得边
        {
            return Mathf.Sin(Angle * Mathf.Deg2Rad) * diameter;//通过直径获得边
        }

        /// <summary>
        /// 解三角形 : 通过外接圆直径获得对角
        /// </summary>
        /// <param name="Edge">角的对边</param>
        /// <param name="diameter">外接圆直径</param>
        /// <returns>return : 小于等于90度的对角</returns>
        public static float Triangle_GetAngle(float Edge, float diameter)//通过外接圆直径获得<=90的角
        {
            return Mathf.Asin(Edge / diameter) * Mathf.Rad2Deg;//获得<=90的角
        }


        /// <summary>
        /// 解三角形 : 获得角A
        /// </summary>
        /// <param name="A_Edge">角A的对边</param>
        /// <param name="B_Edge">角B的对边</param>
        /// <param name="C_Edge">角C的对边</param>
        /// <returns>return : 角A</returns>
        public static float Triangle_Get_A_Angle_EEE(float A_Edge, float B_Edge, float C_Edge)//从三边获得一角
        {
            return Mathf.Acos((B_Edge * B_Edge + C_Edge * C_Edge - A_Edge * A_Edge) / (2 * (B_Edge * C_Edge))) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// 解三角形 : 获得夹角A的对边
        /// </summary>
        /// <param name="A_Angle">夹角A</param>
        /// <param name="B_Edge">角B的对边</param>
        /// <param name="C_Edge">角C的对边</param>
        /// <returns>return : 夹角A的对边</returns>
        public static float Triangle_Get_A_Edge_EAE(float A_Angle, float B_Edge, float C_Edge)//夹角获取对边
        {
            
            return Mathf.Sqrt(B_Edge * B_Edge + C_Edge * C_Edge - 2 * B_Edge * C_Edge * Mathf.Cos(A_Angle * Mathf.Deg2Rad));
        }
    }


}