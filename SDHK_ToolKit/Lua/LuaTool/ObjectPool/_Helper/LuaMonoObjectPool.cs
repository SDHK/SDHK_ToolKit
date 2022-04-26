
/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/20 18:25:27

 * 最后日期: 2021/12/20 18:26:16

 * 最后修改: 闪电黑客

 * 描述:  
    
    基础的LuaMono对象池: 继承 ObjectPool<LuaMonoObject>

    处理对象的委托在Lua中添加完成

******************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;

namespace ObjectFactory
{
    public class TableList : List<Table> { }

    /// <summary>
    /// Lua的mono对象池
    /// </summary>
    public class LuaMonoObjectPool : ObjectPool<LuaMonoObject>
    {
        /// <summary>
        /// 要生成的Lua表
        /// </summary>
        public LuaTable table;

        /// <summary>
        /// 池对象（不销毁）：用于储存回收的游戏对象
        /// </summary>
        public Transform poolTransform { get; private set; }

        /// <summary>
        /// 预制体
        /// </summary>
        public GameObject prefab { get; private set; }

        public string tableName;
        public string objName;

        public LuaMonoObjectPool(LuaTable table, GameObject prefabObj = null)
        {
            ObjectType = typeof(LuaMonoObject);
            tableName = table.Get<string>("TableName");

            if (prefabObj != null)
            {
                prefab = prefabObj;
                objName = tableName + "." + prefabObj.name;
            }
            else
            {
                objName = tableName + ".LuaMonoObject";
            }
            poolTransform = new GameObject(ToString()).transform;
            GameObject.DontDestroyOnLoad(poolTransform);
            this.table = table;
            this.RegisterManager();
        }
        public override void Destroy()
        {
            base.Destroy();
            GameObject.Destroy(poolTransform.gameObject);
        }
        public LuaTable GetTable()
        {
            return Get().table;
        }
        public LuaTable GetTable(Transform parent)
        {
            return Get(parent).table;
        }



        public LuaMonoObject Get(Transform parent)
        {
            destoryCountDown = objectDestoryClock;
            LuaMonoObject obj = DequeueOrNewObject();
            obj.transform.SetParent(parent);
            objectOnGet?.Invoke(obj);
            Preload();
            return obj;
        }



        public override string ToString()
        {
            return "[LuaMonoObjectPool<" + tableName + ">] : " + objName;

        }

    }
}