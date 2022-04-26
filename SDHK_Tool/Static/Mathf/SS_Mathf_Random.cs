using System.Collections.Generic;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 *
 * 日期：2019.6.20
 * 
 * 2020.02.28 从 SS_Mathf文件中 拆分出来
 *
 * 功能：求随机数
 * 
 */


namespace SDHK_Tool.Static
{

    /// <summary>
    /// 随机数                                                                        
    /// </summary>
    public static partial class SS_Mathf
    {

        /// <summary>
        /// 随机数：整数
        /// </summary>
        /// <param name="Min">最小值</param>
        /// <param name="Max">最大值</param>
        /// <returns>随机浮点数</returns>
        public static int Random_Vector1(int Min, int Max)
        {
            return Random.Range(Min, Max);
        }

        /// <summary>
        /// 随机数：浮点数
        /// </summary>
        /// <param name="Min">最小值</param>
        /// <param name="Max">最大值</param>
        /// <returns>随机浮点数</returns>
        public static float Random_Vector1(float Min, float Max)
        {
            return Random.Range(Min, Max);
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

    }
}