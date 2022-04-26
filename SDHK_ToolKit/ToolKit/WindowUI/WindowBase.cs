using System;
using System.Collections;
using System.Collections.Generic;
using ObjectFactory;
using StateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace WindowUI
{

    public interface IWindow : IStackState, IFocusState{}
    

    /// <summary>
    /// 窗体基类
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]//必须要CanvasGroup组件
    [DisallowMultipleComponent]//同物体上只存在一个
    public abstract class WindowBase : MonoBehaviour, IObjectPoolItem, IWindow
    {
        public ObjectPoolBase RecyclePool { get; set; }
        public StackStateMachine stackStateMachine { get; set; }
        public FocusStateMachine focusStateMachine { get; set; }

        /// <summary>
        /// 获得CanvasGroup
        /// </summary>
        public CanvasGroup CanvasGroup
        {
            get => (canvasGroup == null) ? canvasGroup = gameObject.GetComponent<CanvasGroup>() : canvasGroup;
        }
        public ObjectPoolBase thisPool { get; set; }

        private CanvasGroup canvasGroup;

        public abstract void ObjectOnNew();
        public abstract void ObjectOnGet();
        public abstract void ObjectOnRecycle();
        public abstract void ObjectOnClear();

        /// <summary>
        /// 等待状态进入：可用于 入栈动画，调用 EnterDone() 让状态机继续运行。默认功能，位置归0，尺寸为1，UI锚点展开铺满
        /// </summary>
        public virtual void WaitStackStateEnter(Action EnterDone)
        {
            transform.localRotation = Quaternion.identity;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            RectAllStretch();

            EnterDone();
        }
        public virtual void WaitStackStateExit(Action ExitDone) { ExitDone(); }


        public virtual void StackStateEnter() { }
        public virtual void StackStateExit() { RecyclePool.Recycle(this); }
        public virtual void StackStateUpdate() { }


        /// <summary>
        /// 栈顶窗口提示： 在请求关闭的不是栈顶时，当前窗口为栈顶时调用
        /// </summary>
        public virtual void StackTopPrompt() { transform.SetAsLastSibling(); }

        /// <summary>
        /// 等待焦点状态进入：可用于 进入动画，调用 EnterDone() 让状态机继续运行。默认功能，可交互激活
        /// </summary>
        public virtual void WaitFocusStateEnter(Action EnterDone) { EnterDone(); CanvasGroup.interactable = true; }

        /// <summary>
        /// 等待焦点状态退出：可用于 退出动画，调用 ExitDone() 让状态机继续运行。默认功能，可交互关闭
        /// </summary>
        public virtual void WaitFocusStateExit(Action ExitDone)
        {

            CanvasGroup.interactable = false; ExitDone();
        }

        public virtual void FocusStateEnter() { }
        public virtual void FocusStateExit() { }
        public virtual void FocusStateUpdate() { }



        /// <summary>
        /// 全拉伸：锚点拉伸到最大到四角，页面全展开
        /// </summary>
        public RectTransform RectAllStretch()
        {
            RectTransform rtf = transform as RectTransform;
            if (rtf != null)
            {
                rtf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
                rtf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
                rtf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
                rtf.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);

                rtf.anchorMin = Vector2.zero;//设置锚点为全屏四角
                rtf.anchorMax = Vector2.one;//设置锚点为全屏四角}


            }
            return rtf;
        }

        public void ObjectRecycle()
        {
            throw new NotImplementedException();
        }

        public void ObjectOnDestroy()
        {
            throw new NotImplementedException();
        }


        //窗口全部子控件导航设置,已通过CanvasGroup.interactable解决
        // private void SetNavigation(GameObject gameObj, Navigation.Mode navigationMode)
        // {
        //     Selectable[] selectables = gameObj.GetComponentsInChildren<Selectable>();

        //     foreach (var item in selectables)
        //     {
        //         Navigation navigation = item.navigation;
        //         navigation.mode = navigationMode;
        //         item.navigation = navigation;
        //     }
        // }


    }
}