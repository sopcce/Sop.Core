using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Sop.Common.Img.Utility
{
  /// <summary>
  /// 通用磁盘(IO)数据访问操作 
  /// </summary>
  public class FileUtility
  {


    /// <summary>
    /// 获取文件编码
    /// </summary>
    /// <param name="filePath">文件绝对路径</param>
    /// <returns></returns>
    public static Encoding GetEncoding(string filePath)
    {
      return GetEncoding(filePath, Encoding.Default);
    }

    /// <summary>
    /// 获取文件编码
    /// </summary>
    /// <param name="filePath">文件绝对路径</param>
    /// <param name="defaultEncoding">找不到则返回这个默认编码</param>
    /// <returns></returns>
    public static Encoding GetEncoding(string filePath, Encoding defaultEncoding)
    {
      Encoding targetEncoding = defaultEncoding;
      using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4))
      {
        if (fs != null && fs.Length >= 2)
        {
          long pos = fs.Position;
          fs.Position = 0;
          int[] buffer = new int[4];
          //long x = fs.Seek(0, SeekOrigin.Begin);
          //fs.Read(buffer,0,4);
          buffer[0] = fs.ReadByte();
          buffer[1] = fs.ReadByte();
          buffer[2] = fs.ReadByte();
          buffer[3] = fs.ReadByte();

          fs.Position = pos;

          if (buffer[0] == 0xFE && buffer[1] == 0xFF)//UnicodeBe
          {
            targetEncoding = Encoding.BigEndianUnicode;
          }
          if (buffer[0] == 0xFF && buffer[1] == 0xFE)//Unicode
          {
            targetEncoding = Encoding.Unicode;
          }
          if (buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)//UTF8
          {
            targetEncoding = Encoding.UTF8;
          }
        }
      }

      return targetEncoding;
    }



    /// <summary>
    /// 获取指定目录及子目录中所有子目录列表
    /// </summary>
    /// <param name="directoryPath">指定目录的绝对路径</param>
    /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
    /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
    /// <param name="isSearchChild">是否搜索子目录</param>
    public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
    {
      try
      {
        if (isSearchChild)
        {
          return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
        }
        else
        {
          return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
        }
      }
      catch (IOException ex)
      {
        throw ex;
      }
    }


    /// <summary>
    /// 获取指定目录及子目录中所有文件列表
    /// </summary>
    /// <param name="directoryPath">指定目录的绝对路径</param>
    /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
    /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
    /// <param name="isSearchChild">是否搜索子目录</param>
    public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
    {
      //如果目录不存在，则抛出异常
      if (!Directory.Exists(directoryPath))
      {
        throw new FileNotFoundException();
      }

      try
      {
        if (isSearchChild)
        {
          return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
        }
        else
        {
          return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
        }
      }
      catch (IOException ex)
      {
        throw ex;
      }
    }

    /// <summary>
    /// 递归删除文件夹目录及文件
    /// </summary>
    /// <param name="dir"></param>  
    /// <returns></returns>
    public static void DeleteFolder(string dir)
    {
      if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
      {
        foreach (string d in Directory.GetFileSystemEntries(dir))
        {
          if (System.IO.File.Exists(d))
            System.IO.File.Delete(d); //直接删除其中的文件                        
          else
            DeleteFolder(d); //递归删除子文件夹 
        }
        Directory.Delete(dir, true); //删除已空文件夹                 
      }
    }


    /// <summary>
    /// 指定文件夹下面的所有内容copy到目标文件夹下面
    /// </summary>
    /// <param name="srcPath">原始路径</param>
    /// <param name="aimPath">目标文件夹</param>
    public static void CopyDir(string srcPath, string aimPath)
    {
      try
      {
        if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
          aimPath += Path.DirectorySeparatorChar;
        if (!Directory.Exists(aimPath))
          Directory.CreateDirectory(aimPath);
        string[] fileList = Directory.GetFileSystemEntries(srcPath);
        foreach (string file in fileList)
        {

          if (Directory.Exists(file))
            CopyDir(file, aimPath + Path.GetFileName(file));
          else
            System.IO.File.Copy(file, aimPath + Path.GetFileName(file), true);
        }
      }
      catch (Exception ee)
      {
        throw new Exception(ee.ToString());
      }
    }


    /// <summary>
    /// 获取文件夹大小
    /// </summary>
    /// <param name="dirPath">文件夹路径</param>
    /// <returns></returns>
    public static long GetDirectoryLength(string dirPath)
    {
      if (!Directory.Exists(dirPath))
        return 0;
      long len = 0;
      DirectoryInfo di = new DirectoryInfo(dirPath);
      foreach (FileInfo fi in di.GetFiles())
      {
        len += fi.Length;
      }
      DirectoryInfo[] dis = di.GetDirectories();
      if (dis.Length > 0)
      {
        for (int i = 0; i < dis.Length; i++)
        {
          len += GetDirectoryLength(dis[i].FullName);
        }
      }
      return len;
    }




    /// <summary>
    /// 从指定文本文件中读取数据，读取的文件类型要在AllowReadeOrWriteFile允许列表中
    /// </summary>
    /// <param name="filePath">文件物理磁盘路径</param>
    /// <param name="encodingCodepage">编码代码页标识符。自动识别UTF-8及GB2312：0(建议使用)， GB2312：936， UTF-8：65001</param>
    /// <returns>返回读取到的数据</returns>
    public static string ReadTextFile(string filePath, int encodingCodepage = 0)
    {

      if (!System.IO.File.Exists(filePath))
      {
        return string.Empty;
      }
      string result = string.Empty;
      Encoding encoding;
      if (encodingCodepage == 0)
      {
        encoding = Encoding.GetEncoding(936);
      }
      else
      {
        encoding = Encoding.GetEncoding(encodingCodepage);
      }
      try
      {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
          using (StreamReader streamReader = new StreamReader(fileStream, encoding))
          {
            result = streamReader.ReadToEnd();
          }
        }
      }
      catch
      {
        // ignored
      }
      return result;
    }
    /// <summary>
    /// 向指定文本文件中写入数据，写入的文件类型要在FileConfig.AllowReadeOrWriteFileTypes允许列表中。
    /// </summary>
    /// <param name="filePath">文件物理磁盘路径</param>
    /// <param name="content">写入内容</param>
    /// <param name="encoding">编码</param>
    /// <returns>true：写入成功 false：写入失败</returns>
    public static bool WriteTextFileByAppend(string filePath, string content, Encoding encoding)
    {

      bool result = false;
      try
      {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
        {
          using (StreamWriter streamWriter = new StreamWriter(fileStream, encoding))
          {
            streamWriter.Write(content);
            result = true;
          }
        }
      }
      catch
      {
        // ignored
      }
      return result;
    }

    /// <summary>
    /// 向指定文本文件中写入数据，写入的文件类型要在AllowReadeOrWriteFile允许列表中。
    /// </summary>
    /// <param name="filePath"> 文件物理磁盘路径</param>
    /// <param name="content"> 写入内容</param>
    /// <param name="encodingCodepage">编码代码页标识符。GB2312：936， UTF-8：65001</param>
    /// <returns>true：写入成功 false：写入失败</returns>
    public static bool WriteTextFile(string filePath, string content, int encodingCodepage)
    {

      bool result = false;
      try
      {
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
          var encoding = Encoding.GetEncoding(encodingCodepage);
          using (var streamWriter = new StreamWriter(fileStream, encoding))
          {
            streamWriter.Write(content);
            result = true;
          }
        }
      }
      catch
      {
        // ignored
      }
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
        System.IO.File.Delete(FilePath);
      }
      catch
      {
        result = false;
      }
      return result;
    }
    /// <summary>
    /// 将指定文件移到新位置，提供要指定新文件名的选项。
    /// </summary>
    /// <param name="FilePath"></param>
    /// <param name="NewFilePath"></param>
    /// <returns></returns>
    public static bool MoveOrRenameFile(string FilePath, string NewFilePath)
    {

      if (System.IO.File.Exists(NewFilePath))
      {
        return false;
      }
      bool result = false;
      try
      {
        if (System.IO.File.Exists(FilePath))
        {
          System.IO.File.Move(FilePath, NewFilePath);
          result = true;
        }
      }
      catch
      {
      }
      return result;
    }
    /// <summary>
    /// 复制文件到指定位置，自动覆盖同名文件。会检测源文件是否存在，但不会检测目标文件的目录是否存在
    /// </summary>
    /// <param name="srcFilePath">源文件完整的磁盘路径</param>
    /// <param name="destFilePath">目标文件完整的磁盘路径</param>
    public static void CopyFile(string srcFilePath, string destFilePath)
    {
      if (System.IO.File.Exists(srcFilePath))
      {
        System.IO.File.Copy(srcFilePath, destFilePath, true);
      }
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
    /// <param name="length"></param>
    /// <returns></returns>
    public static string GetRandomCharsCanUseForFilename(int length)
    {
      if (length <= 0)
      {
        return string.Empty;
      }
      StringBuilder stringBuilder = new StringBuilder();
      while (stringBuilder.Length < length)
      {
        stringBuilder.Append(Path.GetRandomFileName().Replace(".", ""));
      }
      return stringBuilder.ToString().Substring(0, length);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static T ReadSerializedData<T>(string filePath) where T : class
    {

      if (!System.IO.File.Exists(filePath))
      {
        return default(T);
      }
      T result = default(T);
      try
      {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
    /// <param name="filePath"></param>
    /// <param name="serializableObject"></param>
    /// <returns></returns>
    public static bool WriteSerializedData<T>(string filePath, T serializableObject) where T : class
    {

      bool result = true;
      try
      {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
          BinaryFormatter binaryFormatter = new BinaryFormatter();
          binaryFormatter.Serialize(fileStream, serializableObject);
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
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static bool DeleteSerializedFile(string filePath)
    {

      bool result = true;
      try
      {
        System.IO.File.Delete(filePath);
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
    /// <param name="dirPath"></param>
    /// <returns></returns>
    public static FileInfo[] GetAllFilesInfoOfDir(string dirPath)
    {
      FileInfo[] result;
      try
      {
        FileInfo[] array = null;
        DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
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
    /// <param name="dirPath"></param>
    /// <returns></returns>
    public static string[] GetAllFoldersOfDir(string dirPath)
    {
      string[] directories;
      try
      {
        directories = Directory.GetDirectories(dirPath);
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
    /// <param name="dirPath"></param>
    /// <returns></returns>
    public static DirectoryInfo[] GetAllFoldersInfoOfDirectory(string dirPath)
    {
      DirectoryInfo[] result;
      try
      {
        DirectoryInfo[] array = null;
        DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
        if (directoryInfo.Exists)
        {
          array = directoryInfo.GetDirectories();
        }
        result = array;
      }
      catch
      {
        result = null;
      }
      return result;
    }
    /// <summary>
    /// 新建一个目录(父路径不存在的话也将被创建)，能自动判断要创建的目录是否存在
    /// </summary>
    /// <param name="dirPath">目录磁盘路径</param>
    /// <returns>true：创建成功； false：创建失败</returns>
    public static bool CreateDirectory(string dirPath)
    {
      if (Directory.Exists(dirPath))
      {
        return false;
      }
      try
      {
        Directory.CreateDirectory(dirPath);
      }
      catch
      {
        return false;
      }
      return true;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dirPath"></param>
    /// <returns></returns>
    public static bool DeleteDir(string dirPath)
    {
      try
      {
        if (Directory.Exists(dirPath))
        {
          Directory.Delete(dirPath, true);
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
    /// <param name="dirPath"></param>
    /// <param name="andSubdirectorys"></param>
    /// <returns></returns>
    public static long GetDirSize(string dirPath, bool andSubdirectorys)
    {
      long num = 0L;
      try
      {
        DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
        FileInfo[] files = directoryInfo.GetFiles();
        FileInfo[] array = files;
        for (int i = 0; i < array.Length; i++)
        {
          FileInfo fileInfo = array[i];
          num += fileInfo.Length / 1024L + 1L;
        }
        if (andSubdirectorys)
        {
          DirectoryInfo[] directories = directoryInfo.GetDirectories();
          DirectoryInfo[] array2 = directories;
          for (int j = 0; j < array2.Length; j++)
          {
            DirectoryInfo directoryInfo2 = array2[j];
            num += GetDirSize(directoryInfo2.FullName, true);
          }
        }
      }
      catch
      {
      }
      return num;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dirPath"></param>
    /// <returns></returns>
    public static string DirectoryName(string dirPath)
    {
      var charArr = new char[] {
                '\\',
                ' '
            };
      return Path.GetFileName(dirPath.Trim(charArr));
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

    ///////////////////////////


    /// <summary>
    /// 获取文件扩展名 TODO:
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetFileExtension(string fileName)
    {
      string fileExtension = fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal) + 1);

      //return fileExtension.ToLowerInvariant();

      string text = Path.GetExtension(fileName);
      if (text == null)
      {
        text = string.Empty;
      }
      return text.ToLowerInvariant();
    }

    /// <summary>
    /// 获取文件名称
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetFileNameWithoutExtension(string fileName)
    {
      string fileNameWithoutExtension = fileName.Substring(0, fileName.LastIndexOf(".", StringComparison.Ordinal));
      return fileNameWithoutExtension;
    }



    /// <summary>
    /// 友好的文件大小信息
    /// </summary>
    /// <param name="fileSize">文件字节数</param>
    public static string GetFileSize(double fileSize)
    {
      if (fileSize > 0)
      {
        if (fileSize > 1024 * 1024 * 1024)
          return string.Format("{0:F2}GB", (fileSize / (1024 * 1024 * 1024F)));
        else if (fileSize > 1024 * 1024)
          return string.Format("{0:F2}MB", (fileSize / (1024 * 1024F)));
        else if (fileSize > 1024)
          return string.Format("{0:F2}KB", (fileSize / (1024F)));
        else
          return string.Format("{0:F2}B", fileSize);
      }
      else
        return string.Empty;
    }

    /// <summary>
    /// 根据格式化的文件大小（KB、MB、GB）获取文件的字节数
    /// </summary>
    /// <param name="formattedFileSize">式化的文件大小（KB、MB、GB）</param>
    /// <returns>文件的字节数</returns>
    public static long GetFileBytes(string formattedFileSize)
    {
      long fileBytes = 0;
      string _formattedFileSize = formattedFileSize.ToUpper();

      if (_formattedFileSize.EndsWith("KB"))
      {
        fileBytes = long.Parse(_formattedFileSize.Remove(_formattedFileSize.IndexOf("KB", StringComparison.Ordinal))) * 1024;
      }
      else if (_formattedFileSize.EndsWith("MB"))
      {
        fileBytes = long.Parse(_formattedFileSize.Remove(_formattedFileSize.IndexOf("MB", StringComparison.Ordinal))) * 1024 * 1024;
      }
      else if (_formattedFileSize.EndsWith("GB"))
      {
        fileBytes = long.Parse(_formattedFileSize.Remove(_formattedFileSize.IndexOf("GB", StringComparison.Ordinal))) * 1024 * 1024 * 1024;
      }
      else if (_formattedFileSize.EndsWith("TB"))
      {
        fileBytes = long.Parse(_formattedFileSize.Remove(_formattedFileSize.IndexOf("TB", StringComparison.Ordinal))) * 1024 * 1024 * 1024 * 1024;
      }
      else if (_formattedFileSize.EndsWith("B"))
      {
        fileBytes = long.Parse(_formattedFileSize.Remove(_formattedFileSize.IndexOf("B", StringComparison.Ordinal)));
      }

      return fileBytes;
    }

    /// <summary>
    /// 获取物理磁盘文件路径
    /// </summary>
    /// <param name="filePath">
    ///     <remarks>
    ///         <para>filePath支持以下格式：</para>
    ///         <list type="bullet">
    ///         <item>~/abc/</item>
    ///         <item>c:\abc\</item>
    ///         <item>\\192.168.0.1\abc\</item>
    ///         </list>
    ///     </remarks>
    /// </param>
    /// <returns>物理磁盘文件路径</returns>
    public static string GetDiskFilePath(string filePath)
    {
      var result = "";
      if (filePath.IndexOf(":\\", StringComparison.Ordinal) != -1 || filePath.IndexOf("\\\\", StringComparison.Ordinal) != -1)
        result = filePath;
      try
      {
        if (System.Web.Hosting.HostingEnvironment.IsHosted)
          result = System.Web.Hosting.HostingEnvironment.MapPath(filePath);
        else
        {
          filePath = filePath.Replace('/', System.IO.Path.DirectorySeparatorChar).Replace("~", "");
          result = Combine(System.AppDomain.CurrentDomain.BaseDirectory, filePath);
        }
      }
      catch
      {
        result = filePath;
      }
      return result;
    }


    /// <summary>
    /// System.IO.Path.Combine 扩展，主要是用于路径拼接
    /// 1、当path2 是相对路径的时候，返回的是path2，path1会被丢弃
    /// 2、路径是驱动器，返回的结果不正确
    /// 3、无法连接http路径 
    /// </summary>
    /// <example>
    /// <code> Combine(p1, "p2", "\\p3/", p4));</code>
    /// </example>
    /// <see>
    /// <![CDATA[
    /// http://www.jb51.net/article/36744.htm
    /// http://www.cnblogs.com/LoveJenny/archive/2012/03/05/2381094.html]]>
    /// </see>
    /// <param name="paths">拼接路径</param>
    /// <returns>返回拼接之后的路径</returns>
    private static string Combine(params string[] paths)
    {
      if (paths.Length == 0)
        throw new ArgumentException("please input path");
      var builder = new StringBuilder();
      var spliter = "\\";
      var firstPath = paths[0];
      if (firstPath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        spliter = "/";
      if (!firstPath.EndsWith(spliter))
        firstPath = firstPath + spliter;
      builder.Append(firstPath);
      for (var i = 1; i < paths.Length; i++)
      {
        var nextPath = paths[i];
        if (nextPath.StartsWith("/") || nextPath.StartsWith("\\"))
          nextPath = nextPath.Substring(1);
        if (i != paths.Length - 1)
        {
          if (nextPath.EndsWith("/") || nextPath.EndsWith("\\"))
          {
            nextPath = nextPath.Substring(0, nextPath.Length - 1) + spliter;
          }
          else
          {
            nextPath = nextPath + spliter;
          }
        }

        builder.Append(nextPath);
      }

      return builder.ToString();
    }
  }
}