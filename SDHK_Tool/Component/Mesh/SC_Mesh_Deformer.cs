
using System.Collections.Generic;
using SDHK_Tool.Static;
using UnityEngine;
// using SDHK_Tool.Extension;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.8.29
 * 
 * 2019.10.25 与绑定器功能合并
 * 
 * 功能：用于Mesh的扭曲器
 */

namespace SDHK_Tool.Component
{
    /// <summary>
    /// Mesh扭曲器
    /// </summary>
    public class SC_Mesh_Deformer : MonoBehaviour
    {

        /// <summary>
        /// 动态刷新激活
        /// </summary>
        [Tooltip("动态刷新激活")]
        public bool Dynamic;

        /// <summary>
        /// 绑定画线
        /// </summary>
        [Tooltip("绑定画线")]
        public bool Debug_Line = true;

        /// <summary>
        /// 模型绑定存档路径
        /// </summary>
        [Tooltip("模型绑定存档路径")]
        public string FilePath = "Mesh_Save";

        /// <summary>
        /// 绑定骨骼
        /// </summary>
        [Tooltip("绑定骨骼")]
        public SB_Mesh_Skeleton mesh_Skeleton;

        private MeshBinder Mesh_Binder = new MeshBinder();
        private Mesh mesh;
        private List<Vector3> mesh_vertices;



        private float ShortDistance = 0;//最短距离
        private Vector3 OffsetSave;//偏移存档
        private int index;//节点编号


        // Use this for initialization

        private void Awake()
        {
            RefreshDeformer();
        }

        // Update is called once per frame
        void Update()
        {
            if (Dynamic) Deformer();
        }

        private void Initialize()
        {
            try { mesh = GetComponent<MeshFilter>().mesh; } catch { }
            mesh_vertices = new List<Vector3>();
            mesh_vertices.AddRange(mesh.vertices);
        }

        [ContextMenu("模型绑定到存档")]
        void StartBind()
        {
            Initialize();
            Bind();
        }

        [ContextMenu("模型扭曲刷新")]
        private void RefreshDeformer()
        {
            Initialize();
            if (FilePath != "") Mesh_Binder = SS_File.GetFile_ByteObject<MeshBinder>(Application.streamingAssetsPath + FilePath);
            Deformer();
        }

        /// <summary>
        /// 绑定方法
        /// </summary>
        private void Bind()
        {
            Mesh_Binder = new MeshBinder();

            for (int i = 0; i < mesh.vertices.Length; i++)//遍历mesh全点
            {
                ShortDistance = Mathf.Infinity;

                for (int i1 = 0; i1 < mesh_Skeleton.Pionts.Count; i1++)//遍历骨架线节点
                {
                    Vector3 Offset = (mesh_Skeleton.Pionts[i1].SE_World_To_local(transform.SE_Local_To_World(mesh.vertices[i])));

                    if (Offset.magnitude < ShortDistance)//取最近骨架节点
                    {
                        ShortDistance = Offset.magnitude;
                        OffsetSave = Offset;
                        index = i1;
                    }
                }

                if (Mesh_Binder.MeshBind.ContainsKey(index.ToString()))//骨架编号与mesh节点编号绑定
                {
                    Mesh_Binder.MeshBind[index.ToString()].Add(i.ToString(), OffsetSave);
                }
                else
                {
                    Mesh_Binder.MeshBind.Add(index.ToString(), new Dictionary<string, SF_Vector3>());
                    Mesh_Binder.MeshBind[index.ToString()].Add(i.ToString(), OffsetSave);
                }
                index = 0;
            }

            string path = Application.streamingAssetsPath + FilePath;
            SS_File.SetFile_byteObject(Mesh_Binder, path);//绑定过程太慢所以需要存档
        }

        /// <summary>
        /// 扭曲变形
        /// </summary>
        private void Deformer()
        {
            for (int i = 0; i < mesh_Skeleton.Pionts.Count; i++)
            {
                if (Mesh_Binder.MeshBind.ContainsKey(i.ToString()))
                {
                    foreach (var Mesh_Ids in Mesh_Binder.MeshBind[i.ToString()])
                    {
                        mesh_vertices[int.Parse(Mesh_Ids.Key)] = transform.SE_World_To_local(mesh_Skeleton.Pionts[i].SE_Local_To_World(Mesh_Ids.Value));
                    }
                }
            }
            mesh.vertices = mesh_vertices.ToArray();
            mesh.RecalculateNormals();//刷新法线
        }





#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (mesh_Skeleton != null && Debug_Line) DeBugDraw();
        }
#endif
        private void DeBugDraw()
        {
            for (int i = 0; i < mesh_Skeleton.Pionts.Count; i++)
            {
                if (Mesh_Binder.MeshBind.ContainsKey(i.ToString()))
                {
                    foreach (var Mesh_Ids in Mesh_Binder.MeshBind[i.ToString()])
                    {
                        Debug.DrawLine(mesh_Skeleton.Pionts[i].position, mesh_Skeleton.Pionts[i].SE_Local_To_World(Mesh_Ids.Value), (i % 2 == 0) ? Color.yellow : Color.red);
                    }
                }
            }

        }

    }



    /// <summary>
    /// 用于序列化存档的网格绑定类
    /// </summary>
    [System.Serializable]
    class MeshBinder
    {
        public Dictionary<string, Dictionary<string, SF_Vector3>> MeshBind = new Dictionary<string, Dictionary<string, SF_Vector3>>();
    }
}