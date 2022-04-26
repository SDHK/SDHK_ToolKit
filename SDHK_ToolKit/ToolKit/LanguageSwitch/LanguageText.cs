
/******************************

 * 作者: 闪电黑客

 * 日期: 2021/06/3 07:02:29

 * 最后日期: 。。。

 * 描述: 
    语言切换组件：Text切换

    继承了LanguageBoxBase，在激活时会注册到LanguageManager
    用于重加载后的刷新

    启动时也会刷新一次值

******************************/

using System;
using PathAssets;
using UnityEngine;
using UnityEngine.UI;


namespace LanguageSwitch
{



    /// <summary>
    /// 多语言切换组件：Text
    /// </summary>
    [DisallowMultipleComponent]//同物体上只存在一个
    [RequireComponent(typeof(Text))]//必须要Text组件
    public class LanguageText : LanguageBoxBase
    {
        [NonSerialized]
        public Text text_;

        public string text { get => text_.text; set => text_.text = value; }


        private void Start()
        {
            if (text_ == null) text_ = GetComponent<Text>();
            LanguageRefresh();
        }


        [ContextMenu("刷新")]
        public override void LanguageRefresh()
        {
            if (text_ != null && path != "")
            {
                string str = languageManager.Data.Get<string>(path);
                text_.text = str == null ? path : str;
            }
        }


    }


}