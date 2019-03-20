using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;


namespace Sop.Framework.Imaging
{
    /// <summary>
    /// 图片处理类 
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// Converts the PO's byte array to VO's image.
        /// </summary>
        /// <param name="bytes">The byte array in PO.</param>
        /// <returns>The Image object.</returns>
        public static Image ByteToImage(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            Image image = null;
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                image = Image.FromStream(stream);
            }
            return image;
        }

        /// <summary>
        /// Converts the VO's image member to PO's bytes.
        /// </summary>
        /// <param name="image">The Image object in VO.</param>
        /// <returns>The byte array.</returns>
        public static byte[] ImageToByte(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            byte[] bytes = null;
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Jpeg);
                bytes = stream.ToArray();
            }
            return bytes;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldFile"></param>
        /// <param name="newFile"></param>
        public static void WordMark(string oldFile, string newFile)
        {
            string Copyright = "sopcce.com";
            //create a image object containing the photograph to watermark
            Image imgPhoto = Image.FromFile(oldFile);
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //create a Bitmap the Size of the original photograph
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //load the Bitmap into a Graphics object 
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //Set the rendering quality for this Graphics object
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //Draws the photo Image object at original size to the graphics object.
            grPhoto.DrawImage(
                imgPhoto,                               // Photo Image object
                new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                0,                                      // x-coordinate of the portion of the source image to draw. 
                0,                                      // y-coordinate of the portion of the source image to draw. 
                phWidth,                                // Width of the portion of the source image to draw. 
                phHeight,                               // Height of the portion of the source image to draw. 
                GraphicsUnit.Pixel);                    // Units of measure 

            //-------------------------------------------------------
            //to maximize the size of the Copyright message we will 
            //test multiple Font sizes to determine the largest posible 
            //font we can use for the width of the Photograph
            //define an array of point sizes you would like to consider as possiblities
            //-------------------------------------------------------
            int[] sizes = new int[] { 28, 22, 20, 18, 16, 14, 12, 10, 8, 6, 4 };

            Font crFont = null;
            SizeF crSize = new SizeF();

            //Loop through the defined sizes checking the length of the Copyright string
            //If its length in pixles is less then the image width choose this Font size.
            for (int i = 0; i < 11; i++)
            {
                //set a Font object to Arial (i)pt, Bold
                crFont = new Font("arial", sizes[i], FontStyle.Bold);
                //Measure the Copyright string in this Font
                crSize = grPhoto.MeasureString(Copyright, crFont);

                if ((ushort)crSize.Width < (ushort)phWidth)
                    break;
            }

            //Since all photographs will have varying heights, determine a 
            //position 5% from the bottom of the image
            int yPixlesFromBottom = (int)(phHeight * .1);

            //Now that we have a point size use the Copyrights string height 
            //to determine a y-coordinate to draw the string of the photograph
            float yPosFromBottom = ((phHeight - yPixlesFromBottom) - (crSize.Height / 2));

            //Determine its x-coordinate by calculating the center of the width of the image
            float xCenterOfImg = phWidth / 2;

            //Define the text layout by setting the text alignment to centered
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;

            //define a Brush which is semi trasparent black (Alpha set to 153)
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

            //Draw the Copyright string
            grPhoto.DrawString(Copyright,                 //string of text
                crFont,                                   //font
                semiTransBrush2,                           //Brush
                new PointF(xCenterOfImg + 1, yPosFromBottom + 1),  //Position
                StrFormat);

            //define a Brush which is semi trasparent white (Alpha set to 153)
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

            //Draw the Copyright string a second time to create a shadow effect
            //Make sure to move this text 1 pixel to the right and down 1 pixel
            grPhoto.DrawString(Copyright,                 //string of text
                crFont,                                   //font
                semiTransBrush,                           //Brush
                new PointF(xCenterOfImg, yPosFromBottom),  //Position
                StrFormat);                               //Text alignment

            Bitmap bmWatermark = new Bitmap(bmPhoto);

            imgPhoto = bmWatermark;
            grPhoto.Dispose();


            imgPhoto.Save(newFile, imgPhoto.RawFormat);
            imgPhoto.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldFile"></param>
        /// <param name="newFile"></param>
        public static void ImageMark(string oldFile, string newFile)
        {
            string ImgMarkPath = System.Configuration.ConfigurationManager.AppSettings["ImgMark"];
            if (string.IsNullOrEmpty(ImgMarkPath))
            {
                throw new Exception("image mark path is null");
            }

            ImgMarkPath = System.Web.HttpContext.Current.Server.MapPath(ImgMarkPath);
            if (!File.Exists(ImgMarkPath))
            {
                throw new Exception("Can not find the image mark with path:" + ImgMarkPath);
            }


            Image imgWatermark = new Bitmap(ImgMarkPath);
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;

            //create a image object containing the photograph to watermark
            Image imgPhoto = Image.FromFile(oldFile);
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //create a Bitmap the Size of the original photograph
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            //load the Bitmap into a Graphics object 
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //Set the rendering quality for this Graphics object
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //Draws the photo Image object at original size to the graphics object.
            grPhoto.DrawImage(
                imgPhoto,                               // Photo Image object
                new Rectangle(0, 0, phWidth, phHeight), // Rectangle structure
                0,                                      // x-coordinate of the portion of the source image to draw. 
                0,                                      // y-coordinate of the portion of the source image to draw. 
                phWidth,                                // Width of the portion of the source image to draw. 
                phHeight,                               // Height of the portion of the source image to draw. 
                GraphicsUnit.Pixel);                    // Units of measure 

            //Create a Bitmap based on the previously modified photograph Bitmap
            Bitmap bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            //Load this Bitmap into a new Graphic Object
            Graphics grWatermark = Graphics.FromImage(bmWatermark);

            //To achieve a transulcent watermark we will apply (2) color 
            //manipulations by defineing a ImageAttributes object and 
            //seting (2) of its properties.
            ImageAttributes imageAttributes = new ImageAttributes();

            //The first step in manipulating the watermark image is to replace 
            //the background color with one that is trasparent (Alpha=0, R=0, G=0, B=0)
            //to do this we will use a Colormap and use this to define a RemapTable
            ColorMap colorMap = new ColorMap();

            //My watermark was defined with a background of 100% Green this will
            //be the color we search for and replace with transparency
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            //The second color manipulation is used to change the opacity of the 
            //watermark.  This is done by applying a 5x5 matrix that contains the 
            //coordinates for the RGBA space.  By setting the 3rd row and 3rd column 
            //to 0.3f we achive a level of opacity
            float[][] colorMatrixElements = {
                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},
                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);

            //For this example we will place the watermark in the upper right
            //hand corner of the photograph. offset down 10 pixels and to the 
            //left 10 pixles

            int xPosOfWm = Convert.ToInt32(phWidth - (phWidth / 2));
            int yPosOfWm = Convert.ToInt32(phHeight - (phHeight / 4));

            grWatermark.DrawImage(imgWatermark,
                new Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight),  //Set the detination Position
                0,                  // x-coordinate of the portion of the source image to draw. 
                0,                  // y-coordinate of the portion of the source image to draw. 
                wmWidth,            // Watermark Width
                wmHeight,		    // Watermark Height
                GraphicsUnit.Pixel, // Unit of measurment
                imageAttributes);   //ImageAttributes Object

            //Replace the original photgraphs bitmap with the new Bitmap
            imgPhoto = bmWatermark;
            grPhoto.Dispose();
            grWatermark.Dispose();

            //save new image to file system.
            imgPhoto.Save(newFile, imgPhoto.RawFormat);
            imgPhoto.Dispose();
            imgWatermark.Dispose();
        }
        /// <summary>
        ///  默认图片的最大像素宽度限制
        /// </summary>
        public static readonly int DefaultImageWidthLimit = int.Parse(ConfigurationManager.AppSettings["DefaultImageWidthLimit"] ?? "1000");
        /// <summary>
        /// 打水印时使用的字体文件完整的磁盘路径
        /// </summary>
        public static string WaterMarkFontFilePath { get; set; }
        /// <summary>
        /// 智能存储文件函数(优化存储) 
        /// </summary>
        /// <param name="ImageFileData">文件字节数据</param>
        /// <param name="FileName">存储目标磁盘路径</param>
        /// <param name="TargetSize"> 目标图片像素大小限制(0表示自动原图大小)</param>
        /// <param name="TargetSizeOnlyForWidthLimit"> 目标图片大小是否仅用作宽度限制，否则宽高均不得超限</param>
        /// <param name="WaterMark"> 图片的水印字符串(如果为空“string.Empty”则不打印水印)</param>
        /// <param name="WaterMarkOnImageSizeLimit">  打水印时图片尺寸限制，宽高均达到此要求的才打水印</param>
        /// /// 调用示例： if (!IntelligentSaveImageFile(ImageFileData, FileName, TargetSize,
        ///     TargetSizeOnlyForWidthLimit, WaterMark)) UploadFile.SaveAs(FileName); //需要检查保存IntelligentSaveImageFile忽略的文件。
        /// <returns>false没存储，true已经存储</returns>

        public static bool IntelligentSaveImageFile(byte[] ImageFileData, string FileName, int TargetSize, bool TargetSizeOnlyForWidthLimit, string WaterMark, Size WaterMarkOnImageSizeLimit)
        {
            string a;
            if ((a = Path.GetExtension(FileName).ToLower()) != null)
            {
                if (!(a == ".jpg") && !(a == ".jpeg"))
                {
                    if (!(a == ".bmp"))
                    {
                        return false;
                    }
                    FileName = string.Format("{0}.jpg", FileName.Substring(0, FileName.Length - 4));
                }
                using (Image image = Image.FromStream(new MemoryStream(ImageFileData)))
                {
                    if (image.Width < WaterMarkOnImageSizeLimit.Width || image.Height < WaterMarkOnImageSizeLimit.Height)
                    {
                        WaterMark = string.Empty;
                    }
                    int num = ImageFileData.Length / 1024;
                    if (num <= 100 && TargetSize <= 0 && string.IsNullOrEmpty(WaterMark))
                    {
                        return false;
                    }
                    Size size = image.Size;
                    if (TargetSize <= 0 && image.Width > DefaultImageWidthLimit)
                    {
                        TargetSize = DefaultImageWidthLimit;
                        TargetSizeOnlyForWidthLimit = true;
                    }
                    if (TargetSize > 0)
                    {
                        size = CalculateDimensions(image.Size, TargetSize, TargetSizeOnlyForWidthLimit);
                    }
                    using (Bitmap bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format24bppRgb))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            graphics.DrawImage(image, new Rectangle(new Point(-1, -1), new Size(size.Width + 2, size.Height + 2)));
                            if (!string.IsNullOrEmpty(WaterMark))
                            {
                                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                                Font font;
                                if (string.IsNullOrEmpty(WaterMarkFontFilePath))
                                {
                                    font = new Font("华文彩云", 25f, FontStyle.Regular, GraphicsUnit.Pixel);
                                }
                                else
                                {
                                    PrivateFontCollection privateFontCollection = new PrivateFontCollection();
                                    privateFontCollection.AddFontFile(WaterMarkFontFilePath);
                                    font = new Font(privateFontCollection.Families[0], 25f, FontStyle.Regular, GraphicsUnit.Pixel);
                                }
                                SolidBrush brush = new SolidBrush(Color.FromArgb(150, 0, 0, 0));
                                SolidBrush brush2 = new SolidBrush(Color.FromArgb(150, 255, 255, 255));
                                int num2 = size.Height - 45;
                                int num3 = 25;
                                if (num2 >= 0 && num3 >= 0)
                                {
                                    graphics.DrawString(WaterMark, font, brush, (float)num3, (float)num2);
                                    graphics.DrawString(WaterMark, font, brush2, (float)(num3 + 1), (float)(num2 + 1));
                                }
                            }
                            ImageCodecInfo encoder = GetEncoder(ImageFormat.Jpeg);
                            System.Drawing.Imaging.Encoder quality = System.Drawing.Imaging.Encoder.Quality;
                            EncoderParameters encoderParameters = new EncoderParameters(1);
                            EncoderParameter encoderParameter = new EncoderParameter(quality, 91L);
                            encoderParameters.Param[0] = encoderParameter;
                            bitmap.Save(FileName, encoder, encoderParameters);
                            encoderParameter.Dispose();
                            encoderParameters.Dispose();
                        }
                    }
                }
                return true;
            }
            return false;
        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] imageDecoders = ImageCodecInfo.GetImageDecoders();
            ImageCodecInfo[] array = imageDecoders;
            for (int i = 0; i < array.Length; i++)
            {
                ImageCodecInfo imageCodecInfo = array[i];
                if (imageCodecInfo.FormatID == format.Guid)
                {
                    return imageCodecInfo;
                }
            }
            return null;
        }

        private static Size CalculateDimensions(Size OldSize, int TargetSize, bool TargetSizeOnlyForWidthLimit)
        {
            if (OldSize.Width <= TargetSize && OldSize.Height <= TargetSize)
            {
                return OldSize;
            }
            if (TargetSizeOnlyForWidthLimit && OldSize.Width <= TargetSize)
            {
                return OldSize;
            }
            Size result = default(Size);
            if (TargetSizeOnlyForWidthLimit || OldSize.Width >= OldSize.Height)
            {
                result.Width = TargetSize;
                result.Height = (int)((float)OldSize.Height * ((float)TargetSize / (float)OldSize.Width));
            }
            else
            {
                result.Width = (int)((float)OldSize.Width * ((float)TargetSize / (float)OldSize.Height));
                result.Height = TargetSize;
            }
            return result;
        }

        /// <summary>
        /// 获取图片的大小(宽高)。涉及大量服务器端IO操作及内存占用，尽量减少使用
        /// </summary>
        /// <param name="ImgPathName"> 图片文件物理磁盘全路径，仅支持“IoHelper.AllowImageFileTypes”规定的图片类型</param>
        /// <returns>返回值Size中的宽或高为0：文件不存在、不支持文件类型、处理时出错；其它：为实际图片大小。</returns>
        public static Size GetImageSize(string ImgPathName)
        {
            Size size = new Size(0, 0);
            if (!File.Exists(ImgPathName))
            {
                return size;
            }

            try
            {
                using (Stream stream = File.OpenRead(ImgPathName))
                {
                    using (Image image = Image.FromStream(stream))
                    {
                        size = image.Size;
                    }
                }
            }
            catch
            {
            }
            return size;
        }


        private static ImageFormat GetImgFormat(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            switch (extension)
            {
                case "jpg":
                    return ImageFormat.Jpeg;
                case "bmp":
                    return ImageFormat.Bmp;
                case "png":
                    return ImageFormat.Png;
                case "gif":
                    return ImageFormat.Gif;
                case "ico":
                    return ImageFormat.Icon;
                default:
                    return ImageFormat.Jpeg;
            }
        }
        
      
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW":  //指定高宽缩放（可能变形）                
                    break;
                case "W":   //指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H":   //指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut": //指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight), new System.Drawing.Rectangle(x, y, ow, oh), System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
       

        /// <summary>
        /// 图片水印处理方法
        /// </summary>
        /// <param name="path">需要加载水印的图片路径（绝对路径）</param>
        /// <param name="waterpath">水印图片（绝对路径）</param>
        /// <param name="location">水印位置（传送正确的代码）</param>
        public static string ImageWatermark(string path, string waterpath, string location)
        {
            string kz_name = Path.GetExtension(path);
            if (kz_name == ".jpg" || kz_name == ".bmp" || kz_name == ".jpeg")
            {
                DateTime time = DateTime.Now;
                string filename = "" + time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString() + time.Millisecond.ToString();
                Image img = Bitmap.FromFile(path);
                Image waterimg = Image.FromFile(waterpath);
                Graphics g = Graphics.FromImage(img);
                ArrayList loca = GetLocation(location, img, waterimg);
                g.DrawImage(waterimg, new Rectangle(int.Parse(loca[0].ToString()), int.Parse(loca[1].ToString()), waterimg.Width, waterimg.Height));
                waterimg.Dispose();
                g.Dispose();
                string newpath = Path.GetDirectoryName(path) + filename + kz_name;
                img.Save(newpath);
                img.Dispose();
                File.Copy(newpath, path, true);
                if (File.Exists(newpath))
                {
                    File.Delete(newpath);
                }
            }
            return path;
        }

        /// <summary>
        /// 图片水印位置处理方法
        /// </summary>
        /// <param name="location">水印位置</param>
        /// <param name="img">需要添加水印的图片</param>
        /// <param name="waterimg">水印图片</param>
        private static ArrayList GetLocation(string location, Image img, Image waterimg)
        {
            ArrayList loca = new ArrayList();
            int x = 0;
            int y = 0;

            if (location == "LT")
            {
                x = 10;
                y = 10;
            }
            else if (location == "T")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height - waterimg.Height;
            }
            else if (location == "RT")
            {
                x = img.Width - waterimg.Width;
                y = 10;
            }
            else if (location == "LC")
            {
                x = 10;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "C")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "RC")
            {
                x = img.Width - waterimg.Width;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "LB")
            {
                x = 10;
                y = img.Height - waterimg.Height;
            }
            else if (location == "B")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height - waterimg.Height;
            }
            else
            {
                x = img.Width - waterimg.Width;
                y = img.Height - waterimg.Height;
            }
            loca.Add(x);
            loca.Add(y);
            return loca;
        }
       

     
        /// <summary>
        /// 文字水印处理方法
        /// </summary>
        /// <param name="path">图片路径（绝对路径）</param>
        /// <param name="size">字体大小</param>
        /// <param name="letter">水印文字</param>
        /// <param name="color">颜色</param>
        /// <param name="location">水印位置</param>
        public static string LetterWatermark(string path, int size, string letter, Color color, string location)
        {
            #region

            string kz_name = Path.GetExtension(path);
            if (kz_name == ".jpg" || kz_name == ".bmp" || kz_name == ".jpeg")
            {
                DateTime time = DateTime.Now;
                string filename = "" + time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString() + time.Millisecond.ToString();
                Image img = Bitmap.FromFile(path);
                Graphics gs = Graphics.FromImage(img);
                ArrayList loca = GetLocation(location, img, size, letter.Length);
                Font font = new Font("宋体", size);
                Brush br = new SolidBrush(color);
                gs.DrawString(letter, font, br, float.Parse(loca[0].ToString()), float.Parse(loca[1].ToString()));
                gs.Dispose();
                string newpath = Path.GetDirectoryName(path) + filename + kz_name;
                img.Save(newpath);
                img.Dispose();
                File.Copy(newpath, path, true);
                if (File.Exists(newpath))
                {
                    File.Delete(newpath);
                }
            }
            return path;

            #endregion
        }

        /// <summary>
        /// 文字水印位置的方法
        /// </summary>
        /// <param name="location">位置代码</param>
        /// <param name="img">图片对象</param>
        /// <param name="width">宽(当水印类型为文字时,传过来的就是字体的大小)</param>
        /// <param name="height">高(当水印类型为文字时,传过来的就是字符的长度)</param>
        private static ArrayList GetLocation(string location, Image img, int width, int height)
        {
            ArrayList loca = new ArrayList();  //定义数组存储位置
            float x = 10;
            float y = 10;

            if (location == "LT")
            {
                loca.Add(x);
                loca.Add(y);
            }
            else if (location == "T")
            {
                x = img.Width / 2 - (width * height) / 2;
                loca.Add(x);
                loca.Add(y);
            }
            else if (location == "RT")
            {
                x = img.Width - width * height;
            }
            else if (location == "LC")
            {
                y = img.Height / 2;
            }
            else if (location == "C")
            {
                x = img.Width / 2 - (width * height) / 2;
                y = img.Height / 2;
            }
            else if (location == "RC")
            {
                x = img.Width - height;
                y = img.Height / 2;
            }
            else if (location == "LB")
            {
                y = img.Height - width - 5;
            }
            else if (location == "B")
            {
                x = img.Width / 2 - (width * height) / 2;
                y = img.Height - width - 5;
            }
            else
            {
                x = img.Width - width * height;
                y = img.Height - width - 5;
            }
            loca.Add(x);
            loca.Add(y);
            return loca;

        }
         
 
     
 
    }
}