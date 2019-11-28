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
    /// 序列帧工具:[精灵]
    /// </summary>
    public class SC_SequenceFrame_Sprite : MonoBehaviour
    {
        [SerializeField]
        public enum Mode
        {
            Image,
            SpriteRenderer,
            RawImage
        }

        private Image image;
        private SpriteRenderer spriteRenderer;
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
        public Sprite[] sprites;

        private SD_MarkerClock markerClock;


        // Use this for initialization
        void Start()
        {
            markerClock = new SD_MarkerClock();

            if (transform.GetType() == typeof(Transform))
            {
                spriteRenderer = (GetComponent<SpriteRenderer>() != null) ? GetComponent<SpriteRenderer>() : gameObject.AddComponent<SpriteRenderer>();
                mode = Mode.SpriteRenderer;
            }
            else
            {
                if (GetComponent<Image>() != null)
                {
                    image = GetComponent<Image>();
                    mode = Mode.Image;
                }
                else if (GetComponent<RawImage>() != null)
                {
                    rawImage = GetComponent<RawImage>();
                    mode = Mode.RawImage;
                }
                else
                {
                    image = gameObject.AddComponent<Image>();
                    mode = Mode.Image;
                }
            }
        }

        /// <summary>
        /// 设置图片：精灵图片
        /// </summary>
        /// <param name="sprite">精灵图片</param>
        public void Set_Sprite(Sprite sprite)
        {
            switch (mode)
            {
                case Mode.Image:
                    image.sprite = sprite; break;
                case Mode.SpriteRenderer:
                    spriteRenderer.sprite = sprite; break;
                case Mode.RawImage:
                    rawImage.texture = sprite.texture;break;
                default:
                    image.sprite = sprite; break;
            }

        }

        /// <summary>
        /// 设置图片：序列帧图片
        /// </summary>
        /// <param name="index">序列帧序号</param>
        public void Set_Sprite(int index)
        {
            Set_Sprite(sprites[index]);
        }

        /// <summary>
        /// 设置图片：序列帧第一帧
        /// </summary>
        public void Set_SpriteHead()
        {
            Set_Sprite(sprites[0]);
            this.index = 0;
        }

        /// <summary>
        /// 设置图片：序列帧最后一帧
        /// </summary>
        public void Set_SpriteTail()
        {
            Set_Sprite(sprites[sprites.Length - 1]);
            this.index = sprites.Length - 1;
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
            return sprites.Length - 1 == index;
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
                        Set_Sprite(sprites[index]);
                    }
                    else
                    {
                        if (Loop) { Set_SpriteTail(); }
                    }
                }
                else
                {
                    if (index < sprites.Length - 1)
                    {
                        index++;
                        Set_Sprite(sprites[index]);
                    }
                    else
                    {
                        if (Loop) { Set_SpriteHead(); }
                    }
                }
            }
        }

    }

}