using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Sop.ConsoleBotServer.AppData.FFmpeg
{
    /// <summary>
    /// 视频编码器，使用FFmpeg获取视频信息，进行视频转码，获取缩略图等
    /// </summary>
    public class VideoEncoder
    {
        /// <summary>
        /// FFmpegPath程序的路径
        /// </summary>
        private string FFmpegPath { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fFmpegPath">FFmpegPath程序的路径</param>
        public VideoEncoder(string fFmpegPath)
        {
            FFmpegPath = fFmpegPath;
        }

        /// <summary>
        /// 获取视频信息
        /// </summary>
        /// <param name="input">视频文件</param>
        public void GetVideoInfo(VideoFile input)
        {
            string FFmpegParams = string.Format("-i {0}", input.Path);
            string output = RunProcess(FFmpegParams);

            input.RawInfo = output;
            input.Duration = ExtractDuration(input.RawInfo);
            input.BitRate = ExtractBitrate(input.RawInfo);
            input.RawAudioFormat = ExtractRawAudioFormat(input.RawInfo);
            input.AudioFormat = ExtractAudioFormat(input.RawAudioFormat);
            input.RawVideoFormat = ExtractRawVideoFormat(input.RawInfo);
            input.VideoFormat = ExtractVideoFormat(input.RawVideoFormat);
            input.Width = ExtractVideoWidth(input.RawInfo);
            input.Height = ExtractVideoHeight(input.RawInfo);
            input.infoGathered = true;
        }

        /// <summary>
        /// 获取视频的缩略图
        /// </summary>
        /// <param name="input">视频文件</param>
        /// <param name="saveThumbnailTo">缩略图文件地址</param>
        /// <returns>是否成功</returns>
        public bool GenerateVideoThumbnail(VideoFile input, string saveThumbnailTo)
        {
            if (!input.infoGathered)
            {
                GetVideoInfo(input);
            }

            //默认取第10秒的视频截图，如果视频不足10秒，取第1秒
            int secs = 1;
            if (input.Duration.Ticks >= 10)
            {
                secs = 10;
            }

            string FFmpegParams = string.Format("-i {0} -y -ss {1} -vframes 1 -r 1 -ac 1 -ab 2 -s 480*300 -f image2 {2}", input.Path, secs, saveThumbnailTo);
            string output = RunProcess(FFmpegParams);

            return File.Exists(saveThumbnailTo) && new FileInfo(saveThumbnailTo).Length > 0;
        }

        /// <summary>
        /// 将视频编码输出到新的视频文件
        /// </summary>
        /// <param name="input">视频文件</param>
        /// <param name="encodingParams">编码参数</param>
        /// <param name="outputFile">视频输出文件</param>
        /// <returns>编码后的视频文件信息</returns>
        public bool EncodeVideo(VideoFile input, string encodingParams, string outputFile)
        {
            string FFmpegParams = string.Format("-i {0} {1} {2}", input.Path, encodingParams, outputFile);
            string output = RunProcess(FFmpegParams);

            return File.Exists(outputFile) && new FileInfo(outputFile).Length > 0;
        }

        /// <summary>
        /// 运行FFmpeg
        /// </summary>
        /// <param name="FFmpegParams">运行参数</param>
        /// <returns>控制台输出结果</returns>
        private string RunProcess(string FFmpegParams)
        {
            ProcessStartInfo oInfo = new ProcessStartInfo(FFmpegPath, FFmpegParams);
            oInfo.UseShellExecute = false;          //不使用系统外壳程序启动
            oInfo.CreateNoWindow = true;            //不创建窗口
            oInfo.RedirectStandardOutput = true;    //重定向输出
            oInfo.RedirectStandardError = true;     //重定向错误

            Process proc = null;
            string output = null;

            try
            {
                proc = Process.Start(oInfo);
                output = proc.StandardError.ReadToEnd();
                proc.WaitForExit();
            }
            catch (Exception)
            {
                output = string.Empty;
            }
            finally
            {
                if (proc != null)
                {
                    proc.Close();
                }
            }
            return output;
        }

        #region 从输出内容中解析视频参数

        /// <summary>
        /// 解析视频的时长
        /// </summary>
        /// <param name="rawInfo">FFmpeg控制台信息</param>
        /// <returns>视频的时长</returns>
        private TimeSpan ExtractDuration(string rawInfo)
        {
            TimeSpan t = new TimeSpan(0);
            Regex re = new Regex("[D|d]uration:.((\\d|:|\\.)*)", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);

            if (m.Success)
            {
                string duration = m.Groups[1].Value;
                string[] timepieces = duration.Split(new char[] { ':', '.' });
                if (timepieces.Length == 4)
                {
                    t = new TimeSpan(0, Convert.ToInt16(timepieces[0]), Convert.ToInt16(timepieces[1]), Convert.ToInt16(timepieces[2]), Convert.ToInt16(timepieces[3]));
                }
            }

            return t;
        }

        /// <summary>
        /// 解析视频的比特率
        /// </summary>
        /// <param name="rawInfo">FFmpeg控制台信息</param>
        /// <returns>视频的比特率</returns>
        private double ExtractBitrate(string rawInfo)
        {
            Regex re = new Regex("[B|b]itrate:.((\\d|:)*)", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            double kb = 0.0;
            if (m.Success)
            {
                Double.TryParse(m.Groups[1].Value, out kb);
            }
            return kb;
        }

        /// <summary>
        /// 解析音频的格式
        /// </summary>
        /// <param name="rawInfo">FFmpeg控制台信息</param>
        /// <returns>音频的格式</returns>
        private string ExtractRawAudioFormat(string rawInfo)
        {
            string a = string.Empty;
            Regex re = new Regex("[A|a]udio:.*", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                a = m.Value;
            }
            return a.Replace("Audio: ", "");
        }

        /// <summary>
        /// 解析音频的格式
        /// </summary>
        /// <param name="rawAudioFormat">音频的格式</param>
        /// <returns>音频的格式</returns>
        private string ExtractAudioFormat(string rawAudioFormat)
        {
            string[] parts = rawAudioFormat.Split(new string[] { ", " }, StringSplitOptions.None);
            return parts[0].Replace("Audio: ", "");
        }

        /// <summary>
        /// 解析视频的格式
        /// </summary>
        /// <param name="rawInfo">FFmpeg控制台信息</param>
        /// <returns><视频的格式/returns>
        private string ExtractRawVideoFormat(string rawInfo)
        {
            string v = string.Empty;
            Regex re = new Regex("[V|v]ideo:.*", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                v = m.Value;
            }
            return v.Replace("Video: ", ""); ;
        }

        /// <summary>
        /// 解析视频的格式
        /// </summary>
        /// <param name="rawVideoFormat">视频的格式</param>
        /// <returns>视频的格式</returns>
        private string ExtractVideoFormat(string rawVideoFormat)
        {
            string[] parts = rawVideoFormat.Split(new string[] { ", " }, StringSplitOptions.None);
            return parts[0].Replace("Video: ", "");
        }

        /// <summary>
        /// 解析视频的宽度
        /// </summary>
        /// <param name="rawInfo">FFmpeg控制台信息</param>
        /// <returns>视频的宽度</returns>
        private int ExtractVideoWidth(string rawInfo)
        {
            int width = 0;
            Regex re = new Regex("(\\d{2,4})x(\\d{2,4})", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                int.TryParse(m.Groups[1].Value, out width);
            }
            return width;
        }

        /// <summary>
        /// 解析视频的高度
        /// </summary>
        /// <param name="rawInfo">FFmpeg控制台信息</param>
        /// <returns>视频的高度</returns>
        private int ExtractVideoHeight(string rawInfo)
        {
            int height = 0;
            Regex re = new Regex("(\\d{2,4})x(\\d{2,4})", RegexOptions.Compiled);
            Match m = re.Match(rawInfo);
            if (m.Success)
            {
                int.TryParse(m.Groups[2].Value, out height);
            }
            return height;
        }

        #endregion 从输出内容中解析视频参数
    }
}