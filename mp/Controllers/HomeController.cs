using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.BLL;

namespace mp.Controllers
{
    public class HomeController : ControllerBase
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var packageList = new List<PackageInfo>();
            var imageList = new List<ImageInfo>();
            Service.Images.Items.OrderByDescending(i=>i.ID).Take(40).ToList().ForEach(i =>
            {
                imageList.Add(new ImageInfo(i));
            });
            Service.Packages.Items.OrderByDescending(p => p.ID).Take(6).ToList().ForEach(p =>
            {
                packageList.Add(new PackageInfo(p));
            });

            ViewBag.PackageList = packageList;
            ViewBag.ImageList = imageList;

            return View("index.pc");
        }

    }
}
