using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Common
{
    public class VerifyCode
    {
        private Random random = new Random();


        private Color getRandomColor()
        {
            return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        }

        private Color getRandomColor(int min , int max)
        {
            return Color.FromArgb(random.Next(min,max), random.Next(min,max), random.Next(min,max));
        }

        /// <summary>
        /// 获取验证码 文字 的 方法， 
        /// </summary>
        /// <param name="digits">验证码的文字数量</param>
        /// <returns></returns>
        public string GetVerifyCode(int length)
        {
            string lowers = "abcdefghijklmnopqrsauvwxyz";
            string uppers = lowers.ToUpper();
            string digits = "0123456789";

            string str = lowers + uppers + digits;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(str[random.Next(0, str.Length)]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 根据 验证码的文字， 生成对应的 图片的方法
        /// </summary>
        /// <param name="verifyCode">验证码文字</param>
        /// <returns>验证码图片</returns>
        public Image GetVerifyImage(string verifyCode)
        {
            // "a09I"
            Bitmap image = new Bitmap(65, 20);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.FromArgb(230, 230, 230));

            // 干扰线
            for (int i = 0; i < 10; i++)
            {
                g.DrawLine(
                    new Pen(getRandomColor(150, 256)),
                    new Point(random.Next(0, 65), random.Next(0, 20)),
                    new Point(random.Next(0, 65), random.Next(0, 20))
                );
            }

            // 绘制文字
            for (int i = 0; i < verifyCode.Length ; i++)
            {
                g.DrawString(
                    verifyCode[i].ToString(),
                    new Font("consolas", 13),
                    new SolidBrush(getRandomColor(0,100)),
                    new Point(15 * i, 0)
                );
            }

            // 干扰点
            for (int i = 0; i < 100; i++)
            {
                image.SetPixel(random.Next(0, 65), random.Next(0, 20), getRandomColor(150,256) );
            }

            return image;
        }
    }
}
