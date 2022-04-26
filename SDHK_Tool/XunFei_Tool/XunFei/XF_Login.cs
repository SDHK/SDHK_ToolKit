using System.Collections;
using System.Collections.Generic;
using XunFei_Tool;
using UnityEngine;
using SDHK_Tool.Dynamic;
using System.Runtime.InteropServices;
using System;
using System.Text;






/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2020.03.04
 * 
 * 功能：讯飞平台登录器
 */

namespace XunFei_Tool
{

    /// <summary>
    /// 讯飞登录器                                                              
    /// 注：讯飞登录方式很奇怪，离线和在线共用一个方法，无法判断无网络
    /// </summary>
    public class XF_Login : MonoBehaviour
    {
        /// <summary>
        /// 运行时登录
        /// </summary>
        [Tooltip("运行时登录")]
        public bool LoginOnAwake = true;

        /// <summary>
        /// 登录appId
        /// </summary>
        [Tooltip("登录appId")]
        public string AppId = "5e033242";//登录参数,自己注册后获取的appid

        /// <summary>
        /// 登录标记
        /// </summary>
        [Tooltip("登录标记")]
        public bool isLogin = false;

        private int TagCode = 0;//错误码



        // Use this for initialization
        void Start()
        {

            if (LoginOnAwake)
            {
                XF_Login_Enter();
            }

        }

        private void FixedUpdate()
        {

        }


        /// <summary>
        /// 在线登陆讯飞
        /// </summary>
        public void XF_Login_Enter()
        {

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                isLogin = false;
                Debug.Log("无网络！！！");
                return;
            }


            TagCode = XF_Msc_DLL.MSPLogin(null, null, "appid = " + AppId);
            //第一个参数为用户名，第二个参数为密码，第三个参数是登录参数，用户名和密码需要在http://open.voicecloud.cn
            //MSPLogin方法返回失败
            if (TagCode != (int)ErrorCode.MSP_SUCCESS)
            {
                isLogin = false;
                Debug.Log("[讯飞登录器]:登录失败:" + TagCode);
            }
            else
            {
                isLogin = true;
                Debug.Log("[讯飞登录器]:登陆成功");
            }

        }

        /// <summary>
        /// 退出讯飞
        /// </summary>
        public void XF_Login_Exit()
        {
            TagCode = XF_Msc_DLL.MSPLogout();//退出登录
            if (TagCode != (int)ErrorCode.MSP_SUCCESS)
            {
                Debug.Log("[讯飞登录器]:退出失败:" + TagCode);
            }
            else
            {
                isLogin = false;
                Debug.Log("[讯飞登录器]:退出成功");
            }
        }

        private void OnApplicationQuit()
        {
            XF_Login_Exit();
        }




    }

}