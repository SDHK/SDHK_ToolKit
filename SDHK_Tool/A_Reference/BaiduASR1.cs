using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using SDHK_Tool.Static;
using SDHK_Tool.Dynamic;

//RequireComponent的这两个组件主要用于播放自己录制的声音,不需要刻意删除,同时注意删除使用组件的代码
[RequireComponent(typeof(AudioListener)), RequireComponent(typeof(AudioSource))]
public class BaiduASR1 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public Text text1;


    public Transform transform1;

    public float volume;

    public float Time_ = 3;
    public float max;

    //百度语音识别相关key
    //string appId = "";
    string apiKey = "vVlHvOileNFcf1hXkpmyGue5";              //填写自己的apiKey
    string secretKey = "fXI3niWYUenM17gP4kF9HsX8VoeH3ugC";         //填写自己的secretKey

    //记录accesstoken令牌
    string accessToken = string.Empty;

    //语音识别的结果
    string asrResult = string.Empty;

    //标记是否有麦克风
    private bool isHaveMic = false;

    //当前录音设备名称
    string currentDeviceName = string.Empty;

    //录音频率,控制录音质量(8000,16000)
    int recordFrequency = 8000;

    //上次按下时间戳
    double lastPressTimestamp = 0;

    //表示录音的最大时长
    int recordMaxLength = 20;

    //实际录音长度(由于unity的录音需先指定长度,导致识别上传时候会上传多余的无效字节)
    //通过该字段,获取有效录音长度,上传时候剪切到无效的字节数据即可
    int trueLength = 0;

    //存储录音的片段
    // [HideInInspector]
    public AudioClip saveAudioClip;

    //当前按钮下的文本
    Text textBtn;

    //显示结果的文本
    Text textResult;

    //音源
    AudioSource audioSource;

    public SD_MarkerClock markerClock;
    public bool isMarker = false;

    void Start()
    {
        markerClock = new SD_MarkerClock();

        //获取麦克风设备，判断是否有麦克风设备
        if (Microphone.devices.Length > 0)
        {
            isHaveMic = true;
            currentDeviceName = Microphone.devices[0];
            print("麦克风：" + currentDeviceName);
        }

        //获取相关组件
        // textBtn = this.transform.GetChild(0).GetComponent<Text>();
        audioSource = this.GetComponent<AudioSource>();
        // textResult = this.transform.parent.GetChild(1).GetComponent<Text>();

        // StartCoroutine(_StartBaiduYuYin());
    }

    private void Update()
    {
        volume = GetMaxVolume();

        // transform1.position = new Vector3(0, volume * 5 * Time.deltaTime, 0);

        // print(markerClock.Get_Clock_Game());

        if (volume > max)
        {
            markerClock.Reset_Marker();
            print("计时重置！！");
        }

        if (markerClock.IF_Clock_Game(Time_))
        {
            print("停止录音！！");
            string strText = "录音停止";
            if (text1 != null) text1.text = strText; else print(strText);
            OnPointerUp(new PointerEventData(EventSystem.current));

        }

        // if (max < volume)
        // {
        //     max = volume;
        //     // print(volume);
        // }

    }

    /// <summary>
    /// 开始录音
    /// </summary>
    /// <param name="isLoop"></param>
    /// <param name="lengthSec"></param>
    /// <param name="frequency"></param>
    /// <returns></returns>
    public bool StartRecording(bool isLoop = false) //8000,16000
    {
        if (isHaveMic == false || Microphone.IsRecording(currentDeviceName))
        {
            return false;
        }

        //开始录音
        /*
         * public static AudioClip Start(string deviceName, bool loop, int lengthSec, int frequency);
         * deviceName   录音设备名称.
         * loop         如果达到长度,是否继续记录
         * lengthSec    指定录音的长度.
         * frequency    音频采样率   
         */



        lastPressTimestamp = GetTimestampOfNowWithMillisecond();

        saveAudioClip = Microphone.Start(currentDeviceName, isLoop, recordMaxLength, recordFrequency);

        string strText = "唤醒成功,正在录音...";
        // ceshi001.ShiBie();
        if (text1 != null) text1.text = strText; else print(strText);

        return true;
    }

    /// <summary>
    /// 录音结束,返回实际的录音时长
    /// </summary>
    /// <returns></returns>
    public int EndRecording()
    {
        if (isHaveMic == false || !Microphone.IsRecording(currentDeviceName))
        {
            return 0;
        }

        //结束录音
        Microphone.End(currentDeviceName);

        //向上取整,避免遗漏录音末尾
        return Mathf.CeilToInt((float)(GetTimestampOfNowWithMillisecond() - lastPressTimestamp) / 1000f);
    }

    /// <summary>
    /// 获取毫秒级别的时间戳,用于计算按下录音时长
    /// </summary>
    /// <returns></returns>
    public double GetTimestampOfNowWithMillisecond()
    {
        return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
    }

    /// <summary>
    /// 按下录音按钮
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isMarker)
        {

            // textBtn.text = "松开识别";
            StartRecording();
            isMarker = true;
        }

    }

    /// <summary>
    /// 放开录音按钮
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isMarker)
        {
            // textBtn.text = "按住说话";
            trueLength = EndRecording();
            if (trueLength > 1)
            {

                string strText = "录音结束，开始识别语音...";
                if (text1 != null) text1.text = strText; else print(strText);
                // audioSource.PlayOneShot(saveAudioClip);
                StartCoroutine(_StartBaiduYuYin());




            }
            else
            {
                // textResult.text = "录音时长过短";
                print("录音时长过短");
                // StartCoroutine(_StartBaiduYuYin());
            }

            isMarker = false;
        }
    }

    /// <summary>
    /// 获取accessToken请求令牌
    /// </summary>
    /// <returns></returns>
    IEnumerator _GetAccessToken()
    {
        var uri =
            string.Format(
                "https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id={0}&client_secret={1}",
                apiKey, secretKey);
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(uri);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isDone)
        {
            //这里可以考虑用Json,本人比较懒所以用正则匹配出accessToken
            Match match = Regex.Match(unityWebRequest.downloadHandler.text, @"access_token.:.(.*?).,");
            if (match.Success)
            {
                //表示正则匹配到了accessToken
                accessToken = match.Groups[1].ToString();
            }
            else
            {
                // textResult.text = "验证错误,获取AccessToken失败!!!";
                print("验证错误,获取AccessToken失败!!!");
            }
        }
    }

    /// <summary>
    /// 发起语音识别请求
    /// </summary>
    /// <returns></returns>
    IEnumerator _StartBaiduYuYin()
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            yield return _GetAccessToken();
        }

        asrResult = string.Empty;

        //处理当前录音数据为PCM16
        float[] samples = new float[recordFrequency * trueLength * saveAudioClip.channels];
        saveAudioClip.GetData(samples, 0);
        var samplesShort = new short[samples.Length];
        for (var index = 0; index < samples.Length; index++)
        {
            samplesShort[index] = (short)(samples[index] * short.MaxValue);
        }
        byte[] datas = new byte[samplesShort.Length * 2];
        Buffer.BlockCopy(samplesShort, 0, datas, 0, datas.Length);

        string url = string.Format("{0}?cuid={1}&token={2}", "https://vop.baidu.com/server_api", SystemInfo.deviceUniqueIdentifier, accessToken);

        WWWForm wwwForm = new WWWForm();
        wwwForm.AddBinaryData("audio", datas);

        UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, wwwForm);

        unityWebRequest.SetRequestHeader("Content-Type", "audio/pcm;rate=" + recordFrequency);

        yield return unityWebRequest.SendWebRequest();

        if (string.IsNullOrEmpty(unityWebRequest.error))
        {
            asrResult = unityWebRequest.downloadHandler.text;
            if (Regex.IsMatch(asrResult, @"err_msg.:.success"))
            {
                Match match = Regex.Match(asrResult, "result.:..(.*?)..]");
                if (match.Success)
                {
                    asrResult = match.Groups[1].ToString();
                }
                // bD_TL.Send_TL(asrResult);
            }
            else
            {
                asrResult = "识别结果为空";

                string strText = "识别结果为空！！！";
                // ceshi001.ShiBieN();
                if (text1 != null) text1.text = strText; else print(strText);
                print(asrResult);
            }
            // textResult.text = asrResult;




        }
    }


    float GetMaxVolume()
    {
        float maxVolume = 0f;
        //剪切音频
        float[] volumeData = new float[128];
        int offset = Microphone.GetPosition(currentDeviceName) - 128 + 1;
        if (offset < 0)
        {
            return 0;
        }
        saveAudioClip.GetData(volumeData, offset);

        for (int i = 0; i < 128; i++)
        {
            float tempMax = volumeData[i];//修改音量的敏感值
            if (maxVolume < tempMax)
            {
                maxVolume = tempMax;
            }
        }
        return maxVolume;
    }
}