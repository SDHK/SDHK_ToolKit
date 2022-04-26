using System.Collections.Generic;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 *
 * 日期：2019.6.20
 * 
 * 功能：一些简单的计算
 * 
 */


namespace SDHK_Tool.Static
{

    /// <summary>
    /// 数学类 ：                                    
    /// </summary>
    public static partial class SS_Mathf
    {

        /// <summary>
        /// 循环的正整数                                        
        /// 数值会在 0 ~ MaxIndex 之间循环
        /// </summary>
        /// <param name="Index">正负整数</param>
        /// <param name="MaxIndex">循环最大值</param>
        /// <returns>整数</returns>
        public static int Loop_Int(int Index, int MaxIndex)
        {
            int remainder = Mathf.Abs(Index) % MaxIndex;
            return (Index >= 0) ? Index % MaxIndex : (remainder == 0) ? 0 : MaxIndex - remainder;
            // return (index >= 0) ? index % MaxIndex : MaxIndex - 1 - Mathf.Abs(index + 1) % MaxIndex;
            //不知道怎么算出来的
        }

        /// <summary>
        /// 求一维列表的下标在二维列表中的下标（设定宽无限）                      
        /// 用于动态列表                                  
        /// </summary>
        /// <param name="Index">一维下标(+-都可)</param>
        /// <param name="y">二维列表固定高</param>
        /// <returns>二维中的下标</returns>
        public static Vector2 List1D_To_List2D(int Index, int y)
        {
            Vector2 vector;
            int remainder = Mathf.Abs(Index) % y;

            if (Index >= 0)
            {
                vector.y = remainder;
                vector.x = (Index - vector.y) / y;
            }
            else
            {
                vector.y = (remainder == 0) ? 0 : y - remainder;
                vector.x = (Index - vector.y) / y;
            }
            return vector;
        }
        /// <summary>
        /// 求一维列表的下标在二维列表中的下标：蛇形（设定宽无限）                      
        /// 用于动态列表                                  
        /// </summary>
        /// <param name="Index">一维下标(+-都可)</param>
        /// <param name="y">二维列表固定高</param>
        /// <returns>二维中的下标</returns>
        public static Vector2 List1D_To_List2D_S(int Index, int y)
        {
            Vector2 vector;
            int remainder = Mathf.Abs(Index) % y;

            if (Index >= 0)
            {
                vector.y = remainder;
                vector.x = (Index - vector.y) / y;
            }
            else
            {
                vector.y = (remainder == 0) ? 0 : y - remainder;
                vector.x = (Index - vector.y) / y;
            }

            if (vector.x % 2 != 0) vector.y = y - 1 - vector.y;

            return vector;
        }

        /// <summary>
        /// 矩形计算器：将一个矩形按照比例缩放到合适的尺寸
        /// </summary>
        /// <param name="Origin">要缩放的矩形长宽</param>
        /// <param name="Reference">长宽最大范围</param>
        /// <returns>最大长宽的合适尺寸</returns>
        public static Vector2 Rect_ProperFormat_Max(Vector2 Origin, Vector2 Reference)
        {
            float AspectRatio = Origin.x / Origin.y;//获取长宽比

            if (Reference.x / Reference.y <= AspectRatio)
            {
                Reference.y = Reference.x / AspectRatio;
            }
            else
            {
                Reference.x = Reference.y * AspectRatio;
            }
            return Reference;
        }

        /// <summary>
        /// 矩形计算器：将一个矩形按照比例缩放到合适的尺寸
        /// </summary>
        /// <param name="Origin">要缩放的矩形长宽</param>
        /// <param name="Reference">长宽最小范围</param>
        /// <returns>最小长宽的合适尺寸</returns>
        public static Vector2 Rect_ProperFormat_Min(Vector2 Origin, Vector2 Reference)
        {
            float AspectRatio = Origin.x / Origin.y;//获取长宽比

            if (Reference.x / Reference.y <= AspectRatio)
            {
                Reference.x = Reference.y * AspectRatio;
            }
            else
            {
                Reference.y = Reference.x / AspectRatio;//对
            }
            return Reference;
        }


        /// <summary>
        /// 区间判断：判断数值是否在一个区间内
        /// </summary>
        /// <param name="Value">数值</param>
        /// <param name="Min">最小值</param>
        /// <param name="Max">最大值</param>
        /// <returns>布尔值</returns>
        public static bool If_IntervalValue(float Value, float Min, float Max)
        {
            return (Value >= Min && Value <= Max);
        }

        /// <summary>
        /// 区间判断：判断角度是否在一个区间内
        /// </summary>
        /// <param name="Angle">角度</param>
        /// <param name="Min">最小值</param>
        /// <param name="Max">最大值</param>
        /// <returns>布尔值</returns>
        public static bool If_IntervalAngle(float Angle, float Min, float Max)
        {
            return Mathf.DeltaAngle(Angle, Min) >= 0 && Mathf.DeltaAngle(Angle, Max) <= 0;
        }

        /// <summary>
        /// 整数异或运算换位
        /// </summary>
        /// <param name="a">交换的值</param>
        /// <param name="b">交换的值</param>
        public static void Exchange_Int(ref int a, ref int b)
        {
            b ^= (a ^= b);
            a ^= b;
        }



        /// <summary>
        /// 贝塞尔曲线
        /// </summary>
        /// <param name="TimeRatio">点的位置（时间比例0~1）</param>
        /// <param name="point">曲线拉伸坐标点</param>
        /// <returns> 点的位置 </returns>
        public static Vector3 Curve_Bezier(float TimeRatio, List<Vector3> point)
        {
            while (point.Count > 1)
            {
                List<Vector3> newp = new List<Vector3>();
                for (int i = 0; i < point.Count - 1; i++)
                {
                    Vector3 p0p1 = Vector3.Lerp(point[i], point[i + 1], TimeRatio);
                    newp.Add(p0p1);
                }
                point = newp;
            }
            return point[0];
        }

        //   if (point.Count % 3 == 0) Debug.DrawLine(point[i], point[i + 1], Color.red);
        //   if (point.Count % 3 == 1) Debug.DrawLine(point[i], point[i + 1], Color.green);
        //   if (point.Count % 3 == 2) Debug.DrawLine(point[i], point[i + 1], Color.yellow);





        /// <summary>
        /// 计算多点中心
        /// </summary>
        /// <param name="vectors">多点位置集合</param>
        /// <returns>中心点</returns>
        public static Vector3 VectorsCenter(Vector3[] vectors)
        {
            Vector3 Center = new Vector3();
            for (int i = 0; i < vectors.Length; i++) Center += vectors[i];
            return Center / vectors.Length;
        }

        /// <summary>
        /// 计算多点中心
        /// </summary>
        /// <param name="vectors">多点位置集合</param>
        /// <returns>中心点</returns>
        public static Vector2 VectorsCenter(Vector2[] vectors)
        {
            Vector2 Center = new Vector2();
            for (int i = 0; i < vectors.Length; i++) Center += vectors[i];
            return Center / vectors.Length;
        }



        /// <summary>
        ///计算多点聚集中心点
        /// </summary>
        /// <param name="Radius">聚集半径 </param>
        /// <param name="Points">多点位置集合</param>
        /// <param name="Depth">计算深度 [默认为0，完全计算]</param>
        /// <returns>中心点集合</returns>
        public static List<Vector3> VectorsCenter(float Radius, List<Vector3> Points, int Depth = 0)
        {
            List<Vector3> centerPoints = new List<Vector3>();//创建中心点集合
            List<List<Vector3>> CenterLinks = new List<List<Vector3>>();
            bool bit;//捕捉标记
            int Index = 0;//层级深度
            int LastPointsCount = 0;//上一层的计算量

            do
            {
                centerPoints = new List<Vector3>();//刷新中心点
                CenterLinks.Clear();//刷新中心点链接

                for (int i = 0; i < Points.Count; i++)//遍历点
                {
                    bit = false;//捕捉标记重置
                    for (int i1 = 0; i1 < centerPoints.Count; i1++)//遍历中心点
                    {
                        if ((centerPoints[i1] - Points[i]).magnitude < Radius)//相差距离小于半径
                        {
                            CenterLinks[i1].Add(Points[i]);//在 中心点 半径范围内 则被 中心点 捕捉
                            centerPoints[i1] = SS_Mathf.VectorsCenter(CenterLinks[i1].ToArray());//计算出新中心点坐标
                            bit = true;//标记 被 中心点捕捉
                            break;//一个点不能支撑两个中心点,跳出循环
                        }
                    }
                    if (!bit)//没有被任何中心点捕捉
                    {
                        centerPoints.Add(Points[i]);//新建中心点为点坐标
                        CenterLinks.Add(new List<Vector3>());//被新建的中心点捕捉    
                        CenterLinks[CenterLinks.Count - 1].Add(Points[i]);
                    }
                }
                LastPointsCount = Points.Count;
                Points = centerPoints;

                Index++;
            } while (!(Index == Depth || LastPointsCount == centerPoints.Count));//当 循环到指定深度 或 计算量相等 则退出循环
            // Debug.Log(Index-1);//打印层级

            return centerPoints;
        }






    }


}
