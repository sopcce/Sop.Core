using System;
using System.Collections.Generic;
using System.Text;

namespace Sop.Common.Helper.Img
{
    /// <summary>
    /// 动图合成接口（animate）用于将多张图片合成 GIF 动图。
    /// </summary>
    public interface IAnimate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageFilePaths"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="delay"></param>
        /// <param name="loop">循环是否</param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        bool SetAminmate(string[] imageFilePaths, string outputFilePath, int delay = 500, bool loop = true, int w = 0,
            int h = 0);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageGifPath"></param>
        /// <param name="outputFilePath"></param>
        /// <returns></returns>
        ReturnValues GetAminmate(string imageGifPath, string outputFilePath);
    }
}
