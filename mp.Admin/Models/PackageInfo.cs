using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.DAL;

namespace mp.Admin.Models
{
    public class PackageInfo
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public PackageInfo(Package package)
        {
            ID = package.ID;
            Title = package.Title;
        }
    }
}