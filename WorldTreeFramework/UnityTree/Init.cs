
/****************************************

* 作者： 闪电黑客
* 日期： 2022/7/18 9:35

* 描述：Unity环境下的启动端，一切从这里开始

*/

using UnityEngine;

namespace WorldTree
{

    public class Main : Entity { }


    public class Init : MonoBehaviour
    {
        public EntityManager root;

        UpdateManager update;
        LateUpdateManager lateUpdate;
        FixedUpdateManager fixedUpdate;

        private void Start()
        {
            World.Log = Debug.Log;
            World.LogWarning = Debug.LogWarning;
            World.LogError = Debug.LogError;

            root = new EntityManager();

            update = root.AddComponent<UpdateManager>();
            lateUpdate = root.AddComponent<LateUpdateManager>();
            fixedUpdate = root.AddComponent<FixedUpdateManager>();
            root.AddComponent<Main>();
            Debug.Log(root.ToStringDrawTree());
           
        }

        private void Update()
        {
            update.deltaTime = Time.deltaTime;  
            update.Update();
          
            if (Input.GetKeyDown(KeyCode.Return)) Debug.Log(root.ToStringDrawTree());
        }

        private void LateUpdate()
        {
            lateUpdate.deltaTime = Time.deltaTime;
            lateUpdate.Update();
        }
        private void FixedUpdate()
        {
            fixedUpdate.deltaTime = Time.fixedDeltaTime;
            fixedUpdate.Update();
        }

        private void OnDestroy()
        {
            update = null;
            lateUpdate = null;
            fixedUpdate = null;
            root.Dispose();

            Debug.Log(root.ToStringDrawTree());
        }
        private void OnApplicationQuit()
        {

        }


    }
}
