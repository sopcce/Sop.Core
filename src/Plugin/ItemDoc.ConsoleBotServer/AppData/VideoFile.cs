using System;
using System.IO;

namespace ItemDoc.ConsoleBot.AppData.FFmpeg
{
    /// <summary>
    /// 视频文件
    /// </summary>
    public class VideoFile
    {
        /// <summary>
        /// 视频文件地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// 比特率
        /// </summary>
        public double BitRate { get; set; }

        /// <summary>
        /// 音频格式
        /// </summary>
        public string RawAudioFormat { get; set; }

        /// <summary>
        /// 音频格式
        /// </summary>
        public string AudioFormat { get; set; }

        /// <summary>
        /// 视频格式
        /// </summary>
        public string RawVideoFormat { get; set; }

        /// <summary>
        /// 视频格式
        /// </summary>
        public string VideoFormat { get; set; }

        /// <summary>
        /// 视频高度
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 视频宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// FFmpeg输出的控制台信息
        /// </summary>
        public string RawInfo { get; set; }

        /// <summary>
        /// 是否已经获取了视频信息
        /// </summary>
        public bool infoGathered { get; set; }

        public VideoFile(string path)
        {
            this.Path = path;
            this.infoGathered = false;
            if (string.IsNullOrEmpty(this.Path))
            {
                throw new Exception("视频文件地址不能为空！");
            }
            if (!File.Exists(this.Path))
            {
                throw new Exception("视频文件(" + this.Path + ")不存在！ ");
            }
        }
    }
}