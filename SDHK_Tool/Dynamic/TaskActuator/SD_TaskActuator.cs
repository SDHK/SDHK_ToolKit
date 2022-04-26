using System;
using System.Collections;
using System.Collections.Generic;
// using System.Threading;
// using SDHK_Tool.Dynamic;
// using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.2.6
 *
 * 2020.03.24 添加任务终止功能
 * 
 * 功能：任务执行器 的主体构架，负责主要进程的运行
 * 
 * 由 [解析方法] 和 [任务方法] 组成一条 [任务]
 *
 */

namespace SDHK_Tool.Dynamic
{

    /// <summary>
    /// 任务执行器 ：                            
    /// </summary>
    public partial class SD_TaskActuator
    {


        /// <summary>
        /// 解析地址储存器
        /// </summary>
        private List<Action<int>> Parser_Paths = new List<Action<int>>();

        /// <summary>
        /// 任务地址储存器
        /// </summary>
        private List<int> Task_Paths = new List<int>();

        /// <summary>
        /// 指令指针
        /// </summary>
        public int TA_Pointer = -1;


        /// <summary>
        /// 运行标记
        /// </summary>
        public bool isRun = false;


        /// <summary>
        /// 结束标记
        /// </summary>
        public bool isEnd = false;


        /// <summary>
        /// 指令指针标记器
        /// </summary>
        private int TA_PointerSave = -1;


        /// <summary>
        /// 执行器启动
        /// </summary>
        public void Run()
        {

            TA_Pointer = 0;
            TA_PointerSave = -1;
            isRun = true;
        }

        /// <summary>
        /// 执行器任务终止
        /// </summary>
        public void Stop()
        {

            TA_Pointer = -1;
            TA_PointerSave = -1;
            isRun = false;
        }

        /// <summary>
        /// 执行器启动
        /// </summary>
        public void Run_End()
        {
            this.Event(() => isEnd = true);
            TA_Pointer = 0;
            TA_PointerSave = -1;
            isRun = true;
        }


        /// <summary>
        /// [指令写入]                                  
        /// 注： 任务解析器地址 和 任务地址 合成为一条指令 写入到储存器
        /// </summary>
        /// <param name="ParserPath">任务解析器</param>
        /// <param name="TaskPath">任务地址</param>
        private void Instruction_Add(Action<int> ParserPath, int TaskPath)
        {
            Parser_Paths.Add(ParserPath);
            Task_Paths.Add(TaskPath);
        }

        /// <summary>
        /// 主要运行进程 [放在主进程Update里]
        /// </summary>
        public void Update()
        {
            if (TA_Pointer != -1 && TA_Pointer < Parser_Paths.Count && isRun == true)
            {
                //通过 指令指针 提取 任务解析器 和 任务地址 让解析器运行任务
                Parser_Paths[TA_Pointer](Task_Paths[TA_Pointer]);

            }
            else if (TA_Pointer >= Parser_Paths.Count)
            {
                TA_Pointer = -1;
                isRun = false;
            }
        }

    }

}