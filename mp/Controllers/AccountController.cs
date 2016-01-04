using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using mp.DAL;

namespace mp.Controllers
{
    public class AccountController : ControllerBase
    {
        //
        // GET: /Account/

        [HttpPost]
        public ActionResult Login(string email, string password, bool remember = false)
        {
            var result = new AjaxResult();
            email = email.Trim();
            var user = DB.Users.Where(u => u.Email == email).FirstOrDefault();
            if (user == null || user.Password != (password + user.Salt).MD5())
            {
                result.Success = false;
                result.Message = "邮箱或密码错误";
            }
            else
            {
                Security.Login(user.ID, remember);
            }
            return JsonContent(result);
        }

        [HttpPost]
        public ActionResult Signup(string email, string name, string password1, string password2)
        {
            var result = new AjaxResult();

            if (password1 != password2)
            {
                result.Success = false;
                result.Message = "两次输入的密码不一样";
                return JsonContent(result);
            }

            email = email.Trim();
            var emailExist = DB.Users.Where(u => u.Email == email).Count() > 0;
            if (emailExist)
            {
                result.Success = false;
                result.Message = "邮箱已使用";
                return JsonContent(result);
            }

            name = name.Trim();
            var nameExist = DB.Users.Where(u => u.Name == name).Count() > 0;
            if (nameExist)
            {
                result.Success = false;
                result.Message = "昵称已使用";
                return JsonContent(result);
            }

            var salt = Guid.NewGuid().ToByteArray().ToHexString();
            var password = (password1 + salt).MD5();

            var user = new User
            {
                Name = name,
                Email = email,
                Password = password,
                Salt = salt,
                UseDefaultHead=true
            };

            DB.Insert(user);

            return JsonContent(result);
        }

        public ActionResult Logout()
        {
            Security.Logout();
            return Redirect("/");
        }
    }
}
