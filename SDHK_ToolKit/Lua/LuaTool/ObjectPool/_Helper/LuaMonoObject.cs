
/******************************

 * Author: 闪电黑客

 * 日期: 2021/12/20 18:27:34

 * 最后日期: 2022/01/22 22:41:53

 * 最后修改: 闪电黑客

 * 描述:  
    
    LuaMono对象池对象

    连接Lua和mono脚本
    让 Lua 拥有 MonoBehaviour 的生命周期以及事件响应功能

******************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;


namespace ObjectFactory
{
    public class LuaMonoObject : MonoBehaviour
    {
        public LuaTable table;


        #region Start

        public Action<LuaTable> MonoStart;
        private void Start()
        {
            MonoStart?.Invoke(table);
        }

        public Action<LuaTable> MonoOnDestroy;
        private void OnDestroy()
        {
            MonoOnDestroy?.Invoke(table);
        }
        #endregion


        #region Update

        public Action<LuaTable> MonoUpdate;
        private void Update()
        {
            MonoUpdate?.Invoke(table);
        }

        public Action<LuaTable> MonoLateUpdate;
        private void LateUpdate()
        {
            MonoLateUpdate?.Invoke(table);
        }

        public Action<LuaTable> MonoFixedUpdate;
        private void FixedUpdate()
        {
            MonoFixedUpdate?.Invoke(table);
        }

        #endregion


        #region OnEnable
        public Action<LuaTable> MonoOnEnable;
        private void OnEnable()
        {
            MonoOnEnable?.Invoke(table);
        }

        public Action<LuaTable> MonoOnDisable;
        private void OnDisable()
        {
            MonoOnDisable?.Invoke(table);
        }
        #endregion


        #region OnCollision

        public Action<LuaTable, Collision> MonoOnCollisionEnter;
        private void OnCollisionEnter(Collision other)
        {
            MonoOnCollisionEnter?.Invoke(table, other);
        }

        public Action<LuaTable, Collision> MonoOnCollisionStay;
        private void OnCollisionStay(Collision other)
        {
            MonoOnCollisionStay?.Invoke(table, other);
        }

        public Action<LuaTable, Collision> MonoOnCollisionExit;
        private void OnCollisionExit(Collision other)
        {
            MonoOnCollisionExit?.Invoke(table, other);
        }

        public Action<LuaTable, Collision2D> MonoOnCollisionEnter2D;
        private void OnCollisionEnter2D(Collision2D other)
        {
            MonoOnCollisionEnter2D?.Invoke(table, other);
        }
        public Action<LuaTable, Collision2D> MonoOnCollisionStay2D;
        private void OnCollisionStay2D(Collision2D other)
        {
            MonoOnCollisionStay2D?.Invoke(table, other);
        }

        public Action<LuaTable, Collision2D> MonoOnCollisionExit2D;
        private void OnCollisionExit2D(Collision2D other)
        {
            MonoOnCollisionExit2D?.Invoke(table, other);
        }



        #endregion


        #region OnTrigger
        public Action<LuaTable, Collider> MonoOnTriggerEnter;
        private void OnTriggerEnter(Collider other)
        {
            MonoOnTriggerEnter?.Invoke(table, other);
        }
        public Action<LuaTable, Collider> MonoOnTriggerStay;
        private void OnTriggerStay(Collider other)
        {
            MonoOnTriggerStay?.Invoke(table, other);
        }
        public Action<LuaTable, Collider> MonoOnTriggerExit;

        private void OnTriggerExit(Collider other)
        {
            MonoOnTriggerExit?.Invoke(table, other);
        }

        public Action<LuaTable, Collider2D> MonoOnTriggerEnter2D;
        private void OnTriggerEnter2D(Collider2D other)
        {
            MonoOnTriggerEnter2D?.Invoke(table, other);
        }

        public Action<LuaTable, Collider2D> MonoOnTriggerStay2D;
        private void OnTriggerStay2D(Collider2D other)
        {
            MonoOnTriggerStay2D?.Invoke(table, other);
        }
        public Action<LuaTable, Collider2D> MonoOnTriggerExit2D;
        private void OnTriggerExit2D(Collider2D other)
        {
            MonoOnTriggerExit2D?.Invoke(table, other);
        }



        #endregion


        #region OnApplication

        public Action<LuaTable> MonoOnApplicationQuit;
        private void OnApplicationQuit()
        {
            MonoOnApplicationQuit?.Invoke(table);
        }

        public Action<LuaTable, bool> MonoOnApplicationFocus;
        private void OnApplicationFocus(bool focusStatus)
        {
            MonoOnApplicationFocus?.Invoke(table, focusStatus);
        }

        public Action<LuaTable, bool> MonoOnApplicationPause;
        private void OnApplicationPause(bool pauseStatus)
        {
            MonoOnApplicationPause?.Invoke(table, pauseStatus);
        }

        #endregion


    }
}


