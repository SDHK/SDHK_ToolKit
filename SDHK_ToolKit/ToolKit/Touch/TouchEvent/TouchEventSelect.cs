using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Touch
{

    public class TouchEventSelect : MonoBehaviour
        , ISelectHandler //物体被选中时
        , IUpdateSelectedHandler    //被选中刷新
        , IDeselectHandler //物体从选中到取消选中时
        , IMoveHandler  //物体被选中移动时(与InputManager里的Horizontal和Vertica按键相对应)
        , ISubmitHandler //提交按钮被按下时(与InputManager里的Submit按键相对应，PC上默认的是Enter键)
        , ICancelHandler //取消按钮被按下时(与InputManager里的Cancel按键相对应，PC上默认的是Esc键)
        , IPointerClickHandler  //点击事件
    {


        /// <summary>
        /// 自动选中：开启后，点击事件将会自动选中本物体
        /// </summary>
        public bool autoSelect = true;

        public Action<BaseEventData> SelectStart;
        public Action<BaseEventData> SelectUpdate;
        public Action<BaseEventData> SelectEnd;
        public Action<AxisEventData> SelectMove;
        public Action<BaseEventData> SelectSubmit;
        public Action<BaseEventData> SelectCancel;


        public void OnPointerClick(PointerEventData eventData)
        {
            if (autoSelect)
            {
                EventSystem.current.SetSelectedGameObject(gameObject);//设置点击后选中物体为自己
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (SelectStart != null) SelectStart(eventData);
        }

        public void OnUpdateSelected(BaseEventData eventData)
        {
            if (SelectUpdate != null) SelectUpdate(eventData);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (SelectEnd != null) SelectEnd(eventData);
        }


        public void OnMove(AxisEventData eventData)
        {
            if (SelectMove != null) SelectMove(eventData);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (SelectSubmit != null) SelectSubmit(eventData);
        }
        public void OnCancel(BaseEventData eventData)
        {
            if (SelectCancel != null) SelectCancel(eventData);
        }


    }
}