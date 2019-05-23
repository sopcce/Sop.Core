using System;
using System.DrawingCore;
using System.IO;
using Color = System.DrawingCore.Color;

namespace Sop.Common.Helper.Img.Gif
{
    /// <summary>
    /// Animated Gif  编码器
    /// </summary>
    public class AnimatedGifEncoder
    {
        #region protected
        /// <summary>
        /// Width
        /// </summary>
        protected static int Width { get; set; }
        /// <summary>
        /// height
        /// </summary>
        protected int height { get; set; }
        /// <summary>
        ///  transparent color if given
        /// </summary>
        protected Color transparent { get; set; } = Color.Empty;
        /// <summary>
        ///  transparent index in color table
        /// </summary>
        protected int transIndex { get; set; }
        /// <summary>
        ///  no repeat
        /// </summary>
        protected int repeat { get; set; } = -1;
        /// <summary>
        /// frame delay (hundredths)
        /// </summary>
        protected int Delay { get; set; } = 0;
        /// <summary>
        /// ready to output frames
        /// </summary>
        protected bool Started { get; set; } = false;
        /// <summary>
        ///   Binary Writer
        /// </summary>
        protected BinaryWriter bw { get; set; }
        /// <summary>
        /// protected FileStream fs;
        /// </summary>
        public Stream Fs { get; set; }
        /// <summary>
        /// current frame
        /// </summary>
        protected Image image { get; set; }

        /// <summary>
        /// BGR byte array from frame
        /// </summary>
        protected byte[] pixels { get; set; }
        /// <summary>
        /// converted frame indexed to palette
        /// </summary>
        protected byte[] indexedPixels { get; set; }

        /// <summary>
        /// number of bit planes
        /// </summary>
        protected int colorDepth { get; set; }
        /// <summary>
        /// RGB palette
        /// </summary>
        protected byte[] colorTab { get; set; }
        /// <summary>
        /// active palette entries
        /// </summary>
        protected bool[] usedEntry { get; set; } = new bool[256];

        /// <summary>
        /// color table size (bits-1)
        /// </summary>
        protected int palSize { get; set; } = 7;

        /// <summary>
        /// disposal code (-1 = use default)
        /// </summary>
        protected int dispose { get; set; } = -1;
        /// <summary>
        /// close stream when finished
        /// </summary>
        protected bool closeStream { get; set; } = false;
        /// <summary>
        /// first Frame
        /// </summary>
        protected bool firstFrame { get; set; } = true;
        /// <summary>
        ///  if false, get size from first frame
        /// </summary>
        protected bool sizeSet { get; set; } = false;
        /// <summary>
        /// default sample interval for quantizer
        /// </summary>
        protected int sample { get; set; } = 10;
        #endregion


        /// <summary>
        /// Sets the delay time between each frame, or changes it for subsequent frames (applies to last frame added).
        /// </summary>
        /// <param name="ms">ms int delay time in milliseconds</param>
        public void SetDelay(int ms)
        {
            Delay = (int)Math.Round(ms / 10.0f);
        }
        /// <summary>
        /// Sets frame rate in frames per second.  Equivalent to setDelay
        /// </summary>
        /// <code>setDelay(1000/fps)</code>.
        /// <param name="fps">fps float frame rate (frames per second)</param>
        public void SetFrameRate(float fps)
        {
            if (fps != 0f)
            {
                Delay = (int)Math.Round(100f / fps);
            }
        }
        /// <summary>
        /// 为最后添加的帧和任何后续帧设置GIF帧处理代码。 
        /// </summary>
        /// <param name="code">如果没有设置透明颜色，则默认为0，否则为2。</param>
        public void SetDispose(int code)
        {
            if (code >= 0)
            {
                dispose = code;
            }
        }

        /// <summary>
        /// 设置GIF帧应该被播放的次数
        /// </summary>
        /// <param name="iter">默认值是1；0意味着无限期播放。必须在添加第一个图像之前调用</param>
        public void SetRepeat(int iter)
        {
            if (iter >= 0)
            {
                repeat = iter;
            }
        }

        /// <summary>
        /// Sets the transparent color for the last added frame and any subsequent frames.
        ///  * Since all colors are subject to modification
        ///  * in the quantization process, the color in the final
        ///  * palette for each frame closest to the given color
        ///  * becomes the transparent color for that frame.
        ///  * May be set to null to indicate no transparent color.
        /// </summary>
        /// <param name="c">c Color to be treated as transparent on display.</param>
        public void SetTransparent(Color c)
        {
            transparent = c;
        }

        /// <summary>
        /// Sets quality of color quantization (conversion of images
        /// to the maximum 256 colors allowed by the GIF specification).
        /// Lower values (minimum = 1) produce better colors, but slow
        /// processing significantly.  10 is the default, and produces
        /// good color mapping at reasonable speeds.  Values greater
        /// than 20 do not yield significant improvements in speed.
        /// </summary>
        /// <param name="quality">quality int greater than 0.</param>
        public void SetQuality(int quality)
        {
            if (quality < 1) quality = 1;
            sample = quality;
        }

        /// <summary>
        /// Sets the GIF frame size.  The default size is the
        /// size of the first frame added if this method is not invoked. 
        /// </summary>
        /// <param name="w">w int frame width.</param>
        /// <param name="h">h int frame width.</param>
        public void SetSize(int w, int h)
        {
            if (Started && !firstFrame) return;
            Width = w;
            height = h;
            if (Width < 1) Width = 320;
            if (height < 1) height = 240;
            sizeSet = true;
        }




        /// <summary>
        /// Adds next GIF frame
        ///   * The frame is not written immediately, but is
        ///   * actually deferred until the next frame is received so that timing
        ///   * data can be inserted.  Invoking &lt;code&gt;finish()&lt;/code&gt; flushes all
        ///   * frames.  If &lt;code&gt;setSize&lt;/code&gt; was not invoked, the size of the
        ///   * first image is used for all subsequent frames.
        ///   *
        /// </summary>
        /// <param name="im">im BufferedImage containing frame to write.</param>
        /// <returns> true if successful.</returns>
        public bool AddFrame(Image im)
        {
            if ((im == null) || !Started)
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
                GetImagePixels(); // 
                AnalyzePixels(); // 
                if (firstFrame)
                {
                    WriteLsd(); // logical screen descriptior
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
            catch (IOException e)
            {
                ok = false;
            }

            return ok;
        }
       
        /// <summary>
        /// Flushes any pending data and closes output file. If writing to an OutputStream, the stream is not
        /// </summary>
        /// <returns>true is success</returns>
        public bool Finish()
        {
            if (!Started) return false;
            bool ok = true;
            Started = false;
            try
            {
                Fs.WriteByte(0x3b); // gif trailer
                Fs.Flush();
                if (closeStream)
                {
                    Fs.Close();
                }
            }
            catch (IOException e)
            {
                ok = false;
            }

            // reset for subsequent use
            transIndex = 0;
            Fs = null;
            image = null;
            pixels = null;
            indexedPixels = null;
            colorTab = null;
            closeStream = false;
            firstFrame = true;

            return ok;
        }

      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MemoryResult"></param>
        public void OutPut(ref Stream MemoryResult)
        {
            MemoryResult = this.Fs;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Stream OutPut()
        {
            return Fs;

        }
        
        /// <summary>
        /// Initiates GIF file creation on the given stream.  The stream is not closed automatically.
        /// </summary>
        /// <param name="os">os OutputStream on which GIF images are written.</param>
        /// <returns>false if initial write failed.</returns>
        public bool Start(Stream os)
        {
            if (os == null) return false;
            bool ok = true;
            closeStream = false;
            Fs = os;
            try
            {
                WriteString("GIF89a"); // header
            }
            catch (IOException e)
            {
                ok = false;
            }
            return Started = ok;
        }

        /// <summary>
        /// Initiates writing of a GIF file with the specified name.
        /// </summary>
        /// <param name="path"> file String containing output file name.</param>
        /// <returns>false if open or initial write failed.</returns>
        public bool Start(string path)
        {
            bool ok = true;
            try
            {
                //			bw = new BinaryWriter( new FileStream( file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None ) );
                Fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                ok = Start(Fs);
                closeStream = true;
            }
            catch (IOException e)
            {
                ok = false;
            }
            return Started = ok;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            this.Fs = new MemoryStream();
            this.WriteString("GIF89a");
            this.Started = true;
        }


        #region protected Write  
        /// <summary>
        /// Analyzes image colors and creates color map.
        /// </summary>
        protected void AnalyzePixels()
        {
            int len = pixels.Length;
            int nPix = len / 3;
            indexedPixels = new byte[nPix];
            NeuQuant nq = new NeuQuant(pixels, len, sample);
            // initialize quantizer
            colorTab = nq.Process(); // create reduced palette
            // convert map from BGR to RGB
            // for (int i = 0; i < colorTab.Length; i += 3) 
            // {
            // 	byte temp = colorTab[i];
            // 	colorTab[i] = colorTab[i + 2];
            // 	colorTab[i + 2] = temp;
            // 	usedEntry[i / 3] = false;
            // }
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

        /// <summary>
        ///  Returns index of palette color closest to c
        /// 返回最接近C的调色板颜色索引
        /// </summary>
        /// <param name="c">color</param>
        /// <returns>color  index</returns>
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

        /// <summary>
        /// Extracts image pixels into byte array "pixels"  convert to correct format if necessary
        /// </summary>
        protected void GetImagePixels()
        {
            int w = image.Width;
            int h = image.Height;
            //		int type = image.GetType().;
            if (w != Width || h != height)
            {
                // create new image with right size/format
                Image temp = new Bitmap(Width, height);
                Graphics g = Graphics.FromImage(temp);
                g.DrawImage(image, 0, 0);
                image = temp;
                g.Dispose();
            }
            // ToDo:improve performance: use unsafe code
            try
            {
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
            }
            catch  
            {
            }
            //pixels = ((DataBufferByte)image.getRaster().getDataBuffer()).getData();
        }


        /// <summary>
        /// Writes Graphic Control Extension
        /// </summary>
        protected void WriteGraphicCtrlExt()
        {
            Fs.WriteByte(0x21); // extension introducer
            Fs.WriteByte(0xf9); // GCE label
            Fs.WriteByte(4); // data block size
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
            Fs.WriteByte(Convert.ToByte(0 | // 1:3 reserved
                                        disp | // 4:6 disposal
                                        0 | // 7   user input - 0 = none
                                        transp)); // 8   transparency flag

            WriteShort(Delay); // delay x 1/100 sec
            Fs.WriteByte(Convert.ToByte(transIndex)); // transparent color index
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
            WriteShort(height);
            // packed fields
            if (firstFrame)
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
                                            palSize)); // 6-8 size of color table
            }
        }

        /// <summary>
        /// Writes Logical Screen Descriptor
        /// </summary>
        protected void WriteLsd()
        {
            // logical screen size
            WriteShort(Width);
            WriteShort(height);
            // packed fields
            Fs.WriteByte(Convert.ToByte(0x80 | // 1   : global color table flag = 1 (gct used)
                                        0x70 | // 2-4 : color resolution = 7
                                        0x00 | // 5   : gct sort flag = 0
                                        palSize)); // 6-8 : gct size

            Fs.WriteByte(0); // background color index
            Fs.WriteByte(0); // pixel aspect ratio - assume 1:1
        }

        /// <summary>
        /// Writes Netscape application extension to define repeat count.
        /// </summary>
        protected void WriteNetscapeExt()
        {
            Fs.WriteByte(0x21); // extension introducer
            Fs.WriteByte(0xff); // app extension label
            Fs.WriteByte(11); // block size
            WriteString("NETSCAPE" + "2.0"); // app id + auth code
            Fs.WriteByte(3); // sub-block size
            Fs.WriteByte(1); // loop sub-block id
            WriteShort(repeat); // loop count (extra iterations, 0=repeat forever)
            Fs.WriteByte(0); // block terminator
        }

        /// <summary>
        /// Writes color table
        /// </summary>
        protected void WritePalette()
        {
            Fs.Write(colorTab, 0, colorTab.Length);
            int n = (3 * 256) - colorTab.Length;
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
                new LzwEncoder(Width, height, indexedPixels, colorDepth);
            encoder.Encode(Fs);
        }

        /// <summary>
        ///  Write 16-bit value to output stream, LSB first
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
        /// <param name="s">s is string</param>
        protected void WriteString(string s)
        {
            char[] chars = s.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                Fs.WriteByte((byte)chars[i]);
            }
        } 
        #endregion
    }

}