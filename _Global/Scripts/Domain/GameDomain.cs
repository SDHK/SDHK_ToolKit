using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{

    // 域单例组件的思考
    // UpdateManager系列的 特殊化的的自我域嵌套
    // 系统管理和对象池在根的思考
    // 域的减责思考
    // 减责后域应该能自我化。从而减去子节点的判断

    public class GameDomain : Entity
    {

    }

    class GameDomainAddSystem : AddSystem<GameDomain>
    {
        public override void OnAdd(GameDomain self)
        {
        
            Debug.Log(self.Domain);
        }
    }


}
