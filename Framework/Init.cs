
/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/18 9:35

* 描述： Mono的启动端，一切从这里开始

*/

using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SDHK
{
    /// <summary>
    /// 主节点
    /// </summary>
    public class MainEntity : SingletonEntityBase<MainEntity> { }


    public class Init : MonoBehaviour
    {
        public MainEntity entity;

        SoloistFramework soloist;


        UpdateManager update;
        LateUpdateManager lateUpdate;
        FixedUpdateManager fixedUpdate;

        private void Start()
        {
            soloist = SoloistFramework.GetInstance();

            update = UpdateManager.GetInstance();
            lateUpdate = LateUpdateManager.GetInstance();
            fixedUpdate = FixedUpdateManager.GetInstance();

            entity= MainEntity.GetInstance();
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
