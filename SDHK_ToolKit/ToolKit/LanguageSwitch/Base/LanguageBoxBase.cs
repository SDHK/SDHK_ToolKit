

/******************************

 * 作者: 闪电黑客

 * 日期: 2021/06/3 07:02:29

 * 最后日期: 。。。

 * 描述: 
    语言切换组件基类：

    简易的语言键值替换功能。

******************************/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace LanguageSwitch
{

    /// <summary>
    /// 多语言切换刷新接口
    /// </summary>
    public interface ILanguageRefresh
    {
        /// <summary>
        /// 多语言切换刷新
        /// </summary>
        void LanguageRefresh();
    }


    /// <summary>
    /// 多语言切换组件基类
    /// </summary>
    public abstract class LanguageBoxBase : MonoBehaviour, ILanguageRefresh
    {

        /// <summary>
        /// 路径
        /// </summary>
        public string path="";

      
        protected LanguageManager languageManager = LanguageManager.Instance();


        public abstract void LanguageRefresh();

        /// <summary>
        /// 设置组名和键名
        /// </summary>
        /// <param name="group">组名</param>
        /// <param name="key">键名</param>
        public LanguageBoxBase SetPath(string path)
        {
            this.path = path;
            LanguageRefresh();
            return this;
        }


        private void Awake()
        {
            languageManager.AddBox(this);
        }

        private void OnDestroy()
        {
            languageManager.RemoveBox(this);
        }

    }
}
