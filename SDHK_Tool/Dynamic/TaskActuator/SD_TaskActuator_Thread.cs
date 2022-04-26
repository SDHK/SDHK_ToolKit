using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.2.6
 * 
 * 功能：SD_TaskActuator 的分支部分：委托线程功能添加
 *
 */

namespace SDHK_Tool.Dynamic {
	
	/// <summary>
	/// 线程任务                            
	/// </summary>
	public partial class SD_TaskActuator {

		/// <summary>
		/// 任务储存器：[委托线程]
		/// </summary>
		private List<SD_Thread> Task_Threads = new List<SD_Thread> ();

		/// <summary>
		/// 任务解析器：[委托线程]                                  
		/// 注：任务执行的具体流程方法                                  
		/// </summary>
		/// <param name="TaskPath">任务地址</param>
		private void Parser_Thread (int TaskPath) 
		{
			//通过 任务地址 来获取 任务 执行启动线程。
			Task_Threads[TaskPath].Run();
			//指令指针+1，跳转执行下一条指令。
			this.TA_Pointer++;
		}
		
		/// <summary>
		/// 任务：[委托线程]                                  
		/// 注：执行委托线程任务。无法获取线程的句柄，建议别用。                                  
		/// </summary>
		/// <param name="Event">委托事件</param>
		/// <returns>任务执行器</returns>
		public SD_TaskActuator Thread (Action<SD_Thread> Event) 
		{
			//指令添加：线程事件解析器地址 ，线程事件任务地址
			Instruction_Add(Parser_Thread,Task_Threads.Count);
			Task_Threads.Add (new SD_Thread(Event));//线程写入任务储存器

			return this;
		}

		/// <summary>
		/// 任务执行器:[全部线程关闭]                                  
		/// 注：如果线程有死循环任务，则需要在程序结束时释放线程                                  
		/// </summary>
		/// <returns>任务执行器</returns>
		public SD_TaskActuator End_Threads()
		{
			foreach (var Task_Thread in Task_Threads)Task_Thread.End();
			return this;
		}
	}

}