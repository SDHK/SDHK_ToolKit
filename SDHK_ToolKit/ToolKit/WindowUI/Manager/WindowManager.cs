using Singleton;
using StateMachine;
using UnityEngine;

namespace WindowUI
{
    /// <summary>
    /// 窗口管理器
    /// </summary>
    public class WindowManager : SingletonMonoBase<WindowManager>
    {
        private StackStateMachine stackStateMachine;
        private FocusStateMachine focusStateMachine;

        public override void OnInstance()
        {
            stackStateMachine = StackStateMachine.Get();
            focusStateMachine = FocusStateMachine.Get();
        }

        private bool isCloseAll = false;

        /// <summary>
        /// 窗体动画完成
        /// </summary>
        public bool IsDone => stackStateMachine.IsDone && focusStateMachine.IsDone;

        /// <summary>
        /// 请求显示窗口
        /// </summary>
        public IWindow Show(IWindow windowObj)
        {
            if (IsDone)
            {

                if (stackStateMachine.Push(windowObj))
                {
                    focusStateMachine.Set(null);
                }
                return windowObj;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 请求关闭窗口：若请求不是栈顶，则会提示栈顶
        /// </summary>
        public void Close(IWindow windowObj)
        {
            if (IsDone)
            {
                if (stackStateMachine.Pop(windowObj))
                {
                    focusStateMachine.Set(null);
                }
            }
        }

        /// <summary>
        /// 请求关闭栈顶窗口
        /// </summary>
        public void Close()
        {
            if (IsDone)
            {
                if (stackStateMachine.Pop())
                {
                    focusStateMachine.Set(null);
                }
            }
        }

        /// <summary>
        /// 请求关闭全部窗口
        /// </summary>
        public void CloseAll()
        {
            if (stackStateMachine.Count > 0 && IsDone)
            {
                isCloseAll = true;
                Close();//当前帧主动关闭一个
            }
        }

        /// <summary>
        /// 强制直接清空全部窗口
        /// </summary>
        public void Clear()
        {
            stackStateMachine.Clear();
            focusStateMachine.Clear();
        }

        private void Update()
        {
            if (stackStateMachine.Count > 0)
            {
                if (focusStateMachine.State == null)//焦点替换
                {
                    if (IsDone)
                    {
                        focusStateMachine.Set(stackStateMachine.Peek() as IWindow);
                    }
                }
            }

            if (isCloseAll)
            {
                if (stackStateMachine.Count == 0) isCloseAll = false;
                Close();
            }
        }

    }























}