using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.9.6
 * 
 * 功能：对Texture的转换处理
 */

namespace SDHK_Tool.Static
{

    /// <summary>
    /// 对Texture的转换处理
    /// </summary>
    public static class SS_Texture
    {

        /// <summary>
        /// Texture2D纹理翻转：
        /// </summary>
        /// <param name="texture">Texture2D纹理</param>
        /// <param name="Flip">翻转模式：0~4</param>
        /// <returns>翻转后的Texture2D纹理</returns>
        public static Texture2D Texture2D_Flip(Texture2D texture, TextureFlip Flip = TextureFlip.无翻转)
        {
            Texture2D newTexture;

            switch (Flip)
            {
                case TextureFlip.无翻转: return texture;
                case TextureFlip.顺时针水平翻转:
                case TextureFlip.逆时针水平翻转:
                case TextureFlip.顺时针90度:
                case TextureFlip.逆时针90度: newTexture = new Texture2D(texture.height, texture.width); break;
                case TextureFlip.旋转180度:
                case TextureFlip.垂直翻转:
                case TextureFlip.水平翻转: newTexture = new Texture2D(texture.width, texture.height); break;

                default: newTexture = texture; break;
            }

            int width = texture.width - 1;
            int height = texture.height - 1;

            for (int i = 0; i < width + 1; i++)
            {
                for (int j = 0; j < height + 1; j++)
                {
                    Color color = texture.GetPixel(i, j);
                    switch (Flip)
                    {
                        case TextureFlip.顺时针90度: newTexture.SetPixel(j, width - i, color); break;//顺时针90度
                        case TextureFlip.逆时针90度: newTexture.SetPixel(height - j, i, color); break;//逆时针90度
                        case TextureFlip.旋转180度: newTexture.SetPixel(width - i, height - j, color); break;//旋转180度
                        case TextureFlip.水平翻转: newTexture.SetPixel(width - i, j, color); break;//水平翻转
                        case TextureFlip.顺时针水平翻转: newTexture.SetPixel(height - j, width - i, color); break;//顺时针水平翻转
                        case TextureFlip.逆时针水平翻转: newTexture.SetPixel(j, i, color); break;//逆时针水平翻转
                        case TextureFlip.垂直翻转: newTexture.SetPixel(i, height - j, color); break;//垂直翻转
                    }
                }
            }
            newTexture.Apply();
            return newTexture;
        }


        /// <summary>
        /// Texture2D纹理裁剪
        /// </summary>
        /// <param name="texture2D">要裁剪的Texture2D</param>
        /// <param name="x">裁剪x轴起点(原点为左下角)</param>
        /// <param name="y">裁剪y轴起点(原点为左下角)</param>
        /// <param name="width">裁剪图形宽度</param>
        /// <param name="height">裁剪图形高度</param>
        /// <returns>裁剪后的Texture2D纹理</returns>
        public static Texture2D Texture2D_Shear(Texture2D texture2D, int x, int y, int width, int height)
        {
            Texture2D newTexture = new Texture2D(width, height);
            newTexture.SetPixels(texture2D.GetPixels(x, y, width, height));
            newTexture.Apply();
            return newTexture;
        }

        // 从屏幕读取像素保存到纹理数据中。
        // texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);

    }


    public enum TextureFlip
    {
        无翻转,
        顺时针90度,
        逆时针90度,
        旋转180度,

        水平翻转,
        顺时针水平翻转,
        逆时针水平翻转,
        垂直翻转

    }

}
