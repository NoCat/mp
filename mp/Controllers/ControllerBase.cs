using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.BLL;
using mp.DAL;
using System.Web.Mvc;

namespace mp.Controllers
{
    public class ControllerBase : Controller
    {
        MiaopassContext _db;
        public MiaopassContext DB
        {
            get
            {
                if (_db == null)
                    _db = new MiaopassContext();
                return _db;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_db != null)
                _db.Dispose();
            base.Dispose(disposing);
        }

        protected ContentResult JsonResult(object obj)
        {
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return Content(str, "application/json");
        }
    }
}