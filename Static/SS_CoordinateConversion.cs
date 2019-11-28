using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.10.9
 * 
 * 功能: 不同坐标系之间的坐标转换
 */

namespace SDHK_Tool.Static
{

    /// <summary>
    /// 坐标系坐标角度转换
    /// </summary>
    public class SS_CoordinateConversion
    {

        /// <summary>
        /// 屏幕像素坐标转世界坐标:Z为物体与相机的水平距离
        /// </summary>
        /// <param name="ScreenPoint">屏幕像素坐标</param>
        /// <param name="PlaneDistance">画布距离</param>
        /// <returns>世界坐标</returns>
        public static Vector3 Screen_To_World(Vector2 ScreenPoint, float PlaneDistance)
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(ScreenPoint.x, ScreenPoint.y, PlaneDistance));
        }

        /// <summary>
        /// 世界坐标转屏幕像素坐标
        /// </summary>
        /// <param name="WorldPoint">世界坐标</param>
        /// <returns>屏幕像素坐标</returns>
        public static Vector3 World_To_Screen(Vector3 WorldPoint)
        {
            return Camera.main.WorldToScreenPoint(WorldPoint);
        }

        /// <summary>
        /// 屏幕像素坐标转世界坐标:Z为物体与相机的水平距离
        /// </summary>
        /// <param name="camera">相机</param>
        /// <param name="ScreenPoint">屏幕像素坐标</param>
        /// <param name="PlaneDistance">画布距离</param>
        /// <returns>世界坐标</returns>
        public static Vector3 Screen_To_World(Camera camera, Vector2 ScreenPoint, float PlaneDistance)
        {
            return camera.ScreenToWorldPoint(new Vector3(ScreenPoint.x, ScreenPoint.y, PlaneDistance));
        }

        /// <summary>
        /// 世界坐标转屏幕像素坐标
        /// </summary>
        /// <param name="camera">相机</param>
        /// <param name="WorldPoint">世界坐标</param>
        /// <returns>屏幕像素坐标</returns>
        public static Vector3 World_To_Screen(Camera camera, Vector3 WorldPoint)
        {
            return camera.WorldToScreenPoint(WorldPoint);
        }


        /// <summary>
        /// 世界坐标转局部坐标
        /// </summary>
        /// <param name="transform">局部坐标系</param>
        /// <param name="WorldPoint">世界坐标点</param>
        /// <returns>局部坐标</returns>
        public static Vector3 World_To_local(Transform transform, Vector3 WorldPoint)
        {
            return transform.InverseTransformPoint(WorldPoint);
        }

        /// <summary>
        /// 局部坐标转世界坐标
        /// </summary>
        /// <param name="transform">局部坐标系</param>
        /// <param name="LocalPoint">局部坐标点</param>
        /// <returns>世界坐标</returns>
        public static Vector3 Local_To_World(Transform transform, Vector3 LocalPoint)
        {
            return transform.TransformPoint(LocalPoint);
        }

        /// <summary>
        /// 局部坐标转局部坐标
        /// </summary>
        /// <param name="TargetTransform">目标坐标系</param>
        /// <param name="CurrentTransform">当前坐标系</param>
        /// <param name="LocalPoint">当前局部坐标点</param>
        /// <returns>局部坐标点</returns>
        public static Vector3 Local_To_Local(Transform TargetTransform, Transform CurrentTransform, Vector3 LocalPoint)
        {
            return TargetTransform.InverseTransformPoint(CurrentTransform.TransformPoint(LocalPoint));
        }

        /// <summary>
        /// 屏幕坐标转局部坐标：用于可移动世界画布
        /// </summary>
        /// <param name="TargetTransform">目标坐标系</param>
        /// <param name="ScreenPoint">屏幕像素坐标</param>
        /// <param name="PlaneDistance">画布距离</param>
        /// <returns>局部坐标</returns>
        public static Vector3 Screen_To_Local(Transform TargetTransform, Vector2 ScreenPoint, float PlaneDistance)
        {
            return World_To_local(TargetTransform, Screen_To_World(ScreenPoint, PlaneDistance));
        }


    }
}