﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mp.Controllers
{
    public class AccountController : ControllerBase
    {
        //
        // GET: /Account/

        [HttpPost]
        public ActionResult Login(string email,string password,bool remember=false)
        {
            var result = new AjaxResult();
            email = email.Trim();
            var user = DB.Users.Where(u => u.Email == email).FirstOrDefault();
            var pwd = Tools.Md5(password).ToHexString();
            if(user==null||user.Password!=pwd)
            {
                result.Success = false;
                result.Message = "邮箱或密码错误";
            }
            else
            {
                Security.Login(user.ID,remember);
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            var result = new AjaxResult();
            Security.Logout();
            return Json(result);
        }
    }
}
