using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mp.Admin.Controllers
{
    public class AdminUserController : ControllerBase
    {
        //系统用户管理
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(string username,string password)
        {
            return View();
        }
    }
}
