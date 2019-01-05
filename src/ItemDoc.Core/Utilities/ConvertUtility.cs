using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemDoc.Core.Utilities
{
    public static class ConvertUtility
    {
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return buffer;
        }
    }
}
