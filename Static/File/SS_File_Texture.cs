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
        /// <param name="Path">路径</param>
        /// <returns>return : 图片</returns>
        public static Texture2D GetFile_Texture2D_WWW(string Path)
        {
            string Name = Path.Split(':')[0].ToUpper();
            WWW www = new WWW(("http".ToUpper() == Name || "https".ToUpper() == Name) ? Path : "file://" + Path);
            while (!www.isDone) { }
            return www.texture;
        }

        /// <summary>
        /// 读取文件：精灵图片读取
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>return : 精灵图片</returns>
        public static Sprite GetFile_Sprite_WWW(string Path)
        {
            string Name = Path.Split(':')[0].ToUpper();
            WWW www = new WWW(("http".ToUpper() == Name || "https".ToUpper() == Name) ? Path : "file://" + Path);
            while (!www.isDone) { }
            return Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), Vector2.zero);
        }


        /// <summary>
        /// 读取文件：图片读取
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>return : 图片</returns>
        public static Texture2D GetFile_Texture2D(string Path)
        {
            //创建文件读取流
            FileStream fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read);
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
        /// <param name="Paths">路径集合</param>
        /// <returns>return : 图片集合</returns>
        public static List<Texture2D> GetFile_Texture2D(List<string> Paths)
        {
            List<Texture2D> Texture2Ds = new List<Texture2D>();
            foreach (string Path in Paths)
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
        public static List<Texture2D> GetFile_Texture2D(string Path, string type)
        {
            return GetFile_Texture2D(GetPaths_File(Path, type));
        }

        /// <summary>
        /// 读取文件：图片批量读取(后缀集合过滤)
        /// </summary>
        /// <param name="Paths">路径集合</param>
        /// <param name="types">后戳集合</param>
        /// <returns>return : 图片集合</returns>
        public static List<Texture2D> GetFile_Texture2D(string Path, string[] types)
        {
            return GetFile_Texture2D(GetPaths_File(Path, types));
        }




        /// <summary>
        /// 读取文件：精灵图片读取
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>return : 精灵图片</returns>
        public static Sprite GetFile_Sprite(string Path)
        {
            Texture2D texture = GetFile_Texture2D(Path);
            //创建Sprite
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            return sprite;
        }

        /// <summary>
        /// 读取文件：精灵图片批量读取
        /// </summary>
        /// <param name="Paths">路径集合</param>
        /// <returns>return : 精灵图片集合</returns>
        public static List<Sprite> GetFile_Sprite(List<string> Paths)
        {
            List<Sprite> Sprites = new List<Sprite>();
            foreach (string Path in Paths)
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
        public static List<Sprite> GetFile_Sprite(string Path, string type)
        {
            return GetFile_Sprite(GetPaths_File(Path, type));
        }

        /// <summary>
        /// 读取文件：精灵图片批量读取（后缀集合过滤）
        /// </summary>
        /// <param name="Paths">路径集合</param>
        /// <param name="types">后戳集合</param>
        /// <returns>return : 精灵图片集合</returns>
        public static List<Sprite> GetFile_Sprite(string Path, string[] types)
        {
            return GetFile_Sprite(GetPaths_File(Path, types));
        }
        #endregion



        #region 图片写入
        /// <summary>
        /// 写入文件：图片保存为JPG
        /// </summary>
        /// <param name="texture2D">Texture2D图片纹理</param>
        /// <param name="Path">保存路径</param>
        public static void SetFile_Texture2D_JPG(Texture2D texture2D, string Path)
        {
            string Folder = Path.Substring(0, Path.LastIndexOf('/'));//去除文件名
            Directory.CreateDirectory(Folder);//如果文件夹不存在就创建它
            byte[] imageTytes = texture2D.EncodeToJPG();
            File.WriteAllBytes(Path, imageTytes);
        }

        /// <summary>
        /// 写入文件：图片保存为PNG
        /// </summary>
        /// <param name="texture2D">Texture2D图片纹理</param>
        /// <param name="Path">保存路径</param>
        public static void SetFile_Texture2D_PNG(Texture2D texture2D, string Path)
        {
            string Folder = Path.Substring(0, Path.LastIndexOf('/'));//去除文件名
            Directory.CreateDirectory(Folder);//如果文件夹不存在就创建它
            byte[] imageTytes = texture2D.EncodeToPNG();
            File.WriteAllBytes(Path, imageTytes);
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



    }
}