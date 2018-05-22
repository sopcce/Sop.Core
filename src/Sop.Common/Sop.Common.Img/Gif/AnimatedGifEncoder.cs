using System;
using System.Drawing;
using System.IO;

namespace Sop.Common.Img.Gif
{
    /// <summary>
    ///
    /// </summary>
    public class AnimatedGifEncoder_old
    {
        #region MyRegion

        /// <summary>
        ///
        /// </summary>
        protected MemoryStream Memory;

        /// <summary>
        /// image size
        /// </summary>
        protected int width;

        /// <summary>
        ///
        /// </summary>
        protected int height;

        /// <summary>
        /// 透明色如果
        /// </summary>
        protected Color transparent = Color.Empty; //

        /// <summary>
        ///
        /// </summary>
        protected int transIndex; // transparent index in color table

        /// <summary>
        /// // no repeat
        /// </summary>
        protected int repeat = -1;

        /// <summary>
        /// // frame delay (hundredths) 帧延迟（秒）
        /// </summary>
        protected int delay = 0;

        /// <summary>
        /// ready to output frames
        /// </summary>
        protected bool started = false; //

        /// <summary>
        /// 二进制流输出bw;
        /// </summary>
        protected BinaryWriter bw;

        /// <summary>
        ///受保护的文件流fs; protected FileStream fs;
        /// </summary>
        protected Stream fs;

        /// <summary>
        /// // current frame
        /// </summary>
        protected Image image;

        /// <summary>
        ///  // BGR byte array from frame
        /// </summary>
        protected byte[] pixels;

        /// <summary>
        /// // converted frame indexed to palette
        /// </summary>
        protected byte[] indexedPixels;

        /// <summary>
        /// // number of bit planes
        /// </summary>
        protected int colorDepth;

        /// <summary>
        /// // RGB palette
        /// </summary>
        protected byte[] colorTab;

        /// <summary>
        ///  // active palette entries
        /// </summary>
        protected bool[] usedEntry = new bool[256];

        /// <summary>
        /// // color table size (bits-1)
        /// </summary>
        protected int palSize = 7;

        /// <summary>
        ///  // disposal code (-1 = use default)
        /// </summary>
        protected int dispose = -1;

        /// <summary>
        /// 完成后关闭流//close stream when finished
        /// </summary>
    protected bool closeStream = false; // close stream when finished

        /// <summary>
        /// // if false, get size from first frame
        /// </summary>
        protected bool firstFrame = true;

        /// <summary>
        /// // if false, get size from first frame
        /// </summary>
        protected bool sizeSet = false;

        /// <summary>
        ///
        /// </summary>
        protected int sample = 10; // default sample interval for quantizer

        #endregion MyRegion



        /// <summary>
        ///  //集每一帧之间的延迟时间,或改变它对后续帧(适用于最后一帧添加)。
        /// </summary>
        /// <param name="ms">int延迟时间,以毫秒为单位</param>
        public void SetDelay(int ms)
        {
            delay = (int)Math.Round(ms / 10.0f);
        }

        /// <summary>
        /// 集最后的GIF帧处理代码添加框架和任何后续帧。默认是0如果没有设置透明色,否则2。
        /// </summary>
        /// <param name="code">int处理代码</param>
        public void SetDispose(int code)
        {
            if (code >= 0)
            {
                dispose = code;
            }
        }

        /// <summary>
        /// Sets the number of times the set of GIF frames should be played.
        /// Default is 1; 0 means play indefinitely.
        /// Must be invoked before the first image is added.
        /// </summary>
        /// <param name="iter">iter int number of iterations.</param>
        public void SetRepeat(int iter)
        {
            if (iter >= 0)
            {
                repeat = iter;
            }
        }

        /// <summary>
        /// Sets the transparent color for the last added frame
        /// and any subsequent frames.
        /// Since all colors are subject to modification
        /// in the quantization process, the color in the final
        /// palette for each frame closest to the given color
        /// becomes the transparent color for that frame.
        /// May be set to null to indicate no transparent color.
        /// </summary>
        /// <param name="c">Color to be treated as transparent on display.</param>
        public void SetTransparent(Color c)
        {
            transparent = c;
        }

        /**
     * Adds next GIF frame.  The frame is not written immediately, but is
     * actually deferred until the next frame is received so that timing
     * data can be inserted.  Invoking <code>finish()</code> flushes all
     * frames.  If <code>setSize</code> was not invoked, the size of the
     * first image is used for all subsequent frames.
     *
     * @param im BufferedImage containing frame to write.
     * @return true if successful.
     */

        public bool AddFrame(Image im)
        {
            if ((im == null) || !started)
            {
                return false;
            }
            bool ok = true;
            try
            {
                if (!sizeSet)
                {
                    // use first frame's size
                    SetSize(im.Width, im.Height);
                }
                image = im;
                GetImagePixels(); // convert to correct format if necessary
                AnalyzePixels(); // build color table & map pixels
                if (firstFrame)
                {
                    WriteLSD(); // logical screen descriptior
                    WritePalette(); // global color table
                    if (repeat >= 0)
                    {
                        // use NS app extension to indicate reps
                        WriteNetscapeExt();
                    }
                }
                WriteGraphicCtrlExt(); // write graphic control extension
                WriteImageDesc(); // image descriptor
                if (!firstFrame)
                {
                    WritePalette(); // local color table
                }
                WritePixels(); // encode and write pixel data
                firstFrame = false;
            }
            catch
            {
                ok = false;
            }

            return ok;
        }

        /**
     * Flushes any pending data and closes output file.
     * If writing to an OutputStream, the stream is not
     * closed.
     */

        public bool Finish()
        {
            if (!started) return false;
            bool ok = true;
            started = false;
            try
            {
                fs.WriteByte(0x3b); // gif trailer
                fs.Flush();
                if (closeStream)
                {
                    fs.Close();
                }
            }
            catch
            {
                ok = false;
            }

            // reset for subsequent use
            transIndex = 0;
            fs = null;
            image = null;
            pixels = null;
            indexedPixels = null;
            colorTab = null;
            closeStream = false;
            firstFrame = true;

            return ok;
        }

        /**
     * Sets frame rate in frames per second.  Equivalent to
     * <code>setDelay(1000/fps)</code>.
     *
     * @param fps float frame rate (frames per second)
     */

        public void SetFrameRate(float fps)
        {
            if (fps != 0f)
            {
                delay = (int)Math.Round(100f / fps);
            }
        }

        /**
     * Sets quality of color quantization (conversion of images
     * to the maximum 256 colors allowed by the GIF specification).
     * Lower values (minimum = 1) produce better colors, but slow
     * processing significantly.  10 is the default, and produces
     * good color mapping at reasonable speeds.  Values greater
     * than 20 do not yield significant improvements in speed.
     *
     * @param quality int greater than 0.
     * @return
     */

        public void SetQuality(int quality)
        {
            if (quality < 1) quality = 1;
            sample = quality;
        }

        /**
     * Sets the GIF frame size.  The default size is the
     * size of the first frame added if this method is
     * not invoked.
     *
     * @param w int frame width.
     * @param h int frame width.
     */

        public void SetSize(int w, int h)
        {
            if (started && !firstFrame) return;
            width = w;
            height = h;
            if (width < 1) width = 320;
            if (height < 1) height = 240;
            sizeSet = true;
        }

        /**
     * Initiates GIF file creation on the given stream.  The stream
     * is not closed automatically.
     *
     * @param os OutputStream on which GIF images are written.
     * @return false if initial write failed.
     */

        public bool Start(Stream os)
        {
            if (os == null) return false;
            bool ok = true;
            closeStream = false;
            fs = os;
            try
            {
                WriteString("GIF89a"); // header
            }
            catch
            {
                ok = false;
            }
            return started = ok;
        }

        /// <summary>
        ///
        /// </summary>
        public void Start()
        {
            this.Memory = new MemoryStream();
            this.WriteString("GIF89a");
            this.started = true;
        }

        /**
     * Initiates writing of a GIF file with the specified name.
     *
     * @param file String containing output file name.
     * @return false if open or initial write failed.
     */

        public bool Start(String file)
        {
            bool ok = true;
            try
            {
                //bw = new BinaryWriter( new FileStream( file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None ) );
                fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                ok = Start(fs);
                closeStream = true;
            }
            catch
            {
                ok = false;
            }
            return started = ok;
        }

        /**
     * Analyzes image colors and creates color map.
     */

        protected void AnalyzePixels()
        {
            int len = pixels.Length;
            int nPix = len / 3;
            indexedPixels = new byte[nPix];
            NeuQuant nq = new NeuQuant(pixels, len, sample);
            // initialize quantizer
            colorTab = nq.Process(); // create reduced palette
                                     // convert map from BGR to RGB
                                     //			for (int i = 0; i < colorTab.Length; i += 3)
                                     //			{
                                     //				byte temp = colorTab[i];
                                     //				colorTab[i] = colorTab[i + 2];
                                     //				colorTab[i + 2] = temp;
                                     //				usedEntry[i / 3] = false;
                                     //			}
                                     // map image pixels to new palette
            int k = 0;
            for (int i = 0; i < nPix; i++)
            {
                int index =
                    nq.Map(pixels[k++] & 0xff,
                    pixels[k++] & 0xff,
                    pixels[k++] & 0xff);
                usedEntry[index] = true;
                indexedPixels[i] = (byte)index;
            }
            pixels = null;
            colorDepth = 8;
            palSize = 7;
            // get closest match to transparent color if specified
            if (transparent != Color.Empty)
            {
                transIndex = FindClosest(transparent);
            }
        }

        /**
     * Returns index of palette color closest to c
     *
     */

        protected int FindClosest(Color c)
        {
            if (colorTab == null) return -1;
            int r = c.R;
            int g = c.G;
            int b = c.B;
            int minpos = 0;
            int dmin = 256 * 256 * 256;
            int len = colorTab.Length;
            for (int i = 0; i < len;)
            {
                int dr = r - (colorTab[i++] & 0xff);
                int dg = g - (colorTab[i++] & 0xff);
                int db = b - (colorTab[i] & 0xff);
                int d = dr * dr + dg * dg + db * db;
                int index = i / 3;
                if (usedEntry[index] && (d < dmin))
                {
                    dmin = d;
                    minpos = index;
                }
                i++;
            }
            return minpos;
        }

        /**
     * Extracts image pixels into byte array "pixels"
     */

        protected void GetImagePixels()
        {
            int w = image.Width;
            int h = image.Height;
            //		int type = image.GetType().;
            if ((w != width)
                || (h != height)
                )
            {
                // create new image with right size/format
                Image temp =
                    new Bitmap(width, height);
                Graphics g = Graphics.FromImage(temp);
                g.DrawImage(image, 0, 0);
                image = temp;
                g.Dispose();
            }
            /*
        ToDo:
        improve performance: use unsafe code
      */
            pixels = new Byte[3 * image.Width * image.Height];
            int count = 0;
            Bitmap tempBitmap = new Bitmap(image);
            for (int th = 0; th < image.Height; th++)
            {
                for (int tw = 0; tw < image.Width; tw++)
                {
                    Color color = tempBitmap.GetPixel(tw, th);
                    pixels[count] = color.R;
                    count++;
                    pixels[count] = color.G;
                    count++;
                    pixels[count] = color.B;
                    count++;
                }
            }

            //		pixels = ((DataBufferByte) image.getRaster().getDataBuffer()).getData();
        }

        /// <summary>
        /// Writes Graphic Control Extension
        /// </summary>
        protected void WriteGraphicCtrlExt()
        {
            fs.WriteByte(0x21); // extension introducer
            fs.WriteByte(0xf9); // GCE label
            fs.WriteByte(4); // data block size
            int transp, disp;
            if (transparent == Color.Empty)
            {
                transp = 0;
                disp = 0; // dispose = no action
            }
            else
            {
                transp = 1;
                disp = 2; // force clear if using transparent color
            }
            if (dispose >= 0)
            {
                disp = dispose & 7; // user override
            }
            disp <<= 2;

            // packed fields
            fs.WriteByte(Convert.ToByte(0 | // 1:3 reserved
                disp | // 4:6 disposal
                0 | // 7   user input - 0 = none
                transp)); // 8   transparency flag

            WriteShort(delay); // delay x 1/100 sec
            fs.WriteByte(Convert.ToByte(transIndex)); // transparent color index
            fs.WriteByte(0); // block terminator
        }

        /// <summary>
        /// Writes Image Descriptor
        /// </summary>
        protected void WriteImageDesc()
        {
            fs.WriteByte(0x2c); // image separator
            WriteShort(0); // image position x,y = 0,0
            WriteShort(0);
            WriteShort(width); // image size
            WriteShort(height);
            // packed fields
            if (firstFrame)
            {
                // no LCT  - GCT is used for first (or only) frame
                fs.WriteByte(0);
            }
            else
            {
                // specify normal LCT
                fs.WriteByte(Convert.ToByte(0x80 | // 1 local color table  1=yes
                    0 | // 2 interlace - 0=no
                    0 | // 3 sorted - 0=no
                    0 | // 4-5 reserved
                    palSize)); // 6-8 size of color table
            }
        }

        /// <summary>
        /// Writes Logical Screen Descriptor
        /// </summary>
        protected void WriteLSD()
        {
            // logical screen size
            WriteShort(width);
            WriteShort(height);
            // packed fields
            fs.WriteByte(Convert.ToByte(0x80 | // 1   : global color table flag = 1 (gct used)
                0x70 | // 2-4 : color resolution = 7
                0x00 | // 5   : gct sort flag = 0
                palSize)); // 6-8 : gct size

            fs.WriteByte(0); // background color index
            fs.WriteByte(0); // pixel aspect ratio - assume 1:1
        }

        /// <summary>
        /// Writes Netscape application extension to define repeat count.
        /// </summary>
        protected void WriteNetscapeExt()
        {
            fs.WriteByte(0x21); // extension introducer
            fs.WriteByte(0xff); // app extension label
            fs.WriteByte(11); // block size
            WriteString("NETSCAPE" + "2.0"); // app id + auth code
            fs.WriteByte(3); // sub-block size
            fs.WriteByte(1); // loop sub-block id
            WriteShort(repeat); // loop count (extra iterations, 0=repeat forever)
            fs.WriteByte(0); // block terminator
        }

        /// <summary>
        /// Writes color table
        /// </summary>
        protected void WritePalette()
        {
            fs.Write(colorTab, 0, colorTab.Length);
            int n = (3 * 256) - colorTab.Length;
            for (int i = 0; i < n; i++)
            {
                fs.WriteByte(0);
            }
        }

        /// <summary>
        /// Encodes and writes pixel data
        /// </summary>
        protected void WritePixels()
        {
            LzwEncoder encoder =
                new LzwEncoder(width, height, indexedPixels, colorDepth);
            encoder.Encode(fs);
        }

        /// <summary>
        /// Write 16-bit value to output stream, LSB first
        /// </summary>
        /// <param name="value"></param>
        protected void WriteShort(int value)
        {
            fs.WriteByte(Convert.ToByte(value & 0xff));
            fs.WriteByte(Convert.ToByte((value >> 8) & 0xff));
        }

        /// <summary>
        /// Writes string to output stream
        /// </summary>
        /// <param name="s"></param>
        protected void WriteString(String s)
        {
            char[] chars = s.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                fs.WriteByte((byte)chars[i]);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="MemoryResult"></param>
        public void OutPut(ref MemoryStream MemoryResult)
        {
            this.started = false;
            this.Memory.WriteByte(59);
            this.Memory.Flush();
            MemoryResult = this.Memory;
            this.Memory.Close();
            this.Memory.Dispose();
            this.transIndex = 0;
            this.Memory = null;
            this.image = null;
            this.pixels = null;
            this.indexedPixels = null;
            this.colorTab = null;
            this.firstFrame = true;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class AnimatedGifEncoder
    {
        #region MyRegion
        /// <summary>
        /// 
        /// </summary>
        protected int colorDepth;
        /// <summary>
        /// 
        /// </summary>
        protected byte[] colorTab;
        /// <summary>
        /// 
        /// </summary>
        protected int delay;
        /// <summary>
        /// 
        /// </summary>
        protected int dispose = -1;
        /// <summary>
        /// 
        /// </summary>
        protected bool firstFrame = true;
        /// <summary>
        /// 
        /// </summary>
        protected int height;
        /// <summary>
        /// 
        /// </summary>
        protected System.Drawing.Image image;
        /// <summary>
        /// 
        /// </summary>
        protected byte[] indexedPixels;
        /// <summary>
        /// 
        /// </summary>
        protected MemoryStream Memory;
        /// <summary>
        /// 
        /// </summary>
        protected int palSize = 7;
        /// <summary>
        /// 
        /// </summary>
        protected byte[] pixels;
        /// <summary>
        /// 
        /// </summary>
        protected int repeat = -1;
        /// <summary>
        /// 
        /// </summary>
        protected int sample = 10;
        /// <summary>
        /// 
        /// </summary>
        protected bool sizeSet;
        /// <summary>
        /// 
        /// </summary>
        protected bool started;
        /// <summary>
        /// 
        /// </summary>
        protected int transIndex;
        /// <summary>
        /// 
        /// </summary>
        protected Color transparent = Color.Empty;
        /// <summary>
        /// 
        /// </summary>
        protected bool[] usedEntry = new bool[256];
        /// <summary>
        /// 
        /// </summary>
        protected int width;

        #endregion MyRegion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="im"></param>
        /// <returns></returns>
        public bool AddFrame(Image im)
        {
            if (im == null || !started)
            {
                return false;
            }
            bool result = true;
            try
            {
                if (!this.sizeSet)
                {
                    SetSize(im.Width, im.Height);
                }
                image = im;
                GetImagePixels();
                AnalyzePixels();
                if (firstFrame)
                {
                    WriteLSD();
                    WritePalette();
                    if (repeat >= 0)
                    {
                        WriteNetscapeExt();
                    }
                }
                WriteGraphicCtrlExt();
                WriteImageDesc();
                if (!firstFrame)
                {
                    WritePalette();
                }
                WritePixels();
                firstFrame = false;
            }
            catch (IOException)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        protected void AnalyzePixels()
        {
            int num = this.pixels.Length;
            int num2 = num / 3;
            this.indexedPixels = new byte[num2];
            NeuQuant neuQuant = new NeuQuant(this.pixels, num, this.sample);
            this.colorTab = neuQuant.Process();
            int num3 = 0;
            for (int i = 0; i < num2; i++)
            {
                int num4 = neuQuant.Map((int)(this.pixels[num3++] & 255), (int)(this.pixels[num3++] & 255), (int)(this.pixels[num3++] & 255));
                this.usedEntry[num4] = true;
                this.indexedPixels[i] = (byte)num4;
            }
            this.pixels = null;
            this.colorDepth = 8;
            this.palSize = 7;
            if (this.transparent != Color.Empty)
            {
                this.transIndex = this.FindClosest(this.transparent);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected int FindClosest(Color c)
        {
            if (this.colorTab == null)
            {
                return -1;
            }
            int r = (int)c.R;
            int g = (int)c.G;
            int b = (int)c.B;
            int result = 0;
            int num = 16777216;
            int num2 = this.colorTab.Length;
            for (int i = 0; i < num2; i++)
            {
                int num3 = r - (int)(this.colorTab[i++] & 255);
                int num4 = g - (int)(this.colorTab[i++] & 255);
                int num5 = b - (int)(this.colorTab[i] & 255);
                int num6 = num3 * num3 + num4 * num4 + num5 * num5;
                int num7 = i / 3;
                if (this.usedEntry[num7] && num6 < num)
                {
                    num = num6;
                    result = num7;
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        protected void GetImagePixels()
        {
            int num = this.image.Width;
            int num2 = this.image.Height;
            if (num != this.width || num2 != this.height)
            {
                Image image = new Bitmap(this.width, this.height);
                Graphics graphics = Graphics.FromImage(image);
                graphics.DrawImage(this.image, 0, 0);
                this.image = image;
                graphics.Dispose();
            }
            this.pixels = new byte[3 * this.image.Width * this.image.Height];
            int num3 = 0;
            Bitmap bitmap = new Bitmap(this.image);
            for (int i = 0; i < this.image.Height; i++)
            {
                for (int j = 0; j < this.image.Width; j++)
                {
                    Color pixel = bitmap.GetPixel(j, i);
                    this.pixels[num3] = pixel.R;
                    num3++;
                    this.pixels[num3] = pixel.G;
                    num3++;
                    this.pixels[num3] = pixel.B;
                    num3++;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="MemoryResult"></param>
        public void OutPut(ref MemoryStream MemoryResult)
        {
            this.started = false;
            this.Memory.WriteByte(59);
            this.Memory.Flush();
            MemoryResult = this.Memory;
            this.Memory.Close();
            this.Memory.Dispose();
            this.transIndex = 0;
            this.Memory = null;
            this.image = null;
            this.pixels = null;
            this.indexedPixels = null;
            this.colorTab = null;
            this.firstFrame = true;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            this.Memory = new MemoryStream();
            this.WriteString("GIF89a");
            this.started = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        public void SetDelay(int ms)
        {
            this.delay = (int)Math.Round((double)((float)ms / 10f));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public void SetDispose(int code)
        {
            if (code >= 0)
            {
                this.dispose = code;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fps"></param>
        public void SetFrameRate(float fps)
        {
            if (fps != 0f)
            {
                this.delay = (int)Math.Round((double)(100f / fps));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="quality"></param>
        public void SetQuality(int quality)
        {
            if (quality < 1)
            {
                quality = 1;
            }
            this.sample = quality;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iter"></param>
        public void SetRepeat(int iter)
        {
            if (iter >= 0)
            {
                this.repeat = iter;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void SetSize(int w, int h)
        {
            if (!this.started || this.firstFrame)
            {
                this.width = w;
                this.height = h;
                if (this.width < 1)
                {
                    this.width = 320;
                }
                if (this.height < 1)
                {
                    this.height = 240;
                }
                this.sizeSet = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public void SetTransparent(Color c)
        {
            this.transparent = c;
        }
        /// <summary>
        /// 
        /// </summary>
        protected void WriteGraphicCtrlExt()
        {
            this.Memory.WriteByte(33);
            this.Memory.WriteByte(249);
            this.Memory.WriteByte(4);
            int num;
            int num2;
            if (this.transparent == Color.Empty)
            {
                num = 0;
                num2 = 0;
            }
            else
            {
                num = 1;
                num2 = 2;
            }
            if (this.dispose >= 0)
            {
                num2 = (this.dispose & 7);
            }
            num2 <<= 2;
            this.Memory.WriteByte(Convert.ToByte(num2 | num));
            this.WriteShort(this.delay);
            this.Memory.WriteByte(Convert.ToByte(this.transIndex));
            this.Memory.WriteByte(0);
        }
        /// <summary>
        /// 
        /// </summary>
        protected void WriteImageDesc()
        {
            this.Memory.WriteByte(44);
            this.WriteShort(0);
            this.WriteShort(0);
            this.WriteShort(this.width);
            this.WriteShort(this.height);
            if (this.firstFrame)
            {
                this.Memory.WriteByte(0);
                return;
            }
            this.Memory.WriteByte(Convert.ToByte(128 | this.palSize));
        }
        /// <summary>
        /// 
        /// </summary>
        protected void WriteLSD()
        {
            this.WriteShort(this.width);
            this.WriteShort(this.height);
            this.Memory.WriteByte(Convert.ToByte(240 | this.palSize));
            this.Memory.WriteByte(0);
            this.Memory.WriteByte(0);
        }
        /// <summary>
        /// 
        /// </summary>
        protected void WriteNetscapeExt()
        {
            this.Memory.WriteByte(33);
            this.Memory.WriteByte(255);
            this.Memory.WriteByte(11);
            this.WriteString("NETSCAPE2.0");
            this.Memory.WriteByte(3);
            this.Memory.WriteByte(1);
            this.WriteShort(this.repeat);
            this.Memory.WriteByte(0);
        }

        /// <summary>
        ///
        /// </summary>
        protected void WritePalette()
        {
            this.Memory.Write(this.colorTab, 0, this.colorTab.Length);
            int num = 768 - this.colorTab.Length;
            for (int i = 0; i < num; i++)
            {
                this.Memory.WriteByte(0);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected void WritePixels()
        {
            new LzwEncoder(this.width, this.height, this.indexedPixels, this.colorDepth).Encode(this.Memory);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        protected void WriteShort(int value)
        {
            this.Memory.WriteByte(Convert.ToByte(value & 255));
            this.Memory.WriteByte(Convert.ToByte(value >> 8 & 255));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        protected void WriteString(string s)
        {
            char[] array = s.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                this.Memory.WriteByte((byte)array[i]);
            }
        }
    }
}