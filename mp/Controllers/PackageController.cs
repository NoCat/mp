using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.Models;

namespace mp.Controllers
{
    public class PackageController : ControllerBase
    {
        public ActionResult Index(int packageId = 0)
        {
            if (packageId == 0)
                return Redirect("/");

            var package = Manager.Packages.Find(packageId);
            if (package == null)
                return Redirect("/");

            ViewBag.packageInfo =new PackageInfo(package);
            return View();
        }

        public ActionResult Images(int packageId = 0, int max = 0,string thumb="fw236")
        {
            if (max == 0)
                max = int.MaxValue;

            var list = new List<ImageInfo>();
            Manager.Images.Items.Where(i => i.PackageID == packageId && i.ID < max).OrderByDescending(i=>i.ID).Take(20).ToList().ForEach(i =>
            {
                list.Add(new ImageInfo(i));
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
