using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDHK
{
    public class Init:SingletonMonoEagerBase<Init>
    {

        private void Start()
        {
            SoloistFramework.Instance.Start();

        }

        private void Update()
        {
            SoloistFramework.Instance.Update();

        }

        private void LateUpdate()
        {
            SoloistFramework.Instance.LateUpdate();

        }
        private void FixedUpdate()
        {
            SoloistFramework.Instance.FixedUpdate();

        }

        private void OnDestroy()
        {
            SoloistFramework.Instance.OnInstance();

        }
        private void OnApplicationQuit()
        {
            
        }
    }
}
