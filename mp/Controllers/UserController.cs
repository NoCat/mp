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
        public ActionResult Index(UserSubPages subPage = UserSubPages.Packages, int userId = 0, int max = 0)
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
            switch (subPage)
            {
                case UserSubPages.Packages:
                    {
                        var list = new List<PackageInfo>();
                        Manager.Packages.Items.Where(p => p.UserID == userId && p.ID < max).OrderByDescending(p => p.ID).Take(20).ToList().ForEach(p =>
                        {
                            list.Add(new PackageInfo(p));
                        });
                        return PartialView("PackageList.pc", list);
                    }
                case UserSubPages.Images:
                    {
                        var list = new List<ImageInfo>();
                        Manager.Images.Items.Where(i => i.UserID == userId && i.ID < max && i.State == DAL.ImageStates.Ready).OrderByDescending(i => i.ID).Take(20).ToList().ForEach(i =>
                        {
                            list.Add(new ImageInfo(i));
                        });
                        return PartialView("ImageListFw236.pc", list);
                    }
                case UserSubPages.Praises:
                    {
                        var list = new List<ImageInfo>();
                        Manager.Praises.Items.Where(p => p.UserID == userId && p.ID < max).OrderByDescending(p => p.ID).Take(20).ToList().ForEach(p =>
                        {
                            list.Add(new ImageInfo(p.Image));
                        });
                        return PartialView("ImageListFw236.pc", list);
                    }
                case UserSubPages.Followers:
                    {
                        var list = new List<UserInfo>();
                        Manager.Followings.Items.Where(f => f.Type == DAL.FollowingTypes.User && f.Info == userId && f.ID < max).OrderByDescending(f => f.ID).Take(20).ToList().ForEach(f =>
                        {
                            list.Add(new UserInfo(f.User));
                        });
                        return PartialView("UserList.pc", list);
                    }
                case UserSubPages.FollowingUsers:
                    {
                        var list = new List<UserInfo>();
                        Manager.Followings.Items.Where(f => f.Type == DAL.FollowingTypes.User && f.UserID == userId && f.ID < max).OrderByDescending(f => f.ID).Take(20).ToList().ForEach(f =>
                        {
                            list.Add(new UserInfo(f.User));
                        });
                        return PartialView("UserList.pc", list);
                    }
                case UserSubPages.FollowingPackages:
                    {
                        var list = new List<PackageInfo>();
                        Manager.Followings.Items.Where(f => f.Type == DAL.FollowingTypes.Package && f.UserID == userId && f.ID < max).OrderByDescending(f => f.ID).Take(20).ToList().ForEach(f =>
                        {
                            list.Add(new PackageInfo(Manager.Packages.Find(f.Info)));
                        });
                        return PartialView("PackageList.pc", list);
                    }
            }
            return View("index.pc");
        }

        [MPAuthorize, HttpPost]
        public ActionResult Follow(int userId)
        {
            var result = new AjaxResult();

            if (userId == Security.User.ID)
            {
                result.Success = false;
                result.Message = "不能关注自己";
                return JsonContent(result);
            }

            var user = Manager.Users.Find(userId);
            if (user == null)
            {
                result.Success = false;
                result.Message = "关注用户不存在";
                return JsonContent(result);
            }

            var exist = Manager.Followings.Items.Where(f => f.UserID == Security.User.ID && f.Info == userId && f.Type == DAL.FollowingTypes.User).Count() > 0;
            if (exist)
            {
                result.Success = false;
                result.Message = "用户已经关注过";
                return JsonContent(result);
            }

            var follow = new Following { UserID = Security.User.ID, Type = FollowingTypes.User, Info = userId };
            Manager.Followings.Add(follow);

            return JsonContent(result);
        }

        [MPAuthorize, HttpPost]
        public ActionResult CancelFollow(int userid)
        {
            var result = new AjaxResult();

            var follow = Manager.Followings.Items.Where(f => f.UserID == Security.User.ID && f.Type == FollowingTypes.User && f.Info == userid).FirstOrDefault();
            if(follow!=null)
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
