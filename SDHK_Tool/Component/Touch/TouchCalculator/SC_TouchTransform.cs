
using System.Collections.Generic;
using SDHK_Tool.Static;
using UnityEngine;
using UnityEngine.EventSystems;
// using SDHK_Tool.Extension;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.7.22 
 *
 * 2019.10.11 触摸池合并，坐标计算从世界坐标改为局部坐标系，添加三种不同的画布模式计算功能
 * 
 * 2019.11.16 添加拖拽角度限制
 *
 * 功能：计算出触摸拖拽后的位置
 *
 */
namespace SDHK_Tool.Component
{

    /// <summary>
    /// 触摸位置计算器
    /// </summary>
    public class SC_TouchTransform : SB_Touch
    , IPointerDownHandler
    , IDragHandler
    , IPointerUpHandler
    , IScrollHandler
    {

        /// <summary>
        /// 点击顶置
        /// </summary>
        [Tooltip("点击顶置")]
        public bool Overhead = false;

        [Space()]

        /// <summary>
        /// 画布模式：将影响拖拽计算
        /// </summary>
        [Tooltip("画布模式")]
        public RenderMode renderMode;

        /// <summary>
        /// 屏幕距离：用于画布的Camera模式
        /// </summary>
        [Tooltip("画布距离")]
        public float PlaneDistance = 100;


        [Space()]

        /// <summary>
        /// 拖拽作用物体
        /// </summary>
        [Tooltip("拖拽作用物体：为null则默认为本身")]
        public Transform TouchObject;

        [Space()]

        /// <summary>
        /// 电机接口（当电机不为空时，数值将会通过电机计算后再作用到物体上）
        /// </summary>
        [Tooltip("电机接口（当电机不为空时，数值将会通过电机计算后再作用到物体上）")]
        public SB_TouchMotor TouchMotor;//电机接口

        [Space()]
        [Space()]

        /// <summary>
        /// 触摸优先：优先使用最新的触摸点进行计算
        /// </summary>
        [Tooltip("触摸优先级")]
        public bool TouchPriority = false;

        /// <summary>
        /// 计算结果直接作用于物体
        /// </summary>
        [Tooltip("拖拽结果作用于物体")]
        public bool UseResults = true;

        [Space()]
        [Space()]

        /// <summary>
        /// 启用移动
        /// </summary>
        [Tooltip("移动")]
        public bool Mobile = true;

        /// <summary>
        /// 启用旋转 
        /// </summary>
        [Tooltip("旋转")]
        public bool Rotation = true;

        /// <summary>
        /// 开启缩放
        /// </summary>
        [Tooltip("缩放")]
        public bool Zoom = true;


        [Space()]
        [Space()]

        /// <summary>
        /// 触摸点数量限制
        /// </summary>
        [Tooltip("触摸点限制")]
        public Vector2 TouchLimit = new Vector2(1, 10);

        /// <summary>
        /// 拖拽角度限制：限制范围为 X~Y 之间
        /// </summary>
        [Tooltip("拖拽角度限制：限制范围为 X~Y 之间")]
        public List<Vector2> TouchAngleLimit = new List<Vector2>();

        [Space()]
        [Space()]

        [Tooltip("触摸调试画线")]
        public bool DebugLine = false;

        [Space()]
        [Space()]

        /// <summary>
        /// 点击触摸id顺序链表[顺序列表]，通过列表id顺序去字典提取触摸
        /// </summary>
        [Tooltip("触摸点Id顺序表")]
        public List<int> TouchIds = new List<int>();

        /// <summary>
        /// 触摸字典[无序]
        /// </summary>
        public Dictionary<int, PointerEventData> TouchPool = new Dictionary<int, PointerEventData>();



        //第一第二触摸位置

        /// <summary>
        /// 从点击池筛选出的第一个触摸点
        /// </summary>
        [System.NonSerialized]
        public Vector2 Touch_First;
        /// <summary>
        /// 从点击池筛选出的第二个触摸点，移动禁止情况下为物体的中心点
        /// </summary>
        [System.NonSerialized]
        public Vector2 Touch_Second;


        //触摸拖拽刷新位置

        /// <summary>
        /// 拖拽时计算出的双指中心点
        /// </summary>
        [System.NonSerialized]
        public Vector2 TouchCenterPointer;

        /// <summary>
        /// 上一帧的双指中心点
        /// </summary>
        [System.NonSerialized]
        public Vector2 TouchCenterPointer_Last;

        /// <summary>
        /// 拖拽时计算出的双指向量在世界坐标的角度
        /// </summary>
        [System.NonSerialized]
        public float TouchAngle;

        /// <summary>
        /// 拖拽时计算出的双指向量缩放比例差值
        /// </summary>
        [System.NonSerialized]
        public float TouchScale;

        /// <summary>
        /// 拖拽时计算出的中心点移动角度
        /// </summary>
        [System.NonSerialized]
        public float TouchDragAngle;

        /// <summary>
        /// 拖拽角度限制标志
        /// </summary>
        [System.NonSerialized]
        public bool DragAngleLimit = false;

        //触摸点之间的向量  

        /// <summary>
        /// 双指向量
        /// </summary>
        [System.NonSerialized]
        public Vector2 FirstToSecond;
        private Vector2 FirstToSecond_Last;


        //初始点击位置存档

        /// <summary>
        /// 每次点击时保存的双指中心点
        /// </summary>
        [System.NonSerialized]
        public Vector2 Save_TouchCenterPointer;

        /// <summary>
        /// 每次点击时保存的双指向量角度
        /// </summary>
        [System.NonSerialized]
        public float Save_TouchAngle;

        /// <summary>
        /// 每次点击时保存的双指向量长度
        /// </summary>
        [System.NonSerialized]
        public float Save_TouchScale;


        //初始点击偏差存档

        /// <summary>
        /// 每次点击时保存的双指中心点到物体中心的距离
        /// </summary>
        [System.NonSerialized]
        public Vector2 Save_TouchCenterToTransform_Position;

        /// <summary>
        /// 每次点击时保存的双指角度与物体z轴的角度差
        /// </summary>
        [System.NonSerialized]
        public float Save_TouchToTransform_Angle;


        //计算后的位置

        /// <summary>
        /// 计算拖拽后得到的位置
        /// </summary>
        [System.NonSerialized]
        public Vector2 Calculation_Position;

        /// <summary>
        /// 计算拖拽后得到的角度
        /// </summary>
        [System.NonSerialized]
        public float Calculation_Angle;

        /// <summary>
        /// 计算拖拽后得到的尺寸
        /// </summary>
        [System.NonSerialized]
        public Vector3 Calculation_Scale;


        //最终结果

        /// <summary>
        /// 电机计算后的位置
        /// </summary>
        [System.NonSerialized]
        public Vector2 Transform_Position;

        /// <summary>
        /// 电机计算后的角度
        /// </summary>
        [System.NonSerialized]
        public float Transform_Angle;

        /// <summary>
        /// 电机计算后的尺寸
        /// </summary>
        [System.NonSerialized]
        public Vector3 Transform_Scale;



        void Awake()
        {
            if (TouchObject == null) TouchObject = this.transform;
            if (TouchMotor != null) TouchMotor.Initialize(TouchObject);
        }

        void Start()
        {
            Calculation_Position = TouchObject.localPosition;
            Calculation_Angle = TouchObject.localEulerAngles.z;
            Calculation_Scale = TouchObject.localScale;
        }

        /// <summary>
        /// 添加触摸点
        /// </summary>
        /// <param name="eventData">触摸点</param>
        public void AddTouchData(PointerEventData eventData)
        {
            TouchIds.Add(eventData.pointerId);
            TouchPool.Add(eventData.pointerId, eventData);
        }

        /// <summary>
        /// 删除触摸点
        /// </summary>
        /// <param name="eventData">触摸点</param>
        public void RemoveTouchData(PointerEventData eventData)
        {
            TouchIds.Remove(eventData.pointerId);
            TouchPool.Remove(eventData.pointerId);
        }

        // OnPointerEnter,OnPointerDown
        public void OnPointerDown(PointerEventData eventData)//点击事件
        {
            if (TouchPool.ContainsKey(eventData.pointerId)) return;
            if (IgnoreMouse && eventData.pointerId < 0) return;//忽略鼠标

            if (Overhead) TouchObject.SetAsLastSibling();//顶置自身

            AddTouchData(eventData);

            if (TouchPool.Count < TouchLimit.x || TouchPool.Count > TouchLimit.y) return;//触摸限制

            TouchScreening();//稳定点筛选

            FirstToSecond = Touch_Second - Touch_First; //当前手势向量
            FirstToSecond_Last = FirstToSecond;         //上一次的手势向量

            //===================================

            TouchCenterPointer = Touch_First + FirstToSecond * 0.5f;    //当前手势中心点位置
            Save_TouchCenterPointer = TouchCenterPointer;               //手势中心位置存档
            TouchCenterPointer_Last = TouchCenterPointer;               //上一帧手势中心位置

            //当前手势向量角度 = 角度计算类.角度转换正负无限数值转换为正360度角（角度计算类.获得两个二维向量的角度差（二维向量.上，当前手势的向量，轴方向翻转））;
            TouchAngle = SS_EulerAngleConversion.Angle_PN_To_P360(SS_EulerAngleConversion.Get_Angle_In_Vector2Deviation(Vector2.up, FirstToSecond, false));

            Save_TouchAngle = TouchAngle;   //手势初始角度存档

            Save_TouchScale = FirstToSecond.magnitude;  //手势向量长度存档

            Calculation_Scale = TouchObject.localScale;     //缩放位置赋值

            //触摸中心到本物体的向量存档 = 真实位置- 触摸中心位置存档；
            Save_TouchCenterToTransform_Position = (Vector2)TouchObject.localPosition - Save_TouchCenterPointer;

            //手势向量角度与本物体角度差值存档 = 数学类.计算角度差（触摸向量角度存档，物体真实角度）;
            Save_TouchToTransform_Angle = Mathf.DeltaAngle(Save_TouchAngle, TouchObject.localEulerAngles.z);

            OnDrag(eventData);

            if (TouchMotor != null) TouchMotor.TouchOnDown(Calculation_Position, Calculation_Angle, Calculation_Scale);

        }

        public void OnDrag(PointerEventData eventData)//拖拽事件
        {


            if (IgnoreMouse && eventData.pointerId < 0) return;//忽略鼠标

            if (TouchPool.Count < TouchLimit.x || TouchPool.Count > TouchLimit.y) return;//触摸限制

            TouchScreening();//稳定点筛选

            FirstToSecond = Touch_Second - Touch_First;//手势向量计算

            //===================================

            TouchCenterPointer = Touch_First + FirstToSecond * 0.5f;//手势中心点位置

            if (TouchCenterPointer_Last - TouchCenterPointer != Vector2.zero)
                TouchDragAngle = SS_EulerAngleConversion.Get_Angle_In_Vector2(TouchCenterPointer_Last - TouchCenterPointer);


            if (TouchAngleLimit.Count > 0)//触摸角度限制
            {
                for (int i = 0; i < TouchAngleLimit.Count; i++)
                {
                    if (SS_Mathf.If_IntervalAngle(TouchDragAngle, TouchAngleLimit[i].x, TouchAngleLimit[i].y)) break;

                    if (i == TouchAngleLimit.Count - 1) DragAngleLimit = true;
                }
            }

            if (!DragAngleLimit)
            {

                //当前手势向量角度 = 角度计算类.角度转换正负无限数值转换为正360度角（角度计算类.获得两个二维向量的角度差（二维向量.上，当前手势的向量，轴方向翻转））;
                TouchAngle = SS_EulerAngleConversion.Angle_PN_To_P360(SS_EulerAngleConversion.Get_Angle_In_Vector2Deviation(Vector2.up, FirstToSecond, false));

                //缩放后的大小
                if ((Zoom && !Mobile) || (Zoom && TouchPool.Count > 1))
                {
                    //缩放比例差值 = ((当前手势向量长度-上一次手势向量长度)/上一次手势向量长度)
                    TouchScale = ((FirstToSecond.magnitude - FirstToSecond_Last.magnitude) / FirstToSecond_Last.magnitude);

                    //缩放后的虚拟计算大小 += 缩放比例差值 * 当前虚拟大小；
                    Calculation_Scale += TouchScale * Calculation_Scale;

                    //缩放后的 触摸点到物体中心向量的存档 += 缩放比例差值 * 触摸点到物体中心向量的存档
                    if (Mobile) Save_TouchCenterToTransform_Position += TouchScale * Save_TouchCenterToTransform_Position;
                }

                //移动后的虚拟计算位置 = 触摸中心点 + 触摸中心点到物体中心向量存档；
                if (Mobile) Calculation_Position = TouchCenterPointer + Save_TouchCenterToTransform_Position;


                //旋转后的虚拟计算位置 = 角度计算类.获取_位置_环绕旋转（环绕物：虚拟计算位置，环绕中心点：当前触摸手势中心点，轴向：世界坐标.z轴 ，旋转角度：数学类.获取角度差（手势角度存档，当前手势角度））；
                if (Mobile && Rotation) Calculation_Position = SS_EulerAngleConversion.Get_Vector3_RotateRound(Calculation_Position, TouchCenterPointer, Vector3.forward, Mathf.DeltaAngle(Save_TouchAngle, TouchAngle));

                //物体旋转后的虚拟计算角度 = 手势角度与物体角度差存档 + 当前手势角度
                if (Rotation) Calculation_Angle = Save_TouchToTransform_Angle + TouchAngle;

            }

            DragAngleLimit = false;

            //更新双指的向量值

            FirstToSecond_Last = FirstToSecond;

            TouchCenterPointer_Last = TouchCenterPointer;


            if (TouchMotor != null) TouchMotor.TouchOnDrag(Calculation_Position, Calculation_Angle, Calculation_Scale);


        }


        // OnPointerExit,OnPointerUp
        public void OnPointerUp(PointerEventData eventData)//抬起事件
        {
            if (!TouchPool.ContainsKey(eventData.pointerId)) return;
            if (IgnoreMouse && eventData.pointerId < 0) return;//忽略鼠标

            if (TouchPool.Count < TouchLimit.x || TouchPool.Count > TouchLimit.y) return;//触摸限制

            RemoveTouchData(eventData);

            TouchScreening();//稳定点筛选

            FirstToSecond = Touch_Second - Touch_First; //当前手势向量
            FirstToSecond_Last = FirstToSecond;         //上一次的手势向量

            //===================================

            TouchCenterPointer = Touch_First + FirstToSecond * 0.5f;    //当前手势中心点位置
            Save_TouchCenterPointer = TouchCenterPointer;               //手势中心位置存档

            //当前手势向量角度 = 角度计算类.角度转换正负无限数值转换为正360度角（角度计算类.获得两个二维向量的角度差（二维向量.上，当前手势的向量，轴方向翻转））;
            TouchAngle = SS_EulerAngleConversion.Angle_PN_To_P360(SS_EulerAngleConversion.Get_Angle_In_Vector2Deviation(Vector2.up, FirstToSecond, false));

            Save_TouchAngle = TouchAngle;   //手势初始角度存档

            Save_TouchScale = FirstToSecond.magnitude;  //手势向量长度存档

            //触摸中心到本物体的向量存档 =虚拟计算位置 - 触摸中心位置存档；
            Save_TouchCenterToTransform_Position = Calculation_Position - Save_TouchCenterPointer;

            //手势向量角度与本物体角度差值存档 = 数学类.计算角度差（触摸向量角度存档，虚拟计算角度）;
            Save_TouchToTransform_Angle = Mathf.DeltaAngle(Save_TouchAngle, Calculation_Angle);

            if (TouchMotor != null && TouchPool.Count < 1) TouchMotor.TouchOnEndUp(Calculation_Position, Calculation_Angle, Calculation_Scale);

        }

        public void OnScroll(PointerEventData eventData)//鼠标滚动
        {
            if (IgnoreMouse && eventData.pointerId < 0) return;//忽略鼠标
            if (!Zoom) return;

            Calculation_Scale = TouchObject.localScale;     //缩放位置赋值

            //缩放比例差值 = ((当前手势向量长度-上一次手势向量长度)/上一次手势向量长度)；
            TouchScale = eventData.scrollDelta.y * 0.1f;

            //缩放后的虚拟计算大小 +=缩放比例差值*当前虚拟大小；
            Calculation_Scale += TouchScale * Calculation_Scale;

            //缩放后的 触摸点到物体中心向量的存档 += ((当前手势向量长度-上一次手势向量长度)/上一次手势向量长度)*触摸点到物体中心向量的存档
            if (Mobile) Save_TouchCenterToTransform_Position += eventData.scrollDelta.y * 0.1f * Save_TouchCenterToTransform_Position;

        }


        /// <summary>
        /// 触摸点过滤：过滤计算出两个稳定点进行触摸计算
        /// </summary>
        private void TouchScreening()
        {
            if (TouchPool.Count == 0) return;

            if (!Mobile && (Rotation || Zoom)) //移动禁用情况
            {

                Vector3 ScreenTouchPointFirst = (TouchPriority)//触摸优先判断
                ? TouchPool[TouchIds[TouchIds.Count - 1]].position//最后一个触摸点
                : TouchPool[TouchIds[0]].position//第一个触摸点
                ;

                if (renderMode == RenderMode.ScreenSpaceOverlay)//假如没有父物体则要转世界坐标
                {
                    Touch_First = SS_CoordinateConversion.World_To_local(TouchObject.parent, ScreenTouchPointFirst);
                }
                else
                {
                    Touch_First = SS_CoordinateConversion.Screen_To_Local(TouchObject.parent, ScreenTouchPointFirst, PlaneDistance);
                }

                Touch_Second = TouchObject.localPosition;              //第二个触摸点为物体中心
            }
            else//开启移动情况
            {
                if (TouchPriority)//触摸优先判断
                {
                    //优先筛选出最新的两个触摸点
                    Vector3 ScreenTouchPointFirst = (TouchPool.Count < 2)
                    ? TouchPool[TouchIds[TouchIds.Count - 1]].position
                    : TouchPool[TouchIds[TouchIds.Count - 2]].position
                    ;
                    Vector3 ScreenTouchPointSecond = TouchPool[TouchIds[TouchIds.Count - 1]].position;

                    if (renderMode == RenderMode.ScreenSpaceOverlay)
                    {
                        Touch_First = SS_CoordinateConversion.World_To_local(TouchObject.parent, ScreenTouchPointFirst);
                        Touch_Second = SS_CoordinateConversion.World_To_local(TouchObject.parent, ScreenTouchPointSecond);
                    }
                    else//屏幕转世界坐标和屏幕直接当做世界坐标的区别//!!
                    {
                        Touch_First = SS_CoordinateConversion.Screen_To_Local(TouchObject.parent, ScreenTouchPointFirst, PlaneDistance);
                        Touch_Second = SS_CoordinateConversion.Screen_To_Local(TouchObject.parent, ScreenTouchPointSecond, PlaneDistance);
                    }

                }
                else
                {
                    //使用最早的两个触摸点
                    Vector3 ScreenTouchPointFirst = TouchPool[TouchIds[0]].position;
                    Vector3 ScreenTouchPointSecond = (TouchPool.Count < 2)
                   ? TouchPool[TouchIds[0]].position
                   : TouchPool[TouchIds[1]].position
                   ;

                    if (renderMode == RenderMode.ScreenSpaceOverlay)
                    {
                        Touch_First = SS_CoordinateConversion.World_To_local(TouchObject.parent, ScreenTouchPointFirst);
                        Touch_Second = SS_CoordinateConversion.World_To_local(TouchObject.parent, ScreenTouchPointSecond);
                    }
                    else
                    {
                        Touch_First = SS_CoordinateConversion.Screen_To_Local(TouchObject.parent, ScreenTouchPointFirst, PlaneDistance);
                        Touch_Second = SS_CoordinateConversion.Screen_To_Local(TouchObject.parent, ScreenTouchPointSecond, PlaneDistance);
                    }
                }
            }

        }


        /// <summary>
        /// 刷新Transform为当前：用于解除限制时的虚拟位置矫正
        /// </summary>
        public void Refresh_Touch()
        {
            Calculation_Position = TouchObject.localPosition;
            Calculation_Angle = TouchObject.localEulerAngles.z;
            Calculation_Scale = TouchObject.localScale;

            if (TouchMotor != null) TouchMotor.Refresh();
        }

        private void FixedUpdate()//SDHK临时修改:Update肯能造成视觉卡顿？？？
        {
            if (TouchMotor != null)//判断电机是否存在
            {
                Transform_Position = TouchMotor.TouchMortor_Mobile(Calculation_Position);
                Transform_Angle = TouchMotor.TouchMortor_Rotation(Calculation_Angle);
                Transform_Scale = TouchMotor.TouchMortor_Zoom(Calculation_Scale);
            }
            else//不存在直接赋值
            {
                Transform_Position = Calculation_Position;
                Transform_Angle = Calculation_Angle;
                Transform_Scale = Calculation_Scale;
            }

            if (UseResults)//判断是否计算结果直接作用于物体
            {
                TouchObject.localPosition = Transform_Position;
                TouchObject.SE_LocalEulerAngles_Z(Transform_Angle);
                TouchObject.localScale = Transform_Scale;
            }


            if (DebugLine)
            {
                Debug.DrawLine(TouchObject.parent.SE_Local_To_World(Touch_First), TouchObject.parent.SE_Local_To_World(Touch_Second), Color.red);
                Debug.DrawLine(TouchObject.parent.SE_Local_To_World(Touch_First), TouchObject.parent.SE_Local_To_World(Calculation_Position), Color.green);
                Debug.DrawLine(TouchObject.parent.SE_Local_To_World(Touch_Second), TouchObject.parent.SE_Local_To_World(Calculation_Position), Color.green);
                Debug.DrawLine(TouchObject.parent.SE_Local_To_World(TouchCenterPointer), TouchObject.parent.SE_Local_To_World(Calculation_Position), Color.yellow);
            }
        }
        void Update()
        {


            // if (TouchMotor != null)//判断电机是否存在
            // {
            //     Transform_Position = TouchMotor.TouchMortor_Mobile(Calculation_Position);
            //     Transform_Angle = TouchMotor.TouchMortor_Rotation(Calculation_Angle);
            //     Transform_Scale = TouchMotor.TouchMortor_Zoom(Calculation_Scale);
            // }
            // else//不存在直接赋值
            // {
            //     Transform_Position = Calculation_Position;
            //     Transform_Angle = Calculation_Angle;
            //     Transform_Scale = Calculation_Scale;
            // }

            // if (UseResults)//判断是否计算结果直接作用于物体
            // {
            //     TouchObject.localPosition = Transform_Position;
            //     TouchObject.SE_LocalEulerAngles_Z(Transform_Angle);
            //     TouchObject.localScale = Transform_Scale;
            // }


            // if (DebugLine)
            // {
            //     Debug.DrawLine(TouchObject.parent.SE_Local_To_World(Touch_First), TouchObject.parent.SE_Local_To_World(Touch_Second), Color.red);
            //     Debug.DrawLine(TouchObject.parent.SE_Local_To_World(Touch_First), TouchObject.parent.SE_Local_To_World(Calculation_Position), Color.green);
            //     Debug.DrawLine(TouchObject.parent.SE_Local_To_World(Touch_Second), TouchObject.parent.SE_Local_To_World(Calculation_Position), Color.green);
            //     Debug.DrawLine(TouchObject.parent.SE_Local_To_World(TouchCenterPointer), TouchObject.parent.SE_Local_To_World(Calculation_Position), Color.yellow);
            // }

        }



    }
}