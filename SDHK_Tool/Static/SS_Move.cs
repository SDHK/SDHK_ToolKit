using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.02.20
 * 
 * 功能：用于方便向量移动的类
 */

namespace SDHK_Tool.Static
{
	/// <summary>
	/// 移动集合
	/// </summary>
    public static class SS_Move
    {

		/// <summary>
		/// 一维匀速运动
		/// </summary>
		/// <param name="CurrentVector">当前向量</param>
		/// <param name="TargetVector">目标向量</param>
		/// <param name="MoveSpeed">移动速度</param>
		/// <returns>移动后的向量</returns>
		public static float Vector1_MoveTowards(float CurrentVector,float TargetVector,float MoveSpeed)
		{
			return Mathf.MoveTowards(CurrentVector, TargetVector, MoveSpeed);
		}

		/// <summary>
		/// 一维差值运动
		/// </summary>
		/// <param name="CurrentVector">当前向量</param>
		/// <param name="TargetVector">目标向量</param>
		/// <param name="MoveSpeed">移动速度</param>
		/// <returns>移动后的向量</returns>
		public static float Vector1_Lerp(float CurrentVector,float TargetVector,float MoveSpeed)
		{
			return Mathf.Lerp(CurrentVector, TargetVector, MoveSpeed);
		}

		/// <summary>
		/// 一维平滑运动
		/// </summary>
		/// <param name="CurrentVector">当前向量</param>
		/// <param name="TargetVector">目标向量</param>
		/// <param name="MotorVelocity">每帧速度</param>
		/// <param name="MoveTime">到达目标的近似时间</param>
		/// <returns>移动后的向量</returns>
		public static float Vector1_SmoothDamp(float CurrentVector,float TargetVector,ref float MotorVelocity,float MoveTime)
		{
			return Mathf.SmoothDamp(CurrentVector, TargetVector,ref MotorVelocity, MoveTime);
		}



		/// <summary>
		/// 二维匀速运动
		/// </summary>
		/// <param name="CurrentVector">当前向量</param>
		/// <param name="TargetVector">目标向量</param>
		/// <param name="MoveSpeed">移动速度</param>
		/// <returns>移动后的向量</returns>
		public static Vector2 Vector2_MoveTowards(Vector2 CurrentVector,Vector2 TargetVector,float MoveSpeed)
		{
			return Vector2.MoveTowards(CurrentVector, TargetVector, MoveSpeed);
		}

		/// <summary>
		/// 二维差值运动
		/// </summary>
		/// <param name="CurrentVector">当前向量</param>
		/// <param name="TargetVector">目标向量</param>
		/// <param name="MoveSpeed">移动速度</param>
		/// <returns>移动后的向量</returns>
		public static Vector2 Vector2_Lerp(Vector2 CurrentVector,Vector2 TargetVector,float MoveSpeed)
		{
			return Vector2.Lerp(CurrentVector, TargetVector, MoveSpeed);
		}

		/// <summary>
		/// 二维平滑运动
		/// </summary>
		/// <param name="CurrentVector">当前向量</param>
		/// <param name="TargetVector">目标向量</param>
		/// <param name="MotorVelocity">每帧速度</param>
		/// <param name="MoveTime">到达目标的近似时间</param>
		/// <param name="Time_">时间系数</param>
		/// <param name="MaxSpeed">最大速度</param>
		/// <returns>移动后的向量</returns>
		public static Vector2 Vector2_SmoothDamp(Vector2 CurrentVector,Vector2 TargetVector,ref Vector2 MotorVelocity,float MoveTime,float Time_,float MaxSpeed = Mathf.Infinity )
		{
			return Vector2.SmoothDamp(CurrentVector, TargetVector,ref MotorVelocity,MoveTime,MaxSpeed,Time_ );
		}



		/// <summary>
		/// 三维匀速运动
		/// </summary>
		/// <param name="CurrentVector">当前向量</param>
		/// <param name="TargetVector">目标向量</param>
		/// <param name="MoveSpeed">移动速度</param>
		/// <returns>移动后的向量</returns>
		public static Vector3 Vector3_MoveTowards(Vector3 CurrentVector,Vector3 TargetVector,float MoveSpeed)
		{
			return Vector3.MoveTowards(CurrentVector, TargetVector, MoveSpeed);
		}

		/// <summary>
		/// 三维差值运动
		/// </summary>
		/// <param name="CurrentVector">当前向量</param>
		/// <param name="TargetVector">目标向量</param>
		/// <param name="MoveSpeed">移动速度</param>
		/// <returns>移动后的向量</returns>
		public static Vector3 Vector3_Lerp(Vector3 CurrentVector,Vector3 TargetVector,float MoveSpeed)
		{
			return Vector3.Lerp(CurrentVector, TargetVector, MoveSpeed);
		}

		/// <summary>
		/// 三维平滑运动
		/// </summary>
		/// <param name="CurrentVector">当前向量</param>
		/// <param name="TargetVector">目标向量</param>
		/// <param name="MotorVelocity">每帧速度</param>
		/// <param name="MoveTime">到达目标的近似时间</param>
		/// <param name="MaxSpeed">最大速度</param>
		/// <returns>移动后的向量</returns>
		public static Vector3 Vector3_SmoothDamp(Vector3 CurrentVector,Vector3 TargetVector,ref Vector3 MotorVelocity,float MoveTime,float MaxSpeed=Mathf.Infinity)
		{
			return Vector3.SmoothDamp(CurrentVector, TargetVector,ref MotorVelocity,MoveTime,MaxSpeed );
		}

		
      
    }
}
