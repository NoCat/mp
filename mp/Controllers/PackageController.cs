using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mp.Controllers
{
    public class PackageController : ControllerBase
    {
        public ActionResult Index(int packageId = 0)
        {
            if (packageId == 0)
                return Redirect("/");

            var package = DB.Packages.Find(packageId);
            if (package == null)
                return Redirect("/");

            ViewBag.packageInfo = new BLL.PackageInfo(package);
            return View();
        }

        public ActionResult Images(int packageId = 0, int max = 0)
        {
            if (max == 0)
                max = int.MaxValue;

            var list = new List<BLL.ImageInfo>();
            DB.Images.Where(i => i.PackageID == packageId && i.ID < max).OrderByDescending(i=>i.ID).Take(20).ToList().ForEach(i =>
            {
                list.Add(new BLL.ImageInfo(i));
            });

            return PartialView("ImageList.pc",list);
        }
    }
}
