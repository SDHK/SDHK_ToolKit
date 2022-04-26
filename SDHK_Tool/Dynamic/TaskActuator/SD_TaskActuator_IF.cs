using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.2.20
 * 
 * 功能：SD_TaskActuator 的分支部分：If判断功能添加
 *
 */

namespace SDHK_Tool.Dynamic
{
    /// <summary>
    /// IF判断任务                               
    /// </summary>
    public partial class SD_TaskActuator
    {

        /// <summary>
        /// IF判断类_结构
        /// </summary>
        private class IF_Struct
        {
            public Func<bool> IF;//IF事件
            public int IF_Else = -1;//Else地址
            public int IF_End = -1;//End地址
        }

        /// <summary>
        /// 任务储存器：[IF判断]
        /// </summary>
        private List<IF_Struct> Task_IFs = new List<IF_Struct>();

        /// <summary>
        /// 地址栈：用于指令写入时，对多层嵌套IF的标记地址匹配
        /// </summary>
        private Stack<int> IF_Address = new Stack<int>();


        /// <summary>
        /// 任务解析器：[IF判断]                                  
        /// 注：任务执行的具体流程方法                                  
        /// </summary>
        /// <param name="TaskPath">任务地址</param>
        private void Parser_IF(int TaskPath)
        {
            if (Task_IFs[TaskPath].IF()) this.TA_Pointer++;

            else

            if (Task_IFs[TaskPath].IF_Else != -1)
                this.TA_Pointer = Task_IFs[TaskPath].IF_Else + 1;
            else

            if (Task_IFs[TaskPath].IF_End != -1)
                this.TA_Pointer = Task_IFs[TaskPath].IF_End;
            else
                this.TA_Pointer += 2;
        }

        /// <summary>
        /// 任务解析器：[IF_Eles]                                  
        /// 注：任务执行的具体流程方法                                  
        /// </summary>
        /// <param name="TaskPath">任务地址</param>
        private void Parser_IF_Else(int TaskPath)
        {
            this.TA_Pointer = Task_IFs[TaskPath].IF_End;
        }

        /// <summary>
        /// 任务解析器：[IF_End]                                  
        /// 注：任务执行的具体流程方法                                  
        /// </summary>
        /// <param name="TaskPath">任务地址</param>
        private void Parser_IF_End(int TaskPath)
        {
            this.TA_Pointer++;
        }

        /// <summary>
        /// 任务：[IF判断]                                  
        /// 注：根据委托返回的bool进行标记跳转                                  
        /// </summary>
        /// <param name="IF_Event">委托的bool事件</param>
        /// <returns>任务执行器</returns>
        public SD_TaskActuator IF_Event(Func<bool> IF_Event)
        {
            //地址栈添加当前任务地址
            IF_Address.Push(Task_IFs.Count);
            //指令添加：IF判断解析器地址 ，IF判断任务地址
            Instruction_Add(Parser_IF, Task_IFs.Count);
            //新建IF类
            IF_Struct iF_Struct = new IF_Struct();
            iF_Struct.IF = IF_Event;//IF事件添加
            Task_IFs.Add(iF_Struct);//写入任务储存器

            return this;
        }

        /// <summary>
        /// 任务：[IF_Else标记]                                  
        /// 注：写入时会更具内部地址栈，匹配最近的IF判断                                  
        /// </summary>
        /// <returns>任务执行器</returns>
        public SD_TaskActuator IF_Else()
        {
            //指令添加：IF_Else解析器地址 ，IF判断任务地址
            Instruction_Add(Parser_IF_Else, IF_Address.Peek());
            //地址栈获取当前任务，匹配Else位置标记
            Task_IFs[IF_Address.Peek()].IF_Else = this.Parser_Paths.Count - 1;
            return this;
        }

        /// <summary>
        /// 任务：[IF_End标记]                                  
        /// 注：写入时会更具内部地址栈，匹配最近的IF判断                                  
        /// </summary>
        /// <returns>任务执行器</returns>
        public SD_TaskActuator IF_End()
        {
            // Instruction_Add(Parser_IF_End, 0);
            //地址栈取出当前任务，匹配End位置标记
            Task_IFs[IF_Address.Pop()].IF_End = this.Parser_Paths.Count;
            
            return this;
        }

    }

}