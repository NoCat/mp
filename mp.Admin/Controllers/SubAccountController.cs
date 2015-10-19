using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mp.Admin.Controllers
{
    public class SubAccountController : Controller
    {
        //
        // GET: /SubAccount/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PackageCreate(int id)
        {
            return View();
        }

        public ActionResult PackageAddImages(int id)
        {
            return View();
        }

        public ActionResult PackageEdit(int id)
        {
            return View();
        }
    }
}
