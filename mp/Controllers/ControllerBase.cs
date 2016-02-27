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
        public ManagerCollection Manager
        {
            get
            {
                return Security.Manager;
            }
        }

        protected ContentResult JsonContent(object obj)
        {
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return Content(str, "application/json");
        }
        
    }
}