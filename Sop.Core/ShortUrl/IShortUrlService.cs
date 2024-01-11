namespace Sop.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IShortUrlService
    {
        /// <summary>
        /// 获取Url别名
        /// </summary>
        /// <param name="url">需要处理的Url</param>
        /// <param name="shortUrlCount"></param>
        /// <returns>返回Url别名</returns>
        string[] GetShortUrl(string url, int shortUrlCount = 4);


    }
}