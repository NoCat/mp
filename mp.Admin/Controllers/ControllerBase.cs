using mp.DAL;
using mp.BLL;
using System.Web.Mvc;

namespace mp.Admin.Controllers
{
    public class ControllerBase : Controller
    {
        //MiaopassContext _db;
        //public MiaopassContext DB
        //{
        //    get
        //    {
        //        if (_db == null)
        //            _db = new MiaopassContext();
        //        return _db;
        //    }
        //}

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
    }
}