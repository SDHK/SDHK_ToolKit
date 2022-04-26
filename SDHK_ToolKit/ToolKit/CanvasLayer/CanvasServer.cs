

using System.Collections.Generic;
using UnityEngine;
using Singleton;


namespace CanvasLayer
{
    /// <summary>
    /// 画布获取服务
    /// </summary>
    public class CanvasServer : SingletonBase<CanvasServer>
    {

        private GameObject canvas;

        private string canvasName;

        private Dictionary<int, Transform> windowLayers = new Dictionary<int, Transform>();

        private string layerName = "Layer";


        /// <summary>
        /// 设置画布
        /// </summary>
        /// <param name="canvas">画布对象</param>
        public void SetCanvas(GameObject canvas)
        {
            this.canvas = canvas;
            canvasName = canvas.name;
        }

        /// <summary>
        /// 寻找画布
        /// </summary>
        public Transform FindCanvas(string name)
        {
            if (name != "")
            {
                if (name != canvasName)
                {
                    canvasName = name;

                    canvas = GameObject.Find(canvasName);
                }
                else
                {
                    if (canvas == null)
                    {
                        canvas = GameObject.Find(canvasName);
                    }
                }
            }

            if (canvas == null)
            {
                canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
            }


            return canvas.transform;
        }

        /// <summary>
        /// 从当前场景中获取画布Transform
        /// </summary>
        /// <param name="name">寻找的画布名字，为""则以画布组件进行寻找</param>
        public Transform GetCanvas(string name = "Canvas")
        {
            return FindCanvas(name);
        }


        /// <summary>
        /// 从当前场景 获取画布中的层级 Transform
        /// </summary>
        /// <param name="windowsLayer">窗口显示层级</param>
        /// <param name="canvasName">寻找的画布名字，为""则以画布组件进行寻找</param>
        public Transform GetLayer(int windowsLayer, string canvasName = "")
        {
            FindCanvas(canvasName);

            if (canvas != null)
            {
                if (windowLayers.ContainsKey(windowsLayer))
                {
                    if (windowLayers[windowsLayer] != null)//存在
                    {
                        windowLayers[windowsLayer].SetSiblingIndex(windowsLayer);
                        return windowLayers[windowsLayer];
                    }
                }
                else
                {
                    windowLayers.Add(windowsLayer, null);
                }

                Transform panel = canvas.transform.Find(layerName + windowsLayer.ToString());//尝试在场景中寻找
                if (panel == null)//没找到则新建
                {
                    for (int i = 0; i <= windowsLayer; i++)//为了排序需要把不存在的层一起生成
                    {
                        if (windowLayers.ContainsKey(i))//判断键存在
                        {
                            if (windowLayers[i] == null)//键存在但空了
                            {
                                panel = canvas.transform.Find(layerName + i.ToString());//尝试在场景中寻找
                                if (panel == null)//没找到
                                {
                                    panel = NewLayer(layerName + i.ToString(), canvas.transform);//新建
                                }

                                windowLayers[i] = panel;
                            }
                        }
                        else//键不存在
                        {
                            panel = NewLayer(layerName + i.ToString(), canvas.transform);//新建
                            windowLayers.Add(i, panel);
                        }

                        windowLayers[i].SetSiblingIndex(i);//排序
                    }
                }
                else//找到则保存并排序
                {
                    windowLayers[windowsLayer] = panel;
                    panel.SetSiblingIndex(windowsLayer);
                }

                return panel;
            }
            else
            {
                return null;
            }

        }

        private Transform NewLayer(string layerName, Transform Canvas)
        {
            GameObject panelObj = new GameObject(layerName);
            panelObj.AddComponent<CanvasRenderer>();
            panelObj.AddComponent<RectTransform>();

            RectTransform panel = (RectTransform)panelObj.transform;

            panel.SetParent(Canvas);

            panel.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            panel.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
            panel.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            panel.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);

            panel.anchorMin = Vector2.zero;//设置锚点为全屏四角
            panel.anchorMax = Vector2.one;//设置锚点为全屏四角

            panel.localRotation = Quaternion.identity;
            panel.localPosition = Vector3.zero;
            panel.localScale = Vector3.one;

            return panel;
        }



    }
}