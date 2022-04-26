using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Touch;

public class TouchMove : MonoBehaviour
{

    public float PlaneDistance = 100;


    public TouchEventDown touchDown;


    public RenderMode renderMode;


    public bool Mobile = true;
    public bool Rotation = true;
    public bool Zoom = true;


    public Vector2 touchFirst;

    public Vector2 touchSecond;




    // Start is called before the first frame update
    void Start()
    {

        touchDown = gameObject.TouchDown();

        touchDown
        .OnDown((eventDate) =>
        {
            PlaneDistance = (Camera.main.transform.InverseTransformPoint(transform.position).z);

            // Debug.Log(Camera.main.WorldToScreenPoint(new Vector3(1920, 1080, 0)));

        })
        .OnDragUpdate((eventDate) =>
        {

        })
        .OnUp((eventDate) =>
        {

        })
        .OnScrollUpdate((date) =>
        {

        })
        ;


    }

    // Update is called once per frame
    void Update()
    {

    }


    public void TouchFilter()
    {

        if (touchDown.touchPool.Count == 0) return;

        if (!Mobile && (Rotation || Zoom)) //移动禁用情况
        {


        }
        else//开启移动情况
        {

            if (renderMode == RenderMode.ScreenSpaceOverlay)
            {

            }



            // touchFirst = 

        }

    }
}
