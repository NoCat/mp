﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mp.Controllers
{
    public class ImageController : ControllerBase
    {
        //
        // GET: /Image/

        public ActionResult Index(int imageId=0)
        {
            var image = DB.Images.Find(imageId);
            if (image == null)
                return Redirect("/");

            var next = DB.Images.Where(i => i.ID < imageId && i.PackageID==image.PackageID).OrderByDescending(i=>i.ID).Select(i=>i.ID).FirstOrDefault();
            var prev = DB.Images.Where(i => i.ID > imageId && i.PackageID == image.PackageID).Select(i => i.ID).FirstOrDefault();

            ViewBag.ImageInfo = new BLL.ImageInfo(image);

            if (next != 0)
                ViewBag.Next = next;
            if (prev != 0)
                ViewBag.Prev = prev;
           
            return View("Index.pc");
        }
    }
}