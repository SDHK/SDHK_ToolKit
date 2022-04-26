
/******************************

 * Author: 闪电黑客

 * 日期: 2022/01/28 21:48:56

 * 最后日期: 2022/02/05 21:23:58

 * 最后修改: 闪电黑客

 * 描述:  

******************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace SDHK_Extension
{

    public static class Vector3Extension
    {
        /// <summary>
        /// 欧拉角匀速旋转
        /// </summary>
        public static Vector3 MoveTowardsAngle(this Vector3 current, Vector3 target, float RotationSpeed)
        {
            current.x = Mathf.MoveTowardsAngle(current.x, target.x, RotationSpeed);
            current.y = Mathf.MoveTowardsAngle(current.y, target.y, RotationSpeed);
            current.z = Mathf.MoveTowardsAngle(current.z, target.z, RotationSpeed);
            return current;
        }

        /// <summary>
        /// 欧拉角插值旋转
        /// </summary>
        public static Vector3 LerpAngle(Vector3 current, Vector3 target, float t)
        {
            current.x = Mathf.LerpAngle(current.x, target.x, t);
            current.y = Mathf.LerpAngle(current.y, target.y, t);
            current.z = Mathf.LerpAngle(current.z, target.z, t);
            return current;
        }

        /// <summary>
        /// 欧拉角平滑旋转
        /// </summary>
        /// <param name="smoothTime">到达目标的近似时间</param>
        public static Vector3 SmoothDampAngle(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime)
        {
            current.x = Mathf.SmoothDampAngle(current.x, target.x, ref currentVelocity.x, smoothTime);
            current.y = Mathf.SmoothDampAngle(current.y, target.y, ref currentVelocity.y, smoothTime);
            current.z = Mathf.SmoothDampAngle(current.z, target.z, ref currentVelocity.z, smoothTime);
            return current;
        }

        /// <summary>
        /// 欧拉角自轴旋转
        /// </summary>
        /// <param name="current">当前物体欧拉角(transform.eulerAngles)</param>
        /// <param name="direction">旋转轴向量及角度大小（Vector3.up）</param>
        /// <returns>return : 自转后的欧拉角</returns>
        public static Vector3 AxisRotation(Vector3 current, Vector3 direction)
        {
            return (Quaternion.Euler(current) * Quaternion.Euler(direction)).eulerAngles;
        }
    }


    public static class TransformExtension
    {
        /// <summary>
        /// 将Transform数值设置为默认，旋转0，位置0，大小1
        /// </summary>
        public static Transform Default(this Transform tf)
        {
            tf.localRotation = Quaternion.identity;
            tf.localPosition = Vector3.zero;
            tf.localScale = Vector3.one;
            return tf;
        }

        /// <summary>
        /// 获取RectTransform
        /// </summary>
        public static RectTransform GetRectTransform(this Transform tf)
        {
            if (tf is RectTransform)
            {
                return tf as RectTransform;
            }
            else
            {
                return tf.gameObject.AddComponent<RectTransform>();
            }
        }

        /// <summary>
        /// 深度查找子物体
        /// </summary>
        /// <param name="childName">子物体名</param>
        public static Transform FindChildDeep(this Transform root, string childName)
        {
            Transform x = root.Find(childName);//查找名字为childName的子物体
            if (x != null)
            {
                return x;
            }

            for (int i = 0; i < root.childCount; i++)
            {
                Transform childTF = root.GetChild(i);
                x = childTF.FindChildDeep(childName);
                if (x != null)
                {
                    return x;
                }
            }
            return null;
        }

    }
}