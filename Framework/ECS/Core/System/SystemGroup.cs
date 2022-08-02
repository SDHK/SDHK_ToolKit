using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{

    /// <summary>
    /// 系统集合组
    /// </summary>
    public class SystemGroup : UnitDictionary<Type, UnitList<ISystem>>, IUnitPoolItem
    {
        /// <summary>
        /// 单位对象池：获取对象
        /// </summary>
        public new static SystemGroup GetObject()
        {
            return UnitPoolManager.Instance.Get<SystemGroup>();
        }


        /// <summary>
        /// 获取系统类列表
        /// </summary>
        public UnitList<ISystem> GetSystems(Type type)
        {
            UnitList<ISystem> Isystems;
            if (!TryGetValue(type, out Isystems))
            {
                Isystems = UnitList<ISystem>.GetObject();
                Add(type, Isystems);
            }

            return Isystems;
        }


        public override void OnRecycle()
        {
            foreach (var systemList in this)
            {
                systemList.Value.Clear();
                systemList.Value.Recycle();
            }
            Clear();
        }
    }
}
