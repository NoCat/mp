using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.DAL;

namespace mp.BLL
{
    public class PackageManager
    {
        MiaopassContext _db = null;

        public PackageManager(MiaopassContext db)
        {
            _db = db;
        }
    }
}