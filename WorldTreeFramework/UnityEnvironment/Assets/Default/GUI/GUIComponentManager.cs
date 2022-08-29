/****************************************

* 作者： 闪电黑客
* 日期： 2022/8/29 21:41

* 描述： 

*/
namespace WorldTree
{
    public class GUIComponentManager : Entity
    {
        public int Size;
        public UnitDictionary<long, Entity> Guis = new UnitDictionary<long, Entity>();




    }

    class GUIComponentManagerEntitySystem : EntitySystem<GUIComponentManager>
    {
        public override void OnAddEntity(GUIComponentManager self, Entity entity)
        {

        }

        public override void OnRemoveEntity(GUIComponentManager self, Entity entity)
        {

        }
    }


}
