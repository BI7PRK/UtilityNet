using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Hosting;

namespace UtilityNet.FileIO
{

    /// <summary>
    /// 上传消息代码
    /// </summary>
    public enum UploadCode
    {
        NoFile = 0,
        BadFile = 1,
        Success = 2,
        Exceed = 3,
        CencelType = 4,
        Unknown = 5
    }

    /// <summary>
    /// 为创建一个Bmp文件提供的一个基类
    /// </summary>
    public class StringBmp
    {
        private string text = "";
        private Font font = new Font("", 20, FontStyle.Bold);
        private Color color = Color.Black;
        private Color bgcolor = Color.White;
        private string file = "test.png";

        public StringBmp(string text)
        {
            this.text = text;
        }
        public StringBmp(string text, string file)
        {
            this.text = text;
            this.file = file;
        }

        public StringBmp(string text, Font font)
        {
            this.text = text;
            this.font = font;
        }

        public StringBmp(string text, string file, Font font)
        {
            this.text = text;
            this.font = font;
            this.file = file;
        }

        public StringBmp(string text, Color color)
        {
            this.text = text;
            this.color = color;
        }
        public StringBmp(string text, string file, Color color)
        {
            this.text = text;
            this.color = color;
            this.file = file;
        }
        public StringBmp(string text, Color color, Color bgcolor)
        {
            this.text = text;
            this.color = color;
            this.bgcolor = bgcolor;
        }
        public StringBmp(string text, string file, Color color, Color bgcolor)
        {
            this.text = text;
            this.color = color;
            this.bgcolor = bgcolor;
            this.file = file;
        }
        public StringBmp(string text, Font font, Color color)
        {
            this.text = text;
            this.font = font;
            this.color = color;
        }
        public StringBmp(string text, string file, Font font, Color color)
        {
            this.text = text;
            this.font = font;
            this.color = color;
            this.file = file;
        }
        public StringBmp(string text, Font font, Color color, Color bgcolor)
        {
            this.text = text;
            this.font = font;
            this.color = color;
            this.bgcolor = bgcolor;
        }

        public string Text
        {
            get { return this.text; }
        }
        public Font BmpFont
        {
            get { return this.font; }
        }
        public Color TextColor
        {
            get { return this.color; }
        }
        public Color FillColor
        {
            get { return this.bgcolor; }
        }

        public string Filename
        {
            get { return this.file; }
        }
    }

    /// <summary>
    ///FileUploader 的摘要说明
    ///作者：http://goslam.cn
    ///日期：2010-12-13 17:19:35
    /// </summary>
    public class FileUploader : IDisposable
    {
        private HttpPostedFile postFile = null;
        public FileUploader(HttpPostedFile postFile)
        {
            this.postFile = postFile;
        }
        public void Dispose() { }

        /// <summary>
        /// 获取HttpPostedFile 对象
        /// </summary>
        public HttpPostedFile PostedFile
        {
            get { return this.postFile; }
        }

        private int postSize = 300; // 默认300KB
        /// <summary>
        /// 设置或获取限制文件大小,单位KB
        /// </summary>
        public int PostSize
        {
            get { return this.postSize; }
            set { this.postSize = value; }
        }

        private string[] fileExts = { "jpg", "jpeg", "gif", "png", "bmp", "png" };
        /// <summary>
        /// 设置或获取限制文件后缀,默认为: { "jpg", "jpeg", "gif", "png", "bmp", "png" };
        /// </summary>
        public string[] FileExts
        {
            get { return this.fileExts; }
            set { this.fileExts = value; }
        }

        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        public string FileExtension
        {
            get
            {
                var filename = this.postFile.FileName;
                return filename.Substring(filename.LastIndexOf("."));
            }
        }

        private string savePath = string.Empty;
        /// <summary>
        /// 设置或获取保存路径
        /// </summary>
        public string SavePath
        {
            set { this.savePath = value; }
        }
        private string thumPath = string.Empty;
        /// <summary>
        /// 设置缩略图保存路径
        /// </summary>
        public string ThumPath
        {
            set { this.thumPath = value; }
        }

        private Size thumbnailSize = new Size(120, 120);
        /// <summary>
        /// 设置或获取缩略图的尺寸
        /// </summary>
        public Size ThumbnailSize
        {
            get { return this.thumbnailSize; }
            set { this.thumbnailSize = value; }
        }

        private UploadCode errorCode = UploadCode.Unknown;
        /// <summary>
        /// 获取上传后的消息代码
        /// </summary>
        public UploadCode StatusCode
        {
            get { return this.errorCode; }
        }
        /// <summary>
        /// 将文件大小(KB)换算成字节数
        /// </summary>
        private int FormatSize
        {
            get
            {
                return postSize * 1024;
            }
        }
        private int _length;
        /// <summary>
        /// 获取上传文件的大小
        /// </summary>
        public int GetLength
        {
            get
            {
                return this._length;
            }
        }

        private string MapPath(string abs)
        {
            return FormatAbsPath(HostingEnvironment.MapPath(@"/" + abs));
        }

        private string MapPath(string abs, out string rel)
        {
            rel = FormaRelPath(abs);
            return FormatAbsPath(HostingEnvironment.MapPath(@"/" + abs));
        }

        private string FormatAbsPath(string abs)
        {

            abs = abs.Replace(@"/", @"\");
            abs = abs.Replace(@"\\", @"\");
            return abs;
        }

        private string FormaRelPath(string rel)
        {

            rel = rel.Replace(@"\", @"/");
            rel = rel.Replace(@"//", @"/");
            return rel;
        }

        private string GetVirtualPath(string path)
        {
            string root = HostingEnvironment.MapPath("/");
            return "/" + this.FormaRelPath(path.Replace(root, ""));
        }

        private string fileName = "";
        private string webUrl = "";

        public string SaveToFile(string fileName)
        {
            this.fileName = fileName;
            return SaveToFile(false);
        }
        public string SaveToFile()
        {
            return SaveToFile(true);
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="autoName">是否自动命名</param>
        /// <returns></returns>
        private string SaveToFile(bool autoName)
        {

            this._length = postFile.ContentLength;
            string mFile = postFile.FileName;
            string mType = postFile.ContentType;
            if (mFile == "")
            {
                errorCode = UploadCode.NoFile;
                return "";
            }

            if (this._length == 0)
            {
                errorCode = UploadCode.BadFile;
                return "";
            }

            if (!CheckFileExe(mFile))
            {
                errorCode = UploadCode.CencelType;
                return "";
            }

            if (this._length > this.FormatSize)
            {
                errorCode = UploadCode.Exceed;
                return "";
            }

            if (autoName)
            {
                this.fileName = this.HexNewFileName + "." + this.GetFileExe(mFile);
            }
            else if (string.IsNullOrEmpty(this.fileName))
            {
                this.fileName = mFile;
            }
            else {
                var index = this.fileName.LastIndexOf(".");
                if (index < 0)
                {
                    this.fileName += "." + this.GetFileExe(mFile);
                }
                else {
                    this.fileName = this.fileName.Substring(0, index + 1) + this.GetFileExe(mFile);
                }
            }
            DateTime d = DateTime.Now;
            string tmp = "";
            if (autoName)
            {
                tmp = string.Format(@"{0}-{1}\{2}", new object[] { d.Year, d.Month, d.Day });
            }
            this.savePath = this.savePath + tmp + "\\";
            this.savePath = this.MapPath(this.savePath, out webUrl);

            if (!Directory.Exists(this.savePath))
                Directory.CreateDirectory(this.savePath);
            try
            {
                webUrl += this.fileName;
                this.fileName = this.savePath + this.fileName;
                postFile.SaveAs(this.fileName);
                errorCode = UploadCode.Success;
                return webUrl;
            }
            catch
            {
                errorCode = UploadCode.Unknown;
            }
            return "";
        }

        public string GetThumbnail()
        {
            return this.GetThumbnail(this.webUrl);
        }
        /// <summary>
        /// 获取生成的缩略图
        /// </summary>
        /// <returns></returns>
        public string GetThumbnail(string orgFile)
        {
            string _org = orgFile;
            if ((this.thumbnailSize.Width == 0) || (this.thumbnailSize.Height == 0))
                return _org;
            if (orgFile.IndexOf(":") == -1) orgFile = this.MapPath(orgFile);
            if ((!File.Exists(orgFile))) return "";
            int bitmapWidth = this.thumbnailSize.Width;
            int bitmapHeight = this.thumbnailSize.Height;
            
            string thumFileUrl = "";
            try
            {
                using (Image loadImg = Image.FromFile(orgFile)) //原文件路径
                {
                    int imageFromWidth = loadImg.Width;
                    int imageFromHeight = loadImg.Height;
                    if (bitmapWidth > imageFromWidth && bitmapHeight > imageFromHeight)
                    {
                        return _org;
                    }
                    if (bitmapHeight * imageFromWidth > bitmapWidth * imageFromHeight)
                    {
                        bitmapHeight = imageFromHeight * bitmapWidth / imageFromWidth;
                    }
                    else
                    {
                        bitmapWidth = imageFromWidth * bitmapHeight / imageFromHeight;
                    }
                    bitmapWidth = (bitmapWidth < 1) ? 1 : bitmapWidth;
                    bitmapHeight = (bitmapHeight < 1) ? 1 : bitmapHeight;

                    Bitmap NewBmp = new Bitmap(bitmapWidth, bitmapHeight);
                    Graphics newGrh = System.Drawing.Graphics.FromImage(NewBmp);
                    newGrh.SmoothingMode = SmoothingMode.HighQuality;
                    newGrh.InterpolationMode = InterpolationMode.High;
                    //把原始图像绘制成上面所设置宽高的缩小图
                    Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, bitmapWidth, bitmapHeight);
                    newGrh.DrawImage(loadImg, rectDestination, 0, 0, imageFromWidth, imageFromHeight, GraphicsUnit.Pixel);
                    DateTime dt = DateTime.Now;
                    if (string.IsNullOrEmpty(this.thumPath))
                    {
                        FileInfo fileinfo = new FileInfo(orgFile);
                        this.thumPath = fileinfo.DirectoryName + "\\";
                        fileinfo = null;
                    }
                    else
                    {
                        this.thumPath = string.Format("{0}\\{1}-{2}\\{3}\\",
                        new object[] {
                            this.MapPath(this.thumPath),
                            dt.Year, 
                            dt.Month, 
                            dt.Day,
                        });
                    }

                    if (!Directory.Exists(this.thumPath))
                        Directory.CreateDirectory(this.thumPath);
                    this.thumPath += this.HexNewFileName + "[m]." + this.GetFileExe(orgFile);
                    NewBmp.Save(this.thumPath);
                    NewBmp.Dispose();
                    thumFileUrl = this.GetVirtualPath(this.thumPath);
                }
            }
            catch(Exception ex)
            {
                thumFileUrl = this.GetVirtualPath(orgFile);
            }
            return thumFileUrl;
 

        }

        /// <summary>
        /// 判断图片的PixelFormat 是否在 引发异常的 PixelFormat 之中
        /// </summary>
        /// <param name="imgPixelFormat">原图片的PixelFormat</param>
        /// <returns></returns>
        private bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
        {
            PixelFormat[] indexedPixelFormats = {
                                                    PixelFormat.Undefined,
                                                    PixelFormat.DontCare,
                                                    PixelFormat.Format16bppArgb1555,
                                                    PixelFormat.Format1bppIndexed,
                                                    PixelFormat.Format4bppIndexed,
                                                    PixelFormat.Format8bppIndexed
                                                };

            foreach (PixelFormat pf in indexedPixelFormats)
            {
                if (pf.Equals(imgPixelFormat))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 给图片打水印
        /// </summary>
        /// <param name="text">可以是文字，也可以是图片的绝对路径</param>
        public void DoWatermark(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (!File.Exists(this.fileName))
                return;

            string tmp = this.savePath + DateTime.Now.Ticks.ToString() + "." + FileExt;
            using (Image loadPic = Image.FromFile(this.fileName))
            {
                if (loadPic.Width <= 100)
                    return; // 100宽度以上才水印
                bool isBmp = false;
                Graphics Grhs;
                Bitmap bmp = null;
                if (IsPixelFormatIndexed(loadPic.PixelFormat))
                {
                    bmp = new Bitmap(loadPic.Width, loadPic.Height, PixelFormat.Format48bppRgb);
                    Grhs = Graphics.FromImage(bmp);
                    Grhs.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    Grhs.SmoothingMode = SmoothingMode.HighQuality;
                    Grhs.CompositingQuality = CompositingQuality.HighQuality;
                    Grhs.DrawImage(loadPic, 0, 0);
                    isBmp = true;
                }
                else
                {
                    Grhs = Graphics.FromImage(loadPic);
                }

                int x = loadPic.Width / 2;
                int y = (loadPic.Height / 3) * 2;

                string printFile = this.MapPath(text);
                if (File.Exists(printFile))
                {
                    //加图片水印
                    Image copyImage = Image.FromFile(printFile);
                    int wProp = copyImage.Width;
                    int hProp = copyImage.Height;
                    x -= wProp / 2;
                    //水印透明
                    Single[][] matrixItems = {
                                                 new Single[]{1, 0, 0, 0, 0},
                                                 new Single[]{0, 1, 0, 0, 0},
                                                 new Single[]{0, 0, 1, 0, 0},
                                                 new Single[]{0, 0, 0, 60.0F / 100.0F, 0}, // 透明度 60%
                                                 new Single[]{0, 0, 0, 0, 1}
                                             };

                    ColorMatrix colorMatrix = new ColorMatrix(matrixItems);
                    ImageAttributes imgAbs = new ImageAttributes();
                    imgAbs.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    Rectangle Rect = new Rectangle(x, y, copyImage.Width, copyImage.Height);
                    Grhs.DrawImage(copyImage, Rect, 0, 0, wProp, hProp, GraphicsUnit.Pixel, imgAbs);
                    copyImage.Dispose();
                }
                else
                {
                    int[] Sizes = new int[] { 36, 34, 32, 30, 28, 26, 24, 22, 20, 18, 16, 14, 12, 10, 8, 6, 4 };
                    Font oFont = new Font("Arial", 2, FontStyle.Bold);//默认为了出现意外错误
                    SizeF fontSize = new SizeF();
                    foreach (int f in Sizes)
                    {
                        oFont = new Font("Arial", f, FontStyle.Bold);
                        fontSize = Grhs.MeasureString(text, oFont);
                        if ((int)fontSize.Width < (int)(loadPic.Width / 2))
                            break;
                    }
                    x -= (int)fontSize.Width / 2;
                    SolidBrush strColor = new SolidBrush(Color.FromArgb(120, 97, 150, 150));
                    SolidBrush bgColor = new SolidBrush(Color.FromArgb(120, 0, 0, 0));
                    Grhs.DrawString(text, oFont, strColor, x, y);
                    Grhs.DrawString(text, oFont, bgColor, x - 1, y - 1);//阴影效果
                }
                if (isBmp && bmp != null)
                {
                    bmp.Save(tmp, ImageFormat.Jpeg);
                    bmp.Dispose();
                }
                else
                {
                    loadPic.Save(tmp);
                }
                Grhs.Dispose();
            }
            File.Delete(this.fileName);
            File.Move(tmp, this.fileName);
        }

        /// <summary>
        /// 建立一张图片
        /// </summary>
        /// <param name="parm">StringBmp基类</param>
        /// <returns></returns>
        public string NewBitmap(StringBmp parm)
        {
            int x = 0; int y = 2;
            string file = parm.Filename;
            file = (string.IsNullOrEmpty(file)) ? "test.png" : file;
            int width, height;
            using (Bitmap temBmp = new Bitmap(1, 1))
            {
                using (Graphics tmpGrs = Graphics.FromImage(temBmp))
                {
                    SizeF size = tmpGrs.MeasureString(parm.Text, parm.BmpFont);
                    width = (int)size.Width;
                    height = (int)size.Height + y;
                }
            }
            using (Bitmap image = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    using (SolidBrush strColor = new SolidBrush(parm.TextColor))
                    {
                        g.FillRectangle(new SolidBrush(parm.FillColor), 0, 0, width, height);
                        g.DrawString(parm.Text, parm.BmpFont, strColor, x, y);
                        image.MakeTransparent(System.Drawing.Color.White);
                        image.Save(this.MapPath(file), ImageFormat.Png);
                    }
                }
            }
            return file;
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <returns></returns>
        private string HexNewFileName
        {
            get
            {
                string D = DateTime.Now.ToString("yyyyMMddhhmmss");
                string rod = new Random().Next(9999).ToString();
                return (D + rod);
            }
        }

        /// <summary>
        /// 是否为允许上传的类型
        /// </summary>
        /// <param name="mFile">转入的文件名</param>
        /// <returns></returns>
        private bool CheckFileExe(string mFile)
        {
            string[] all = { "*" };
            if (string.Join(",", this.fileExts) == string.Join(",", all)) return true;//允许上传所有文件
            foreach (string ex in fileExts)
            {
                if (ex.Equals(GetFileExe(mFile), StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 取得文件的后缀名
        /// </summary>
        /// <param name="mFile">转入的文件名</param>
        /// <returns></returns>
        private string GetFileExe(string mFile)
        {
            return mFile.Substring(mFile.LastIndexOf(".") + 1).ToLower();
        }
        /// <summary>
        /// 取得文件的后缀名
        /// </summary>
        private string FileExt
        {
            get
            {
                return this.fileName.Substring(this.fileName.LastIndexOf(".") + 1).ToLower();
            }
        }

    }
}