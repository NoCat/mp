using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.DAL;
using System.Web.Mvc;

namespace mp.Controllers
{
    public class ControllerBase : Controller
    {
        MiaopassContext _db = new MiaopassContext();
        public MiaopassContext DB
        {
            get
            {
                return _db;
            }
        }
    }
}