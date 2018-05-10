一个Gif图像文件，是有几个文件进行合成的，因此处理此类文件的时候，不能像Jpeg或者Bmp文件那样处理。
需要把Gif文件拆分帧的形式，然后对每一帧进行处理，处理完后再合成Gif。
  
AnimatedGifEncoder 类的实现已封装在 Gif.Components.dll 中,
工程文件中要引入Gif.Components.dll, 并引用如下:
using System; 
using System.Drawing; 
using System.Drawing.Imaging; 
using ItemDoc.Framework.Imaging.Gif;
  

	    String[] imageFilePaths = new String[] { "c:\\01.png", "c:\\02.png", "c:\\03.png" };
            String outputFilePath = "c:\\test.gif";
            AnimatedGifEncoder e1 = new AnimatedGifEncoder();
            e1.Start(outputFilePath);
            e1.SetDelay(500);    // 延迟间隔
            e1.SetRepeat(0);  //-1:不循环,0:总是循环 播放   
            for (int i = 0, count = imageFilePaths.Length; i < count; i++)
            {
                e1.AddFrame(Image.FromFile(imageFilePaths[i]));
            }
            e1.Finish();
            /////////////////////////
            string outputPath = "c:\\";
            GifDecoder gifDecoder = new GifDecoder();
            gifDecoder.Read("c:\\test.gif"); 
            for (int i = 0, count = gifDecoder.GetFrameCount(); i < count; i++)
            {
                Image frame = gifDecoder.GetFrame(i); // frame i  
                frame.Save(outputPath + Guid.NewGuid().ToString() + ".png", ImageFormat.Png);
            }

附上三个PNG图片,放到C盘根目录即可.