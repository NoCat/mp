using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.DAL;

namespace mp.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var db = new MiaopassContext();
            var a= db.Activitys.Find(null);
            return View("index.pc");
        }

    }
}
