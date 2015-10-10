using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Security.Cryptography;


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

    public static string MD5(this Stream stream)
    {
        stream.Position=0;
        var md5 = System.Security.Cryptography.MD5.Create();
        return  md5.ComputeHash(stream).ToHexString();
    }
}