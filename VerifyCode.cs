using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

namespace UtilityNet
{
    /// <summary>
    /// 生成验证码类
    /// Powered by 赵振斌 @2014-03-05
    /// </summary>
    public abstract class VerifyCode
    {

        /// <summary>
        /// 生成随机代码
        /// </summary>
        /// <param name="len">长度</param>
        /// <param name="numOnly">是否仅为数字</param>
        /// <returns></returns>
        public static string RandomCode(int len, bool numOnly = false)
        {
            string strCode = string.Empty;
            if (!numOnly)
            {
                int number = 0;
                char code;
                Random random = new Random();
                for (int i = 0; i < len; i++)
                {
                    number = random.Next();
                    if (number % 2 == 0)
                    {
                        code = (char)('0' + (char)(number % 10));
                    }
                    else
                    {
                        code = (char)('A' + (char)(number % 26));
                    }
                    strCode += code.ToString();
                }
            }
            else
            {
                int min = int.Parse("1".PadRight(len, '0'));
                int max = int.Parse("9".PadRight(len, '9'));
                strCode = new Random().Next(min, max).ToString();
            }

            return strCode;

        }

        // 随机颜色
        private static Color GetRandomColor()
        {
            Random rm = new Random();
            int R = rm.Next(255);
            Thread.Sleep(rm.Next(100));
            int G = rm.Next(255);
            Thread.Sleep(rm.Next(100));
            int B = rm.Next(255);
            return Color.FromArgb(R, G, B);
        }
        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="len">长度</param>
        /// <param name="checkCode">代码字符串</param>
        /// <returns></returns>
        public static byte[] CreateCheckCodeImage(int len, ref string checkCode)
        {
            len = (len > 6) ? 6 : len;
            len = (len < 2) ? 2 : len;
            checkCode = RandomCode(len);

            Font font = new Font("Arial", 18, (FontStyle.Bold | FontStyle.Italic));
            var test = Graphics.FromImage(new Bitmap(5, 5));
            SizeF sizefont = test.MeasureString(checkCode, font);
            test.Dispose();

            Bitmap image = new Bitmap(((int)sizefont.Width + 2), (int)sizefont.Height);
            Graphics g = Graphics.FromImage(image);


            try
            {
                //生成随机生成器
                Random random = new Random();

                //清空图片背景色
                g.Clear(Color.White);
                //画图片的背景噪线
                for (int i = 0; i < 30; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), GetRandomColor(), GetRandomColor(), 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);

                //画图片的前景噪音点
                for (int i = 0; i < 80; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Coral), 0, 0, image.Width - 1, image.Height - 1);
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
    }
}
