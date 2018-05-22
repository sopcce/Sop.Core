using log4net;
using log4net.Config;
using ItemDoc.Framework.Environment;
using System;
using System.Configuration;
using System.IO;
using ItemDoc.Framework.Utilities;


[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", ConfigFileExtension = ".config", Watch = true)]
namespace ItemDoc.Framework.SystemLog
{
  /// <summary>
  /// 系统日志处理【基于Log4Net写日志的原则】
  /// 1、在catch后,把异常写入日志.
  /// 2、在调用第三方控件、第三方接口的开始和结束处.
  /// 3、在连接数据库的开始结束处.
  /// 4、除非必要,不要在循环体中加入日志,否则一旦出问题可能导致日志暴增.
  /// 5、在自己认为很重要的逻辑处写入日志.
  /// </summary>
  public class Log
  {
    /// <summary>
    /// log4net
    /// </summary>
    private ILog _mlog;
    /// <summary>
    /// Gets or sets the configuration file path.
    /// </summary>
    public string ConfigFilePath { get; set; }
    private static volatile Log _instance = null;
    private static readonly object Lock = new object();

    /// <summary>
    /// Log
    /// </summary>
    /// <returns></returns>
    public static Log Instance()
    {
      if (_instance == null)
      {
        lock (Lock)
        {
          if (_instance == null)
          {
            _instance = new Log();
          }
        }
      }
      return _instance;
    }

    /// <summary>
    /// 初始化日志系统
    /// 在系统运行开始初始化,不存在创建目录文件
    /// </summary>
    private Log()
    {
      XmlConfigurator.Configure();
      try
      {

        string filePath = FileUtility.GetDiskFilePath("~/App_Data/");
        var files = Directory.GetFiles(filePath, "log4net.config");
        string file = files.Length > 0 ? files[0] : string.Empty;
        FileInfo configFileInfo = new FileInfo(file);
        if (!configFileInfo.Exists)
        {
          if (!_mlog.IsErrorEnabled)
          {
            object o = ConfigurationManager.GetSection("log4net");
            log4net.Config.XmlConfigurator.Configure(o as System.Xml.XmlElement);

          }
        }
        if (RunningEnvironment.IsFullTrust())
          XmlConfigurator.ConfigureAndWatch(configFileInfo);
        else
          XmlConfigurator.Configure(configFileInfo);
      }
      catch (Exception ex)
      {
        throw new ApplicationException(string.Format("log4net配置文件 {0} 未找到", ex.Message));
      }


    }

    /// <summary>
    /// 写入日志
    /// </summary>
    /// <param name="message">日志信息</param>
    /// <param name="messageType">日志类型</param>
    /// <param name="ex">异常</param>
    /// <param name="type"></param>
    public void Write(object message, Exception ex, Type type, LogLevel messageType = LogLevel.Info)
    {
      if (type == null)
      {
        type = typeof(Log);
      }
      _mlog = LogManager.GetLogger(type);

      switch (messageType)
      {
        case LogLevel.Debug:
          _mlog.Debug(message, ex);
          break;
        case LogLevel.Info:
          _mlog.Info(message, ex);
          break;
        case LogLevel.Warn:
          _mlog.Warn(message, ex);
          break;
        case LogLevel.Error:
          _mlog.Error(message, ex);
          break;
        case LogLevel.Fatal:
          _mlog.Fatal(message, ex);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
      }

    }


    /// <summary>
    /// 关闭log4net
    /// </summary>
    public void ShutDown()
    {
      LogManager.Shutdown();

    }
  }
}
