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
        public PackageListItem[] PackageList { get; set; }
        public string Description { get; set; }

        public class PackageListItem
        {
            public int ID { get; set; }
            public string Title { get; set; }
            public bool InPackage { get; set; }
        }
    }
}