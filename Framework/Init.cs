using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    public class Init : SingletonMonoEagerBase<Init>
    {

        SoloistFramework soloist;
        private void Start()
        {
            soloist = SoloistFramework.Instance;

            soloist.Start();
        }

        private void Update()
        {
            soloist.Update();

        }

        private void LateUpdate()
        {
            soloist.LateUpdate();

        }
        private void FixedUpdate()
        {
            soloist.FixedUpdate();

        }

        private void OnDestroy()
        {
            soloist.OnInstance();

        }
        private void OnApplicationQuit()
        {

        }
    }
}
