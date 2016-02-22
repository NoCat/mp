using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.Models;
using mp.DAL;

namespace mp.Controllers
{
    public class PackageController : ControllerBase
    {
        public ActionResult Index(int Id = 0)
        {
            var package = Manager.Packages.Find(Id);
            if (package == null)
                return Redirect("/");

            ViewBag.packageInfo = new PackageInfo(package);
            return View();
        }

        public ActionResult Images(int packageId = 0, int max = 0, string thumb = "fw236")
        {
            if (max == 0)
                max = int.MaxValue;

            var list = new List<ImageInfo>();
            Manager.Images.Items.Where(i => i.PackageID == packageId && i.ID < max && i.State== DAL.ImageStates.Ready).OrderByDescending(i => i.ID).Take(20).ToList().ForEach(i =>
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

        #region 创建
        [MPAuthorize]
        public ActionResult Create()
        {
            return PartialView("modal");
        }
        [MPAuthorize, HttpPost]
        public ActionResult Create(string title, string description)
        {
            var result = new AjaxResult();

            if (title == null)
            {
                title = "";
            }
            else
            {
                title = title.Trim();
            }
            if (title.Length == 0)
            {
                result.Success = false;
                result.Message = "图包标题不能为空";
                return JsonContent(result);
            }

            var exist = Manager.Packages.Items.Where(p => p.UserID == Security.User.ID && p.Title == title).Count() > 0;
            if (exist)
            {
                result.Success = false;
                result.Message = "已经存在同名图包";
                return JsonContent(result);
            }

            var package = new DAL.Package
            {
                Title = title,
                UserID = Security.User.ID,
                HasCover = false,
                Description = description,
                LastModify = new DateTime(1999, 1, 1)
            };
            Manager.Packages.Add(package);

            result.Data = new { id=package.ID,title=package.Title };

            return JsonContent(result);
        } 
        #endregion

        #region 编辑
        [MPAuthorize]
        public ActionResult Edit(int id)
        {
            var package = Manager.Packages.Find(id);

            ViewBag.Title = package.Title;
            ViewBag.Description = package.Description;

            return PartialView("modal");
        }
        [MPAuthorize,HttpPost]
        public ActionResult Edit(int id,string title,string description)
        {
            var result = new AjaxResult();

            var package = Manager.Packages.Find(id);
            if(package==null|| package.UserID!=Security.User.ID)
            {
                result.Success = false;
                result.Message = "错误操作";
                return JsonContent(result);
            }

            if (title == null)
            {
                title = "";
            }
            else
            {
                title = title.Trim();
            }

            if(title.Length==0)
            {
                result.Success = false;
                result.Message = "图包标题不能为空";
                return JsonContent(result);
            }

            var exist = Manager.Packages.Items.Where(p => p.Title == title && p.UserID == Security.User.ID && p.ID != package.ID).Count() > 0;
            if(exist)
            {
                result.Success = false;
                result.Message = "已经存在同名图包";
                return JsonContent(result);
            }

            package.Title = title;
            package.Description = description;
            Manager.Packages.Update(package);

            result.Data=new {Title=title,Description=description};
            return JsonContent(result);
        }
        #endregion

        [MPAuthorize,HttpPost]
        public ActionResult Follow(int id)
        {
            var result = new AjaxResult();

            var package = Manager.Packages.Find(id);
            if(package==null)
            {
                result.Success = false;
                result.Message = "图包不存在";
                return JsonContent(result);
            }

            if(package.UserID==Security.User.ID)
            {
                result.Success = false;
                result.Message = "不能关注自己的图包";
                return JsonContent(result);
            }

            var exist = Manager.Followings.Items.Where(f => f.UserID == Security.User.ID && f.Type == DAL.FollowingTypes.Package && f.Info == id).Count() > 0;
            if(exist)
            {
                result.Success = false;
                result.Message = "已经关注了";
                return JsonContent(result);
            }

            var follow = new Following { UserID = Security.User.ID, Type = FollowingTypes.Package, Info = id };
            Manager.Followings.Add(follow);

            return JsonContent(result);
        }

        [MPAuthorize,HttpPost]
        public ActionResult CancelFollow(int id)
        {
            var result = new AjaxResult();

            var follow = Manager.Followings.Items.Where(f => f.UserID == Security.User.ID && f.Type == FollowingTypes.Package && f.Info == id).FirstOrDefault();
            if (follow != null)
                Manager.Followings.Remove(follow);

            return JsonContent(result);
        }
    }
}
