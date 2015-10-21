using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.DAL;

namespace mp.Admin.Models
{
    public class PackageInfo
    {        
        MiaopassContext _db=new MiaopassContext();
        public int ID { get; set; }
        public string Title { get; set; }

        int _imageCount = -1;
        public int ImageCount
        {
            get
            {
                if (_imageCount == -1)
                    _imageCount = _db.Images.Where(i => i.PackageID == ID).Count();
                return _imageCount;
            }
        }

        public PackageInfo(Package package)
        {
            ID = package.ID;
            Title = package.Title;
        }
    }
}