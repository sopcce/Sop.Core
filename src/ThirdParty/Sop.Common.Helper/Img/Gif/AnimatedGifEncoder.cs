using System;
using System.DrawingCore;
using System.IO;
using Color = System.DrawingCore.Color;

namespace Sop.Common.Helper.Img.Gif
{
    public class AnimatedGifEncoder
    {
        /// <summary>
        /// image size
        /// </summary>
        protected int Width;
        protected int height;
        protected Color transparent = Color.Empty; // transparent color if given
        protected int transIndex; // transparent index in color table
        protected int repeat = -1; // no repeat
        protected int delay = 0; // frame delay (hundredths)
        protected bool started = false; // ready to output frames
        protected BinaryWriter bw;
        /// <summary>
        /// protected FileStream fs;
        /// </summary>
        public Stream Fs;

        protected Image image; // current frame
        protected byte[] pixels; // BGR byte array from frame
        protected byte[] indexedPixels; // converted frame indexed to palette
        protected int colorDepth; // number of bit planes
        protected byte[] colorTab; // RGB palette
        protected bool[] usedEntry = new bool[256]; // active palette entries
        protected int palSize = 7; // color table size (bits-1)
        protected int dispose = -1; // disposal code (-1 = use default)
        protected bool closeStream = false; // close stream when finished
        protected bool firstFrame = true;
        /// <summary>
        ///  if false, get size from first frame
        /// </summary>
        protected bool sizeSet = false;
        /// <summary>
        /// default sample interval for quantizer
        /// </summary>
        protected int sample = 10;

        /**
             * Sets the delay time between each frame, or changes it
             * for subsequent frames (applies to last frame added).
             *
             * @param ms int delay time in milliseconds
             */
        public void SetDelay(int ms)
        {
            delay = (int)Math.Round(ms / 10.0f);
        }

        /// <summary>
        /// 为最后添加的帧和任何后续帧设置GIF帧处理代码。
        /// 如果没有设置透明颜色，则默认为0，否则为2。
        /// </summary>
        /// <param name="code"></param>
        public void SetDispose(int code)
        {
            if (code >= 0)
            {
                dispose = code;
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
                repeat = iter;
            }
        }

        /**
             * Sets the transparent color for the last added frame
             * and any subsequent frames.
             * Since all colors are subject to modification
             * in the quantization process, the color in the final
             * palette for each frame closest to the given color
             * becomes the transparent color for that frame.
             * May be set to null to indicate no transparent color.
             *
             * @param c Color to be treated as transparent on display.
             */
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

        public void OutPut(ref Stream MemoryResult)
        {
            MemoryResult = this.Fs;
        }

        public Stream OutPut()
        {
            return Fs;

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
            Width = w;
            height = h;
            if (Width < 1) Width = 320;
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
            Fs = os;
            try
            {
                WriteString("GIF89a"); // header
            }
            catch (IOException e)
            {
                ok = false;
            }
            return started = ok;
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
                //			bw = new BinaryWriter( new FileStream( file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None ) );
                Fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                ok = Start(Fs);
                closeStream = true;
            }
            catch (IOException e)
            {
                ok = false;
            }
            return started = ok;
        }
        public void Start()
        {
            this.Fs = new MemoryStream();
            this.WriteString("GIF89a");
            this.started = true;
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
            if ((w != Width)
              || (h != height)
              )
            {
                // create new image with right size/format
                Image temp =
                  new Bitmap(Width, height);
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

        /**
             * Writes Graphic Control Extension
             */
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

            WriteShort(delay); // delay x 1/100 sec
            Fs.WriteByte(Convert.ToByte(transIndex)); // transparent color index
            Fs.WriteByte(0); // block terminator
        }

        /**
             * Writes Image Descriptor
             */
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

        /**
             * Writes Logical Screen Descriptor
             */
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

        /**
             * Writes Netscape application extension to define
             * repeat count.
             */
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

        /**
             * Writes color table
             */
        protected void WritePalette()
        {
            Fs.Write(colorTab, 0, colorTab.Length);
            int n = (3 * 256) - colorTab.Length;
            for (int i = 0; i < n; i++)
            {
                Fs.WriteByte(0);
            }
        }

        /**
             * Encodes and writes pixel data
             */
        protected void WritePixels()
        {
            LzwEncoder encoder =
              new LzwEncoder(Width, height, indexedPixels, colorDepth);
            encoder.Encode(Fs);
        }

        /**
             *    Write 16-bit value to output stream, LSB first
             */
        protected void WriteShort(int value)
        {
            Fs.WriteByte(Convert.ToByte(value & 0xff));
            Fs.WriteByte(Convert.ToByte((value >> 8) & 0xff));
        }

        /**
             * Writes string to output stream
             */
        protected void WriteString(String s)
        {
            char[] chars = s.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                Fs.WriteByte((byte)chars[i]);
            }
        }
    }

}