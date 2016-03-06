﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mp.Models;
using System.IO;
using mp.BLL;
using mp.DAL;
using System.Drawing;
using System.Drawing.Imaging;


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
            var user = new UserInfo(User);
            return View("avt",user);
        }

        [MPAuthorize]
        public ActionResult Secure()
        {
            return View("security");
        }

        [HttpPost, MPAuthorize]
        public ActionResult index(string username, string description)
        {
            var u = Manager.Users.Items.Where(i => i.Name == username.Trim());
            if (u != null)
            {
                var ajaxResult = new AjaxResult() { Success = false };
                return JsonContent(ajaxResult);
            }
            var user = Manager.Users.Items.Find(Security.User.ID);
            user.Name = username.Trim();
            user.Description = description;
            Manager.Users.Update(user);
            return Redirect("/Setting/");
        }

        [HttpPost, MPAuthorize]
        public ActionResult Secure(string oPassword, string nPassword1, string nPassword2)
        {
            var user = Manager.Users.Items.Find(Security.User.ID);
            var salt = user.Salt;
            if ((oPassword + salt).MD5() != user.Password)
            {
                var result = new AjaxResult() { Success = false, Message = "原密码输入不正确" };
                return JsonContent(result);
            }
            if (nPassword1 == null)
            {
                return JsonContent(new AjaxResult() { Success = false, Message = "密码不能为空" });
            }
            if (nPassword1 != nPassword2)
            {
                return JsonContent(new AjaxResult() { Success = false, Message = "两次密码输入不正确" });
            }
            user.Password = (salt + nPassword1).MD5();
            return Redirect("/Setting/Secure");
        }

        [HttpPost, MPAuthorize]
        public ActionResult AvtUpload(string name, int chunk, int chunks, HttpPostedFileBase data)
        {
            var result = new AjaxResult();
            var path = Server.MapPath("~/temp/");
            using (var fs = System.IO.File.Create(path + name + "_" + chunk))
            {
                fs.Write(data.InputStream);
            }

            if (chunk == chunks - 1)
            {
                var mergePath = path + name;
                using (var fs = System.IO.File.Create(mergePath))
                {
                    for (int i = 0; i < chunks; i++)
                    {
                        var chunkPath = path + name + "_" + i;
                        using (var cf = System.IO.File.OpenRead(chunkPath))
                        {
                            fs.Write(cf);
                        }
                        System.IO.File.Delete(chunkPath);
                    }
                    fs.Position = 0;
                    //生成初始图像文件
                    using (System.Drawing.Image bitmap = System.Drawing.Image.FromStream(fs))
                    {
                        if (bitmap.RawFormat.Equals(ImageFormat.Bmp) == false && bitmap.RawFormat.Equals(ImageFormat.Png) == false && bitmap.RawFormat.Equals(ImageFormat.Jpeg) == false)
                        {
                            result.Success = false;
                            result.Message = "请上传图片文件";
                            return Json(result);
                        }
                        fs.Position = 0;
                        var originJpg = System.IO.File.Create(path + Security.User.ID + ".jpg");
                        originJpg.Write(fs);
                        originJpg.Close();
                    }
                }
                result.Data = "/temp/" + Security.User.ID + ".jpg";

            }
            return Json(result);
        }

        public ActionResult AvtCutModel(string src)
        {
            return PartialView("avtcutmodel", src);
        }
        [HttpPost, MPAuthorize]
        public ActionResult AvtSetting(int left, int top, double ratio, int size)
        {
            var result = new AjaxResult();
             var path = Server.MapPath("~/temp/");
             var id = User.ID;

            if(System.IO.File.Exists(path + id + ".jpg")==false)
            {
                result.Success = false;

                return JsonContent(result);
            }
           
            
            using (var fs = System.IO.File.OpenRead(path + id + ".jpg"))
            {
                var img = System.Drawing.Image.FromStream(fs);
                left = (int)Math.Round(left / ratio);
                top = (int)Math.Round(top / ratio);
                size = (int)Math.Round(size / ratio);

                System.Drawing.Image desc = new Bitmap(118, 118);
                using (Graphics g = Graphics.FromImage(desc))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    g.DrawImage(img, new Rectangle(0, 0, 118, 118), new Rectangle(left, top, size, size), GraphicsUnit.Pixel);
                }
                //上传头像到服务器
                OssFile.Create("avt/" + id + "_big", desc.SaveAsJpeg());
                OssFile.Create("avt/" + id, desc.FixWidth(32).SaveAsJpeg());
                var u = Manager.Users.Find(id);
                u.UseDefaultHead = false;
                Manager.Users.Update(u);
            }

            return JsonContent(result);
        }
    }
}
