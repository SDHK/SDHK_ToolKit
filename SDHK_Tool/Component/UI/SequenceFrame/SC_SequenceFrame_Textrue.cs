using SDHK_Tool.Dynamic;
using UnityEngine;
using UnityEngine.UI;



/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.9.29
 * 
 * 功能：序列帧播放工具【精灵图片】
 *
 */

namespace SDHK_Tool.Component
{
    /// <summary>
    /// 序列帧工具:[Textrue2D纹理]
    /// </summary>
    public class SC_SequenceFrame_Textrue : MonoBehaviour
    {

        [SerializeField]
        public enum Mode
        {
            RawImage
        }

        private RawImage rawImage;

        [Tooltip("播放组件")]
        private Mode mode;

        /// <summary>
        /// 序列帧播放速度
        /// </summary>
        [Tooltip("播放速度")]
        public float Speed = 1;

        /// <summary>
        /// 序列帧播放序号
        /// </summary>
        [Tooltip("播放序号")]
        public int index = 0;

        /// <summary>
        /// 播发开关
        /// </summary>
        [Tooltip("播放")]
        public bool isPlay = false;

        /// <summary>
        /// 循环开关
        /// </summary>
        [Tooltip("循环")]
        public bool Loop = false;

        /// <summary>
        /// 倒放开关
        /// </summary>
        [Tooltip("倒放")]
        public bool isRunBack = false;

        /// <summary>
        /// 序列帧图集
        /// </summary>
        [Tooltip("序列帧图集")]
        public Texture2D[] textures;

        private SD_MarkerClock markerClock;

        // Use this for initialization
        void Start()
        {
            markerClock = new SD_MarkerClock();

            if (GetComponent<RawImage>() != null)
            {
                rawImage = GetComponent<RawImage>();
                mode = Mode.RawImage;
            }
            else
            {
                rawImage = gameObject.AddComponent<RawImage>();
                mode = Mode.RawImage;
            }
        }


        /// <summary>
        /// 设置图片：精灵图片
        /// </summary>
        /// <param name="texture2D">精灵图片</param>
        public void Set_Texture2D(Texture2D texture2D)
        {
            switch (mode)
            {
                case Mode.RawImage:
                    rawImage.texture = texture2D; break;
                default:
                    rawImage.texture = texture2D; break;
            }
        }

        /// <summary>
        /// 设置图片：序列帧图片
        /// </summary>
        /// <param name="index">序列帧序号</param>
        public void Set_Texture2D(int index)
        {
            Set_Texture2D(textures[index]);
        }

        /// <summary>
        /// 设置图片：序列帧第一帧
        /// </summary>
        public void Set_Texture2DHead()
        {
            Set_Texture2D(textures[0]);
            this.index = 0;
        }

        /// <summary>
        /// 设置图片：序列帧最后一帧
        /// </summary>
        public void Set_Texture2DTail()
        {
            Set_Texture2D(textures[textures.Length - 1]);
            this.index = textures.Length - 1;
        }

        /// <summary>
        /// 序列帧播放
        /// </summary>
        public void Play()
        {
            isPlay = true;
            isRunBack = false;
        }

        /// <summary>
        /// 序列帧倒放
        /// </summary>
        public void PlayBack()
        {
            isPlay = true;
            isRunBack = true;
        }

        /// <summary>
        /// 序列帧播放
        /// </summary>
        /// <param name="index">序列帧序号</param>
        public void Play(int index)
        {
            isPlay = true;
            isRunBack = false;
            this.index = index;
        }

        /// <summary>
        /// 序列帧倒放
        /// </summary>
        /// <param name="index">序列帧序号</param>
        public void PlayBack(int index)
        {
            isPlay = true;
            isRunBack = true;
            this.index = index;
        }

        /// <summary>
        /// 序列帧暂停
        /// </summary>
        public void Stop()
        {
            isPlay = false;
        }


        /// <summary>
        /// 序列帧判断：播放到第一帧
        /// </summary>
        /// <returns></returns>
        public bool If_PlayHead()
        {
            return 0 == index;
        }

        /// <summary>
        /// 序列帧判断：播放到最后一帧
        /// </summary>
        /// <returns>bool</returns>
        public bool If_PlayTail()
        {
            return textures.Length - 1 == index;
        }

        /// <summary>
        /// 重置序列帧组件
        /// </summary>
        public void Reset()
        {
            index = 0;
            isPlay = false;
            isRunBack = false;
            markerClock.Reset_Marker();
        }


        // Update is called once per frame
        void FixedUpdate()
        {
            if (markerClock.IF_Clock_GameWorld(Speed, true) && isPlay)
            {
                if (isRunBack)
                {
                    if (index > 0)
                    {
                        index--;
                        Set_Texture2D(textures[index]);
                    }
                    else
                    {
                        if (Loop) { Set_Texture2DTail(); }
                    }
                }
                else
                {
                    if (index < textures.Length - 1)
                    {
                        index++;
                        Set_Texture2D(textures[index]);
                    }
                    else
                    {
                        if (Loop) { Set_Texture2DHead(); }
                    }
                }
            }
        }
    }

}