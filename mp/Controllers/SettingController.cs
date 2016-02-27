using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.Models;
using mp.DAL;

namespace mp.Controllers
{
    public class SettingController : ControllerBase
    {
        //
        // GET: /Setting/

        [MPAuthorize]
        public ActionResult Index()
        {
            var user = Manager.Users.Find(Security.User.ID);
            return View(user);
        }

        [MPAuthorize]
        public ActionResult Avt()
        {
            return View("avt");
        }

        [MPAuthorize]
        public ActionResult Secure()
        {
            return View("security");
        }

        [HttpPost, MPAuthorize]
        public ActionResult index(string username,string description)
        {
            var u = Manager.Users.Items.Where(i => i.Name == username.Trim());
            if (u!=null)
            {
                var ajaxResult = new AjaxResult() { Success=false};
                return  JsonContent(ajaxResult);
            }
            var user = Manager.Users.Items.Find(Security.User.ID);
            user.Name = username.Trim();
            user.Description = description;
            Manager.Users.Update(user);
            return Redirect("/Setting/");
        }

        [HttpPost,MPAuthorize]
        public ActionResult Secure(string oPassword,string nPassword1,string nPassword2)
        {
            var user = Manager.Users.Items.Find(Security.User.ID);
            var salt = user.Salt;
            if ((oPassword + salt).MD5() != user.Password)
            {
                var result = new AjaxResult() { Success = false, Message = "原密码输入不正确" };
                return JsonContent(result);
            }
            if (nPassword1==null)
            {
                return JsonContent(new AjaxResult() { Success = false, Message = "密码不能为空" });
            }
            if (nPassword1!=nPassword2)
            {
                return JsonContent(new AjaxResult() { Success = false, Message = "两次密码输入不正确" });
            }
            user.Password = (salt + nPassword1).MD5();
            return Redirect("/Setting/Secure");
        }
    }
}
