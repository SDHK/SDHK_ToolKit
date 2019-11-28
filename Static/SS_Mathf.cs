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
    /// 数学类
    /// </summary>
    public static class SS_Mathf
    {

        /// <summary>
        /// 求最近值:(注意间隔距离要整数)
        /// </summary>
        /// <param name="NowValue">数值</param>
        /// <param name="Distance">间隔距离</param>
        /// <returns>最近值</returns>
        public static float NearlyValue(float NowValue, float Distance)
        {
            float saveValue = 0;
            float RedundantValue = 0;

            RedundantValue = NowValue % Distance;//取余数
            saveValue = NowValue - RedundantValue;//去余数

            saveValue += Mathf.RoundToInt(RedundantValue / Distance) * Distance;// 四舍五入到整数（0/1）*间隔

            return saveValue;
        }

        /// <summary>
        /// 求最近值:(注意间隔距离要整数)
        /// </summary>
        /// <param name="NowValue">数值</param>
        /// <param name="Distance">间隔距离</param>
        /// <param name="Mode">true/false(顶值间隔/底值间隔)</param>
        /// <returns>最近值</returns>
        public static float NearlyValue(float NowValue, float Distance, bool Mode)
        {
            float saveValue = 0;
            float RedundantValue = 0;

            RedundantValue = NowValue % Distance;
            saveValue = NowValue - RedundantValue;

            if (Mode)
            {
                saveValue += (RedundantValue / Distance > 0) ? Distance : 0;// top顶值
            }
            return saveValue;
        }

        /// <summary>
        /// 求最近值
        /// </summary>
        /// <param name="NowValue">数值</param>
        /// <param name="Distances">数组</param>
        /// <returns>最近值</returns>
        public static float NearlyValue(float NowValue, List<float> Distances)
        {
            if (Distances.Count > 0)
            {
                int index = 0;
                float LateDistance = Mathf.Abs(NowValue - Distances[0]);

                for (int i = 0; i < Distances.Count; i++)
                {
                    float newDistance = Mathf.Abs(NowValue - Distances[i]);
                    if (newDistance <= LateDistance)
                    {
                        index = i;
                        LateDistance = newDistance;
                    }
                }
                return Distances[index];
            }
            else
            {
                return NowValue;
            }
        }

        /// <summary>
        /// 求最近值:需要从小到大的排列顺序
        /// </summary>
        /// <param name="NowValue">数值</param>
        /// <param name="Distances">数组</param>
        /// <param name="Mode">true/false(顶值间隔/底值间隔)</param>
        /// <returns>最近值</returns>
        public static float NearlyValue(float NowValue, List<float> Distances, bool Mode)
        {
            if (Distances.Count > 0)
            {
                int MinIndex = 0;
                int MaxIndex = 0;

                for (int i = 1; i < Distances.Count; i++)
                {
                    if (Distances[i - 1] < NowValue && Distances[i] >= NowValue)
                    {
                        MinIndex = i - 1;
                        MaxIndex = i;
                        break;
                    }
                    else if (Distances[i - 1] < NowValue && Distances[i] < NowValue)
                    {
                        MinIndex = Distances.Count - 1;
                        MaxIndex = Distances.Count - 1;
                    }
                    else if (Distances[i - 1] > NowValue && Distances[i] > NowValue)
                    {
                        MinIndex = 0;
                        MaxIndex = 0;
                    }
                }
                return (Mode) ? Distances[MaxIndex] : Distances[MinIndex];
            }
            else
            {
                return NowValue;
            }
        }


        /// <summary>
        /// 求最近值的间隔数:(注意间隔距离要整数)
        /// </summary>
        /// <param name="NowValue">数值</param>
        /// <param name="Distance">间隔距离</param>
        /// <returns>间隔个数</returns>
        public static int NearlyNumber(float NowValue, float Distance)
        {
            return (int)(NearlyValue(NowValue, Distance) / Distance);
        }

        /// <summary>
        /// 求最近值的间隔数:(注意间隔距离要整数)
        /// </summary>
        /// <param name="NowValue">数值</param>
        /// <param name="Distance">间隔距离</param>
        /// <param name="Mode">true/false(顶值间隔/底值间隔)</param>
        /// <returns>间隔数</returns>
        public static int NearlyNumber(float NowValue, int Distance, bool Mode)
        {
            return (int)(NearlyValue(NowValue, Distance, Mode) / Distance);
        }


        /// <summary>
        /// 求最近值的下标
        /// </summary>
        /// <param name="NowValue">数值</param>
        /// <param name="Distances">数组</param>
        /// <returns>最近值下标</returns>
        public static int NearlyNumber(float NowValue, List<float> Distances)
        {
            if (Distances.Count > 0)
            {
                int index = 0;
                float LateDistance = Mathf.Abs(NowValue - Distances[0]);

                for (int i = 0; i < Distances.Count; i++)
                {
                    float newDistance = Mathf.Abs(NowValue - Distances[i]);
                    if (newDistance <= LateDistance)
                    {
                        index = i;
                        LateDistance = newDistance;
                    }
                }
                return index;
            }
            else
            {
                return -1;
            }
        }


        /// <summary>
        /// 求最近值的下标:需要从小到大的排列顺序
        /// </summary>
        /// <param name="NowValue">数值</param>
        /// <param name="Distances">数组</param>
        /// <param name="Mode">true/false(顶值间隔/底值间隔)</param>
        /// <returns>最近值下标</returns>
        public static int NearlyNumber(float NowValue, List<float> Distances, bool Mode)
        {
            if (Distances.Count > 0)
            {
                int MinIndex = 0;
                int MaxIndex = 0;

                for (int i = 1; i < Distances.Count; i++)
                {
                    if (Distances[i - 1] < NowValue && Distances[i] >= NowValue)
                    {
                        MinIndex = i - 1;
                        MaxIndex = i;
                        break;
                    }
                    else if (Distances[i - 1] < NowValue && Distances[i] < NowValue)
                    {
                        MinIndex = Distances.Count - 1;
                        MaxIndex = Distances.Count - 1;
                    }
                    else if (Distances[i - 1] > NowValue && Distances[i] > NowValue)
                    {
                        MinIndex = 0;
                        MaxIndex = 0;
                    }
                }
                return (Mode) ? MaxIndex : MinIndex;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 循环的整数
        /// </summary>
        /// <param name="Index">正负整数</param>
        /// <param name="MaxIndex">循环最大值</param>
        /// <returns>整数</returns>
        public static int Int_Loop(int Index, int MaxIndex)
        {
            int remainder = Mathf.Abs(Index) % MaxIndex;
            return (Index >= 0) ? Index % MaxIndex : (remainder == 0) ? 0 : MaxIndex - remainder;
            // return (index >= 0) ? index % MaxIndex : MaxIndex - 1 - Mathf.Abs(index + 1) % MaxIndex;//不知道怎么算出来的
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
        /// 矩形计算器：将一个矩形按照比例缩放到合适的尺寸
        /// </summary>
        /// <param name="Origin">要缩放的矩形长宽</param>
        /// <param name="Reference">长宽最大范围</param>
        /// <returns>最大长宽的合适尺寸</returns>
        public static Vector2 Rect_ProperFormat(Vector2 Origin, Vector2 Reference)
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
        /// 随机向量：二维向量
        /// </summary>
        /// <param name="Min">最小值</param>
        /// <param name="Max">最大值</param>
        /// <returns>随机二维向量</returns>
        public static Vector2 Random_Vector2(Vector2 Min, Vector2 Max)
        {
            Vector2 vector = new Vector2();
            vector.x = Random.Range(Min.x, Max.x);
            vector.y = Random.Range(Min.y, Max.y);
            return vector;
        }

        /// <summary>
        /// 随机向量：三维向量
        /// </summary>
        /// <param name="Min">最小值</param>
        /// <param name="Max">最大值</param>
        /// <returns>随机三维向量</returns>
        public static Vector3 Random_Vector3(Vector3 Min, Vector3 Max)
        {
            Vector3 vector = new Vector3();
            vector.x = Random.Range(Min.x, Max.x);
            vector.y = Random.Range(Min.y, Max.y);
            vector.z = Random.Range(Min.z, Max.z);
            return vector;
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
        /// 异或运算换位
        /// </summary>
        /// <param name="a">交换的值</param>
        /// <param name="b">交换的值</param>
        public static void Exchange_Int(ref int a, ref int b)
        {
            b ^= (a ^= b);
            a ^= b;
        }

    }
}