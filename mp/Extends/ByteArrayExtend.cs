using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

static public class ByteArrayExtend
{
    static public string ToHexString(this byte[] array)
    {
        StringBuilder strB = new StringBuilder();
        foreach (var b in array)
        {
            strB.AppendFormat("x2", b);
        }
        return strB.ToString();
    }
}