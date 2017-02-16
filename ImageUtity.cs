using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace UtilityNet
{
    public abstract class ImageUtity
    {
        /// <summary>
        /// 将图片转成Base64字符数据
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ImageToBase64(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray(); 
                return Convert.ToBase64String(imageBytes);
            }
        }

        /// <summary>
        /// 将Base64字符数据转成图片
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            ms.Close();
            return image;
        }

        /// <summary>
        /// 创建缩略图
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="size">尺寸</param>
        /// <returns></returns>
        public static Image CreateThumnail(Image image, Size size)
        {
            Size os = new Size(image.Width, image.Height);
            Size newSize = os;
            double ratio = 0;
            if (size.Height * os.Width > size.Width * os.Height) //宽度比率缩放
            {
                if (size.Width < os.Width)
                {
                    //求缩放比例
                    ratio = (double)size.Width / (double)os.Width;
                    double w = (double)os.Width * ratio;
                    double h = (double)os.Height * ratio;
                    newSize = new Size((int)w, (int)h);
                }
            }
            else
            {
                if (size.Height < os.Height)
                {
                    //求缩放比例
                    ratio = (double)size.Height / (double)os.Height;
                    double w = (double)os.Width * ratio;
                    double h = (double)os.Height * ratio;
                    newSize = new Size((int)w, (int)h);
                }
            }

            if (newSize.Width < 1 || newSize.Height < 1)
                return image;

            Bitmap thum = new Bitmap(newSize.Width, newSize.Height);
            using (Graphics g = Graphics.FromImage(thum))
            {
                // 设置画布的描绘质量
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, new Rectangle(0, 0, newSize.Width, newSize.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                g.Dispose();
                image.Dispose();
            }

            return thum;
        }

       /// <summary>
        ///  创建图片缩略图
       /// </summary>
       /// <param name="imgBuff"></param>
       /// <param name="size"></param>
       /// <returns></returns>
        public static Image CreateThumnail(byte[] imgBuff, Size size)
        {
            var ms = new MemoryStream(imgBuff);
            var image = CreateThumnail(Image.FromStream(ms), size);
            ms.Close();
            return image;
        }

        /// <summary>
        /// 保存缩略图
        /// </summary>
        /// <param name="filename">源图片文件的绝对路径</param>
        /// <param name="size"></param>
        /// <param name="newFile">保存文件的绝对路径</param>
        /// <returns>返回保存文件的绝对路径</returns>
        public static string SaveThumnail(string filename, Size size, string newFile = null)
        {
            var image = CreateThumnail(Image.FromFile(filename), size);

            var fileInfo = new FileInfo(filename);

            if (string.IsNullOrEmpty(newFile))
            {
                newFile = Path.Combine(fileInfo.DirectoryName, string.Format("{0}x{1}-{2}", new object[]{
                    image.Width,
                    image.Height,
                    fileInfo.Name
                }));
            }
            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象.
            ImageCodecInfo jpegICI = ImageCodecInfo.GetImageEncoders()
                .Where(w => w.FilenameExtension == fileInfo.Extension.ToUpperInvariant())
                .FirstOrDefault();

            if (jpegICI != null)
            {
                EncoderParameters encoderParams = new EncoderParameters();
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 96L);
                image.Save(newFile, jpegICI, encoderParams);
            }
            else
            {
                image.Save(newFile, image.RawFormat);
            }
            image.Dispose();
            return newFile;
        }
    }
}
