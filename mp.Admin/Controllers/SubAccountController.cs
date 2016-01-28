using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.Admin.Models;
using mp.DAL;
using Newtonsoft.Json.Linq;

namespace mp.Admin.Controllers
{
    [MPAuthorize]
    public class SubAccountController : ControllerBase
    {
        public ActionResult Index(int id)
        {
            var subAccount = Manager.Users.Find(id);
            var result = Manager.Packages.Items.Where(p => p.UserID == id).OrderByDescending(p => p.ID).Take(40);
            var list = new List<PackageInfo>();

            result.ToList().ForEach(p => list.Add(new PackageInfo(p)));
            ViewBag.List = list;
            ViewBag.SubAccount = new UserInfo(subAccount);
            return View();
        }

        public ActionResult PackageCreate(int id, string title, string description)
        {
            var package = new Package { UserID = id, Title = title, Description = description,LastModify=new DateTime(1990,1,1) };
            Manager.Packages.Add(package);
            return RedirectToAction("index", new { id = id });
        }

        public ActionResult PackageEdit(int id)
        {
            return View();
        }
    }
}
