using System;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.6.11
 * 
 * 功能：SS_File的分支部分：存放文件操作需要的类型
 */

namespace SDHK_Tool.Static
{
    /// <summary>
    /// 用于Unity3d的资源文件读写类
    /// </summary>
    public static partial class SS_File { }

    /// <summary>
    /// 用于序列化的Vector3结构
    /// </summary>
    [Serializable]
    public struct SF_Vector3
    {
        public float x;
        public float y;
        public float z;

        public SF_Vector3(float rX, float rY, float rZ)
        {
            x = rX;
            y = rY;
            z = rZ;
        }

        // 以字符串形式返回,方便调试查看
        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2}]", x, y, z);
        }

        // 隐式转换：将SF_Vector3 转换成 Vector3
        public static implicit operator Vector3(SF_Vector3 rValue)
        {
            return new Vector3(rValue.x, rValue.y, rValue.z);
        }

        // 隐式转换：将Vector3 转成 SF_Vector3
        public static implicit operator SF_Vector3(Vector3 rValue)
        {
            return new SF_Vector3(rValue.x, rValue.y, rValue.z);
        }
    }





}
