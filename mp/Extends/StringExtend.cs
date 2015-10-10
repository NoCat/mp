﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

static public class StringExtend
{
    static  public byte[] ToBytes(this string str)
    {
        return System.Text.Encoding.UTF8.GetBytes(str);
    }

    static public string MD5(this string str)
    {
        return str.ToBytes().MD5();
    }

    static public UInt32 CRC32(this string str)
    {
        return Crc32C.Crc32CAlgorithm.Compute(str.ToBytes());
    }
}