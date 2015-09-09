using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Nito.KitchenSink.CRC;

namespace db.Models
{
    public class Download
    {
        public int ID { get; set; }
        public string From { get; set; }
        public string Source { get; set; }
        public uint SourceHash { get; set; }
        public DownloadStates State { get; set; }
        public int FileID { get; set; }
        virtual public File File { get; set; }

        public static uint GetHash(string str)
        {
            str = str.ToLower();
            var crc32 = new CRC32();
            var result=crc32.ComputeHash( UTF8Encoding.UTF8.GetBytes(str));
            return BitConverter.ToUInt32(result,0);
        }
    }
}
