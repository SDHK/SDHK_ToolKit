using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Touch
{

    [Serializable]
    public class TouchPool
    {
        [SerializeField]
        private List<int> IdPool = new List<int>();
        private Dictionary<int, PointerEventData> touchPool = new Dictionary<int, PointerEventData>();


        /// <summary>
        /// 触摸数量
        /// </summary>
        public int Count { get => IdPool.Count; }

        public bool Contains(PointerEventData pointerEventData)
        {
            return IdPool.Contains(pointerEventData.pointerId);
        }


        /// <summary>
        /// 添加触摸点
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Add(PointerEventData pointerEventData)
        {
            if (IdPool.Contains(pointerEventData.pointerId))
            {
                return false;
            }
            else
            {
                IdPool.Add(pointerEventData.pointerId);
                touchPool.Add(pointerEventData.pointerId, pointerEventData);
                return true;
            }
        }


        /// <summary>
        /// 通过下标顺序获取触摸点
        /// </summary>
        public PointerEventData this[int index]
        {
            get { return touchPool[IdPool[index]]; }
        }

        /// <summary>
        /// 通过触摸Id获取触摸点
        /// </summary>
        public PointerEventData GetId(int id)
        {
            return touchPool[id];
        }


        /// <summary>
        /// 移除触摸点
        /// </summary>
        /// <returns>是否成功</returns>
        public bool Remove(PointerEventData pointerEventData)
        {
            if (IdPool.Contains(pointerEventData.pointerId))
            {
                IdPool.Remove(pointerEventData.pointerId);
                touchPool.Remove(pointerEventData.pointerId);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 通过下标移除触摸点
        /// </summary>
        public void RemoveAt(int index)
        {
            if (IdPool.Count > index)
            {
                touchPool.Remove(IdPool[index]);
                IdPool.RemoveAt(index);
            }
        }

        /// <summary>
        /// 通过Id移除触摸点
        /// </summary>
        public void Remove(int id)
        {
            if (IdPool.Contains(id))
            {
                touchPool.Remove(id);
                IdPool.Remove(id);
            }
        }




    }
}