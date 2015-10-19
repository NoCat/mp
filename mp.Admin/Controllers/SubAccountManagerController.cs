using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using mp.Admin.Models;

namespace mp.Admin.Controllers
{
    public class SubAccountManagerController : ControllerBase
    {
        //
        // GET: /SubAccountManager/

        public ActionResult Index(int page=1,int size=20)
        {
            var userIds = DB.AdminSubAccounts.Where(a => a.AdminUserID == Security.User.ID).Select(a => a.UserID);
            var list = DB.Users.Select(u => new UserInfo(u)).Where(u => userIds.Contains(u.ID)).ToPagedList(page, size);
            ViewBag.List = list;
            return View();
        }

        public ActionResult CreateNew(string email,string password,string name)
        {
            email = email.Trim();
            name = name.Trim();

            var exist = DB.Users.Where(u => u.Email == email && u.Name == name).FirstOrDefault();
            if(exist==null)
            {
                DB.UserInsert(new DAL.User { Email = email, Name = name, Password = password.MD5() });
            }

            return RedirectToAction("Index");
        }

        public ActionResult CreateExist(int id)
        {
            if(DB.Users.Find(id)!=null)
            {
                if (DB.AdminSubAccounts.Where(a => a.UserID == id).FirstOrDefault() == null)
                    DB.AdminSubAccountInsert(new DAL.AdminSubAccount { AdminUserID = Security.User.ID, UserID = id });
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
