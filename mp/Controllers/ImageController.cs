using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.Models;

namespace mp.Controllers
{
    public class ImageController : ControllerBase
    {
        public ActionResult Index(int id = 0)
        {
            var image = DB.Images.Find(id);
            if (image == null)
                return Redirect("/");

            var next = DB.Images.Where(i => i.ID < id && i.PackageID == image.PackageID).OrderByDescending(i => i.ID).Select(i => i.ID).FirstOrDefault();
            var prev = DB.Images.Where(i => i.ID > id && i.PackageID == image.PackageID).Select(i => i.ID).FirstOrDefault();

            ViewBag.ImageInfo = new ImageInfo(image);

            if (next != 0)
                ViewBag.Next = next;
            if (prev != 0)
                ViewBag.Prev = prev;

            return View("Index.pc");
        }

        [MPAuthorize]
        public ActionResult Resave(int id)
        {
            var model = GetModalModel(id);

            return PartialView("Modal", model);
        }

        [MPAuthorize]
        public ActionResult Edit(int id)
        {
            var model = GetModalModel(id);
            model.PackageID = model.Image.PackageID;
            return PartialView("Modal", model);
        }
        [MPAuthorize, HttpPost]
        public ActionResult Edit(ImageModalModel model)
        {
            var result = new AjaxResult();

            var image = DB.Images.Find(model.ID);
            if (image == null)
            {
                result.Success = false;
                result.Message = "图片不存在";
                return JsonContent(result);
            }

            var package = DB.Packages.Find(model.PackageID);
            if (package == null)
            {
                result.Success = false;
                result.Message = "图包不存在";
                return JsonContent(result);
            }

            image.PackageID = package.ID;
            image.Description = model.Description;

            DB.Update(image);

            return JsonContent(result);
        }

        ImageModalModel GetModalModel(int id)
        {
            var model = new ImageModalModel();

            var image = DB.Images.Find(id);
            if (image == null)
                return model;

            model.PackageList = DB.Packages.Where(p => p.UserID == Security.User.ID).OrderByDescending(p => p.ID).ToArray();
            model.Image = new ImageInfo(image);
            model.Description = image.Description;

            return model;
        }
    }
}
