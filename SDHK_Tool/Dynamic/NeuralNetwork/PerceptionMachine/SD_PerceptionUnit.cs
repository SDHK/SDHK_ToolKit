using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.12.18
 * 
 * 功能：神经网络感知机单元件
 *
 * 公式：
 *
 *  估算结果 = ( for { 临时结果 += 权重i * 输入i }  + 偏置值 > 阈值 ) ？ 1:0 ;
 *
 *  for { 权重i = 权重i + 学习速率 *（ 预期结果 - 估算结果 ）* 输入i };
 *
 *  偏置 = 偏置 + 学习速率 *（ 预期结果 - 估算结果 ）;
 *
 */



namespace SDHK_Tool.Dynamic
{
    /// <summary>
    /// 神经网络:感知机单元件
    /// </summary>
    public class SD_PerceptionUnit
    {

        /// <summary>
        /// 学习速率
        /// </summary>
        public float SpeedRate = 0.1f;

        /// <summary>
        /// 判断阈值
        /// </summary>
        public float Threshold = 0.5f;//阈值

        private List<float> Inputs;//输入

        private float Expected_Results;//预期结果

        private float ThresholdResults = 0;//阈值结果

        private float Estimate_Results;//估算结果

        private List<float> weight;//权重

        private float Bias;//偏置值



        /// <summary>
        /// 设置：值集合
        /// </summary>
        /// <param name="inputs">值集合</param>
        /// <returns>单元件</returns>
        public SD_PerceptionUnit Set_Inputs(List<float> inputs)
        {
            Inputs = inputs;
            weight = new List<float>(inputs.Count);

            return this;
        }

        /// <summary>
        /// 设置：预期结果
        /// </summary>
        /// <param name="expected_Results">预期结果</param>
        /// <returns>单元件</returns>
        public SD_PerceptionUnit Set_Output(float expected_Results)
        {
            Expected_Results = expected_Results;

            return this;
        }

        /// <summary>
        /// 获取：估算结果
        /// </summary>
        /// <returns>估算结果</returns>
		public float Get_Output()
        {
            return Estimate_Results;
        }

        /// <summary>
        /// 感知机解析
        /// </summary>
        /// <returns>单元件</returns>
        public SD_PerceptionUnit Parsing()//解析
        {
            ThresholdResults = Bias;

            for (int i = 0; i < Inputs.Count; i++)
            {
                ThresholdResults += Inputs[i] * weight[i];
            }

            Estimate_Results = (ThresholdResults > Threshold) ? 1 : 0;

            return this;
        }

        /// <summary>
        /// 感知机训练
        /// </summary>
        /// <returns>单元件</returns>
        public SD_PerceptionUnit Training()//训练
        {
            Parsing();

            for (int i = 0; i < weight.Count; i++)
            {
                weight[i] += SpeedRate * (Expected_Results - Estimate_Results) * Inputs[i];
            }
            Bias += SpeedRate * (Expected_Results - Estimate_Results);
            return this;
        }

    }
}