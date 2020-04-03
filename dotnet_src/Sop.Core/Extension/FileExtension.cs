namespace System
{
    public static class FileExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string ToFriendlyFileSize(decimal size)
        {
            var units = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };
            decimal mod = 1024.0M;
            int i = 0;
            while (size >= mod)
            {
                size /= mod;
                i++;
            }
            return Math.Round(size) + units[i];
        }

    }
}
