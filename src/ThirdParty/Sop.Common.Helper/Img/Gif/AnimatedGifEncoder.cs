using System;
using System.DrawingCore;
using System.IO;
using Color = System.DrawingCore.Color;

namespace Sop.Common.Helper.Img.Gif
{
    /// <summary>
    /// Animated Gif Encoder
    /// </summary>
    public class AnimatedGifEncoder
    {
        #region { get; set; }

        /// <summary>
        /// Transparent
        /// </summary>
        public Color Transparent { get; set; } = Color.Empty;
        /// <summary>
        /// 
        /// </summary>
        public int Width
        {
            get
            {
                if (Width < 1)
                {
                    Width = 320;
                }
                return Width;
            }
            set
            {
                if (Width < 1)
                {
                    Width = 320;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Height
        {
            get
            {
                if (Width < 1)
                {
                    Width = 240;
                }
                return Width;
            }
            set
            {
                if (Width < 1)
                {
                    Width = 240;
                }
            }
        }
        /// <summary>
        /// transparent index in color table 颜色表中的透明索引
        /// </summary>
        public int TransIndex { get; set; }

        /// <summary>
        /// no repeat
        /// </summary>
        public int Repeat { get; set; } = 1;


        /// <summary>
        ///设置每帧之间的延迟时间  delay time in milliseconds
        /// </summary>
        public int Delay {
            get
            {
                return Delay;
            }
            set
            {

                Delay = (int)Math.Round(Delay / 10.0f);
            }
        }
        /// <summary>
        /// ready to output frames
        /// </summary>
        public bool Started { get; private set; } = true;

        /// <summary>
        ///  Binary Writer
        /// </summary>
        protected BinaryWriter bw { get; set; }
        /// <summary>
        /// protected FileStream fs;
        /// </summary>
        protected Stream Fs { get; set; }
        /// <summary>
        ///  current frame
        /// </summary>
        protected Image Image { get; set; }
        /// <summary>
        /// BGR byte array from frame
        /// </summary>
        protected byte[] Pixels { get; set; }
        /// <summary>
        ///  converted frame indexed to palette
        /// </summary>
        protected byte[] IndexedPixels { get; set; }
        /// <summary>
        /// number of bit planes
        /// </summary>
        protected int ColorDepth { get; set; }
        /// <summary>
        /// // RGB palette
        /// </summary>
        protected byte[] ColorTab { get; set; }
        /// <summary>
        /// active palette entries
        /// </summary>
        protected bool[] UsedEntry { get; set; } = new bool[256];
        /// <summary>
        ///  color table size (bits-1)
        /// </summary>
        protected int PalSize { get; set; } = 7; 
        /// <summary>
        ///  // disposal code (-1 = use default)
        /// </summary>
        protected int Dispose { get; set; } = -1;
        /// <summary>
        ///  // close stream when finished
        /// </summary>
        protected bool CloseStream { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        protected bool FirstFrame { get; set; } = true;
        /// <summary>
        ///  if false, get size from first frame
        /// </summary>
        protected bool SizeSet { get; set; } = false;
        /// <summary>
        /// default sample interval for quantizer
        /// </summary>
        protected int Sample { get; set; } = 10;
        #endregion

 

        /// <summary>
        /// 为最后添加的帧和任何后续帧设置GIF帧处理代码。
        /// 
        /// </summary>
        /// <param name="code">如果没有设置透明颜色，则默认为0，否则为2。</param>
        public void SetDispose(int code)
        {
            if (code >= 0)
            {
                Dispose = code;
            }
        }

        /// <summary>
        /// 设置GIF帧应该被播放的次数。默认值是1；
        /// 0意味着无限期播放。必须在添加第一个图像之前调用。
        /// </summary>
        /// <param name="iter">设置播放的次数</param>
        public void SetRepeat(int iter)
        {
            if (iter >= 0)
            {
                Repeat = iter;
            }
        }





        /// <summary>
        ///    * Sets frame rate in frames per second.  Equivalent to
        ///          * &lt;code&gt;setDelay(1000/fps)&lt;/code&gt;.
        /// </summary>
        /// <param name="fps"> fps float frame rate (frames per second)</param>
        public void SetFrameRate(float fps)
        {
            if (fps != 0f)
            {
                Delay = (int)Math.Round(100f / fps);
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
            Sample = quality;
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
            if (Started && !FirstFrame) return;
            Width = w;
            Height = h;
            if (Width < 1) Width = 320;
            if (Height < 1) Height = 240;
            SizeSet = true;
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
            if ((im == null) || !Started)
            {
                return false;
            }
            bool ok = true;
            try
            {
                if (!SizeSet)
                {
                    // use first frame's size
                    SetSize(im.Width, im.Height);
                }
                Image = im;
                GetImagePixels(); // convert to correct format if necessary
                AnalyzePixels(); // build color table & map pixels
                if (FirstFrame)
                {
                    WriteLsd(); // logical screen descriptior
                    WritePalette(); // global color table
                    if (Repeat >= 0)
                    {
                        // use NS app extension to indicate reps
                        WriteNetscapeExt();
                    }
                }
                WriteGraphicCtrlExt(); // write graphic control extension
                WriteImageDesc(); // image descriptor
                if (!FirstFrame)
                {
                    WritePalette(); // local color table
                }
                WritePixels(); // encode and write pixel data
                FirstFrame = false;
            }
            catch (IOException e)
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
            if (!Started) return false;
            bool ok = true;
            Started = false;
            try
            {
                Fs.WriteByte(0x3b); // gif trailer
                Fs.Flush();
                if (CloseStream)
                {
                    Fs.Close();
                }
            }
            catch (IOException e)
            {
                ok = false;
            }

            // reset for subsequent use
            TransIndex = 0;
            Fs = null;
            Image = null;
            Pixels = null;
            IndexedPixels = null;
            ColorTab = null;
            CloseStream = false;
            FirstFrame = true;

            return ok;
        }

        public void OutPut(ref Stream MemoryResult)
        {
            MemoryResult = this.Fs;
        }

        public Stream OutPut()
        {
            return Fs;

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
            if (os == null)
                return false;
            bool ok = true;
            CloseStream = false;
            Fs = os;
            try
            {

                WriteString("GIF89a");
            }
            catch (IOException exception)
            {
                ok = false;
            }
            return Started = ok;
        }

        /**
             * Initiates writing of a GIF file with the specified name.
             *
             * @param file String containing output file name.
             * @return false if open or initial write failed.
             */
        public bool Start(string file)
        {
            bool ok = true;
            try
            {
                //bw = new BinaryWriter( new FileStream( file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None ) );
                Fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                ok = Start(Fs);
                CloseStream = true;
            }
            catch (IOException e)
            {
                ok = false;
            }
            return Started = ok;
        }
        public void Start()
        {
            this.Fs = new MemoryStream();
            this.WriteString("GIF89a");
            this.Started = true;
        }
        /**
             * Analyzes image colors and creates color map.
             */
        protected void AnalyzePixels()
        {
            int len = Pixels.Length;
            int nPix = len / 3;
            IndexedPixels = new byte[nPix];
            NeuQuant nq = new NeuQuant(Pixels, len, Sample);
            // initialize quantizer
            ColorTab = nq.Process(); // create reduced palette
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
                  nq.Map(Pixels[k++] & 0xff,
                  Pixels[k++] & 0xff,
                  Pixels[k++] & 0xff);
                UsedEntry[index] = true;
                IndexedPixels[i] = (byte)index;
            }
            Pixels = null;
            ColorDepth = 8;
            PalSize = 7;
            // get closest match to transparent color if specified
            if (Transparent != Color.Empty)
            {
                TransIndex = FindClosest(Transparent);
            }
        }

        /**
             * Returns index of palette color closest to c
             *
             */
        protected int FindClosest(Color c)
        {
            if (ColorTab == null) return -1;
            int r = c.R;
            int g = c.G;
            int b = c.B;
            int minpos = 0;
            int dmin = 256 * 256 * 256;
            int len = ColorTab.Length;
            for (int i = 0; i < len;)
            {
                int dr = r - (ColorTab[i++] & 0xff);
                int dg = g - (ColorTab[i++] & 0xff);
                int db = b - (ColorTab[i] & 0xff);
                int d = dr * dr + dg * dg + db * db;
                int index = i / 3;
                if (UsedEntry[index] && (d < dmin))
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
            int w = Image.Width;
            int h = Image.Height;
            //		int type = image.GetType().;
            if ((w != Width)
              || (h != Height)
              )
            {
                // create new image with right size/format
                Image temp =
                  new Bitmap(Width, Height);
                Graphics g = Graphics.FromImage(temp);
                g.DrawImage(Image, 0, 0);
                Image = temp;
                g.Dispose();
            }
            /*
                      ToDo:
                      improve performance: use unsafe code 
                  */
            Pixels = new Byte[3 * Image.Width * Image.Height];
            int count = 0;
            Bitmap tempBitmap = new Bitmap(Image);
            for (int th = 0; th < Image.Height; th++)
            {
                for (int tw = 0; tw < Image.Width; tw++)
                {
                    Color color = tempBitmap.GetPixel(tw, th);
                    Pixels[count] = color.R;
                    count++;
                    Pixels[count] = color.G;
                    count++;
                    Pixels[count] = color.B;
                    count++;
                }
            }

            //		pixels = ((DataBufferByte) image.getRaster().getDataBuffer()).getData();
        }

        /**
             * Writes Graphic Control Extension
             */
        protected void WriteGraphicCtrlExt()
        {
            Fs.WriteByte(0x21); // extension introducer
            Fs.WriteByte(0xf9); // GCE label
            Fs.WriteByte(4); // data block size
            int transp, disp;
            if (Transparent == Color.Empty)
            {
                transp = 0;
                disp = 0; // dispose = no action
            }
            else
            {
                transp = 1;
                disp = 2; // force clear if using transparent color
            }
            if (Dispose >= 0)
            {
                disp = Dispose & 7; // user override
            }
            disp <<= 2;

            // packed fields
            Fs.WriteByte(Convert.ToByte(0 | // 1:3 reserved
              disp | // 4:6 disposal
              0 | // 7   user input - 0 = none
              transp)); // 8   transparency flag

            WriteShort(Delay); // delay x 1/100 sec
            Fs.WriteByte(Convert.ToByte(TransIndex)); // transparent color index
            Fs.WriteByte(0); // block terminator
        }

        /// <summary>
        /// Writes Image Descriptor
        /// </summary>
        protected void WriteImageDesc()
        {
            Fs.WriteByte(0x2c); // image separator
            WriteShort(0); // image position x,y = 0,0
            WriteShort(0);
            WriteShort(Width); // image size
            WriteShort(Height);
            // packed fields
            if (FirstFrame)
            {
                // no LCT  - GCT is used for first (or only) frame
                Fs.WriteByte(0);
            }
            else
            {
                // specify normal LCT
                Fs.WriteByte(Convert.ToByte(0x80 | // 1 local color table  1=yes
                  0 | // 2 interlace - 0=no
                  0 | // 3 sorted - 0=no
                  0 | // 4-5 reserved
                  PalSize)); // 6-8 size of color table
            }
        }

        /// <summary>
        /// Writes Logical Screen Descriptor
        /// </summary>
        protected void WriteLsd()
        {
            // logical screen size
            WriteShort(Width);
            WriteShort(Height);
            // packed fields
            Fs.WriteByte(Convert.ToByte(0x80 | // 1   : global color table flag = 1 (gct used)
              0x70 | // 2-4 : color resolution = 7
              0x00 | // 5   : gct sort flag = 0
              PalSize)); // 6-8 : gct size

            Fs.WriteByte(0); // background color index
            Fs.WriteByte(0); // pixel aspect ratio - assume 1:1
        }

        /// <summary>
        ///  Writes Netscape application extension to define repeat count.
        /// </summary>
        protected void WriteNetscapeExt()
        {
            Fs.WriteByte(0x21); // extension introducer
            Fs.WriteByte(0xff); // app extension label
            Fs.WriteByte(11); // block size
            WriteString("NETSCAPE" + "2.0"); // app id + auth code
            Fs.WriteByte(3); // sub-block size
            Fs.WriteByte(1); // loop sub-block id
            WriteShort(Repeat); // loop count (extra iterations, 0=repeat forever)
            Fs.WriteByte(0); // block terminator
        }

        /// <summary>
        /// Writes color table
        /// </summary>
        protected void WritePalette()
        {
            Fs.Write(ColorTab, 0, ColorTab.Length);
            int n = (3 * 256) - ColorTab.Length;
            for (int i = 0; i < n; i++)
            {
                Fs.WriteByte(0);
            }
        }

        /// <summary>
        /// Encodes and writes pixel data
        /// </summary>
        protected void WritePixels()
        {
            LzwEncoder encoder =
              new LzwEncoder(Width, Height, IndexedPixels, ColorDepth);
            encoder.Encode(Fs);
        }

        /// <summary>
        /// Write 16-bit value to output stream, LSB first
        /// </summary>
        /// <param name="value"></param>
        protected void WriteShort(int value)
        {
            Fs.WriteByte(Convert.ToByte(value & 0xff));
            Fs.WriteByte(Convert.ToByte((value >> 8) & 0xff));
        }

        /// <summary>
        /// Writes string to output stream
        /// </summary>
        /// <param name="s"></param>
        protected void WriteString(string s)
        {
            char[] chars = s.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                Fs.WriteByte((byte)chars[i]);
            }
        }
    }

}