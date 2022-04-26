


/******************************

 * 作者: 闪电黑客

 * 日期: 2021/07/15 14:44:29

 * 最后日期: 。。。

 * 描述: 
    按键输入管理器

    简易的按键绑定功能，实现自定义按键绑定

    自动全局单例化，需要先载入文件数据才可以使用。

    未来可能新增功能：按键组合招式判断

    

******************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using System.IO;
using System.Text.RegularExpressions;
using Singleton;
using SDHK_Extension;
using UnityEngine.Events;
using System.Threading.Tasks;
using AsyncAwaitEvent;

namespace InputKeys
{


    public class InputKeysManager : SingletonMonoBase<InputKeysManager>
    {

        private void Start()
        {
            if (instance == null)
            {
                instance = this;
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                Debug.Log("InputKeysManager 启动！！");
                Load();
            }
        }

        public ApplicationPathEnum RootPath = ApplicationPathEnum.Path;
        [Space()]

        public string path = "";
        public bool isDone = true;

        public Dictionary<string, Dictionary<string, InputKeyCodes>> inputKeyGroups = new Dictionary<string, Dictionary<string, InputKeyCodes>>();

        private bool isReplace = false;

        /// <summary>
        /// 判断当前是否在记录替换组合键
        /// </summary>
        public bool IsReplace { get => isReplace; }


        /// <summary>
        /// 按键录制事件：(组名，键名，记录的新组合键) 启动后，每输入一个按键回调一次
        /// </summary>
        public Action<string, string, InputKeyCodes> recordUpdateEvent;

        /// <summary>
        /// 按键录制完成事件：(组名，键名，被替换组合键，记录的新组合键，与新组合键重复 的同组 键值名列表)
        /// </summary>
        public Action<string, string, InputKeyCodes, InputKeyCodes, List<string>> recordDoneEvent;

        /// <summary>
        /// 事件：加载
        /// </summary>
        public event Action<UnityWebRequestAsyncOperation> EventLoad;

        /// <summary>
        /// 事件：加载完成
        /// </summary>
        public event Action<UnityWebRequest> EventLoadDone;

        /// <summary>
        /// 事件：数据更新
        /// </summary>
        public event Action<InputKeysManager> EventDataUpdate;

        public InputKeysManager SetPath(string path, ApplicationPathEnum rootPath = ApplicationPathEnum.Path)
        {
            this.path = path;
            this.RootPath = rootPath;
            return this;
        }

        private string FullPath()
        {
            return ApplicationExtension.GetPath(RootPath) + path;
        }

        /// <summary>
        /// 加载
        /// </summary>
        public UnityWebRequestAsyncOperation Load()
        {
            return Load(FullPath());
        }

        /// <summary>
        /// 加载：完整路径
        /// </summary>
        public UnityWebRequestAsyncOperation Load(string uri)
        {
            if (isDone)
            {
                var asyncOperation = UnityWebRequest.Get(uri).SendWebRequest();
                AsyncLoad(asyncOperation);
                return asyncOperation;
            }
            else
            {
                return null;
            }
        }

        private async void AsyncLoad(UnityWebRequestAsyncOperation asyncOperation)
        {
            isDone = false;
            EventLoad?.Invoke(asyncOperation);
            await asyncOperation;
            var request = asyncOperation.webRequest;
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.Log("InputKeys Load Error");
            }
            else
            {
                inputKeyGroups = JsonMapper.ToObject<Dictionary<string, Dictionary<string, InputKeyCodes>>>(request.downloadHandler.text);
                EventDataUpdate?.Invoke(this);
            }
            isDone = true;
            EventLoadDone?.Invoke(request);
        }


        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            if (Path.IsPathRooted(FullPath()))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(FullPath()));//如果文件夹不存在就创建它
                File.WriteAllText(FullPath(), Convert_String(JsonMapper.ToJson(inputKeyGroups)));
            }
        }


        /// <summary>
        /// 判断任意键：包含鼠标滚轮
        /// </summary>
        public bool AnyKeyDown()
        {
            return Input.anyKeyDown ? true
            : Input.GetAxis("Mouse ScrollWheel") != 0 ? true
            : false
            ;
        }


        /// <summary>
        /// 获取当前帧按键：包含鼠标滚轮
        /// </summary>
        public int GetAnyKeyCode()
        {

            int inputCode = 0;
            if (Input.anyKeyDown)//这段来自咸鱼肆的帮助
            {
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        inputCode = (int)keyCode;
                    }
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                inputCode = (Input.GetAxis("Mouse ScrollWheel") > 0) ? (int)MouseCode.ScrollUp : (int)MouseCode.ScrollDown;
            }

            else
            {
                inputCode = (int)KeyCode.None;
            }

            return inputCode;
        }

        public Dictionary<string, InputKeyCodes> GetGroup(string group)
        {
            return inputKeyGroups[group];
        }

        /// <summary>
        /// 获取组合键，不存在返回Null
        /// </summary>
        /// <param name="group">组名</param>
        /// <param name="key">键值</param>
        public InputKeyCodes GetKeyCodes(string group, string key)
        {
            if (inputKeyGroups.ContainsKey(group))
            {
                if (inputKeyGroups[group].ContainsKey(key))
                {
                    return inputKeyGroups[group][key];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 直接设置组合键
        /// </summary>
        /// <param name="group">组名</param>
        /// <param name="key">键值</param>
        /// <param name="keyCodes">按键</param>
        public void SetKeyCodes(string group, string key, InputKeyCodes keyCodes)
        {
            if (inputKeyGroups.ContainsKey(group))
            {
                if (inputKeyGroups[group].ContainsKey(key))
                {
                    inputKeyGroups[group][key] = keyCodes;
                }
                else
                {
                    inputKeyGroups[group].Add(key, keyCodes);
                }
            }
            else
            {
                inputKeyGroups.Add(group, new Dictionary<string, InputKeyCodes>() { [key] = keyCodes });
            }

            EventDataUpdate?.Invoke(this);
        }

        /// <summary>
        /// 判断组合键按下
        /// </summary>
        /// <param name="group">组名</param>
        /// <param name="key">按键名</param>
        public bool GetKeysDown(string group, string key)
        {
            InputKeyCodes keyCodes = GetKeyCodes(group, key);
            if (keyCodes != null && !isReplace)
            {
                return keyCodes.IsKeysDown();
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断组合键按住
        /// </summary>
        /// <param name="group">组名</param>
        /// <param name="key">按键名</param>
        public bool GetKeys(string group, string key)
        {
            InputKeyCodes keyCodes = GetKeyCodes(group, key);
            if (keyCodes != null && !isReplace)
            {
                return keyCodes.IsKeys();
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断组合键抬起
        /// </summary> 
        /// <param name="group">组名</param>
        /// <param name="key">按键名</param>
        public bool GetKeysUp(string group, string key)
        {
            InputKeyCodes keyCodes = GetKeyCodes(group, key);
            if (keyCodes != null && !isReplace)
            {
                return keyCodes.IsKeysUp();
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 启动替换录制，完毕后回调：录制期间会停用按键判断（需要先注册回调委托才能获得结果）
        /// </summary>
        /// <param name="group">组名</param>
        /// <param name="key">按键名</param>
        /// <param name="keyCode">替换按键</param>
        public void RecordKeys(string group, string key)
        {
            InputKeyCodes keyCodes = GetKeyCodes(group, key);

            if (keyCodes != null && !isReplace)
            {
                InputKeyCodes replaceKeyCodes = new InputKeyCodes();//新建一个组合键
                replaceKeyCodes.limit = keyCodes.limit;//限制数量相等

                isReplace = true;  //替换标记启动
                StartCoroutine(GetRecordKeys(group, key, replaceKeyCodes));//启动协程，开始记录按键
            }
        }

        /// <summary>
        /// 查询同组，包含相同组合键的键名集合
        /// </summary>
        /// <param name="group">组名</param>
        /// <param name="key">键值</param>
        /// <param name="keyCodes">组合键</param>
        /// <returns>相同组合键，键值列表</returns>
        public List<string> InputKeysContains(string group, string key, InputKeyCodes keyCodes)
        {
            return inputKeyGroups[group].Where((item) => item.Key != key && item.Value.IsEqual(keyCodes)).Select((item) => item.Key).ToList();
        }


        //协程记录当前按键
        private IEnumerator GetRecordKeys(string group, string key, InputKeyCodes newKeyCodes)
        {
            while (true)//任意键按下跳出循环
            {
                yield return null;
                if (AnyKeyDown()) break;
            }

            while (isReplace)//开始记录按键
            {
                if (AnyKeyDown())//按键按下时加入队列
                {
                    newKeyCodes.Codes.Add(GetAnyKeyCode());
                    if (recordUpdateEvent != null) recordUpdateEvent(group, key, newKeyCodes);
                }

                if (newKeyCodes.IsKeysUp())//任意按键抬起时结束记录
                {
                    RecordDoneConfirm(group, key, newKeyCodes);
                    isReplace = false;
                }
                yield return null;
            }
        }

        //查询并回调，确认是否替换按键
        private void RecordDoneConfirm(string group, string key, InputKeyCodes newKeyCodes)
        {
            InputKeyCodes keyCodes = GetKeyCodes(group, key);
            List<string> keyNames = new List<string>();
            if (keyCodes != null)
            {
                keyNames = InputKeysContains(group, key, newKeyCodes);

                recordDoneEvent?.Invoke(group, key, keyCodes, newKeyCodes, keyNames);
            }
        }



        /// <summary>
        /// 乱码转换：用于解决LitJson把类转换成string时出现的乱码
        /// </summary>
        /// <param name="source">乱码字符串</param>
        /// <returns>正常字符串</returns>
        private string Convert_String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase)
            .Replace(source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }




    }
}