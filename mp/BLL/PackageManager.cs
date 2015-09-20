using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.DAL;

namespace mp.BLL
{
    public class PackageManager
    {

        public class PackageInfo
        {
            Package _package=null;
            public PackageInfo(Package package)
            {
                _package = package;
            }
        }
    }
}