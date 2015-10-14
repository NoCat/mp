using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.Models;

namespace mp.Controllers
{
    public class UserController : ControllerBase
    {
        public ActionResult Index(UserSubPages subPage = UserSubPages.Packages, int userId = 0, int max = 0)
        {
            if (max == 0)
            {
                var user = Service.Users.Items.Where(u=>u.ID==userId).FirstOrDefault();
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
                        Service.Packages.Items.Where(p => p.UserID == userId && p.ID < max).OrderByDescending(p => p.ID).Take(20).ToList().ForEach(p =>
                        {
                            list.Add(new PackageInfo(p));
                        });
                        return PartialView("PackageList.pc", list);
                    }
                case UserSubPages.Images:
                    {
                        var list = new List<ImageInfo>();
                        Service.Images.Items.Where(i => i.UserID == userId && i.ID < max).OrderByDescending(i => i.ID).Take(20).ToList().ForEach(i =>
                        {
                            list.Add(new ImageInfo(i));
                        });
                        return PartialView("ImageListFw236.pc", list);
                    }
                case UserSubPages.Praises:
                    {
                        var list = new List<ImageInfo>();
                        Service.Praises.Items.Where(p => p.UserID == userId && p.ID < max).OrderByDescending(p => p.ID).Take(20).ToList().ForEach(p =>
                        {
                            list.Add(new ImageInfo(p.Image));
                        });
                        return PartialView("ImageList.pc", list);
                    }
                case UserSubPages.Followers:
                    {
                        var list = new List<UserInfo>();
                        Service.Followings.Items.Where(f => f.Type == DAL.FollowingTypes.User && f.Info == userId && f.ID < max).OrderByDescending(f => f.ID).Take(20).ToList().ForEach(f =>
                        {
                            list.Add(new UserInfo(f.User));
                        });
                        return PartialView("UserList.pc", list);
                    }
                case UserSubPages.FollowingUsers:
                    {
                        var list = new List<UserInfo>();
                        Service.Followings.Items.Where(f => f.Type == DAL.FollowingTypes.User && f.UserID == userId && f.ID < max).OrderByDescending(f => f.ID).Take(20).ToList().ForEach(f =>
                        {
                            list.Add(new UserInfo(f.User));
                        });
                        return PartialView("UserList.pc", list);
                    }
                case UserSubPages.FollowingPackages:
                    {
                        var list = new List<PackageInfo>();
                        Service.Followings.Items.Where(f => f.Type == DAL.FollowingTypes.Package && f.UserID == userId && f.ID < max).OrderByDescending(f => f.ID).Take(20).ToList().ForEach(f =>
                        {
                            list.Add(new PackageInfo(Service.Packages.Items.Where(i=>i.ID==f.Info).FirstOrDefault()));
                        });
                        return PartialView("PackageList.pc", list);
                    }
            }
            return View("index.pc");
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
