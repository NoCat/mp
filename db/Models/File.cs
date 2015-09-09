using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace db.Models
{
    public class File
    {
        public int ID { get; set; }
        public int Width { get; set; }
        public int Heigth { get; set; }
        [MaxLength(32)]
        public string MD5 { get; set; }
    }
}
