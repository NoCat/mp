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
            var model = GetModalModel(id, ModelTypes.Resave);
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
            Manager.ResaveChains.Add(new ResaveChain { Parent = image.ID, Child = nImage.ID });

            return JsonContent(result);
        }
        #endregion

        #region 编辑
        [MPAuthorize]
        public ActionResult Edit(int id)
        {
            var model = GetModalModel(id, ModelTypes.Edit);
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
            result.Data = new { url = string.Format("/package/{0}", image.PackageID) };

            return JsonContent(result);
        }
        #endregion

        #region 上传后添加新图片
        [MPAuthorize]
        public ActionResult Add(int id)
        {
            var model = GetModalModel(id, ModelTypes.Add);
            return PartialView("modal", model);
        }
        [MPAuthorize, HttpPost]
        public ActionResult Add(List<int> fileid, int packageid, List<string> filename, string description)
        {
            var result = new AjaxResult();

            var package = Manager.Packages.Find(packageid);
            if(package==null || package.UserID!=Security.User.ID)
            {
                result.Message = "错误操作";
                result.Success = false;
                return JsonContent(result);
            }

            var images = new List<Image>();
            if (string.IsNullOrWhiteSpace(description) == false)
            {
                for (int i = 0; i < filename.Count; i++)
                {
                    filename[i] = description;
                }
            }

            var validFileIds = Manager.Files.Items.Where(f => fileid.Contains(f.ID)).Select(f=>f.ID).ToList();

            foreach (var id in validFileIds)
            {
                var index = fileid.IndexOf(id);
                if (index == -1)
                    continue;

                var image = new Image();
                image.FileID = fileid[index];
                image.Description = filename[index];
                image.PackageID = packageid;
                image.UserID = Security.User.ID;
                images.Add(image);
            }

            try
            {
                Manager.Images.AddRange(images);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return JsonContent(result);
        }
        #endregion

        #region 赞
        [MPAuthorize, HttpPost]
        public ActionResult Praise(int id)
        {
            var result = new AjaxResult();

            var image = Manager.Images.Find(id);
            if (image == null)
            {
                result.Success = false;
                result.Message = "错误操作";
                return JsonContent(result);
            }

            var praise = Manager.Praises.Items.Where(p => p.ImageID == image.ID && p.UserID == Security.User.ID).FirstOrDefault();
            if (praise != null)
            {
                result.Success = false;
                result.Message = "已经赞过了哦~";
                return JsonContent(result);
            }

            praise = new Praise { ImageID = image.ID, UserID = Security.User.ID };
            Manager.Praises.Add(praise);

            var count = Manager.Praises.Items.Where(p => p.ImageID == image.ID).Count();

            result.Data = new { count };

            return JsonContent(result);
        }
        #endregion

        #region 取消赞
        [MPAuthorize, HttpPost]
        public ActionResult CancelPraise(int id)
        {
            var result = new AjaxResult();

            var image = Manager.Images.Find(id);

            if (image == null)
            {
                result.Success = false;
                result.Message = "错误操作";
                return JsonContent(result);
            }

            var praise = Manager.Praises.Items.Where(p => p.ImageID == image.ID && p.UserID == Security.User.ID).FirstOrDefault();
            if (praise != null)
                Manager.Praises.Remove(praise);

            var count = Manager.Praises.Items.Where(p => p.ImageID == image.ID).Count();
            result.Data = new { count };

            return JsonContent(result);
        }
        #endregion

        ImageModalModel GetModalModel(int id, ModelTypes type)
        {
            var model = new ImageModalModel();

            model.PackageList = Manager.Packages.Items.Where(p => p.UserID == Security.User.ID).OrderByDescending(p => p.ID).ToArray();

            switch (type)
            {
                case ModelTypes.Resave:
                    {
                        var image = Manager.Images.Find(id);
                        if (image != null)
                        {
                            model.ID = image.ID;
                            model.PackageID = model.PackageList.Select(p => p.ID).FirstOrDefault();
                            model.ImagePath = new ImageInfo(image).ThumbFW236.Url;
                            model.Description = image.Description;
                        }
                        break;
                    }
                case ModelTypes.Add:
                    {
                        var file = Manager.Files.Find(id);
                        model.ImagePath = new ImageInfo(new Image { File = file }).ThumbFW236.Url;
                        model.PackageID = model.PackageList.Select(p => p.ID).FirstOrDefault();
                        break;
                    }

                case ModelTypes.Edit:
                    {
                        var image = Manager.Images.Find(id);
                        model.ImagePath = new ImageInfo(image).ThumbFW236.Url;
                        model.Description = image.Description;
                        model.PackageID = image.PackageID;
                        break;
                    }
                default:
                    break;
            }

            return model;
        }
    }
}
