
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
    public class MainEntity :Entity { }


    public class Init : MonoBehaviour
    {
        public MainEntity entity;

        SoloistFramework soloist;


        UpdateManager update;
        LateUpdateManager lateUpdate;
        FixedUpdateManager fixedUpdate;

        private void Start()
        {
            soloist =  SoloistFramework.GetInstance();

            update= soloist.root.GetComponent<UpdateManager>();
            lateUpdate = soloist.root.GetComponent<LateUpdateManager>();
            fixedUpdate = soloist.root.GetComponent<FixedUpdateManager>();

            soloist.root.GetComponent<MainEntity>();

            Debug.Log(SoloistFramework.AllEntityString(soloist.root, "\t"));

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
            Debug.Log(SoloistFramework.AllEntityString(soloist.root, "\t"));
        }
        private void OnApplicationQuit()
        {

        }

    }
}
