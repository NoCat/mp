using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mp.Controllers
{
    public class PackageController : ControllerBase
    {
        public ActionResult Index(int id = 0)
        {
            if (id == 0)
                return Redirect("/");

            var package = DB.Packages.Find(id);
            if (package == null)
                return Redirect("/");

            ViewBag.packageInfo = new BLL.PackageInfo(package);
            return View();
        }

        public ActionResult Page(int package = 0, int max = 0)
        {
            if (max == 0)
                max = int.MaxValue;

            var list = new List<BLL.ImageInfo>();
            DB.Images.Where(i => i.PackageID == package && i.ID < max).Take(20).ToList().ForEach(i =>
            {
                list.Add(new BLL.ImageInfo(i));
            });

            return PartialView("ImageList.pc",list);
        }
    }
}
