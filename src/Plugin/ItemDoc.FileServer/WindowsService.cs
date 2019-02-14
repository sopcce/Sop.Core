using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;
using Aliyun.OSS;
using Common.Logging;
using GhostscriptSharp;

namespace ItemDoc.FileServer
{
    public class WindowsService : ServiceBase
    {
        private readonly ILog logger = LogManager.GetLogger<WindowsService>();

        private OssClient ossClient;
        private string bucket;

        private AttachmentService attachmentService;
        private RedisQueue queue;
        private RedisCounter counter;
        private SessionManager sessionManager;

        private string convertAttachmentQueue = "Queue:ConvertAttachment";

        private string asposeWordFileExtensions = "doc,docx,docm,rtf,odt,htm,html,mht,xml,txt";
        private string asposeExcelFileExtensions = "xls,xlsx,ods";
        private string asposePowerPointFileExtensions = "ppt,pptx";

        private string homePath;
        private string tempPath;

        public WindowsService()
        {
            this.ServiceName = "HuobanYunFileServer";
        }

        protected override void OnStart(string[] args)
        {
            var assemblyList = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(n => Assembly.Load(n)).ToList();
            assemblyList.Add(Assembly.GetExecutingAssembly());
            var assemblies = assemblyList.ToArray();

            this.homePath = Assembly.GetExecutingAssembly().Location;
            this.homePath = homePath.Substring(0, homePath.LastIndexOf('\\'));
            this.tempPath = Path.Combine(homePath, "temp");

            if (!Directory.Exists(this.tempPath))
            {
                Directory.CreateDirectory(this.tempPath);
            }

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).SingleInstance().PropertiesAutowired();
            containerBuilder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Service")).AsSelf().AsImplementedInterfaces().SingleInstance().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            //注册Redis服务
            string redisServer = ConfigurationManager.AppSettings["RedisServer"];
            containerBuilder.Register(c => ConnectionMultiplexer.Connect(redisServer)).SingleInstance();
            containerBuilder.Register(c => new RedisQueue()).SingleInstance().PropertiesAutowired();
            containerBuilder.Register(c => new RedisCounter()).SingleInstance().PropertiesAutowired();

            //注册NHibernate的SessionManager
            containerBuilder.Register(c => new SessionManager(assemblies)).AsSelf().SingleInstance().PropertiesAutowired();

            IContainer container = containerBuilder.Build();
            DIContainer.RegisterContainer(container);

            string endpoint = ConfigurationManager.AppSettings["AliyunOSS.Endpoint.API"];
            string accessKeyID = ConfigurationManager.AppSettings["AliyunOSS.AccessKeyID"];
            string accessKeySecret = ConfigurationManager.AppSettings["AliyunOSS.AccessKeySecret"];

            this.ossClient = new OssClient(endpoint, accessKeyID, accessKeySecret);
            this.bucket = ConfigurationManager.AppSettings["AliyunOSS.Bucket.Attachment"];
            this.attachmentService = DIContainer.Resolve<AttachmentService>();
            this.queue = DIContainer.Resolve<RedisQueue>();
            this.counter = DIContainer.Resolve<RedisCounter>();
            this.sessionManager = DIContainer.Resolve<SessionManager>();


            //启动一个新线程处理将文档转换为PDF的任务
            Thread convertAttachment = new Thread(new ThreadStart(Attachment2PDF));
            convertAttachment.IsBackground = true;
            convertAttachment.Start();

            logger.Info("文件服务器已启动");
        }

        protected override void OnStop()
        {
            logger.Info("文件服务器已停止");
        }

        /// <summary>
        /// 将文档转换为PDF
        /// </summary>
        private void Attachment2PDF()
        {
            //线程一直运行，监控附件队列
            while (true)
            {
                var attachmentId = this.queue.Pop(this.convertAttachmentQueue);
                if (attachmentId.IsNullOrEmpty)
                {
                    Thread.Sleep(100);
                    continue;
                }

                try
                {
                    bool success = false;

                    var attachment = this.attachmentService.Get(long.Parse(attachmentId));

                    //对于PDF文件，下载至本地后直接加入转换PDF的队列
                    if (attachment.MediaType == MediaType.PDF)
                    {
                        string fileName = attachment.Path.Substring(attachment.Path.LastIndexOf('/') + 1);
                        string fileLocalPathWithFileName = Path.Combine(this.tempPath, fileName);

                        var ossObject = this.ossClient.GetObject(new GetObjectRequest(this.bucket, attachment.Path));

                        using (Stream sourceStream = ossObject.Content)
                        {
                            using (FileStream fileStream = new FileStream(fileLocalPathWithFileName, FileMode.CreateNew))
                            {
                                byte[] buffer = new byte[1024 * 1024];
                                int bytesRead = 0;

                                while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    fileStream.Write(buffer, 0, bytesRead);
                                }

                                fileStream.Flush();
                            }
                        }

                        success = true;
                    }
                    else
                    {

                        //检查是否是支持转换的格式
                        if (!asposeWordFileExtensions.Contains(attachment.Extension) && !asposeExcelFileExtensions.Contains(attachment.Extension) && !asposePowerPointFileExtensions.Contains(attachment.Extension))
                        {
                            attachment.HtmlPreview = false;
                            attachmentService.Update(attachment);
                            continue;
                        }


                        //对不同的文件格式进行转换
                        if (asposeWordFileExtensions.Contains(attachment.Extension))
                        {
                            success = this.WordToPDF(attachment);
                        }
                        else if (asposeExcelFileExtensions.Contains(attachment.Extension))
                        {
                            success = this.ExcelToPDF(attachment);
                        }
                        else if (asposePowerPointFileExtensions.Contains(attachment.Extension))
                        {
                            success = this.PowerPointToPDF(attachment);
                        }
                    }

                    if (success)
                    {
                        this.PDF2HTML(attachment);
                    }
                    else
                    {
                        attachment.HtmlPreview = false;
                        attachmentService.Update(attachment);
                        logger.Error("PDF转换失败：" + attachment.Path);
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
                finally
                {
                    this.sessionManager.CloseSession();
                }
            }
        }

        /// <summary>
        /// PDF转换为HTML
        /// </summary>
        private void PDF2HTML(Attachment attachment)
        {
            string fileName = attachment.Path.Substring(attachment.Path.LastIndexOf('/') + 1);
            string fileNameWithoutExtension = FileUtility.GetFileNameWithoutExtension(fileName);
            string fileLocalPathWithFileName = Path.Combine(this.tempPath, fileName.Replace("." + attachment.Extension, ".pdf"));

            long companyId = long.Parse(attachment.Path.Substring(0, attachment.Path.IndexOf('/')));
            long userId = long.Parse(attachment.Path.Substring(attachment.Path.IndexOf('/') + 1).Substring(0, attachment.Path.IndexOf('/')));

            //检查是否有效文件
            if (!File.Exists(fileLocalPathWithFileName) || new FileInfo(fileLocalPathWithFileName).Length == 0)
            {
                attachment.HasThumbnail = false;
                attachment.HtmlPreview = false;
                attachmentService.Update(attachment);

                //删除无效文件(空文件)
                if (File.Exists(fileLocalPathWithFileName))
                {
                    File.Delete(fileLocalPathWithFileName);
                }

                return;
            }


            #region 提取文档缩略图

            string imageFile = Path.Combine(this.tempPath, fileNameWithoutExtension + ".thumbnail.jpg");

            try
            {
                GhostscriptWrapper.GeneratePageThumb(fileLocalPathWithFileName, imageFile, 1, 15, 15);
            }
            catch (Exception e)
            {
                logger.Error("获取PDF缩略图失败：" + attachment.Path, e);
            }

            var imageFileInfo = new FileInfo(imageFile);

            if (File.Exists(imageFile) && imageFileInfo.Length > 0)
            {
                //将缩略图上传至OSS
                string imageFileKey = attachment.Path.Replace("." + attachment.Extension, ".thumbnail.jpg");
                this.ossClient.PutObject(this.bucket, imageFileKey, imageFile);

                //磁盘空间计数
                counter.ChangeCount(CountTypes.Instance().DiskSpace(), TenantTypes.Instance().User(), userId, imageFileInfo.Length);
                counter.ChangeCount(CountTypes.Instance().DiskSpace(), TenantTypes.Instance().Company(), companyId, imageFileInfo.Length);

                attachment.HasThumbnail = true;
                logger.Info("获取PDF缩略图成功：" + attachment.Path);
            }
            else
            {
                attachment.HasThumbnail = false;
                logger.Error("获取PDF缩略图失败：" + attachment.Path);
            }

            //删除本地文件
            if (File.Exists(imageFile))
            {
                File.Delete(imageFile);
            }

            #endregion

            #region PDF转换HTML预览

            string htmlOutputPath = Path.Combine(this.tempPath, fileNameWithoutExtension);


            string binPath = Path.Combine(this.homePath, "pdf2htmlEX", "pdf2htmlEX.exe");

            //详见pdf2htmlEX的命令行参数说明：https://github.com/coolwanglu/pdf2htmlEX/wiki/Command-line
            string binParams = string.Format("--embed CFIJO --first-page 1 --last-page 100 --fit-width 1800 --hdpi 96 --vdpi 96 --embed-external-font 0 --font-size-multiplier 1 --split-pages 1 --dest-dir {0} --page-filename page-%d.html {1} preview.html", htmlOutputPath, fileLocalPathWithFileName.Replace('\\', '/'));

            ProcessStartInfo oInfo = new ProcessStartInfo(binPath, binParams);
            oInfo.UseShellExecute = false;          //不使用系统外壳程序启动
            oInfo.CreateNoWindow = true;            //不创建窗口
            oInfo.RedirectStandardOutput = true;    //重定向输出
            oInfo.RedirectStandardError = true;     //重定向错误

            Process proc = new Process();
            proc.StartInfo = oInfo;

            //个别的pdf文档在转换时会假死，即在某一页停止转换，控制台无输出，cpu高居不下，因此不得不用计时器来将其进程强制杀死
            System.Timers.Timer timer = new System.Timers.Timer(60000);
            timer.Elapsed += new ElapsedEventHandler((object source, ElapsedEventArgs e) =>
            {
                proc.Kill();
            });
            timer.AutoReset = false;
            timer.Enabled = true;

            //如果有新的输出内容，则重置计时器，计时器的到期时间随着内容最后输出时间而推迟
            Action<object, DataReceivedEventArgs> outputHandler = (object sendingProcess, DataReceivedEventArgs outLine) =>
            {
                if (!String.IsNullOrEmpty(outLine.Data))
                {
                    timer.Interval = 60000;
                }
            };

            proc.OutputDataReceived += new DataReceivedEventHandler(outputHandler);
            proc.ErrorDataReceived += new DataReceivedEventHandler(outputHandler);

            try
            {
                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();
            }
            catch (Exception e)
            {
                logger.Error("PDF转换为HTML失败：" + binParams, e);
            }
            finally
            {
                if (timer != null)
                {
                    timer.Close();
                    timer.Dispose();
                }

                if (proc != null)
                {
                    proc.Close();
                    proc.Dispose();
                }
            }


            DirectoryInfo htmlOutputFolder = new DirectoryInfo(htmlOutputPath);

            string previewFile = Path.Combine(htmlOutputPath, "preview.html");

            if (File.Exists(previewFile) && new FileInfo(previewFile).Length > 0)
            {
                //将生成的文件上传至OSS
                foreach (FileInfo htmlFileInfo in htmlOutputFolder.GetFiles())
                {
                    string htmlFileKey = attachment.Path.Replace(fileName, fileNameWithoutExtension + "/" + htmlFileInfo.Name);
                    this.ossClient.PutObject(this.bucket, htmlFileKey, htmlFileInfo.FullName);

                    //磁盘空间计数
                    counter.ChangeCount(CountTypes.Instance().DiskSpace(), TenantTypes.Instance().User(), userId, htmlFileInfo.Length);
                    counter.ChangeCount(CountTypes.Instance().DiskSpace(), TenantTypes.Instance().Company(), companyId, htmlFileInfo.Length);
                }

                attachment.HtmlPreview = true;
                logger.Info("PDF转换为HTML成功：" + attachment.Path);
            }
            else
            {
                attachment.HtmlPreview = false;
                logger.Error("PDF转换为HTML失败：" + binParams);
            }

            //删除转换后的HTML文件
            htmlOutputFolder.Delete(true);

            #endregion

            //提取文档的页码
            attachment.PageCount = this.GetPdfPageCount(fileLocalPathWithFileName);

            //删除本地PDF文件
            File.Delete(fileLocalPathWithFileName);

            attachmentService.Update(attachment);
        }

        /// <summary>
        /// 把Excel文件转换成PDF格式文件
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true=转换成功</returns>
        private bool ExcelToPDF(Attachment attachment)
        {
            string fileName = attachment.Path.Substring(attachment.Path.LastIndexOf('/') + 1);
            string fileLocalPathWithFileName = Path.Combine(this.tempPath, fileName.Replace("." + attachment.Extension, ".pdf"));

            try
            {
                using (var sourceStream = new MemoryStream())
                {
                    var ossObject = this.ossClient.GetObject(new GetObjectRequest(this.bucket, attachment.Path));

                    using (Stream stream = ossObject.Content)
                    {
                        stream.CopyTo(sourceStream);
                    }

                    Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(sourceStream);
                    Aspose.Cells.PdfSaveOptions saveOptions = new Aspose.Cells.PdfSaveOptions();
                    saveOptions.OnePagePerSheet = true;

                    workbook.Save(fileLocalPathWithFileName, saveOptions);
                }

                return true;
            }
            catch (Exception e)
            {
                logger.Error("PDF转换失败：" + attachment.Path, e);
                return false;
            }
            finally
            {
                //Aspose有内存泄漏的问题，转换后GC执行垃圾回收
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// 把PowerPoint文件转换成PDF格式文件
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true=转换成功</returns>
        private bool PowerPointToPDF(Attachment attachment)
        {
            string fileName = attachment.Path.Substring(attachment.Path.LastIndexOf('/') + 1);
            string fileLocalPathWithFileName = Path.Combine(this.tempPath, fileName.Replace("." + attachment.Extension, ".pdf"));

            try
            {
                using (var sourceStream = new MemoryStream())
                {
                    var ossObject = this.ossClient.GetObject(new GetObjectRequest(this.bucket, attachment.Path));

                    using (Stream stream = ossObject.Content)
                    {
                        stream.CopyTo(sourceStream);
                        sourceStream.Position = 0;
                    }

                    Aspose.Slides.Presentation pres = new Aspose.Slides.Presentation(sourceStream);

                    pres.Save(fileLocalPathWithFileName, Aspose.Slides.Export.SaveFormat.Pdf);
                }

                return true;
            }
            catch (Exception e)
            {
                logger.Error("PDF转换失败：" + attachment.Path, e);
                return false;
            }
            finally
            {
                //Aspose有内存泄漏的问题，转换后GC执行垃圾回收
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// 把Word文件转换成为PDF格式文件
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true=转换成功</returns>
        private bool WordToPDF(Attachment attachment)
        {
            string fileName = attachment.Path.Substring(attachment.Path.LastIndexOf('/') + 1);
            string fileLocalPathWithFileName = Path.Combine(this.tempPath, fileName.Replace("." + attachment.Extension, ".pdf"));

            try
            {

                using (var sourceStream = new MemoryStream())
                {
                    var ossObject = this.ossClient.GetObject(new GetObjectRequest(this.bucket, attachment.Path));

                    using (Stream stream = ossObject.Content)
                    {
                        stream.CopyTo(sourceStream);
                    }

                    Aspose.Words.Document doc = null;

                    if (attachment.Extension == "txt")
                    {
                        Aspose.Words.LoadOptions loadOptions = new Aspose.Words.LoadOptions();
                        loadOptions.Encoding = Encoding.Default;
                        doc = new Aspose.Words.Document(sourceStream, loadOptions);

                        doc.Save(fileLocalPathWithFileName, Aspose.Words.SaveFormat.Pdf);
                    }
                    else if (attachment.Extension == "xml")
                    {
                        Aspose.Words.LoadOptions loadOptions = new Aspose.Words.LoadOptions();
                        loadOptions.Encoding = Encoding.UTF8;
                        doc = new Aspose.Words.Document(sourceStream, loadOptions);

                        doc.Save(fileLocalPathWithFileName, Aspose.Words.SaveFormat.Pdf);
                    }
                    else
                    {
                        doc = new Aspose.Words.Document(sourceStream);
                        Aspose.Words.Saving.PdfSaveOptions saveOptions = new Aspose.Words.Saving.PdfSaveOptions();
                        saveOptions.OutlineOptions.HeadingsOutlineLevels = 3;

                        doc.Save(fileLocalPathWithFileName, saveOptions);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                logger.Error("PDF转换失败：" + attachment.Path, e);
                return false;
            }
            finally
            {
                //Aspose有内存泄漏的问题，转换后GC执行垃圾回收
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// 获取pdf文档的页数
        /// </summary>
        /// <param name="filePath">pdf文档路径</param>
        /// <returns>文档页数</returns>
        private int GetPdfPageCount(string filePath)
        {
            int pageNumer = 0;
            using (PdfReader pdfReader = new PdfReader(filePath))
            {
                pageNumer = pdfReader.NumberOfPages;
                pdfReader.Close();
            }

            return pageNumer;
        }

    }
}
