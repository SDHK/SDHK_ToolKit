
/****************************************

* 作者： 闪电黑客
* 日期： 2022/8/1 9:47

* 描述： 系统集合组

*/

using System;

namespace WorldTree
{

    /// <summary>
    /// 系统集合组
    /// </summary>
    public class SystemGroup : UnitDictionary<Type, UnitList<ISystem>>, IUnitPoolItem
    {
       

        /// <summary>
        /// 获取系统类列表
        /// </summary>
        public UnitList<ISystem> GetSystems(Type type)
        {
            UnitList<ISystem> Isystems;
            if (!TryGetValue(type, out Isystems))
            {
                Isystems = new UnitList<ISystem>();
                Add(type, Isystems);
            }

            return Isystems;
        }


        public override void OnRecycle()
        {
            foreach (var systemList in this)
            {
                systemList.Value.Clear();
            }
            Clear();
        }
    }
}
