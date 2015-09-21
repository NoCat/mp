using System;
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
        public ActionResult Login(string email,string password)
        {
            var result = new ResultJSON();
            email = email.Trim();
            var user = DB.Users.Where(u => u.Email == email).FirstOrDefault();
            if(user==null||user.Password!=Tools.Md5(password).ToHexString())
            {
                result.Code = 1;
                result.Message = "邮箱或密码错误";
            }
            return Json(result);
        }

    }
}
