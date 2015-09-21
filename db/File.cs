using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace mp.DAL
{
    public class File
    {
        public int ID { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        [MaxLength(32)]
        public string MD5 { get; set; }
    }
}
