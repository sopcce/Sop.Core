using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Sop.Common.Helper.Img.Gif;

namespace Sop.Common.Helper.Img
{
    /// <summary>
    /// 
    /// </summary>
    public class Animate : IAnimate
    {
        #region Instance

        private static volatile IAnimate _instance = null;
        private static readonly object Lock = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IAnimate Instance()
        {
            if (_instance == null)
            {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Animate();
                    }
                }
            }
            return _instance;
        }

        #endregion Instance


        //接口简介
        //    动图合成接口（animate）用于将多张图片合成 GIF 动图。
        //注：
        //接口可支持处理的原图片格式有 jpeg 和 png。



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool GetAminmate(string[] imageFilePaths, string outputFilePath, int delay = 500)
        {
            foreach (var imgFilePath in imageFilePaths)
            {
                if (!File.Exists(imgFilePath))
                {
                    throw new Exception("imageFilePaths have not Exist file");
                }
            }
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
            AnimatedGifEncoder e1 = new AnimatedGifEncoder();
            e1.Start(outputFilePath);
            e1.Delay= delay;    // 延迟间隔
            e1.SetRepeat(0);  //-1:不循环,0:总是循环 播放  

            foreach (var imgFilePath in imageFilePaths)
            {
                e1.AddFrame(System.DrawingCore.Image.FromFile(imgFilePath));
            }
            e1.Finish();
            return File.Exists(outputFilePath);
        }


    }
}
