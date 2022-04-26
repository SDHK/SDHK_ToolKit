using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.2.24
 * 
 * 功能：SD_TaskActuator 的分支部分：Loop循环功能添加
 *
 */

namespace SDHK_Tool.Dynamic
{
    /// <summary>
    /// 循环任务                           
    /// </summary>
    public partial class SD_TaskActuator
    {

        /// <summary>
        /// Loop循环类_结构
        /// </summary>
        private class Loop_Struct
        {
            public Func<bool> Loop;//Loop事件
            public int Loop_Enter = -1;//Enter地址
            public int Loop_End = -1;//End地址
        }

        /// <summary>
        /// 任务储存器：[Loop循环]
        /// </summary>
        private List<Loop_Struct> Task_Loops = new List<Loop_Struct>();

        /// <summary>
        /// 地址栈：用于指令写入时，对多层嵌套Loop的标记地址匹配
        /// </summary>
        private Stack<int> Loop_Address = new Stack<int>();


        /// <summary>
        /// 任务解析器：[LoopEnter]                                  
        /// 注：任务执行的具体流程方法                                  
        /// </summary>
        /// <param name="TaskPath">任务地址</param>
        private void Parser_LoopEnter(int TaskPath)
        {
            if (Task_Loops[TaskPath].Loop())
                this.TA_Pointer++;
            else
                this.TA_Pointer = Task_Loops[TaskPath].Loop_End + 1;
        }

        /// <summary>
        /// 任务解析器：[LoopEnd]                                  
        /// 注：任务执行的具体流程方法                                  
        /// </summary>
        /// <param name="TaskPath">任务地址</param>
        private void Parser_LoopEnd(int TaskPath)
        {
            if (Task_Loops[TaskPath].Loop())
                this.TA_Pointer = Task_Loops[TaskPath].Loop_Enter;
            else
                this.TA_Pointer++;
        }

        /// <summary>
        /// 任务解析器：[Loop_End]                                  
        /// 注：任务执行的具体流程方法                                  
        /// </summary>
        /// <param name="TaskPath">任务地址</param>
        private void Parser_Loop_End(int TaskPath)
        {
            this.TA_Pointer = Task_Loops[TaskPath].Loop_Enter;
        }



        /// <summary>
        /// 任务：[Loop_Enter标记]                                  
        /// 注：写入时会更具内部地址栈，匹配最近的Loop_End循环                                  
        /// </summary>
        /// <returns>任务执行器</returns>
        public SD_TaskActuator Loop_Enter()
        {
            //地址栈添加当前任务地址
            Loop_Address.Push(Task_Loops.Count);
            //新建Loop循环类结构
            Loop_Struct loop_Struct = new Loop_Struct();
            loop_Struct.Loop_Enter = this.Parser_Paths.Count;//匹配循环开始位置标记
            Task_Loops.Add(loop_Struct);//写入任务储存器

            return this;
        }

        /// <summary>
        /// 任务：[Loop_End标记]                                  
        /// 注：写入时会更具内部地址栈，匹配最近的Loop_Enter循环                                  
        /// </summary>
        /// <returns>任务执行器</returns>
        public SD_TaskActuator Loop_End()
        {
            //地址栈取出任务地址，匹配循环退出位置标记
            int Address = Loop_Address.Pop();
            Task_Loops[Address].Loop_End = this.Parser_Paths.Count;
            //指令添加：Loop循环退出 解析器地址 ，任务地址
            Instruction_Add(Parser_Loop_End, Address);

            return this;
        }


        /// <summary>
        /// 任务：[Loop_Enter循环]                                  
        /// 注：写入时会更具内部地址栈，匹配最近的Loop_End标记                                  
        /// </summary>
        /// <returns>任务执行器</returns>
        public SD_TaskActuator Loop_Enter(Func<bool> Loop_Event)
        {
            //地址栈添加当前任务地址
            Loop_Address.Push(Task_Loops.Count);
            //新建Loop循环类结构
            Loop_Struct loop_Struct = new Loop_Struct();
            loop_Struct.Loop = Loop_Event;//添加循环判断事件
            loop_Struct.Loop_Enter = this.Parser_Paths.Count;//匹配循环开始位置标记
            //指令添加：Loop循环进入 解析器地址 ，任务地址
            Instruction_Add(Parser_LoopEnter, Task_Loops.Count);
            Task_Loops.Add(loop_Struct);//写入任务储存器

            return this;
        }

        /// <summary>
        /// 任务：[Loop_End循环]                                  
        /// 注：写入时会更具内部地址栈，匹配最近的Loop_Enter标记                                  
        /// </summary>
        /// <returns>任务执行器</returns>
        public SD_TaskActuator Loop_End(Func<bool> Loop_Event)
        {
            //地址栈取出任务地址
            int Address = Loop_Address.Pop();
            Task_Loops[Address].Loop = Loop_Event;//添加循环判断事件
            Task_Loops[Address].Loop_End = this.Parser_Paths.Count;//匹配循环退出位置标记
            //指令添加：Loop循环结尾 解析器地址 ，任务地址
            Instruction_Add(Parser_LoopEnd, Address);

            return this;
        }


    }

}