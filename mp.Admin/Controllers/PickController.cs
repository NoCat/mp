using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.DAL;

namespace mp.Admin.Controllers
{
    [MPAuthorize]
    public class PickController : ControllerBase
    {
        public ActionResult Index()
        {
            var list = Manager.AdminPixivPickUsers.Items.OrderByDescending(p => p.ID).Take(40).ToList();
            ViewBag.List = list;
            return View();
        }

        public ActionResult Create(int pixivUserId,int packageId)
        {
            var userPick = new AdminPixivPickUser { PackageID = packageId, PixivUserID = pixivUserId, LastPickTime = new DateTime(1999,1,1) };
            Manager.AdminPixivPickUsers.Add(userPick);
            return Redirect("~/pick");
        }

        public ActionResult Delete(int id)
        {
            var pick = Manager.AdminPixivPickUsers.Find(id);
            Manager.AdminPixivPickUsers.Remove(pick);
            return Redirect("~/pick");
        }
    }
}
