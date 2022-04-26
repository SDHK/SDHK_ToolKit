using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * 作者：闪电Y黑客
 *
 * 日期：2019.6.20 
 *
 * 创建源于某拖拽窗口项目，用于动态滚动列表的位置计算
 *
 * 2020.02.28 从 SS_Mathf文件中 拆分出来
 * 
 * 功能：求数值在一个数组中的最近值
 * 
 */


namespace SDHK_Tool.Static
{

    /// <summary>
    /// 最近值                                                                             
    /// </summary>
    public static partial class SS_Mathf
    {

        /// <summary>
        /// 求最近值:(注意间隔距离要整数)
        /// </summary>
        /// <param name="NowValue">数值</param>
        /// <param name="Distance">间隔距离</param>
        /// <returns>最近值</returns>
        public static float Recent_Value(float NowValue, float Distance)
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
        public static float Recent_Value(float NowValue, float Distance, bool Mode)
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
        public static float Recent_Value(float NowValue, List<float> Distances)
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
        public static float Recent_Value(float NowValue, List<float> Distances, bool Mode)
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
        public static int Recent_Number(float NowValue, float Distance)
        {
            return (int)(Recent_Value(NowValue, Distance) / Distance);
        }

        /// <summary>
        /// 求最近值的间隔数:(注意间隔距离要整数)
        /// </summary>
        /// <param name="NowValue">数值</param>
        /// <param name="Distance">间隔距离</param>
        /// <param name="Mode">true/false(顶值间隔/底值间隔)</param>
        /// <returns>间隔数</returns>
        public static int Recent_Number(float NowValue, int Distance, bool Mode)
        {
            return (int)(Recent_Value(NowValue, Distance, Mode) / Distance);
        }


        /// <summary>
        /// 求最近值的下标
        /// </summary>
        /// <param name="NowValue">数值</param>
        /// <param name="Distances">数组</param>
        /// <returns>最近值下标</returns>
        public static int Recent_Number(float NowValue, List<float> Distances)
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
        public static int Recent_Number(float NowValue, List<float> Distances, bool Mode)
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
        /// 求最近值:[3D]
        /// </summary>
        /// <param name="NowVector">当前坐标</param>
        /// <param name="vector3s">坐标集合</param>
        /// <returns>集合中最近的坐标</returns>
        public static Vector3 Recent_Vector(Vector3 NowVector, List<Vector3> vector3s)
        {
            if (vector3s.Count > 0)
            {
                int index = 0;
                float LateDistance = (NowVector - vector3s[0]).magnitude;

                for (int i = 0; i < vector3s.Count; i++)
                {
                    float newDistance = (NowVector - vector3s[i]).magnitude;
                    if (newDistance <= LateDistance)
                    {
                        index = i;
                        LateDistance = newDistance;
                    }
                }
                return vector3s[index];
            }
            else
            {
                return NowVector;
            }
        }

        /// <summary>
        /// 求最近值:[3D]
        /// </summary>
        /// <param name="NowVector">当前坐标</param>
        /// <param name="vector3s">坐标集合</param>
        /// <returns>集合中最近向量的下标</returns>
        public static int Recent_Vector<T>(Vector3 NowVector, List<T> vector3s, Func<int, Vector3> GetVector3)
        {
            if (vector3s.Count > 0)
            {
                int index = 0;
                float LateDistance = (NowVector - GetVector3(0)).magnitude;

                for (int i = 0; i < vector3s.Count; i++)
                {
                    float newDistance = (NowVector - GetVector3(i)).magnitude;
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

    }
}