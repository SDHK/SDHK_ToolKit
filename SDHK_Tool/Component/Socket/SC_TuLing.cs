using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDHK_Tool.Dynamic;
using System.Text;
using LitJson;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.03.05
 * 
 * 功能：用于图灵机器人的文字聊天组件
 *
 * 官方网址 ：http://www.tuling123.com/member/robot/index.jhtml 
 */

namespace SDHK_Tool.Component
{

    /// <summary>
    /// 图灵机器人
    /// </summary>
    public class SC_TuLing : MonoBehaviour
    {
        /// <summary>
        ///请求地址
        /// </summary>
        [Tooltip("请求地址")]
        public string Url = "http://openapi.tuling123.com/openapi/api/v2";

        /// <summary>
        /// apikey
        /// </summary>
        public string apiKey = "4f42597c359a4c4bb1593188e77fef10";

        /// <summary>
        /// userId
        /// </summary>
        public string userId = "SDHK";

        /// <summary>
        /// 发送的消息
        /// </summary>
        [Tooltip("发送的消息")]
        public string message;//消息


        /// <summary>
        /// 回复的回调函数
        /// </summary>
        public Action<string> CallBack;

        private SD_TaskActuator TA_send;//请求会话的任务

        private WWW post;//www网络

        private JsonData request;//请求的josn格式

        private string result;//回复的text消息


        // Use this for initialization
        void Start()
        {
            TA_send = new SD_TaskActuator() //任务注册
            .Event(TA_Set)                  //www网络请求图灵会话
            .WaitEvent(() => post.isDone)   //等待网络回话
            .IF_Event(() => CallBack != null)//判断回调不为Null
            .Event(TA_Get)                  //回复解析
            .Event(() => CallBack(result))  //回调
            .IF_End()                       //IF_End标记
            ;
        }

        public void Set_TL(string message)
        {
            this.message = message;
            if (!TA_send.isRun) TA_send.Run();//任务执行
        }

        private void TA_Set()
        {

            //perception
            request = new JsonData();//空节点初始化
            request["perception"] = new JsonData();
            request["perception"]["inputText"] = new JsonData();
            request["perception"]["inputText"]["text"] = message;
            //userInfo
            request["userInfo"] = new JsonData();
            request["userInfo"]["apiKey"] = apiKey;
            request["userInfo"]["userId"] = userId;

            Debug.Log("[图灵机器人]：请求会话");
            post = new WWW(Url, Encoding.UTF8.GetBytes(JsonMapper.ToJson(request)));
        }

        private void TA_Get()
        {
            Debug.Log("[图灵机器人]：收到回复");
            result = JsonMapper.ToObject(post.text)["results"][0]["values"]["text"].ToString();
        }

        // Update is called once per frame
        void Update()
        {
            TA_send.Update();
        }

    }
}