using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using mp.DAL;
using mp.Extends;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Security.Cryptography;

static public class Tools
{
    static public string ImageHost = System.Configuration.ConfigurationManager.AppSettings["ImageHost"];
    public static byte[] Md5(string str)
    {
        var buffer = Encoding.UTF8.GetBytes(str);
        return MD5.Create().ComputeHash(buffer);
    }
    public static string FileMd5(string path)
    {
        using (var fs = System.IO.File.OpenRead(path))
        {
            return FileMd5(fs);
        }
    }
    public static string FileMd5(Stream s)
    {
        s.Position = 0;
        return MD5.Create().ComputeHash(s).ToHexString();
    }
}