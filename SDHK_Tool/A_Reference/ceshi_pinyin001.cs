using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPinyin;

public class ceshi_pinyin001 : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        string text = Pinyin.GetPinyin("前面的工程项目配置和语音识别差不多，但是需要从SDK的res文件夹中复制ivw文件夹粘贴到main下面的assets文件夹下面。具体的文件配置结构，我截个图给大家看看");
        print(text);

        string text1 = Pinyin.GetInitials("嗯吃是");
        print(text1);

        string text2 = Pinyin.GetChineseText("ni hao");
        print(text2);


    }

    // Update is called once per frame
    void Update()
    {

    }
}
