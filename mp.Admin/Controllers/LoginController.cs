using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.Admin.Models;

namespace mp.Admin.Controllers
{
    //登录
    public class LoginController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            var user = Manager.AdminUsers.Items.Where(u => u.Name == username).FirstOrDefault();
            if (user != null)
            {
                if (user.Password == password.MD5())
                {
                    Security.Login(user.ID, false);
                    return Redirect("/");
                }
            }
            ViewBag.Error = true;
            return View();
            
        }
    }
}
