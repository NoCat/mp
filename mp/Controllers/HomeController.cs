using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.BLL;
using mp.Models;

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

            Manager.Images.Items
                .Where(i => i.State == DAL.ImageStates.Ready)
                .OrderByDescending(i => i.Weight)
                .ThenByDescending(i => i.ID)
                .Take(40).ToList().ForEach(i =>
                    {
                        imageList.Add(new ImageInfo(i));
                    });

            var packages = Manager.Packages.Items
                .Select(p => new
                    {
                        p,
                        weight = Manager.Images.Items.Where(i => i.PackageID == p.ID).Sum(i=>i.Weight)
                    })
                .OrderByDescending(p => p.weight)
                .ThenByDescending(p => p.p.LastModify)
                .Select(p => p.p)
                .Take(6)
                .ToList();

            packages.ForEach(p =>
            {
                packageList.Add(new PackageInfo(p));
            });

            ViewBag.PackageList = packageList;
            ViewBag.ImageList = imageList;

            return View("index.pc");
        }
        
        public ActionResult Latest()
        {
            var imageList = new List<ImageInfo>();

            Manager.Images.Items.Where(i => i.State == DAL.ImageStates.Ready).Take(40).ToList().ForEach(i =>
            {
                imageList.Add(new ImageInfo(i));
            });

            ViewBag.ImageList = imageList;

            return View("Latest");
            
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}
