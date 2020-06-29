
using System;
using System.Collections.Generic;
using System.IO;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace WebApi.Models
{

    public class CaptchaHelper
    {
        /// <summary>
        /// Image
        /// </summary>
        /// <returns></returns>
        public static byte[] GetCaptcha()
        {

            var width = 60;
            var height = 40;
            using (Image img = new Image<Rgba32>(width, height))
            {
                PathBuilder pathBuilder = new PathBuilder();
                pathBuilder.SetOrigin(new PointF(0, 0));
                var list = new List<PointF>();
                for (int i = 0; i < 4; i++)
                {
                    var widthPointF = new PointF(new Random().Next(1, width), new Random().Next(1, width));
                    var heightPointF = new PointF(new Random().Next(1, height), new Random().Next(1, height));
                    list.Add(widthPointF);
                    list.Add(heightPointF);
                }
                pathBuilder.AddSegment(new LinearLineSegment(list.ToArray()));

                IPath path = pathBuilder.Build();

                var font = SystemFonts.CreateFont("Arial", 36F, FontStyle.Regular);
                string text = "CCaD";
                var textGraphicsOptions = new TextGraphicsOptions() // draw the text along the path wrapping at the end of the line
                {
                    TextOptions = {
                        WrapTextWidth = width 
                    }
                }; 

                var glyphs = TextBuilder.GenerateGlyphs(text, path, new RendererOptions(font, textGraphicsOptions.TextOptions.DpiX, textGraphicsOptions.TextOptions.DpiY)
                {
                    HorizontalAlignment = textGraphicsOptions.TextOptions.HorizontalAlignment,
                    TabWidth = textGraphicsOptions.TextOptions.TabWidth,
                    VerticalAlignment = textGraphicsOptions.TextOptions.VerticalAlignment,
                    WrappingWidth = textGraphicsOptions.TextOptions.WrapTextWidth,
                    ApplyKerning = textGraphicsOptions.TextOptions.ApplyKerning
                });

                img.Mutate(ctx => ctx
                    .Fill(Color.White) // white background image
                                .Draw(Color.Gray, 3, path) // draw the path so we can see what the text is supposed to be following
                                .Fill(Color.Black, glyphs));

                //img.Save("output/wordart100.png");
                using (var ms = new MemoryStream())
                {
                    img.SaveAsGif(ms);
                    return ms.ToArray();
                }
            }
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static byte[] GetCaptcha1()
        {
   


            var width = 60;
            var height = 40;
            using (Image img = new Image<Rgba32>(width, height))
            {
                PathBuilder pathBuilder = new PathBuilder();
                pathBuilder.SetOrigin(new PointF(0, 0));
                var list = new List<PointF>();
                for (int i = 0; i < 4; i++)
                {
                    var widthPointF = new PointF(new Random().Next(1, width), new Random().Next(1, width));
                    var heightPointF = new PointF(new Random().Next(1, height), new Random().Next(1, height));
                    list.Add(widthPointF);
                    list.Add(heightPointF);
                }
                pathBuilder.AddSegment(new LinearLineSegment(list.ToArray()));

                pathBuilder.AddLine()


                IPath path = pathBuilder.Build();

                var font = SystemFonts.CreateFont("Arial", 36F, FontStyle.Regular);
                string text = "1234";
                var textGraphicsOptions = new TextGraphicsOptions() // draw the text along the path wrapping at the end of the line
                {
                    TextOptions = {
                        WrapTextWidth = width
                    }
                };

                var glyphs = TextBuilder.GenerateGlyphs(text, path, new RendererOptions(font, textGraphicsOptions.TextOptions.DpiX, textGraphicsOptions.TextOptions.DpiY)
                {
                    HorizontalAlignment = textGraphicsOptions.TextOptions.HorizontalAlignment,
                    TabWidth = textGraphicsOptions.TextOptions.TabWidth,
                    VerticalAlignment = textGraphicsOptions.TextOptions.VerticalAlignment,
                    WrappingWidth = textGraphicsOptions.TextOptions.WrapTextWidth,
                    ApplyKerning = textGraphicsOptions.TextOptions.ApplyKerning
                });

                img.Mutate(ctx => ctx
                    .Fill(Color.White) // white background image
                                .Draw(Color.Gray, 3, path) // draw the path so we can see what the text is supposed to be following
                                .Fill(Color.Black, glyphs));

                //img.Save("output/wordart100.png");
                using (var ms = new MemoryStream())
                {
                    img.SaveAsGif(ms);
                    return ms.ToArray();
                }
            }

        }
        /// <summary>  
        /// 该方法用于生成指定位数的随机数  
        /// </summary>  
        /// <param name="VcodeNum">参数是随机数的位数</param>  
        /// <returns>返回一个随机数字符串</returns>  
        public string RndNum(int VcodeNum)
        {
            //验证码可以显示的字符集合  
            string Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,p" +
                           ",q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,P,Q" +
                           ",R,S,T,U,V,W,X,Y,Z";
            string[] VcArray = Vchar.Split(new Char[] { ',' });//拆分成数组   
            string code = "";//产生的随机数  
            int temp = -1;//记录上次随机数值，尽量避避免生产几个一样的随机数  

            Random rand = new Random();
            //采用一个简单的算法以保证生成随机数的不同  
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));//初始化随机类  
                }
                int t = rand.Next(61);//获取随机数  
                if (temp != -1 && temp == t)
                {
                    return RndNum(VcodeNum);//如果获取的随机数重复，则递归调用  
                }
                temp = t;//把本次产生的随机数记录起来  
                code += VcArray[t];//随机数的位数加一  
            }
            return code;
        }
    }

}
 