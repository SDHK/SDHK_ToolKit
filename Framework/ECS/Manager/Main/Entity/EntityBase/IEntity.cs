using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {

        /// <summary>
        /// 回收标记
        /// </summary>
        public bool IsRecycle { get; set; }
        /// <summary>
        /// 释放标记
        /// </summary>
        public bool IsDisposed { get; set; }

        /// <summary>
        /// 组件标记
        /// </summary>
        public bool IsComponent { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public ulong Id { get; set; }
        /// <summary>
        /// 实体类型
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 根节点
        /// </summary>
        public static IEntity Root { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>
        public IEntity Parent { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public UnitDictionary<ulong, IEntity> Children { get; }
        /// <summary>
        /// 组件
        /// </summary>
        public UnitDictionary<Type, IEntity> Components { get; }

        /// <summary>
        /// 添加子节点
        /// </summary>
        public void AddChildren(IEntity entity);


        /// <summary>
        /// 添加新的子节点
        /// </summary>
        public T GetChildren<T>() where T : class, IEntity;
        

        /// <summary>
        /// 移除子节点
        /// </summary>
        public void RemoveChildren(IEntity entity);
        /// <summary>
        /// 移除全部子节点
        /// </summary>
        public void RemoveAllChildren();

        /// <summary>
        /// 添加组件
        /// </summary>
        public T GetComponent<T>() where T : class, IEntity;
        /// <summary>
        /// 添加组件
        /// </summary>
        public void AddComponent(IEntity entity);

        /// <summary>
        /// 移除组件
        /// </summary>
        public void RemoveComponent<T>() where T : class, IEntity;
       
        /// <summary>
        /// 移除组件
        /// </summary>
        public void RemoveComponent(IEntity entity);
        
        /// <summary>
        /// 移除全部组件
        /// </summary>
        public void RemoveAllComponent();
        
        /// <summary>
        /// 移除全部组件和子节点
        /// </summary>
        public void RemoveAll();

        /// <summary>
        /// 回收自己
        /// </summary>
        public void Recycle();
    }
}
