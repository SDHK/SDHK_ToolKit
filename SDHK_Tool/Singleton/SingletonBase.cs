/****************************************

* �� �� �ߣ�  ����ڿ�
* ����ʱ�䣺  2022/4/18 14:52
* ��    ��:

****************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Singleton
{
    public class SingletonBase<T>
    where T : SingletonBase<T>, new()
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {


        }
    }
}