using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mp.Controllers
{
    public class SettingController : ControllerBase
    {
        //
        // GET: /Setting/

        [MPAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [MPAuthorize]
        public ActionResult Avt()
        {
            return View("avt");
        }

        [MPAuthorize]
        public ActionResult Security()
        {
            return View("security");
        }
    }
}
