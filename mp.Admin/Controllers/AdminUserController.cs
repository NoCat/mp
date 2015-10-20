using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.DAL;
using mp.Admin.Models;
using PagedList;

namespace mp.Admin.Controllers
{
    public class AdminUserManagerController : ControllerBase
    {
        //系统用户管理
        public ActionResult Index(string find = "")
        {
            IEnumerable<AdminUser> result = DB.AdminUsers;
            if (find != "")
                result = result.Where(u => u.Name.StartsWith(find));

            result = result.Take(40);
            var list = new List<AdminUserInfo>();
            result.ToList().ForEach(a => list.Add(new AdminUserInfo(a)));
            ViewBag.List = list;
            return View();
        }

        public ActionResult Create(string name, string password)
        {
            name = name.Trim();
            var exist = DB.AdminUsers.Where(u => u.Name == name).FirstOrDefault();
            if (exist == null)
                DB.AdminUserInsert(new AdminUser { Name = name, Password = password.MD5() });
            return RedirectToAction("index");
        }
        public ActionResult Delete(int id)
        {
            DB.AdminUserDelete(new AdminUser { ID = id });
            return RedirectToAction("index");
        }
        [HttpPost]
        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}
