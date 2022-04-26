using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using SDHK_Tool.Static;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.2.8
 * 
 * 功能：SD_TaskActuator 的分支部分：等待时间功能添加
 *
 */

namespace SDHK_Tool.Dynamic
{
    /// <summary>
    /// 等待计时任务                          
    /// </summary>
    public partial class SD_TaskActuator
    {

        /// <summary>
        /// 任务储存器：[等待时间]
        /// </summary>
        private List<Func<float>> Task_WaitTimes = new List<Func<float>>();

        /// <summary>
        /// 任务储存器：[计时器事件]
        /// </summary>
        private List<Func<bool>> Task_TimerEvents = new List<Func<bool>>();

        /// <summary>
        /// 任务储存器：[计时器定时]
        /// </summary>
        private List<Func<float>> Task_TimerTimes = new List<Func<float>>();

        /// <summary>
        /// 计时器
        /// </summary>
        private DateTime TimerClock = DateTime.MinValue;


        /// <summary>
        /// 任务解析器：[等待时间]                                  
        /// 注：任务执行的具体流程方法                                  
        /// </summary>
        /// <param name="TaskPath">任务地址</param>
        private void Parser_WaitTime(int TaskPath)
        {
            if (TA_Pointer != TA_PointerSave) { TA_PointerSave = TA_Pointer; TimerClock = DateTime.Now; }//计时重置
            if (SS_TriggerMarker.Clock_System(TimerClock, Task_WaitTimes[TaskPath]()))//计时判断
            {
                TA_PointerSave = -1;
                this.TA_Pointer++;//指令指针+1，跳转执行下一条指令。
            }
        }


        /// <summary>
        /// 任务解析器：[事件计时器]                                  
        /// 注：任务执行的具体流程方法                                  
        /// </summary>
        /// <param name="TaskPath">任务地址</param>
        private void Parser_TimerEvent(int TaskPath)
        {
            if (TA_Pointer != TA_PointerSave || Task_TimerEvents[TaskPath]()) { TA_PointerSave = TA_Pointer; TimerClock = DateTime.Now; }//计时重置
            if (SS_TriggerMarker.Clock_System(TimerClock, Task_TimerTimes[TaskPath]()))//计时判断
            {
                TA_PointerSave = -1;
                this.TA_Pointer++;//指令指针+1，跳转执行下一条指令。
            }
        }


        /// <summary>
        /// 任务：[等待时间]                                  
        /// 注：阻塞任务执行，计时结束后解除阻塞                                  
        /// </summary>
        /// <param name="time">时间：秒</param>
        /// <returns>任务执行器</returns>
        public SD_TaskActuator WaitTime(Func<float> time)
        {
            //指令添加：等待时间解析器地址 ，等待时间任务地址
            Instruction_Add(Parser_WaitTime, Task_WaitTimes.Count);
            Task_WaitTimes.Add(time);//等待时间写入任务储存器
            return this;
        }


        /// <summary>
        /// 任务：[事件计时器]                                      
        /// 注：阻塞任务执行，计时结束后解除阻塞，事件为true时会重置时钟从0计时                
        /// </summary>
        /// <param name="time">计时时间：秒</param>
        /// <param name="TimerEvent">计时重置事件</param>
        /// <returns>任务执行器</returns>
        public SD_TaskActuator WaitTime(Func<float> time, Func<bool> TimerEvent)
        {
            //指令添加：等待时间解析器地址 ，等待时间任务地址
            Instruction_Add(Parser_TimerEvent, Task_TimerEvents.Count);
            Task_TimerEvents.Add(TimerEvent);//闹钟任务写入任务储存器
            Task_TimerTimes.Add(time);//等待时间写入任务储存器
            return this;
        }


    }

}