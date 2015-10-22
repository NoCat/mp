using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mp.Admin.Controllers
{
    [MPAuthorize]
    public class PickController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int pixivUserId,int packageId)
        {
            return View();
        }

        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
