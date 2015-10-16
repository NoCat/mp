using mp.DAL;
using System.Web.Mvc;

namespace mp.Admin.Controllers
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
    }
}