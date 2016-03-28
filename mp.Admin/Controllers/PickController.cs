﻿using System;
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
        public ActionResult Index(string keyword,string filter="wait")
        {
            var result = Manager.AdminPixivUsers.Items.AsQueryable();

            if(string.IsNullOrWhiteSpace(keyword)==false)
            {
                keyword = keyword.Trim();
                result = result.Where(u => u.UserName.Contains(keyword));
            }

            switch (filter)
            {
                case "wait":
                    result = result.Where(u => u.State == AdminPixivUserStates.Wait);
                    break;
                case "skip":
                    result = result.Where(u => u.State == AdminPixivUserStates.Skip);
                    break;
                case "pick":
                    result = result.Where(u => u.State == AdminPixivUserStates.Pick);
                    break;
                default:
                    break;
            }

            var list = result.OrderByDescending(p => p.RankCount).Take(40).ToList();
            ViewBag.Keyword = keyword;
            ViewBag.List = list;
            ViewBag.Filter = filter;
            return View();
        }

        public ActionResult Add()
        {
            var model = new mp.DAL.AdminPixivUser { LastPickTime = new DateTime(1990, 1, 1) };
            return PartialView("Modal", model);
        }
        [HttpPost]
        public ActionResult Add(AdminPixivUser model)
        {
            var result = new AjaxResult();
            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                result.Success = false;
                result.Message = "p站用户名不能为空";
                return JsonContent(result);
            }
            model.UserName = model.UserName.Trim();

            var exist = Manager.AdminPixivUsers.Items.Where(u => u.UserID == model.UserID).Count() > 0;
            if (exist)
            {
                result.Success = false;
                result.Message = "要采集的用户已经存在";
                return JsonContent(result);
            }

            Manager.AdminPixivUsers.Add(model);
            return JsonContent(result);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.IsEdit = true;
            var model = Manager.AdminPixivUsers.Find(id);
            return PartialView("Modal", model);
        }
        [HttpPost]
        public ActionResult Edit(AdminPixivUser model)
        {
            var result = new AjaxResult();

            if(string.IsNullOrWhiteSpace(model.UserName))
            {
                result.Success = false;
                result.Message = "p站用户名不能为空";
                return JsonContent(result);
            }

            model.UserName = model.UserName.Trim();
            Manager.AdminPixivUsers.Update(model);
            return JsonContent(result);
        }

        public ActionResult Delete(int id)
        {
            var pick = Manager.AdminPixivUsers.Find(id);
            Manager.AdminPixivUsers.Remove(pick);
            return Redirect("~/pick");
        }
    }
}
