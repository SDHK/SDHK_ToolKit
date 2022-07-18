using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{

    public class MainEntity : SingletonEntityBase<MainEntity> { }
   
    public class Init : MonoBehaviour
    {

        UpdateManager update;
        LateUpdateManager lateUpdate;
        FixedUpdateManager fixedUpdate;

        SoloistFramework soloist;
        private void Start()
        {
            soloist = SoloistFramework.GetInstance();

            update = UpdateManager.GetInstance();
            lateUpdate = LateUpdateManager.GetInstance();
            fixedUpdate = FixedUpdateManager.GetInstance();

            MainEntity.GetInstance();
            Debug.Log(soloist.AllEntityString(RootEntity.Root, "\t"));

        }

        private void Update()
        {
            update.Update();
        }

        private void LateUpdate()
        {
            lateUpdate.Update();
        }
        private void FixedUpdate()
        {
            fixedUpdate.Update();
        }

        private void OnDestroy()
        {
            soloist.Dispose();
            update = null;
            lateUpdate = null;
            fixedUpdate = null;
            Debug.Log(soloist.AllEntityString(RootEntity.Root, "\t"));
        }
        private void OnApplicationQuit()
        {

        }
    }
}
