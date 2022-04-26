using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDHK_Tool.Dynamic;
using UnityEngine.SceneManagement;
using Msg;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.8.4
 * 
 * 功能：任务执行器 紧急组件版
 * 
 * 参考Loom方式,建立任务执行池统一执行
 * 
 * 时间紧张，写的不完善。
 *
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// 任务执行器
    /// </summary>
    public class SC_TaskActuator : MonoBehaviour
    {
        private static SC_TaskActuator Current;

        //任务池
        private List<SD_TaskActuator> task_Pool = new List<SD_TaskActuator>();
        //删除池
        private List<SD_TaskActuator> Del_Pool = new List<SD_TaskActuator>();

        //让静态方法在程序启动时执行  
        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            print("简易版TaskActuator启动！");
            var g = new GameObject("TaskActuator");
            Current = g.AddComponent<SC_TaskActuator>();
#if !ARTIST_BUILD
            UnityEngine.Object.DontDestroyOnLoad(g);
#endif
        }

        /// <summary>
        /// 获取一个任务执行器
        /// </summary>
        /// <returns>任务执行器</returns>
        public static SD_TaskActuator Get_Actuator()
        {
            lock (Current.task_Pool)
            {
                SD_TaskActuator TA = new SD_TaskActuator();
                Current.task_Pool.Add(TA);
                return TA;
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                "01".MsgSend(1);
                "01".MsgSend("11");
            }
            if (Input.GetKeyDown(KeyCode.E))
            {

                "01".MsgRemove<int>(ceshi002.L004);
            }
            
            if (Input.GetKeyDown(KeyCode.G))
            {
                SceneManager.LoadSceneAsync("cj005");

            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                SceneManager.LoadSceneAsync("cj004");

            }



            //foreach循环修改会报错

            lock (Current.task_Pool)
            {
                task_Pool.Remove(null);

                for (int i = 0; i < task_Pool.Count; i++)
                {
                    if (task_Pool[i].isEnd) Del_Pool.Add(task_Pool[i]);
                    task_Pool[i].Update();
                }
            }

            lock (Current.Del_Pool)
            {
                Del_Pool.Remove(null);

                for (int i = 0; i < Del_Pool.Count; i++)
                {
                    task_Pool.Remove(Del_Pool[i]);

                }
                Del_Pool.Clear();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("task_Pool:" + task_Pool.Count + " Del_Pool:" + Del_Pool.Count);
            }
        }
    }
}