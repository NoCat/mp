using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.Models;
using mp.DAL;

namespace mp.Controllers
{
    public class UserController : ControllerBase
    {
        public ActionResult Packages(int id = 0, int max = 0)
        {
            return Show(UserSubPages.Packages, id, max);
        }

        public ActionResult Images(int id = 0, int max = 0)
        {
            return Show(UserSubPages.Images, id, max);
        }

        public ActionResult Praises(int id = 0, int max = 0)
        {
            return Show(UserSubPages.Praises, id, max);
        }

        public ActionResult Followers(int id = 0, int max = 0)
        {
            return Show(UserSubPages.Followers, id, max);
        }

        public ActionResult FollowingUsers(int id = 0, int max = 0)
        {
            return Show(UserSubPages.FollowingUsers, id, max);
        }

        public ActionResult FollowingPackages(int id = 0, int max = 0)
        {
            return Show(UserSubPages.FollowingPackages, id, max);
        }

        ActionResult Show(UserSubPages subPage, int userId = 0, int max = 0)
        {
            if (max == 0)
            {
                var user = Manager.Users.Find(userId);
                if (user == null)
                    return Redirect("/");

                ViewBag.SubPage = subPage;
                ViewBag.UserInfo = new UserInfo(user);
                return View("index.pc");
            }

            var list = new List<WaterfallItem>();
            switch (subPage)
            {
                case UserSubPages.Packages:
                    {
                        Manager.Packages.Items
                            .Where(p => p.UserID == userId && p.ID < max)
                            .OrderByDescending(p => p.ID)
                            .Take(20)
                            .ToList()
                            .ForEach(p =>
                            {
                                list.Add(new WaterfallItem { ID = p.ID, Item = new PackageInfo(p) });
                            });
                        return PartialView("PackageList.pc", list);
                    }
                case UserSubPages.Images:
                    {
                        Manager.Images.Items
                            .Where(i => i.UserID == userId && i.ID < max && i.State == DAL.ImageStates.Ready)
                            .OrderByDescending(i => i.ID)
                            .Take(20)
                            .ToList()
                            .ForEach(i =>
                            {
                                list.Add(new WaterfallItem { ID = i.ID, Item = new ImageInfo(i) });
                            });
                        return PartialView("ImageListFw236.pc", list);
                    }
                case UserSubPages.Praises:
                    {
                        Manager.Praises.Items
                            .Include("Image")
                            .Where(p => p.UserID == userId && p.ID < max)
                            .OrderByDescending(p => p.ID)
                            .Take(20)
                            .ToList()
                            .ForEach(p =>
                            {
                                list.Add(new WaterfallItem { ID = p.ID, Item = new ImageInfo(p.Image) });
                            });
                        return PartialView("ImageListFw236.pc", list);
                    }
                case UserSubPages.Followers:
                    {
                        Manager.Followings.Items
                            .Include("User")
                            .Where(f => f.Type == DAL.FollowingTypes.User && f.Info == userId && f.ID < max)
                            .OrderByDescending(f => f.ID)
                            .Take(20)
                            .ToList()
                            .ForEach(f =>
                            {
                                list.Add(new WaterfallItem { ID = f.ID, Item = new UserInfo(f.User) });
                            });
                        return PartialView("UserList.pc", list);
                    }
                case UserSubPages.FollowingUsers:
                    {
                        Manager.Followings.Items
                            .Include("User")
                            .Where(f => f.Type == DAL.FollowingTypes.User && f.UserID == userId && f.ID < max)
                            .OrderByDescending(f => f.ID)
                            .Take(20)
                            .ToList()
                            .ForEach(f =>
                            {
                                list.Add(new WaterfallItem { ID = f.ID, Item = new UserInfo(f.User) });
                            });
                        return PartialView("UserList.pc", list);
                    }
                case UserSubPages.FollowingPackages:
                    {
                        Manager.Followings.Items
                            .Where(f => f.Type == DAL.FollowingTypes.Package && f.UserID == userId && f.ID < max)
                            .OrderByDescending(f => f.ID)
                            .Take(20)
                            .ToList()
                            .ForEach(f =>
                            {
                                var package = Manager.Packages.Find(f.Info);
                                list.Add(new WaterfallItem { ID = f.ID, Item = new PackageInfo(package) });
                            });
                        return PartialView("PackageList.pc", list);
                    }
            }
            return View("index.pc");
        }

        [MPAuthorize, HttpPost]
        public ActionResult Follow(int id)
        {
            var result = new AjaxResult();

            if (id == Security.User.ID)
            {
                result.Success = false;
                result.Message = "不能关注自己";
                return JsonContent(result);
            }

            var user = Manager.Users.Find(id);
            if (user == null)
            {
                result.Success = false;
                result.Message = "关注用户不存在";
                return JsonContent(result);
            }

            var exist = Manager.Followings.Items.Where(f => f.UserID == Security.User.ID && f.Info == id && f.Type == DAL.FollowingTypes.User).Count() > 0;
            if (exist)
            {
                result.Success = false;
                result.Message = "用户已经关注过";
                return JsonContent(result);
            }

            var follow = new Following { UserID = Security.User.ID, Type = FollowingTypes.User, Info = id };
            Manager.Followings.Add(follow);

            return JsonContent(result);
        }

        [MPAuthorize, HttpPost]
        public ActionResult CancelFollow(int id)
        {
            var result = new AjaxResult();

            var follow = Manager.Followings.Items.Where(f => f.UserID == Security.User.ID && f.Type == FollowingTypes.User && f.Info == id).FirstOrDefault();
            if (follow != null)
            {
                Manager.Followings.Remove(follow);
            }

            return JsonContent(result);
        }
    }

    public enum UserSubPages
    {
        Packages,
        Images,
        Praises,
        Followers,
        FollowingUsers,
        FollowingPackages
    }
}
