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
 * 功能：SD_TaskActuator 的分支部分：委托事件功能添加
 *
 */

namespace SDHK_Tool.Dynamic
{

    /// <summary>
    /// 委托事件任务                                  
    /// </summary>
    public partial class SD_TaskActuator
    {

        /// <summary>
        /// 任务储存器：[委托事件]
        /// </summary>
        private List<Action> Task_Events = new List<Action>();

        /// <summary>
        /// 任务解析器：[委托事件]                                  
        /// 注：任务执行的具体流程方法                                  
        /// </summary>
        /// <param name="TaskPath">任务地址</param>
        private void Parser_Event(int TaskPath)
        {
            //通过 任务地址 来获取 任务 执行。
            Task_Events[TaskPath]();
            //指令指针+1，跳转执行下一条指令。
            this.TA_Pointer++;
        }

        /// <summary>
        /// 任务：[委托事件]                                  
        /// 注：执行委托任务                                  
        /// </summary>
        /// <param name="Event">委托事件</param>
        /// <returns>任务执行器</returns>
        public SD_TaskActuator Event(Action Event)
        {
            //指令添加：委托事件解析器地址 ，委托事件任务地址
            Instruction_Add(Parser_Event, Task_Events.Count);
            Task_Events.Add(Event);//委托写入任务储存器

            return this;
        }
    }

}