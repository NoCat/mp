using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mp.Admin.Controllers
{
    public class SubAccountManagerController : Controller
    {
        //
        // GET: /SubAccountManager/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateNew(string email,string password,string name)
        {
            return View();
        }

        public ActionResult CreateExist(int id)
        {
            return View();
        }
        
        public ActionResult Delete(int id)
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}
