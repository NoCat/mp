using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.DAL;
using mp.Admin.Models;

namespace mp.Admin.Controllers
{
    [MPAuthorize]
    public class PickController : ControllerBase
    {
        public ActionResult Index()
        {
            var list = Manager.AdminPixivPickUsers.Items.OrderByDescending(p => p.ID).Take(40).ToList();
            ViewBag.List = list;
            return View();
        }

        public ActionResult Add()
        {
            var model = new mp.DAL.AdminPixivPickUser { LastPickTime = new DateTime(1990, 1, 1) };
            return PartialView("Modal", model);
        }
        [HttpPost]
        public ActionResult Add(AdminPixivPickUser model)
        {
            var result = new AjaxResult();
            if (string.IsNullOrWhiteSpace(model.PixivUserName))
            {
                result.Success = false;
                result.Message = "p站用户名不能为空";
                return JsonContent(result);
            }
            model.PixivUserName = model.PixivUserName.Trim();

            var exist = Manager.AdminPixivPickUsers.Items.Where(u => u.PixivUserID == model.PixivUserID).Count() > 0;
            if (exist)
            {
                result.Success = false;
                result.Message = "要采集的用户已经存在";
                return JsonContent(result);
            }

            Manager.AdminPixivPickUsers.Add(model);
            return JsonContent(result);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.IsEdit = true;
            var model = Manager.AdminPixivPickUsers.Find(id);
            return PartialView("Modal", model);
        }
        [HttpPost]
        public ActionResult Edit(AdminPixivPickUser model)
        {
            var result = new AjaxResult();

            if(string.IsNullOrWhiteSpace(model.PixivUserName))
            {
                result.Success = false;
                result.Message = "p站用户名不能为空";
                return JsonContent(result);
            }

            model.PixivUserName = model.PixivUserName.Trim();
            Manager.AdminPixivPickUsers.Update(model);
            return JsonContent(result);
        }

        public ActionResult Delete(int id)
        {
            var pick = Manager.AdminPixivPickUsers.Find(id);
            Manager.AdminPixivPickUsers.Remove(pick);
            return Redirect("~/pick");
        }
    }
}
