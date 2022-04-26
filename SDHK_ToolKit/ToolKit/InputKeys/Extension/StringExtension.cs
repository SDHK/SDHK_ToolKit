
/******************************

 * 作者: 闪电黑客

 * 日期: 2021/07/15 14:44:29

 * 最后日期: 。。。

 * 描述: 
    按键输入管理器

    string扩展：
    简化书写


******************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InputKeys
{

    public static class StringExtension
    {
        private static InputKeysManager inputKeysManager = InputKeysManager.Instance();

        /// <summary>
        /// 启动协程:记录替换按键，完毕后回调
        /// </summary>
        /// <param name="group">组名</param>
        /// <param name="key">按键名</param>
        public static void InputRecordKeys(this string group, string key)
        {
            inputKeysManager.RecordKeys(group, key);
        }

        /// <summary>
        /// 获取组合键，不存在返回Null
        /// </summary>
        /// <param name="group">组名</param>
        /// <param name="key">按键名</param>
        public static InputKeyCodes InputGetKeyCodes(this string group, string key)
        {
            return inputKeysManager.GetKeyCodes(group, key);
        }

        /// <summary>
        /// 直接设置组合键
        /// </summary>
        /// <param name="group">组名</param>
        /// <param name="key">键值</param>
        /// <param name="keyCodes">按键</param>
        public static void InputSetKeyCodes(this string group, string key, InputKeyCodes keyCodes)
        {
            inputKeysManager.SetKeyCodes(group, key, keyCodes);
        }

        /// <summary>
        /// 判断组合键按下
        /// </summary>
        /// <param name="group">组名</param>
        /// <param name="key">按键名</param>
        public static bool InputGetKeysDown(this string group, string key)
        {
            return inputKeysManager.GetKeysDown(group, key);
        }
        /// <summary>
        /// 判断组合键按住
        /// </summary>
        /// <param name="group">组名</param>
        /// <param name="key">按键名</param>
        public static bool InputGetKeys(this string group, string key)
        {
            return inputKeysManager.GetKeys(group, key);
        }

        /// <summary>
        /// 判断组合键抬起
        /// </summary>
        /// <param name="group">组名</param>
        /// <param name="key">按键名</param>
        public static bool InputGetKeysUp(this string group, string key)
        {
            return inputKeysManager.GetKeysUp(group, key);
        }

        /// <summary>
        /// 查询同组，包含相同组合键
        /// </summary>
        /// <param name="group">组名</param>
        /// <param name="key">键值</param>
        /// <param name="keyCodes">组合键</param>
        /// <returns>相同组合键，键值列表</returns>
        public static List<string> InputKeysContains(string group, string key, InputKeyCodes keyCodes)
        {
            return inputKeysManager.InputKeysContains(group, key, keyCodes);
        }

    }
}