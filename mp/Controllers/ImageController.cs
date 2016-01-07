using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.Models;

using mp.DAL;

namespace mp.Controllers
{
    public class ImageController : ControllerBase
    {
        public ActionResult Index(int id = 0)
        {
            var image = Manager.Images.Find(id);
            if (image == null)
                return Redirect("/");

            var next = Manager.Images.Items.Where(i => i.ID < id && i.PackageID == image.PackageID).OrderByDescending(i => i.ID).Select(i => i.ID).FirstOrDefault();
            var prev = Manager.Images.Items.Where(i => i.ID > id && i.PackageID == image.PackageID).Select(i => i.ID).FirstOrDefault();

            ViewBag.ImageInfo = new ImageInfo(image);

            if (next != 0)
                ViewBag.Next = next;
            if (prev != 0)
                ViewBag.Prev = prev;

            return View("Index.pc");
        }

        #region 转存
        [MPAuthorize]
        public ActionResult Resave(int id)
        {
            var model = GetModalModel(id);
            model.PackageID = model.PackageList.Select(p => p.ID).FirstOrDefault();
            return PartialView("Modal", model);
        }
        [MPAuthorize, HttpPost]
        public ActionResult Resave(ImageModalModel model)
        {
            var result = new AjaxResult();

            var image = Manager.Images.Find(model.ID);
            if (image == null)
            {
                result.Success = false;
                result.Message = "错误操作";
                return JsonContent(result);
            }

            var package = Manager.Packages.Find(model.PackageID);
            if (package == null || package.UserID != Security.User.ID)
            {
                result.Success = false;
                result.Message = "错误操作";
                return JsonContent(result);
            }

            var nImage = new DAL.Image
            {
                UserID = Security.User.ID,
                PackageID = package.ID,
                CreatedTime = DateTime.Now,
                FromUrlID = image.FromUrlID,
                Description = model.Description,
                FileID = image.FileID
            };
            Manager.Images.Add(nImage);

            return JsonContent(result);
        }
        #endregion

        #region 编辑
        [MPAuthorize]
        public ActionResult Edit(int id)
        {
            var model = GetModalModel(id);
            return PartialView("Modal", model);
        }
        [MPAuthorize, HttpPost]
        public ActionResult Edit(ImageModalModel model)
        {
            var result = new AjaxResult();

            var image = Manager.Images.Find(model.ID);
            if (image == null || image.UserID != Security.User.ID)
            {
                result.Success = false;
                result.Message = "错误操作";
                return JsonContent(result);
            }

            var package = Manager.Packages.Find(model.PackageID);
            if (package == null || package.UserID != Security.User.ID)
            {
                result.Success = false;
                result.Message = "错误操作";
                return JsonContent(result);
            }

            image.PackageID = package.ID;
            image.Description = model.Description;

            Manager.Images.Update(image);

            return JsonContent(result);
        }
        #endregion

        #region 删除
        [MPAuthorize, HttpPost]
        public ActionResult Delete(int id)
        {
            var result = new AjaxResult();

            var image = Manager.Images.Find(id);
            if (image == null || image.UserID != Security.User.ID)
            {
                result.Success = false;
                result.Message = "错误操作";
                return JsonContent(result);
            }

            Manager.Images.Remove(image);

            //返回跳转网址
            result.Data = string.Format("/package/{0}", image.PackageID);

            return JsonContent(result);
        }
        #endregion

        #region 上传后添加新图片
        [MPAuthorize]
        public ActionResult Add(int id)
        {
            var file = Manager.Files.Find(id);

            var model = GetModalModel();
            model.ImagePath = new ImageInfo(new Image { File = file }).ThumbFW236.Url ;

            return PartialView("modal", model);
        }
        [MPAuthorize, HttpPost]
        public ActionResult Add(ImageModalModel model)
        {
            var result = new AjaxResult();
            return JsonContent(result);
        }
        #endregion

        ImageModalModel GetModalModel(int id = 0)
        {
            var model = new ImageModalModel();

            model.PackageList = Manager.Packages.Items.Where(p => p.UserID == Security.User.ID).OrderByDescending(p => p.ID).ToArray();

            var image = Manager.Images.Find(id);
            if (image != null)
            {
                model.ID = image.ID;
                model.PackageID = image.PackageID;
                model.ImagePath = new ImageInfo(image).ThumbFW236.Url;
                model.Description = image.Description;
            }
            return model;
        }
    }
}
