using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using ItemDoc.Framework.Utilities;
using ItemDoc.Framework.Utility;
using Sop.Common.Img.Gif;
using static ItemDoc.Framework.Utility.EncryptionUtility;

namespace ItemDoc.Framework.Validation
{
    /// <summary>
    /// 验证码操作类。
    /// 1、实现动态生成GIF验证码
    /// 2、可以配置验证字符
    /// </summary>
    public class Captcha
    {
        /// <summary>
        /// 定义验证码中所有的字符
        /// </summary>
        public static string AllChar = "&,A,B,C,D,E,F,G,H,J,K,L,M,N,%,P,Q,R,S,T,U,V,W,X,Y,Z,@,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,#,j,k,m,n,p,q,r,s,t,u,v,w,x,y";

        /// <summary>
        /// 获取或设置验证码字数。不小于4位字数
        /// </summary>
        private static int _validateCodeCount = 4;

        /// <summary>
        /// 
        /// </summary>
        public Captcha()
        {
            if (int.TryParse(ConfigurationManager.AppSettings["Captcha.CodeCount"], out _validateCodeCount))
            {
                _validateCodeCount = _validateCodeCount > 4 || _validateCodeCount < 16 ? 4 : _validateCodeCount;
            }
            string code = ConfigurationManager.AppSettings["Captcha.Code"] ?? string.Empty;
            AllChar = code.Length > 7 ? code : AllChar;
        }
        #region 私有变量/函数方法
        /// <summary>
        /// 验证码存储时用的Key值。
        /// </summary>
        private static readonly string SopCaptchaKey = "_SopCaptcha";



        /// <summary>
        /// 是否消除锯齿
        /// </summary>
        private static bool FontTextRenderingHint = false;
        /// <summary>
        /// 图像的width
        /// </summary>
        private static int ImageHeight = 27;
        /// <summary>
        /// private Random random = new Random();
        /// </summary>
        private static Random random = new Random();




        /// <summary>
        /// 验证码
        /// </summary>
        public static string ValidateCode = "";
        /// <summary>
        /// 验证码字体
        /// </summary>
        private static string ValidateCodeFont = "Segoe UI";
        /// <summary>
        /// 验证码型号(像素)
        /// </summary>
        private static float ValidateCodeSize = 13f;


        /// <summary>
        /// 生成一帧的BMP图象
        /// </summary>
        /// <param name="ImageFrame"></param>
        private static void CreateImageBmp(out Bitmap ImageFrame)
        {
            //获得验证码字符
            char[] array = ValidateCode.ToCharArray(0, (int)_validateCodeCount);
            //图像的宽度-与验证码的长度成一定比例
            int width = (int)((double)((float)_validateCodeCount * ValidateCodeSize) * 1.3 + 7.0);
            //创建一个长20，宽iwidth的图像对象
            ImageFrame = new Bitmap(width, ImageHeight);
            //创建一个新绘图对象
            Graphics graphics = Graphics.FromImage(ImageFrame);
            //清除背景色，并填充背景色|Color.Transparent为透明
            graphics.Clear(Color.White);
            //绘图用的字体和字号
            Font font = new Font(ValidateCodeFont, ValidateCodeSize, FontStyle.Bold);
            //绘图用的刷子大小,改为颜色随机
            //Brush ImageBrush = new SolidBrush(Color.Red);
            //字体高度计算
            int maxValue = (int)Math.Max((float)ImageHeight - ValidateCodeSize - 3f, 2f);
            //开始随机安排字符的位置，并画到图像里，验证码位数
            for (int i = 0; i < (int)_validateCodeCount; i++)
            {
                //获取随机生成的颜色
                Brush brush = new SolidBrush(GetRandomColor());
                int[] array2 = new int[]
                {
                    (int)((float)i * ValidateCodeSize) + random.Next(1) + 3,
                    random.Next(maxValue)
                };
                Point p = new Point(array2[0], array2[1]);
                //消除锯齿操作
                if (FontTextRenderingHint)
                {
                    graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
                }
                else
                {
                    graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                }
                //格式化刷子属性-用指定的刷子、颜色等在指定的范围内画图
                graphics.DrawString(array[i].ToString(), font, brush, p);
            }
            graphics.Dispose();
        }
        /// <summary>
        /// 创建GIF动画
        /// </summary>
        /// <returns>byte数组图片</returns>
        private static byte[] CreateImageGif()
        {
           
            AnimatedGifEncoder animatedGifEncoder = new AnimatedGifEncoder();

            MemoryStream memoryStream = new MemoryStream();
            animatedGifEncoder.Start();
            //确保视觉残留
            animatedGifEncoder.SetDelay(5);
            animatedGifEncoder.SetRepeat(0);
            animatedGifEncoder.SetTransparent(Color.White);
            for (int i = 0; i < 10; i++)
            {
                Bitmap bitmap;
                //创建一帧的图像
                CreateImageBmp(out bitmap);
                //生成随机线条
                DisposeImageBmp(ref bitmap);
                //输出绘图,将图像保存到指定的流
                bitmap.Save(memoryStream, ImageFormat.Png);
                animatedGifEncoder.AddFrame(Image.FromStream(memoryStream));
                memoryStream = new MemoryStream();
            }
            animatedGifEncoder.OutPut(ref memoryStream);

            //输出二级制图片数组内容
            byte[] imgbyte = memoryStream.ToArray();
            return imgbyte;
        }

        /// <summary>
        /// 保存生成的验证码值，以备后来与用户输入作比较。
        /// </summary>
        /// <param name="captcha">生成的验证码</param>
        private static void SaveCheckCode(string captcha)
        {
            string sValue = EncryptString(captcha.ToLower(), AllChar);
            WebUtility.SetCookie(SopCaptchaKey, sValue, DateTime.Now);
        }




        /// <summary>
        /// 创建验证码
        /// </summary>
        private static void CreateValidate()
        {
            ValidateCode = "";
            //将验证码中所有的字符保存在一个字符串数组中
            string[] array = AllChar.Split(new char[] { ',' });
            int num = -1;

            for (int i = 0; i < (int)_validateCodeCount; i++)
            {
                //主要是防止生成相同的验证码
                if (num != -1)
                {
                    //生成一个随机对象，加入时间的刻度
                    random = new Random(i * num * (int)DateTime.Now.Ticks);
                }
                int num2 = random.Next(array.Length);
                if (num == num2)
                {
                    //相等的话重新生成
                    CreateValidate();
                }
                num = num2;
                ValidateCode += array[num];
            }
            //错误检测,去除超过指定位数的验证码
            if (ValidateCode.Length > (int)_validateCodeCount)
            {
                ValidateCode = ValidateCode.Remove((int)_validateCodeCount);
            }
        }
        /// <summary>
        /// 处理生成的BMP
        /// </summary>
        /// <param name="ImageFrame"></param>
        private static void DisposeImageBmp(ref Bitmap ImageFrame)
        {
            //创建绘图对象
            Graphics graphics = Graphics.FromImage(ImageFrame);
            //创建铅笔对象
            Pen pen = new Pen(GetRandomColor(), 1f);
            //创建随机点
            Point[] array = new Point[2];
            //随机画线
            for (int i = 0; i < 10; i++)
            {
                array[0] = new Point(random.Next(ImageFrame.Width), random.Next(ImageFrame.Height));
                graphics.DrawLine(pen, array[0], new Point(array[0].X + 1, array[0].Y));
            }
            graphics.Dispose();
        }
        /// <summary>
        /// 获取随机颜色
        /// </summary>
        /// <returns></returns>
        private static Color GetRandomColor()
        {
            return Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
        }


        #endregion


        /// <summary>
        /// 校验用户输入的验证码是否正确。
        /// </summary>
        /// <param name="captcha">用户输入的验证码</param>
        /// <returns>true:验证通过，false:验证失败</returns>
        public static bool ValidateCheckCode(string captcha)
        {
            if (string.IsNullOrEmpty(captcha))
            {
                return false;
            }
            captcha = captcha.Replace(" ", "");
            //匹配中文（全角）空格
            captcha = captcha.Replace("\u3000", "");
            string normalCookie = WebUtility.GetCookie(SopCaptchaKey);
            string value = EncryptString(captcha.ToLower(), AllChar);
            if (string.IsNullOrEmpty(normalCookie))
            {
                return false;
            }

            if (normalCookie.Equals(value))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 输出验证码图片，并存储验证码值以待核验。
        /// </summary>
        /// <returns>返回二级制流byte[]</returns>
        public static byte[] SetByteValidate()
        {
            //把生成的验证码
            CreateValidate();
            byte[] asd = CreateImageGif();
            //保存到cookie中
            Captcha.SaveCheckCode(ValidateCode);
            return asd;
        }

        /// <summary>
        /// 输出验证码图片，并存储验证码值以待核验
        /// </summary>
        /// <returns></returns>
        public static Stream SetStreamValidate()
        {
            //把生成的验证码输入到cookie
            CreateValidate();
            Stream ms = new MemoryStream(CreateImageGif());
            //保存到cookie中
            Captcha.SaveCheckCode(ValidateCode);
            return ms;
        }

    }

  
}
