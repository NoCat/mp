using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using mp.Admin.Models;

namespace mp.Admin.Controllers
{
    [MPAuthorize]
    public class SubAccountManagerController : ControllerBase
    {
        //
        // GET: /SubAccountManager/

        public ActionResult Index()
        {
            var userIds = Manager.AdminSubAccounts.Items.Where(a => a.AdminUserID == Security.User.ID).Select(a => a.UserID);
            var result = Manager.Users.Items.Where(u => userIds.Contains(u.ID)).Take(40);

            var list = new List<UserInfo>();
            result.ToList().ForEach(u => list.Add(new UserInfo(u)));
            ViewBag.List = list;
            return View();
        }

        public ActionResult CreateNew(string email, string password, string name)
        {
            email = email.Trim();
            name = name.Trim();

            var exist = Manager.Users.Items.Where(u => u.Email == email && u.Name == name).FirstOrDefault();
            if (exist == null)
            {
                Manager.Users.Add(new DAL.User { Email = email, Name = name, Password = password.MD5() });
            }

            return RedirectToAction("Index");
        }

        public ActionResult CreateExist(int id)
        {
            if (Manager.Users.Find(id) != null)
            {
                if (Manager.AdminSubAccounts.Items.Where(a => a.UserID == id).FirstOrDefault() == null)
                    Manager.AdminSubAccounts.Add(new DAL.AdminSubAccount { AdminUserID = Security.User.ID, UserID = id });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {            
            return View();
        }

        public ActionResult Edit(int id)
        {
            return View();
        }
    }
}
