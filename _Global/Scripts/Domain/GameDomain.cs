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

    public class GameDomain : EntityDomain
    {
        public UpdateManager update;
        public LateUpdateManager lateUpdate;
        public FixedUpdateManager fixedUpdate;
    }

    class GameDomainAddSystem : NewSystem<GameDomain>
    {
        public override void OnNew(GameDomain self)
        {
            self.OnNew();
            //UpdateService
            self.update = self.GetComponent<UpdateManager>();
            self.lateUpdate = self.GetComponent<LateUpdateManager>();
            self.fixedUpdate = self.GetComponent<FixedUpdateManager>();
        }
    }


    class GameDomainRemoveSystem : RecycleSystem<GameDomain>
    {
        public override void OnRecycle(GameDomain self)
        {
            self.OnRecycle();
        }
    }

    class GameDomainUpdateSystem : UpdateSystem<GameDomain>
    {
        public override void Update(GameDomain self)
        {
            self.update.Update();
        }
    }

    class GameDomainLateUpdateSystem : LateUpdateSystem<GameDomain>
    {
        public override void LateUpdate(GameDomain self)
        {
            self.lateUpdate.Update();
        }
    }

    class GameDomainFixedUpdateSystem : FixedUpdateSystem<GameDomain>
    {
        public override void FixedUpdate(GameDomain self)
        {
            self.fixedUpdate.Update();
        }
    }
}
