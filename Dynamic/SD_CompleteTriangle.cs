
/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.6.11
 * 
 * 功能：全解三角形
 */

using SDHK_Tool.Static;

namespace SDHK_Tool.Dynamic
{

    /// <summary>
    /// 全解三角形
    /// </summary>
    public class SD_CompleteTriangle
    {

        private float A_Angle = 0;//角
        private float B_Angle = 0;
        private float C_Angle = 0;
        private float A_Edge = 0;//边
        private float B_Edge = 0;
        private float C_Edge = 0;
        private float diameter;//外接圆直径
        //=================================================
        /// <summary>
        /// 返回三角形的角A  [正数]
        /// </summary>
        public float GetA_Angle { get { return A_Angle; } }
        /// <summary>
        /// 返回三角形的角B  [正数]
        /// </summary>
        public float GetB_Angle { get { return B_Angle; } }
        /// <summary>
        /// 返回三角形的角C  [正数]
        /// </summary>
        public float GetC_Angle { get { return C_Angle; } }

        /// <summary>
        /// 返回三角形的角A对边
        /// </summary>
        public float GetA_Edge { get { return A_Edge; } }
        /// <summary>
        /// 返回三角形的角B对边
        /// </summary>
        public float GetB_Edge { get { return B_Edge; } }
        /// <summary>
        /// 返回三角形的角C对边
        /// </summary>
        public float GetC_Edge { get { return C_Edge; } }
        /// <summary>
        /// 返回三角形的外接圆直径
        /// </summary>
        public float Get_diameter { get { return diameter; } }

        //=[三角形全解]=================================================
        /// <summary>
        /// 边边边 :
        /// 提供 : 三角形的三个边 , 全解三角形
        /// </summary>
        /// <param name="A_Edge">角A的对边</param>
        /// <param name="B_Edge">角B的对边</param>
        /// <param name="C_Edge">角C的对边</param>
        /// <returns>SD_CompleteTriangle</returns>
        public SD_CompleteTriangle SolutionsTriangle_EEE(float A_Edge, float B_Edge, float C_Edge)//只有3边解三角，钝角锐角都可解
        {

            A_Angle = SS_TriangleSolutions.Get_A_Angle_EEE(A_Edge, B_Edge, C_Edge);
            B_Angle = SS_TriangleSolutions.Get_A_Angle_EEE(B_Edge, A_Edge, C_Edge);
            C_Angle = SS_TriangleSolutions.Get_A_Angle_EEE(C_Edge, A_Edge, B_Edge);

            return this;
        }
        /// <summary>
        /// 角边边 :
        /// 提供 : 一个角和两个边 , 全解三角形
        /// </summary>
        /// <param name="A_Angle">角A</param>
        /// <param name="A_Edge">角A的对边</param>
        /// <param name="B_Edge">角B的对边</param>
        /// <param name="B_ObtuseAngle">角B优先解为钝角,一般为false</param>
        /// <returns>SD_CompleteTriangle</returns>
        public SD_CompleteTriangle SolutionsTriangle_AEE(float A_Angle, float A_Edge, float B_Edge, bool B_ObtuseAngle)//1角2边解三角，钝角锐角都可解，B_ObtuseAngle钝角优先解
        {

            diameter = SS_TriangleSolutions.GetDiameter_Edge_Angle(A_Angle, A_Edge);

            B_Angle = SS_TriangleSolutions.GetAngle_Edge_Diameter(B_Edge, diameter);//获得<=90的角
            C_Angle = 90 - SS_TriangleSolutions.Get_A_Angle_AAA(90 - A_Angle, B_Angle);//绕一圈获得c角
            C_Edge = SS_TriangleSolutions.GetEdge_Angle_Diameter(C_Angle, diameter);//通过角获得边
            if (A_Angle <= 90 && B_ObtuseAngle)//b为钝角优先解
            {
                B_Angle = SS_TriangleSolutions.Get_A_Angle_AAA(A_Angle, C_Angle);//角b为钝角重计算
            }
            else
            {
                C_Angle = SS_TriangleSolutions.Get_A_Angle_AAA(A_Angle, B_Angle);//c为钝角优先解
                C_Edge = SS_TriangleSolutions.GetEdge_Angle_Diameter(C_Angle, diameter);//通过角获得边
            }
            return this;
        }
        /// <summary>
        /// 边角边 :
        /// 提供 : 两个边和一个夹角 , 全解三角形
        /// </summary>
        /// <param name="A_Edge">角A的对边</param>
        /// <param name="B_Angle">夹角B</param>
        /// <param name="C_Edge">角C的对边</param>
        /// <returns>SD_CompleteTriangle</returns>
        public SD_CompleteTriangle SolutionsTriangle_EAE(float A_Edge, float B_Angle, float C_Edge)//两边夹一角解三角，钝角锐角都可解
        {

            B_Edge = SS_TriangleSolutions.Get_A_Edge_EAE(B_Angle, A_Edge, C_Edge);
            A_Angle = SS_TriangleSolutions.Get_A_Angle_EEE(A_Edge, C_Edge, B_Edge);
            C_Angle = SS_TriangleSolutions.Get_A_Angle_EEE(C_Edge, A_Edge, B_Edge);

            return this;
        }
        /// <summary>
        /// 角边角 :
        /// 提供 : 两个角夹和一个边 , 全解三角形
        /// </summary>
        /// <param name="A_Angle">角A</param>
        /// <param name="B_Edge">角B的对边</param>
        /// <param name="C_Angle">角C</param>
        /// <returns>SD_CompleteTriangle</returns>
        public SD_CompleteTriangle SolutionsTriangle_AEA(float A_Angle, float B_Edge, float C_Angle)//两角夹一边解三角,钝角锐角都可解
        {

            B_Angle = SS_TriangleSolutions.Get_A_Angle_AAA(A_Angle, C_Angle);//暂时求出不定角
            diameter = SS_TriangleSolutions.GetDiameter_Edge_Angle(B_Angle, B_Edge);//获取外接直径
            A_Edge = SS_TriangleSolutions.GetEdge_Angle_Diameter(A_Angle, diameter);//通过角获得边
            C_Edge = SS_TriangleSolutions.GetEdge_Angle_Diameter(C_Angle, diameter);//通过角获得边

            return this;
        }

    }


}

