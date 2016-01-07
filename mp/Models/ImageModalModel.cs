using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using mp.DAL;

namespace mp.Models
{
    public class ImageModalModel
    {
        public int ID { get; set; }
        public Uri ImagePath { get; set; }
        public int FileID { get; set; }
        public int PackageID { get; set; }
        public Package[] PackageList { get; set; }
        public string Description { get; set; }
    }
}