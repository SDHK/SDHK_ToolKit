

/******************************

 * Author: 闪电黑客

 * 日期: 2021/07/18 03:17:54

 * 最后日期: 2021/12/26 09:24:20

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;

namespace SDHK_Extension
{

    public static class DictionaryExtension
    {

        #region Index
        public static K GetKeyAt<K, V>(this Dictionary<K, V> dic, int index)
        {
            return dic.ElementAt(index).Key;
        }

        public static V GetValueAt<K, V>(this Dictionary<K, V> dic, int index)
        {
            return dic.ElementAt(index).Value;
        }

        public static void SetValueAt<K, V>(this Dictionary<K, V> dic, int index, V newValue)
        {
            var key = dic.ElementAt(index).Key;
            dic[key] = newValue;
        }

        public static Dictionary<K, V> ReplaceKeyAt<K, V>(this Dictionary<K, V> dic, int index, K newKey)
        {
            var key = dic.Keys.ToArray()[index];
            return dic.ToDictionary(kv => (kv.Key.Equals(key) ? newKey : kv.Key), kv => kv.Value);
        }

        public static Dictionary<K, V> ReplaceKey<K, V>(this Dictionary<K, V> dic, object key, K newKey)
        {
            return dic.ToDictionary(kv => (kv.Key.Equals(key) ? newKey : kv.Key), kv => kv.Value);
        }

        public static Dictionary<K, V> InsertAt<K, V>(this Dictionary<K, V> dic, int index, K newKey, V newValue)
        {
            var kvs = dic.ToList();
            kvs.Insert(index, new KeyValuePair<K, V>(newKey, newValue));
            return kvs.ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public static Dictionary<K, V> RemoveAt<K, V>(this Dictionary<K, V> dic, int index)
        {
            var kvs = dic.ToList();
            kvs.RemoveAt(index);
            return kvs.ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        #endregion


    }



}
