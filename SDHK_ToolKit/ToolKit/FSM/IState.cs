/******************************

 * 作者: 闪电黑客

 * 日期: 2021/07/03 7:17:50

 * 最后日期: 2021/07/03 7:17:50

 * 描述: 
    状态接口

    主要功能：
        用接口实现，考虑可以给Mono用

******************************/

namespace FSM
{
    /// <summary>
    /// 状态接口
    /// </summary>
    public interface IFiniteState
    {
        FiniteStateMachine StateMachine { get; set; }

        /// <summary>
        /// 状态运行标记：
        /// 状态切换到当前，会自动变成true。切换其他状态后，状态机 会变成空转等待。主动设置标记为false才可以启动下一个状态。
        /// </summary>
        bool IsRun { get; set; }

        /// <summary>
        /// 状态被Add到状态机时,可以用来初始化
        /// </summary>
        void StateAdd();

        /// <summary>
        /// 状态被移除状态机时
        /// </summary>
        void StateRemove();

        /// <summary>
        /// 状态启动
        /// </summary>
        void StateEnter();
        /// <summary>
        /// 状态刷新
        /// </summary>
        void StateUpdate();
        /// <summary>
        /// 状态退出
        /// </summary>
        void StateExit();
    }

}
