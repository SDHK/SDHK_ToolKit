using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;


public interface ITableKeyValue
{
    
}

/// <summary>
/// 键值对单元
/// </summary>
[Serializable]
public class TableKeyValue<K, V>
{
    public K key;
    public V value;

    public TableKeyValue() { }

    public TableKeyValue(K key, V value)
    {
        this.key = key;
        this.value = value;
    }

    public override string ToString()
    {
        return string.Format("[{0}:{1}]", key, value);
    }
}

public interface ITable
{

    /// <summary>
    /// 键值对总数
    /// </summary>
    int Count { get; }

}

[Serializable]
public class Table : Table<object, object> { }

/// <summary>
/// 键值对表
/// </summary>
/// <typeparam name="K">键</typeparam>
/// <typeparam name="V">值</typeparam>
[Serializable]
public class Table<K, V> : IEnumerable<TableKeyValue<K, V>>, ITable
{
    public List<TableKeyValue<K, V>> keyValues = new List<TableKeyValue<K, V>>();

    public int Count
    {
        get { return keyValues.Count; }
    }

    // 隐式转换：
    public static implicit operator Dictionary<K, V>(Table<K, V> table)
    {
        return table.ToDictionary(tb => tb.key, tb => tb.value);
    }
    public static implicit operator Table<K, V>(Dictionary<K, V> dic)
    {
        return new Table<K, V>() { keyValues = dic.Select(keyValue => new TableKeyValue<K, V>(keyValue.Key, keyValue.Value)).ToList() };
    }

    /// <summary>
    /// 转为Dictionary
    /// </summary>
    public Dictionary<K, V> ToDictionary()
    {
        return this.ToDictionary(tb => tb.key, tb => tb.value);
    }



    #region Add

    public Table<K, V> Add(K key, V value)
    {
        if (!keyValues.Any((KV) => KV.key.Equals(key)))
        {
            keyValues.Add(new TableKeyValue<K, V>(key, value));
        }
        else
        {
            Debug.Log("Table:键值重复");
        }
        return this;
    }


    public void Add(TableKeyValue<K, V> keyValue)
    {
        if (!keyValues.Any((KV) => KV.key.Equals(keyValue.key)))
        {
            keyValues.Add(keyValue);
        }
        else
        {
            Debug.Log("Table:键值重复");
        }
    }

    /// <summary>
    /// 添加键值对:允许相同键
    /// </summary>
    public void AddAt(K key, V value)
    {
        keyValues.Add(new TableKeyValue<K, V>(key, value));
    }

    #endregion


    #region Need

    /// <summary>
    /// 需要一个键值对，尝试通过键值获取，不存在则创建一个。
    /// </summary>
    public TableKeyValue<K, V> Need(K key, V value)
    {
        if (!keyValues.Any((KV) => KV.key.Equals(key)))
        {
            var KV = new TableKeyValue<K, V>(key, value);
            keyValues.Add(KV);
            return KV;
        }
        else
        {
            return keyValues.Find((KV) => KV.key.Equals(key));
        }
    }

    #endregion

    #region Find

    public int FindIndex(TableKeyValue<K, V> keyValue)
    {
        return keyValues.FindIndex((kV) => kV == keyValue);
    }
    public int FindkeyIndex(K key)
    {
        return keyValues.FindIndex((kV) => kV.key.Equals(key));
    }
    public int FindValueIndex(V value)
    {
        return keyValues.FindIndex((kV) => kV.value.Equals(value));
    }
    #endregion


    #region Get

    /// <summary>
    /// 获取键值对单元
    /// </summary>
    public TableKeyValue<K, V> this[int index]
    {
        get
        {
            return keyValues[index];
        }
        set
        {
            keyValues[index] = value;
        }
    }

    public TableKeyValue<K, V> GetAt(int index)
    {
        return keyValues[index];
    }
    public TableKeyValue<K, V> GetKey(K key)
    {
        return keyValues.Find((keyValue) => keyValue.key.Equals(key));
    }

    public TableKeyValue<K, V> GetValue(V value)
    {
        return keyValues.Find((keyValue) => keyValue.value.Equals(value));
    }

    #endregion


    #region Contains

    public bool ContainsKey(K key)
    {
        return keyValues.Any((KV) => KV.key.Equals(key));
    }

    public bool ContainsValue(V value)
    {
        return keyValues.Any((KV) => KV.value.Equals(value));
    }

    #endregion



    #region Insert

    /// <summary>
    /// 插入:允许相同键
    /// </summary>
    public void Insert(int index, TableKeyValue<K, V> keyValue)
    {
        keyValues.Insert(index, keyValue);
    }

    /// <summary>
    /// 插入:允许相同键
    /// </summary>
    public void Insert(int index, K key, V value)
    {
        keyValues.Insert(index, new TableKeyValue<K, V>(key, value));
    }

    /// <summary>
    /// 位置互换
    /// </summary>
    public void Swap(int a, int b)
    {
        var keyValue = keyValues[a];
        keyValues[a] = keyValues[b];
        keyValues[b] = keyValue;
    }

    #endregion


    #region  Gets

    public List<K> GetKeys()
    {
        return keyValues.Select(keyValue => keyValue.key).ToList();
    }

    public List<V> GetValues()
    {
        return keyValues.Select(keyValue => keyValue.value).ToList();
    }

    public List<TableKeyValue<K, V>> GetKeyValues()
    {
        return keyValues;
    }

    #endregion


    #region  Remove

    public void RemoveNull()
    {
        if (keyValues.Any((KV) => ((object)KV.value) == null))
        {
            keyValues.RemoveAll((keyValue) => ((object)keyValue.value) == null);
        }
    }

    public void RemoveAt(int index)
    {
        keyValues.RemoveAt(index);
    }

    public void RemoveKey(K key)
    {
        if (keyValues.Any((KV) => KV.key.Equals(key)))
        {
            keyValues.RemoveAll((keyValue) => keyValue.key.Equals(key));
        }
    }

    public void RemoveValue(V value)
    {
        if (keyValues.Any((KV) => KV.value.Equals(value)))
        {
            keyValues.RemoveAll((keyValue) => keyValue.value.Equals(value));
        }
    }

    public void Remove(TableKeyValue<K, V> keyValue)
    {
        keyValues.Remove(keyValue);
    }

    public void Clear()
    {
        keyValues.Clear();
    }

    #endregion



    IEnumerator IEnumerable.GetEnumerator()
    {
        return keyValues.GetEnumerator();
    }

    /// <summary>
    /// 用于foreach的迭代器
    /// </summary>
    public IEnumerator<TableKeyValue<K, V>> GetEnumerator()
    {
        return keyValues.GetEnumerator();
    }


}
