using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SDHK_Tool.Dynamic;
using SDHK_Tool.Static;
using SDHK_Tool.Extension;
using UnityEngine;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.11.04
 *
 * 2019.11.27 改进为对象池列表
 * 
 * 功能：
 * 
 * 动态列表，发放编号，并进行排列
 * 更具父物体高宽来计算内容盒子高宽
 * 更具自身位置来移动物体并修改编号
 *
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// 动态列表
    /// </summary>
    public class SC_ScrollGroup : MonoBehaviour
    {

        #region 公开属性

        /// <summary>
        /// 预制体
        /// </summary>
        [Tooltip("要生成的预制体")]
        public GameObject Box_Pfb;

        [Space()]
        [Space()]

        /// <summary>
        /// 内容数量
        /// </summary>
        [Tooltip("要生成的内容数")]
        public int BoxCount = 10;

        [Space()]
        [Space()]

        /// <summary>
        /// 列表循环
        /// </summary>
        [Tooltip("列表循环")]
        public bool Loop = false;

        [Space()]
        [Space()]

        /// <summary>
        /// 动态列表内容处理器：必须有
        /// </summary>
        [Tooltip("动态列表内容处理器：必须有")]
        public SB_ScrollGroup_BoxProcessor ScrollGroup_Base;

        [Space()]
        [Space()]

        /// <summary>
        /// 垂直排列
        /// </summary>
        [Tooltip("垂直排列")]
        public bool isVertical = false;//垂直排列

        [Space()]
        [Space()]

        /// <summary>
        /// 预加载数:预先加载的 列/行 数量
        /// </summary>
        [Tooltip("预加载数: 预先加载的 列/行 数量")]
        public int Preload = 2;

        [Space()]
        [Space()]

        /// <summary>
        /// 内容盒子间隔宽高度
        /// </summary>
        [Tooltip("内容盒子间隔宽高度")]
        public Vector2 BoxInterval = new Vector2(10, 10);

        /// <summary>
        /// 列表侧面顶部间隔[设置为间隔的一般即可]
        /// </summary>
        [Tooltip("列表侧面顶部间隔[设置为间隔的一半即可]")]
        public Vector2 GroupGap = new Vector2(5, 5);

        [Space()]
        [Space()]

        /// <summary>
        /// 列行数[x为列，y为行]
        /// </summary>
        [Tooltip("列行数[必须整数]（显示数量）")]
        public Vector2 GroupList = new Vector2(1, 1);

        [Space()]
        [Space()]

        /// <summary>
        /// 脚本计算出来的内容盒子高宽_画布坐标
        /// </summary>
        [Tooltip("脚本计算出来的内容盒子高宽_画布坐标")]
        public Vector2 BoxSize;

        /// <summary>
        /// 脚本计算出来的格子高宽_画布坐标
        /// </summary>
        [Tooltip("脚本计算出来的格子高宽_画布坐标")]
        public Vector2 GroupBox;

        /// <summary>
        /// 对象池句柄
        /// </summary>
        /// <typeparam name="int">编号</typeparam>
        /// <typeparam name="GameObject">储存物体</typeparam>
        public Dictionary<int, GameObject> IdPool = new Dictionary<int, GameObject>();

        /// <summary>
        /// 对象池
        /// </summary>
        public SD_ObjectPool<GameObject> objectPool;


        #endregion

        #region 私有属性

        private List<int> BoxsId = new List<int>(); //当前存在的ID
        private List<int> NewBoxsId = new List<int>();//新生成的ID

        private List<int> EnterIds; //添加ID集合
        private List<int> ExitIds;  //删除ID集合

        private int PointerId = 0;//指针当前指向的Id

        private int LatePointerId = 1;//指针上一次Id

        private int Num;//数据Id

        private RectTransform ThisTransform; //当前物体

        private RectTransform ParentTransform;//父物体

        private float GroupPosition;//列表位置

        private Vector2 groupList; //行列转换器

        private Vector2 BoxSize_Half; //盒子一半大小：用于盒子放置的位置计算
        private Vector2 GroupBox_multiple; //格子两倍大小：用于列表预加载的位置偏移

        #endregion

        private void Start()
        {
            //===[初始化动态列表]=====

            objectPool = new SD_ObjectPool<GameObject>(() => Instantiate(Box_Pfb), (Box) => Destroy(Box));

            ParentTransform = (RectTransform)transform.parent;//获取父物体面板
            ThisTransform = (RectTransform)transform;//获取本物体面板

            ParentTransform.pivot = new Vector2(0, 1);//设置中心点为左上角
            ParentTransform.anchorMin = new Vector2(0, 1);//设置锚点为左上角
            ParentTransform.anchorMax = new Vector2(0, 1);

            ThisTransform.pivot = new Vector2(0, 1);//设置中心点为左上角
            ThisTransform.anchorMin = new Vector2(0, 1);//设置锚点为左上角
            ThisTransform.anchorMax = new Vector2(0, 1);

            //初始化
            Refresh();

            ThisTransform.anchoredPosition3D = Vector3.zero;//动态列表面板归零（需要移动这个）

        }

        private void GroupRefresh()//列表计算刷新
        {
            if (GroupList.x < 1) GroupList.x = 1;//列表行列数不能为0
            if (GroupList.y < 1) GroupList.y = 1;//列表行列数不能为0

            BoxSize.x = ParentTransform.sizeDelta.x / (int)GroupList.x - BoxInterval.x;//计算内容盒子高宽
            BoxSize.y = ParentTransform.sizeDelta.y / (int)GroupList.y - BoxInterval.y;

            GroupBox.x = (BoxSize.x + BoxInterval.x);//计算格子宽高
            GroupBox.y = (Mathf.Abs(-BoxSize.y - BoxInterval.y));

            BoxSize_Half = BoxSize * 0.5f;
            GroupBox_multiple = GroupBox * Preload;
        }

        /// <summary>
        /// 列表刷新
        /// </summary>
        [ContextMenu("列表刷新")]
        public void Refresh()
        {
            Exit(new List<int>(BoxsId));

            objectPool.Clear_ObjectPool(); IdPool.Clear();//对象池清空

            if (ScrollGroup_Base != null)//对象池委托绑定
            {
                objectPool.Object_New = (obj) =>
                {
                    obj.transform.SetParent(transform);
                    obj.transform.localScale = Vector3.one;
                    obj.transform.SE_LocalPosition_Z(0);
                    ScrollGroup_Base.GroupBox_New(obj, Num);
                    obj.transform.SetParent(this.transform);
                };

                objectPool.Object_Del = (obj) => ScrollGroup_Base.GroupBox_Del(obj, Num);

                objectPool.Object_Work = (obj) =>
                {
                    ScrollGroup_Base.GroupBox_Work(obj, Num);//数据加载
                };

                objectPool.Object_Idle = (obj) => ScrollGroup_Base.GroupBox_Idle(obj, Num);

                ScrollGroup_Base.RefreshGroup();
            }


            GroupRefresh();
            Generate_List();

        }

        void Update()
        {
            if (isVertical)
            {
                GroupPosition = ThisTransform.anchoredPosition3D.y;//获取当前物体位置
                PointerId = SS_Mathf.NearlyNumber(GroupPosition - GroupBox_multiple.y, GroupBox.y);//获取当前列表指针 （无法确定整数）
                groupList.x = GroupList.y;//生成的列表长宽
                groupList.y = GroupList.x;
            }
            else
            {
                GroupPosition = -ThisTransform.anchoredPosition3D.x;//获取当前物体位置
                PointerId = SS_Mathf.NearlyNumber(GroupPosition - GroupBox_multiple.x, GroupBox.x);//获取当前列表指针（无法确定整数）
                groupList = GroupList;//生成的列表长宽
            }


            if (PointerId != LatePointerId) Generate_List();//指针变动时刷新
            LatePointerId = PointerId;//ID指针刷新

        }

        private void Generate_List()//列表生成
        {
            NewBoxsId.Clear();//清空ID集合

            for (int x = 0; x < groupList.x + Preload * 2; x++) for (int y = 0; y < groupList.y; y++)//添加要生成的ID集合  （无法确定整数）
                    NewBoxsId.Add(((x + PointerId) * (int)groupList.y + y));

            ExitIds = BoxsId.Except(NewBoxsId).ToList(); //当前集合 剔除 新集合 得到删除列表
            EnterIds = NewBoxsId.Except(BoxsId).ToList(); //新集合 剔除 当前集合 得到生成列表

            Exit(ExitIds);//删除物体
            Enter(EnterIds);//生成物体

        }


        private void Enter(List<int> Ids)//生成物体
        {
            for (int i = 0; i < Ids.Count; i++)
            {
                if (BoxCount < 1) return; //数量不能为0
                Num = (Loop) ? SS_Mathf.Int_Loop(Ids[i], BoxCount) : Ids[i]; //换算成整数循环

                if (!(Loop || SS_Mathf.If_IntervalValue(Num, 0, BoxCount - 1))) continue;//跳过这次循环

                GameObject BoxObject = objectPool.Get_Object();
                // print("工作对象池：" + objectPool.ObjectPool_Work.Count + " 闲置对象池：" + objectPool.ObjectPool_Idle.Count);


                IdPool.Add(Ids[i], BoxObject);

                if (BoxObject != null)
                {
                    Vector2 List2D = SS_Mathf.List1D_To_List2D(Ids[i], (int)groupList.y);//下标换算

                    Vector2 BoxPosition;//根据二维下标计算位置
                    BoxPosition.x = ((isVertical) ? List2D.y : List2D.x) * GroupBox.x + GroupGap.x + BoxSize_Half.x;
                    BoxPosition.y = ((isVertical) ? List2D.x : List2D.y) * -GroupBox.y - GroupGap.y - BoxSize_Half.y;


                    ((RectTransform)BoxObject.transform).sizeDelta = new Vector2(BoxSize.x, BoxSize.y);//设置内容盒子高宽
                    ((RectTransform)BoxObject.transform).anchorMin = new Vector2(0, 1);//设置锚点为中心点
                    ((RectTransform)BoxObject.transform).anchorMax = new Vector2(0, 1);
                    ((RectTransform)BoxObject.transform).anchoredPosition = BoxPosition;//位置放置
                    BoxsId.Add(Ids[i]);//Id存入列表

                }

            }
        }

        private void Exit(List<int> Ids)//删除物体
        {
            for (int i = 0; i < Ids.Count; i++)
            {
                objectPool.Set_Object(IdPool[Ids[i]]);
                IdPool.Remove(Ids[i]);

                BoxsId.Remove(Ids[i]);//Id列表删除
                Resources.UnloadUnusedAssets();//释放无用对象
                // System.GC.Collect();
            }

        }



    }
}