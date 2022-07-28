using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{ 
    public class UiDomain : EntityDomain
    {

        public UpdateManager update;
        public LateUpdateManager lateUpdate;
        public FixedUpdateManager fixedUpdate;

    }

    class UiDomainAddSystem : NewSystem<UiDomain>
    {
        public override void OnNew(UiDomain self)
        {
            self.OnNew();
            self.update = self.GetComponent<UpdateManager>();
            self.lateUpdate = self.GetComponent<LateUpdateManager>();
            self.fixedUpdate = self.GetComponent<FixedUpdateManager>();
        }
    }


    class UiDomainRemoveSystem : RecycleSystem<UiDomain>
    {
        public override void OnRecycle(UiDomain self)
        {
            self.OnRecycle();
        }
    }

    class UiDomainUpdateSystem : UpdateSystem<UiDomain>
    {
        public override void Update(UiDomain self)
        {
            self.update.Update();
        }
    }

    class UiDomainLateUpdateSystem : LateUpdateSystem<UiDomain>
    {
        public override void LateUpdate(UiDomain self)
        {
            self.lateUpdate.Update();
        }
    }

    class UiDomainFixedUpdateSystem : FixedUpdateSystem<UiDomain>
    {
        public override void FixedUpdate(UiDomain self)
        {
            self.fixedUpdate.Update();
        }
    }
}
