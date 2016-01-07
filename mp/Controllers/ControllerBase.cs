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
        ManagerCollection collection;
        public ManagerCollection Manager
        {
            get
            {
                if (collection == null)
                    collection = new ManagerCollection();
                return collection;
            }
        }

        protected ContentResult JsonContent(object obj)
        {
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return Content(str, "application/json");
        }
    }
}