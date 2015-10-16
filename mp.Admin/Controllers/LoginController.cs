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
            var result = new AjaxResult();
            var user = DB.AdminUsers.Where(u => u.Username == username).FirstOrDefault();
            if (user != null)
            {
                if (user.Password == password.MD5())
                {
                    Security.Login(user.ID, false);
                    return Json(result);
                }
            }
            result.Success = false;
            result.Message = "帐号或密码错误";
            return Json(result);
        }
    }
}
