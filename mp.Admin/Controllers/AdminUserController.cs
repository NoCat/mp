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
        public ActionResult Index(string find = "", int page = 1, int page_size = 20)
        {
            IEnumerable<AdminUser> result = DB.AdminUsers;
            if (find != "")
                result = result.Where(u => u.Name.StartsWith(find));

           // var model = result.Select(u => new AdminUserInfo(u)).ToPagedList(page, page_size);
            return View();
        }

        [HttpPost]
        public ActionResult Create(string username, string password)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}
