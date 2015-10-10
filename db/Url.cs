using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mp.DAL
{
    public class Url
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public uint CRC32 { get; set; }
    }
}
