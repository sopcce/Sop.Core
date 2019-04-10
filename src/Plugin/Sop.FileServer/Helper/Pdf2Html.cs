using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Timers;
using Aspose.Pdf;
using Common.Logging;
using GhostscriptSharp;
using Sop.Framework.Utility;

namespace Sop.FileServer.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class Pdf2Html
    {
        public string HomePath { get; private set; }
        public string TempPath { get; private set; }


        private readonly ILog _logger = LogManager.GetLogger<Pdf2Html>();
        private static volatile Pdf2Html _instance = null;
        private static readonly object Lock = new object();
        public static Pdf2Html Instance()
        {
            if (_instance == null)
            {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Pdf2Html();
                    }
                }
            }
            return _instance;
        }



        public Pdf2Html()
        {
            //初始化临时目录
            HomePath = Assembly.GetExecutingAssembly().Location;
            HomePath = HomePath.Substring(0, HomePath.LastIndexOf('\\'));
            TempPath = FileUtility.Combine(HomePath, "pdf2temp");
            if (!Directory.Exists(TempPath))
            {
                Directory.CreateDirectory(TempPath);
            }

            //获取所有的lic文件。
            try
            {
                DirectoryInfo folder = new DirectoryInfo(HomePath);
                foreach (FileInfo file in folder.GetFiles("*.lic"))
                {
                    try
                    {
                        new Aspose.Cells.License().SetLicense(file.FullName);
                        new Aspose.Words.License().SetLicense(file.FullName);
                        new Aspose.Slides.License().SetLicense(file.FullName);
                        new Aspose.Pdf.License().SetLicense(file.FullName);
                        new Aspose.Diagram.License().SetLicense(file.FullName);
                        new Aspose.Tasks.License().SetLicense(file.FullName);
                        new Aspose.Email.License().SetLicense(file.FullName);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("解析Aspose组件授权失败：" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("解析Aspose组件授权失败：" + ex.Message);
            }




        }


        public bool SendEmail()
        {
            Aspose.Email.MailMessage mailAddress = new Aspose.Email.MailMessage();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public bool GetPdfThumbnail(string sourcePath, string targetPath)
        {
            try
            {
                GhostscriptWrapper.GeneratePageThumb(sourcePath, targetPath, 1, 15, 15);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error("获取PDF缩略图失败：" + sourcePath, e);
                return false;
            }
        }
        /// <summary>
        ///  PDF转换为HTML
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public bool PdfToHtml(string sourcePath, string targetPath)
        {

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourcePath);
            string fileLocalPathWithFileName = targetPath;

            #region PDF转换HTML预览

            string htmlOutputPath = Path.Combine(TempPath, fileNameWithoutExtension);


            string binPath = FileUtility.Combine(HomePath, "pdf2htmlEX", "pdf2htmlEX.exe");

            //详见pdf2htmlEX的命令行参数说明：https://github.com/coolwanglu/pdf2htmlEX/wiki
            string binParams = string.Format("--embed CFIJO --first-page 1 --last-page 800 --fit-width 1800 --hdpi 96 --vdpi 96 --embed-external-font 0 --font-size-multiplier 1 --split-pages 1 --dest-dir {0} --page-filename page-%d.html {1} preview.html", htmlOutputPath, fileLocalPathWithFileName.Replace('\\', '/'));

            ProcessStartInfo oInfo = new ProcessStartInfo(binPath, binParams);
            oInfo.UseShellExecute = false;          //不使用系统外壳程序启动
            oInfo.CreateNoWindow = true;            //不创建窗口
            oInfo.RedirectStandardOutput = true;    //重定向输出
            oInfo.RedirectStandardError = true;     //重定向错误

            Process proc = new Process();
            proc.StartInfo = oInfo;

            //计时器来将其进程强制杀死超时的进程
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
                _logger.Error("PDF转换为HTML失败：" + binParams, e);
            }
            finally
            {
                timer.Close();
                timer.Dispose();
                proc.Close();
                proc.Dispose();
            }


            DirectoryInfo htmlOutputFolder = new DirectoryInfo(htmlOutputPath);

            string previewFile = Path.Combine(htmlOutputPath, "preview.html");

            if (File.Exists(previewFile) && new FileInfo(previewFile).Length > 0)
            {

                foreach (FileInfo htmlFileInfo in htmlOutputFolder.GetFiles())
                {

                }
                _logger.Info("PDF转换为HTML成功：" + sourcePath);
                return true;
            }
            else
            {
                _logger.Error("PDF转换为HTML失败：" + sourcePath);
                return true;
            }

            #endregion

            ////提取文档的页码
            //int pageNumer = 0;
            //using (PdfReader pdfReader = new PdfReader(fileLocalPathWithFileName))
            //{
            //    pageNumer = pdfReader.NumberOfPages;
            //    pdfReader.Close();
            //}
            //attachment.PageCount = pageNumer;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public bool PdfToFile(string sourcePath, string targetPath, SaveFormat format = SaveFormat.Html)
        {
            string fileName = Path.GetFileName(sourcePath);
            string fileExtension = Path.GetExtension(sourcePath);
            try
            {
                using (var sourceStream = new MemoryStream())
                {
                    using (Stream stream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        stream.CopyTo(sourceStream);
                    }
                    Aspose.Pdf.Document document = new Aspose.Pdf.Document(sourceStream);

                    document.Save(targetPath, format);
                }
                return true;
            }
            catch (Exception e)
            {
                _logger.Error("PDF转换失败：" + sourcePath, e);
                return false;
            }
            finally
            {
                Collect();
            }
        }




        /// <summary>
        /// 把Excel文件转换成PDF格式文件
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true=转换成功</returns>
        public bool ExcelToPdf(string sourcePath, string targetPath)
        {
            string fileName = Path.GetFileName(sourcePath);
            string fileExtension = Path.GetExtension(sourcePath);
            try
            {
                using (var sourceStream = new MemoryStream())
                {
                    using (Stream stream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        stream.CopyTo(sourceStream);
                    }
                    Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(sourceStream);
                    Aspose.Cells.PdfSaveOptions saveOptions = new Aspose.Cells.PdfSaveOptions();
                    saveOptions.OnePagePerSheet = true;
                    saveOptions.IgnoreError = true;
                    workbook.Save(targetPath, saveOptions);
                }
                return true;
            }
            catch (Exception e)
            {
                _logger.Error("PDF转换失败：" + sourcePath, e);
                return false;
            }
            finally
            {
                Collect();
            }
        }

        /// <summary>
        /// 把PowerPoint文件转换成PDF格式文件
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true=转换成功</returns>
        /// <returns>true=转换成功</returns>
        public bool PowerPointToPdf(string sourcePath, string targetPath)
        {
            string fileName = Path.GetFileName(sourcePath);
            string fileExtension = Path.GetExtension(sourcePath);
            try
            {
                using (var sourceStream = new MemoryStream())
                {
                    using (Stream stream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        stream.CopyTo(sourceStream);
                        sourceStream.Position = 0;
                    }
                    Aspose.Slides.Presentation pres = new Aspose.Slides.Presentation(sourceStream);
                    pres.Save(targetPath, Aspose.Slides.Export.SaveFormat.Pdf);
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.Error("PDF转换失败：" + sourcePath, e);
                return false;
            }
            finally
            {
                Collect();
            }
        }

        /// <summary>
        /// 把Word文件转换成为PDF格式文件
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <param name="targetPath">目标文件路径</param>
        /// <returns>true=转换成功</returns>
        /// <returns>true=转换成功</returns>
        public bool WordToPdf(string sourcePath, string targetPath)
        {
            string fileName = Path.GetFileName(sourcePath);
            string fileExtension = Path.GetExtension(sourcePath);

            try
            {

                using (var sourceStream = new MemoryStream())
                {
                    using (Stream stream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        stream.CopyTo(sourceStream);
                    }
                    Aspose.Words.Document doc = null;

                    if (fileExtension == "txt")
                    {
                        Aspose.Words.LoadOptions loadOptions = new Aspose.Words.LoadOptions();
                        loadOptions.Encoding = Encoding.Default;
                        doc = new Aspose.Words.Document(sourceStream, loadOptions);

                        doc.Save(targetPath, Aspose.Words.SaveFormat.Pdf);
                    }
                    if (fileExtension == "xml")
                    {
                        Aspose.Words.LoadOptions loadOptions = new Aspose.Words.LoadOptions();
                        loadOptions.Encoding = Encoding.UTF8;
                        doc = new Aspose.Words.Document(sourceStream, loadOptions);

                        doc.Save(targetPath, Aspose.Words.SaveFormat.Pdf);
                    }
                    else
                    {
                        doc = new Aspose.Words.Document(sourceStream);
                        Aspose.Words.Saving.PdfSaveOptions saveOptions = new Aspose.Words.Saving.PdfSaveOptions();
                        saveOptions.OutlineOptions.HeadingsOutlineLevels = 3;

                        doc.Save(targetPath, saveOptions);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                _logger.Error("PDF转换失败：" + sourcePath, e);
                return false;
            }
            finally
            {
                Collect();
            }
        }

        public void Collect()
        {
            //Aspose有内存泄漏的问题，转换后GC执行垃圾回收
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            _logger.Info(" Pdf2Html Collect Memory 。" + System.Environment.NewLine);
        }

    }

}
