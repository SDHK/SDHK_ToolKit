using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZXing;
using ZXing.QrCode;

/*
 * 作者：闪电Y黑客
 * 
 * 日期： 2019.12.3
 * 
 * 功能：SS_File的分支部分：对图片文件进行操作的方法
 */

namespace SDHK_Tool.Static
{

    public static partial class SS_File
    {

        #region 图片读取
        /// <summary>
        /// 读取文件：图片读取
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>return : 图片</returns>
        public static Texture2D GetFile_Texture2D_WWW(string path)
        {
            string Name = path.Split(':')[0].ToUpper();
            WWW www = new WWW(("http".ToUpper() == Name || "https".ToUpper() == Name) ? path : "file://" + path);
            while (!www.isDone) { }
            return www.texture;
        }

        /// <summary>
        /// 读取文件：精灵图片读取
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>return : 精灵图片</returns>
        public static Sprite GetFile_Sprite_WWW(string path)
        {
            string Name = path.Split(':')[0].ToUpper();
            WWW www = new WWW(("http".ToUpper() == Name || "https".ToUpper() == Name) ? path : "file://" + path);
            while (!www.isDone) { }
            return Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero);
        }


        /// <summary>
        /// 读取文件：图片读取
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>return : 图片</returns>
        public static Texture2D GetFile_Texture2D(string path)
        {
            //创建文件读取流
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            fileStream.Seek(0, SeekOrigin.Begin);
            //创建文件长度缓冲区
            byte[] bytes = new byte[fileStream.Length];
            //读取文件
            fileStream.Read(bytes, 0, (int)fileStream.Length);
            //释放文件读取流
            fileStream.Close();
            fileStream.Dispose();
            fileStream = null;
            //创建Texture
            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(bytes);
            return texture;
        }

        /// <summary>
        /// 读取文件：图片批量读取
        /// </summary>
        /// <param name="paths">路径集合</param>
        /// <returns>return : 图片集合</returns>
        public static List<Texture2D> GetFile_Texture2D(List<string> paths)
        {
            List<Texture2D> Texture2Ds = new List<Texture2D>();
            foreach (string Path in paths)
            {
                Texture2Ds.Add(GetFile_Texture2D(Path));
            }
            return Texture2Ds;
        }

        /// <summary>
        /// 读取文件：图片批量读取(后缀过滤)
        /// </summary>
        /// <param name="Paths">路径集合</param>
        /// <param name="type">后戳名</param>
        /// <returns>return : 图片集合</returns>
        public static List<Texture2D> GetFile_Texture2D(string path, string type)
        {
            return GetFile_Texture2D(GetPaths_File(path, type));
        }

        /// <summary>
        /// 读取文件：图片批量读取(后缀集合过滤)
        /// </summary>
        /// <param name="Paths">路径集合</param>
        /// <param name="types">后戳集合</param>
        /// <returns>return : 图片集合</returns>
        public static List<Texture2D> GetFile_Texture2D(string path, string[] types)
        {
            return GetFile_Texture2D(GetPaths_File(path, types));
        }




        /// <summary>
        /// 读取文件：精灵图片读取
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>return : 精灵图片</returns>
        public static Sprite GetFile_Sprite(string path)
        {
            Texture2D texture = GetFile_Texture2D(path);
            //创建Sprite
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            return sprite;
        }

        /// <summary>
        /// 读取文件：精灵图片批量读取
        /// </summary>
        /// <param name="paths">路径集合</param>
        /// <returns>return : 精灵图片集合</returns>
        public static List<Sprite> GetFile_Sprite(List<string> paths)
        {
            List<Sprite> Sprites = new List<Sprite>();
            foreach (string Path in paths)
            {
                Sprites.Add(GetFile_Sprite(Path));
            }
            return Sprites;
        }

        /// <summary>
        /// 读取文件：精灵图片批量读取（后缀过滤）
        /// </summary>
        /// <param name="Paths">路径集合</param>
        /// <param name="type">后戳名</param>
        /// <returns>return : 精灵图片集合</returns>
        public static List<Sprite> GetFile_Sprite(string path, string type)
        {
            return GetFile_Sprite(GetPaths_File(path, type));
        }

        /// <summary>
        /// 读取文件：精灵图片批量读取（后缀集合过滤）
        /// </summary>
        /// <param name="Paths">路径集合</param>
        /// <param name="types">后戳集合</param>
        /// <returns>return : 精灵图片集合</returns>
        public static List<Sprite> GetFile_Sprite(string path, string[] types)
        {
            return GetFile_Sprite(GetPaths_File(path, types));
        }
        #endregion



        #region 图片写入
        /// <summary>
        /// 写入文件：图片保存为JPG
        /// </summary>
        /// <param name="texture2D">Texture2D图片纹理</param>
        /// <param name="path">保存路径</param>
        public static void SetFile_Texture2D_JPG(Texture2D texture2D, string path)
        {
            string Folder = path.Substring(0, path.LastIndexOf('/'));//去除文件名
            Directory.CreateDirectory(Folder);//如果文件夹不存在就创建它
            byte[] imageTytes = texture2D.EncodeToJPG();
            File.WriteAllBytes(path, imageTytes);
        }

        /// <summary>
        /// 写入文件：图片保存为PNG
        /// </summary>
        /// <param name="texture2D">Texture2D图片纹理</param>
        /// <param name="path">保存路径</param>
        public static void SetFile_Texture2D_PNG(Texture2D texture2D, string path)
        {
            string Folder = path.Substring(0, path.LastIndexOf('/'));//去除文件名
            Directory.CreateDirectory(Folder);//如果文件夹不存在就创建它
            byte[] imageTytes = texture2D.EncodeToPNG();
            File.WriteAllBytes(path, imageTytes);
        }
        #endregion




        #region 二维码转换

        /// <summary>
        /// 将字符串转换为二维码图片
        /// </summary>
        /// <param name="Str">字符串（网址）</param>
        /// <returns>return : 二维码图片</returns>
        public static Texture2D Convert_StrToQrcodeTexture2D(string Str)
        {
            Texture2D encoded = new Texture2D(256, 256);

            QrCodeEncodingOptions qrEncodeOption = new ZXing.QrCode.QrCodeEncodingOptions();
            qrEncodeOption.CharacterSet = "UTF-8"; //设置编码格式，否则读取'中文'乱码
            qrEncodeOption.Height = encoded.height;
            qrEncodeOption.Width = encoded.width;
            qrEncodeOption.Margin = 0; //设置周围空白边距

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = qrEncodeOption
            };
            encoded.SetPixels32(writer.Write(Str));
            encoded.Apply();
            return encoded;
        }

        /// <summary>
        /// 将字符串转换为二维码精灵图片
        /// </summary>
        /// <param name="Str">字符串（网址）</param>
        /// <returns>return : 二维码精灵图片</returns>
        public static Sprite Convert_StrToQrcodeSprite(string Str)
        {
            Texture2D encoded = Convert_StrToQrcodeTexture2D(Str);
            return Sprite.Create(encoded, new Rect(0, 0, encoded.width, encoded.height), Vector2.zero);
        }


        /// <summary>
        /// 将二维码转为字符串
        /// </summary>
        /// <param name="texture">二维码图片</param>
        /// <returns>return : 字符串</returns>
        public static string Convert_QrcodeTexture2DToStr(Texture2D texture)
        {
            BarcodeReader mReader = new BarcodeReader();
            var result = mReader.Decode(texture.GetPixels32(), texture.width, texture.height);
            return (result != null) ? result.Text : null;
        }



        #endregion

        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="tex">原图片</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns>压缩后的图片</returns>
        public static Texture2D ReSetTextureSize(Texture2D tex, int width, int height)
        {
            var rendTex = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            rendTex.Create();
            Graphics.SetRenderTarget(rendTex);
            GL.PushMatrix();
            GL.Clear(true, true, Color.clear);
            GL.PopMatrix();
            var mat = new Material(Shader.Find("Unlit/Transparent"));
            mat.mainTexture = tex;
            Graphics.SetRenderTarget(rendTex);
            GL.PushMatrix();
            GL.LoadOrtho();
            mat.SetPass(0);
            GL.Begin(GL.QUADS);
            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 0, 0);
            GL.TexCoord2(0, 1);
            GL.Vertex3(0, 1, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(1, 1, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(1, 0, 0);
            GL.End();
            GL.PopMatrix();
            var finalTex = new Texture2D(rendTex.width, rendTex.height, TextureFormat.ARGB32, false);
            RenderTexture.active = rendTex;
            finalTex.ReadPixels(new Rect(0, 0, finalTex.width, finalTex.height), 0, 0);
            finalTex.Apply();
            return finalTex;
        }



    }
}