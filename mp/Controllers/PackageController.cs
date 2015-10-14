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

            var package = Service.Packages.Items.Where(p=>p.ID==packageId).FirstOrDefault();
            if (package == null)
                return Redirect("/");

            ViewBag.packageInfo = PackageInfo(package);
            return View();
        }

        public ActionResult Images(int packageId = 0, int max = 0,string thumb="fw236")
        {
            if (max == 0)
                max = int.MaxValue;

            var list = new List<BLL.ImageInfo>();
            Service.Images.Items.Where(i => i.PackageID == packageId && i.ID < max).OrderByDescending(i=>i.ID).Take(20).ToList().ForEach(i =>
            {
                list.Add(new BLL.ImageInfo(i));
            });

            switch (thumb.ToLower())
            {
                case "fw78":
                    return PartialView("ImageListFw78.pc", list);
                case "fw236":
                default:
                    return PartialView("ImageListFw236.pc", list);
            }
        }
    }
}
