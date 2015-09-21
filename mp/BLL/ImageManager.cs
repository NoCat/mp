using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.DAL;

namespace mp.BLL
{
    public class ImageManager
    {
        MiaopassContext _db = null;
        public ImageManager(MiaopassContext db)
        {
            _db = db;
        }
    }
}