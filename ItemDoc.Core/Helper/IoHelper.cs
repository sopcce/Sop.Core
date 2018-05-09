using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Configuration;


namespace Sop.Framework.Helper
{
    /// <summary>
    ///  通用磁盘(IO)数据访问操作工具类
    /// </summary>
    public static class IoHelper
    {

        /// <summary>
        /// 广告允许的文件类型(小写)
        /// </summary>
        public static readonly string AllowReadeOrWriteFileTypes = ".html|.htm|.txt|.css|.js|.log|.xml";
        /// <summary>
        /// 允许多媒体上传的文件类型
        /// </summary>
        public const string AllowAdFileTypes = ".jpg|.jpeg|.gif|.png|.swf";
        /// <summary>
        /// 允许的图片文件类型(小写)
        /// </summary>
        public const string AllowImageAndMediasFileTypes = ".jpg|.jpeg|.gif|.png|.swf|.mid|.wav|.mp3";
        /// <summary>
        /// 允许读取及写入的文件类型(小写)。
        /// </summary>
        public const string AllowImageFileTypes = ".jpg|.jpeg|.gif|.png";
        /// <summary>
        ///  序列化数据存储文件的扩展名(小写)
        /// </summary>
        public const string SerializedFileExtension = ".sop";


        /// <summary>
        /// 是否允许的文件类型
        /// </summary>
        /// <param name="FileName">文件名字</param>
        /// <param name="FileTypes">文件类型</param>
        /// <returns>返回true  false</returns>
        public static bool IsAllowedFileType(string FileName, string FileTypes)
        {

            if (string.IsNullOrEmpty(FileName))
            {
                return false;
            }
            string text = Path.GetExtension(FileName).ToLower();
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }
            FileTypes = string.Format("|{0}|", FileTypes).ToLower();
            return FileTypes.Contains(string.Format("|{0}|", text));
        }
        /// <summary>
        /// 从指定文本文件中读取数据，读取的文件类型要在AllowReadeOrWriteFile允许列表中
        /// </summary>
        /// <param name="FilePath">文件物理磁盘路径</param>
        /// <param name="EncodingCodepage">编码代码页标识符。自动识别UTF-8及GB2312：0(建议使用)， GB2312：936， UTF-8：65001</param>
        /// <returns>返回读取到的数据</returns>
        public static string ReadTextFile(string FilePath, int EncodingCodepage)
        {
            if (!IoHelper.IsAllowedFileType(FilePath, ".html|.htm|.txt|.css|.js|.log|.xml"))
            {
                return string.Empty;
            }
            if (!File.Exists(FilePath))
            {
                return string.Empty;
            }
            string result = string.Empty;
            Encoding encoding;
            if (EncodingCodepage == 0)
            {
                encoding = Encoding.GetEncoding(936);
            }
            else
            {
                encoding = Encoding.GetEncoding(EncodingCodepage);
            }
            try
            {
                using (FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream, encoding))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
            }
            catch
            {
                throw;
            }
            return result;
        }
        /// <summary>
        /// 向指定文本文件中写入数据，写入的文件类型要在AllowReadeOrWriteFile允许列表中。
        /// </summary>
        /// <param name="FilePath"> 文件物理磁盘路径</param>
        /// <param name="Content"> 写入内容</param>
        /// <param name="EncodingCodepage">编码代码页标识符。GB2312：936， UTF-8：65001</param>
        /// <returns>true：写入成功 false：写入失败</returns>
        public static bool WriteTextFile(string FilePath, string Content, int EncodingCodepage)
        {
            if (!IoHelper.IsAllowedFileType(FilePath, ".html|.htm|.txt|.css|.js|.log|.xml"))
            {
                return false;
            }
            bool result = false;
            try
            {
                using (FileStream fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
                {
                    Encoding encoding = Encoding.GetEncoding(EncodingCodepage);
                    using (StreamWriter streamWriter = new StreamWriter(fileStream, encoding))
                    {
                        streamWriter.Write(Content);
                        result = true;
                    }
                }
            }
            catch
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 向指定文本文件中写入数据，写入的文件类型要在AllowReadeOrWriteFile允许列表中。
        /// </summary>
        /// <param name="FilePath">文件物理磁盘路径</param>
        /// <param name="Content">写入内容</param>
        /// <param name="encoding">编码</param>
        /// <returns>true：写入成功 false：写入失败</returns>
        public static bool WriteTextFileByAppend(string FilePath, string Content, Encoding encoding)
        {
            if (!IoHelper.IsAllowedFileType(FilePath, AllowReadeOrWriteFileTypes))
                return false;
            bool result = false;
            try
            {
                using (FileStream fileStream = new FileStream(FilePath, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream, encoding))
                    {
                        streamWriter.Write(Content);
                        result = true;
                    }
                }
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <returns></returns>
        public static bool DeleteFile(string FilePath)
        {
            bool result = true;
            try
            {
                File.Delete(FilePath);
            }
            catch
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="NewFilePath"></param>
        /// <returns></returns>
        public static bool MoveOrRenameFile(string FilePath, string NewFilePath)
        {

            if (File.Exists(NewFilePath))
            {
                return false;
            }
            bool result = false;
            try
            {
                if (File.Exists(FilePath))
                {
                    File.Move(FilePath, NewFilePath);
                    result = true;
                }
            }
            catch
            {
                throw;
            }
            return result;
        }
        /// <summary>
        /// 复制文件到指定位置，自动覆盖同名文件。会检测源文件是否存在，但不会检测目标文件的目录是否存在
        /// </summary>
        /// <param name="SrcFilePath">源文件完整的磁盘路径</param>
        /// <param name="DestFilePath">目标文件完整的磁盘路径</param>
        public static void CopyFile(string SrcFilePath, string DestFilePath)
        {
            if (File.Exists(SrcFilePath))
            {
                File.Copy(SrcFilePath, DestFilePath, true);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OriginalFilename"></param>
        /// <returns></returns>
        public static string GenerateNewRandomFilename(string OriginalFilename)
        {
            Random random = new Random();
            string str = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmssffff"), random.Next(0, 10));
            return str + Path.GetExtension(OriginalFilename);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static string GetFileExtension(string FilePath)
        {
            string text = Path.GetExtension(FilePath);
            if (text == null)
            {
                text = string.Empty;
            }
            return text.ToLower();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FilePhysicalPath"></param>
        /// <returns></returns>
        public static string[] GetFileInfo(string FilePhysicalPath)
        {
            string[] array = new string[]
            {
                "0",
                DateTime.MinValue.ToString(),
                ""
            };
            if (string.IsNullOrEmpty(FilePhysicalPath))
            {
                return array;
            }
            FileInfo fileInfo = new FileInfo(FilePhysicalPath);
            if (!fileInfo.Exists)
            {
                return array;
            }
            array[0] = fileInfo.Length.ToString();
            DateTime lastWriteTime = fileInfo.LastWriteTime;
            array[1] = lastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
            array[2] = string.Format("\"{0}\"", lastWriteTime.Ticks);
            return array;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool FileExists(string FilePath)
        {
            return File.Exists(FilePath);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InputStream"></param>
        /// <returns></returns>
        public static byte[] ReadByteArrayFromStream(Stream InputStream)
        {
            byte[] result;
            try
            {
                byte[] array = new byte[InputStream.Length];
                int i = 0;
                int num = array.Length;
                while (i < num)
                {
                    i += InputStream.Read(array, i, num - i);
                }
                result = array;
            }
            catch
            {
                throw;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string GetRandomCharsCanUseForFilename(int Length)
        {
            if (Length <= 0)
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            while (stringBuilder.Length < Length)
            {
                stringBuilder.Append(Path.GetRandomFileName().Replace(".", ""));
            }
            return stringBuilder.ToString().Substring(0, Length);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static T ReadSerializedData<T>(string FilePath) where T : class
        {
            if (!Path.HasExtension(FilePath))
            {
                FilePath = string.Format("{0}{1}", FilePath, SerializedFileExtension);
            }
            if (!File.Exists(FilePath))
            {
                return default(T);
            }
            T result = default(T);
            try
            {
                using (FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    result = (T)((object)binaryFormatter.Deserialize(fileStream));
                }
            }
            catch
            {
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="FilePath"></param>
        /// <param name="SerializableObject"></param>
        /// <returns></returns>
        public static bool WriteSerializedData<T>(string FilePath, T SerializableObject) where T : class
        {
            if (!Path.HasExtension(FilePath))
            {
                FilePath = string.Format("{0}{1}", FilePath, SerializedFileExtension);
            }
            bool result = true;
            try
            {
                using (FileStream fileStream = new FileStream(FilePath, FileMode.Create))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, SerializableObject);
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool DeleteSerializedFile(string FilePath)
        {
            if (!Path.HasExtension(FilePath))
            {
                FilePath = string.Format("{0}{1}", FilePath, SerializedFileExtension);
            }
            else
            {
                if (!Path.GetExtension(FilePath).ToLower().Equals(SerializedFileExtension))
                {
                    return false;
                }
            }
            bool result = true;
            try
            {
                File.Delete(FilePath);
            }
            catch
            {
                result = false;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="DirPath"></param>
        /// <returns></returns>
        public static FileInfo[] GetAllFilesInfoOfDir(string DirPath)
        {
            FileInfo[] result;
            try
            {
                FileInfo[] array = null;
                DirectoryInfo directoryInfo = new DirectoryInfo(DirPath);
                if (directoryInfo.Exists)
                {
                    array = directoryInfo.GetFiles();
                }
                result = array;
            }
            catch
            {
                throw;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DirPath"></param>
        /// <returns></returns>
        public static string[] GetAllFoldersOfDir(string DirPath)
        {
            string[] directories;
            try
            {
                directories = Directory.GetDirectories(DirPath);
            }
            catch
            {
                throw;
            }
            return directories;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DirPath"></param>
        /// <returns></returns>
        public static DirectoryInfo[] GetAllFoldersInfoOfDir(string DirPath)
        {
            DirectoryInfo[] result;
            try
            {
                DirectoryInfo[] array = null;
                DirectoryInfo directoryInfo = new DirectoryInfo(DirPath);
                if (directoryInfo.Exists)
                {
                    array = directoryInfo.GetDirectories();
                }
                result = array;
            }
            catch
            {
                throw;
            }
            return result;
        }
        /// <summary>
        /// 新建一个目录(父路径不存在的话也将被创建)，能自动判断要创建的目录是否存在
        /// </summary>
        /// <param name="DirPath">目录磁盘路径</param>
        /// <returns>true：创建成功； false：创建失败</returns>
        public static bool CreateDir(string DirPath)
        {
            if (Directory.Exists(DirPath))
            {
                return false;
            }
            try
            {
                Directory.CreateDirectory(DirPath);
            }
            catch
            {
                 
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DirPath"></param>
        /// <returns></returns>
        public static bool DeleteDir(string DirPath)
        {
            try
            {
                if (Directory.Exists(DirPath))
                {
                    Directory.Delete(DirPath, true);
                }
            }
            catch
            {
                throw;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DirPath"></param>
        /// <param name="AndSubdirectorys"></param>
        /// <returns></returns>
        public static long GetDirSize(string DirPath, bool AndSubdirectorys)
        {
            long num = 0L;
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(DirPath);
                FileInfo[] files = directoryInfo.GetFiles();
                FileInfo[] array = files;
                for (int i = 0; i < array.Length; i++)
                {
                    FileInfo fileInfo = array[i];
                    num += fileInfo.Length / 1024L + 1L;
                }
                if (AndSubdirectorys)
                {
                    DirectoryInfo[] directories = directoryInfo.GetDirectories();
                    DirectoryInfo[] array2 = directories;
                    for (int j = 0; j < array2.Length; j++)
                    {
                        DirectoryInfo directoryInfo2 = array2[j];
                        num += IoHelper.GetDirSize(directoryInfo2.FullName, true);
                    }
                }
            }
            catch
            {
                throw;
            }
            return num;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DirPath"></param>
        /// <returns></returns>
        public static string DirectoryName(string DirPath)
        {
            return Path.GetFileName(DirPath.Trim(new char[]
            {
                '\\',
                ' '
            }));
        }
        /// <summary>
        /// 获取父级目录
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="lockInBasePath"></param>
        /// <returns></returns>
        public static string GetParentDirectoryPath(string dirPath, string lockInBasePath)
        {
            if (string.IsNullOrEmpty(lockInBasePath))
            {
                return dirPath;
            }
            string text = dirPath.TrimEnd(new char[]
            {
                '\\',
                ' '
            });
            text = text.Substring(0, text.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            if (!text.StartsWith(lockInBasePath, StringComparison.CurrentCultureIgnoreCase))
            {
                text = dirPath;
            }
            return text;
        }
    }
}
