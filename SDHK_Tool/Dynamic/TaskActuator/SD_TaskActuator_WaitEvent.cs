using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using SDHK_Tool.Static;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.2.8
 * 
 * 功能：SD_TaskActuator 的分支部分：等待事件功能添加
 *
 */

namespace SDHK_Tool.Dynamic
{

    /// <summary>
    /// 事件阻塞任务                           
    /// </summary>
    public partial class SD_TaskActuator
    {

        /// <summary>
        /// 任务储存器：[等待事件]
        /// </summary>
        private List<Func<bool>> Task_WaitEvents = new List<Func<bool>>();


        /// <summary>
        /// 任务储存器：[超时事件]
        /// </summary>
        private List<Func<bool>> Task_TimeOut_Events = new List<Func<bool>>();

        /// <summary>
        /// 任务储存器：[超时定时]
        /// </summary>
        private List<Func<float>> Task_TimeOut_Times = new List<Func<float>>();



        /// <summary>
        /// 超时计时器
        /// </summary>
        private DateTime TimeOut_Clock = DateTime.MinValue;

        /// <summary>
        /// 任务解析器：[等待事件]                                  
        /// 注：任务执行的具体流程方法                                  
        /// </summary>
        /// <param name="TaskPath">任务地址</param>
        private void Parser_WaitEvent(int TaskPath)
        {
            //持续检测等待委托返回true
            if (Task_WaitEvents[TaskPath]())
                //指令指针+1，跳转执行下一条指令。
                this.TA_Pointer++;
        }

        /// <summary>
        /// 任务解析器：[超时事件]                                  
        /// 注：任务执行的具体流程方法                                  
        /// </summary>
        /// <param name="TaskPath">任务地址</param>
        private void Parser_TimeOut_Event(int TaskPath)
        {
            if (TA_Pointer != TA_PointerSave) { TA_PointerSave = TA_Pointer; TimeOut_Clock = DateTime.Now; }
            if (SS_TriggerMarker.Clock_System(TimeOut_Clock, Task_TimeOut_Times[TaskPath]()) || Task_TimeOut_Events[TaskPath]())
            {
                 TA_PointerSave = -1;
                this.TA_Pointer++;//指令指针+1，跳转执行下一条指令。
            }
        }


        /// <summary>
        /// 任务：[等待事件]                                   
        /// 注：阻塞任务执行，Update检测 委托返回true后解除阻塞                                  
        /// </summary>
        /// <param name="WaitEvent">等待事件</param>
        /// <returns>任务执行器</returns>
        public SD_TaskActuator WaitEvent(Func<bool> WaitEvent)
        {
            //指令添加：等待事件解析器地址 ，等待事件任务地址
            Instruction_Add(Parser_WaitEvent, Task_WaitEvents.Count);
            Task_WaitEvents.Add(WaitEvent);//等待事件写入任务储存器

            return this;
        }



        /// <summary>
        /// 任务：[超时事件]                                   
        /// 注：阻塞任务执行，Update检测计时， 委托返回true会解除阻塞,超时也会解除阻塞                          
        /// </summary>
        /// <param name="time">超时时间：秒</param>
        /// <param name="WaitEvent">等待事件</param>
        /// <returns>任务执行器</returns>
        public SD_TaskActuator WaitEvent(Func<float> time, Func<bool> WaitEvent)
        {
            //指令添加：超时事件解析器地址 ，超时事件任务地址
            Instruction_Add(Parser_TimeOut_Event, Task_TimeOut_Events.Count);
            Task_TimeOut_Events.Add(WaitEvent);//等待事件写入任务储存器
            Task_TimeOut_Times.Add(time);//超时时间写入任务储存器
            return this;
        }
    }

}