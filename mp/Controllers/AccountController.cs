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
            var user = Manager.Users.Items.Where(u => u.Email == email).FirstOrDefault();
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
        public ActionResult Signup(string email = "", string name = "", string password1 = "", string password2 = "")
        {
            var result = new AjaxResult();

            if (password1 != password2)
            {
                result.Success = false;
                result.Message = "两次输入的密码不一样";
                return JsonContent(result);
            }

            if (password1.Length == 0)
            {
                result.Success = false;
                result.Message = "密码长度不能为0";
                return JsonContent(result);
            }

            email = email.Trim();
            //电子邮箱正则验证
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$"))
            {
                result.Success = false;
                result.Message = "请输入有效的邮箱地址";
                return JsonContent(result);
            }

            var emailExist = Manager.Users.Items.Where(u => u.Email == email).Count() > 0;
            if (emailExist)
            {
                result.Success = false;
                result.Message = "邮箱已使用";
                return JsonContent(result);
            }

            name = name.Trim();
            var nameExist = Manager.Users.Items.Where(u => u.Name == name).Count() > 0;
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
                UseDefaultHead = true,
                CreateTime = DateTime.Now
            };

            Manager.Users.Add(user);

            return JsonContent(result);
        }

        [HttpPost]
        public ActionResult SendResetMail(string email)
        {
            return View();
        }

        public ActionResult ResetPassword(string token)
        {
            var reset = Manager.PasswordResets.Items.Where(s => s.Token == token && s.ExpireTime>DateTime.Now).FirstOrDefault();
            if (reset == null)
                return Redirect("~/");

            ViewBag.Email = reset.User.Email;

            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(string token, string pw1,string pw2)
        {
            var result = new AjaxResult();

            if(string.IsNullOrEmpty(pw1))
            {
                result.Success = false;
                result.Message = "密码不能为空";
                return JsonContent(result);
            }

            if(pw1!=pw2)
            {
                result.Success = false;
                result.Message = "两次输入密码不一致";
                return JsonContent(result);
            }

            var reset = Manager.PasswordResets.Items.Where(s => s.Token == token && s.ExpireTime > DateTime.Now).FirstOrDefault();
            if(result==null)
            {
                result.Success = false;
                result.Message = "token不存在或过期";
                return JsonContent(result);
            }

            var salt = Guid.NewGuid().ToByteArray().ToHexString();
            var password = (pw1 + salt).MD5();

            reset.User.Password = password;
            reset.User.Salt = salt;
            Manager.Users.Update(reset.User);

            return JsonContent(result);
        }

        public ActionResult Logout()
        {
            Security.Logout();
            return Redirect("/");
        }
    }
}
