namespace System
{
    public static class FileExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string ToFriendlyFileSize(this long size)
        {
            var units = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };
            long mod = 1024;
            int i = 0;
            while (size >= mod)
            {
                size /= mod;
                i++;
            }
            return size + units[i];
        }

    }
}
