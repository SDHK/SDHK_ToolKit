
/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/19 09:42:42

 * 最后日期: 2021/12/19 09:43:04

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;

namespace ObjectFactory
{
    public class LuaObjectPool : ObjectPool<LuaTable>
    {
        public LuaTable table;

        public LuaObjectPool(LuaTable table)
        {
            this.table = table;
            this.RegisterManager();
        }
    }
}
