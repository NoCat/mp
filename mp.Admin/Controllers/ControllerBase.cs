using mp.DAL;
using mp.BLL;
using System.Web.Mvc;

namespace mp.Admin.Controllers
{
    public class ControllerBase : Controller
    {
        ManagerCollection manager;
        public ManagerCollection Manager
        {
            get
            {
                if (manager == null)
                    manager = new ManagerCollection();
                return manager;
            }
        }

        protected ContentResult JsonContent(object obj)
        {
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(obj), "application/json");
        }
    }
}