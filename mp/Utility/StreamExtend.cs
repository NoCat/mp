using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace mp.Utility
{
    static public class StreamExtend
    {
        public static void Write(this Stream stream, Stream input)
        {
            int bufferSize = 1024 * 4;
            int a = bufferSize;
            byte[] buffer = new byte[bufferSize];
            while (a == bufferSize)
            {
                a = input.Read(buffer, 0, bufferSize);
                stream.Write(buffer, 0, a);
            }
        }
    }
}