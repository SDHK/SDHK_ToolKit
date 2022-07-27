using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
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
            self.update = self.GetComponent<UpdateManager>();
            self.lateUpdate = self.GetComponent<LateUpdateManager>();
            self.fixedUpdate = self.GetComponent<FixedUpdateManager>();
            self.GetChildren<Node>();
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
