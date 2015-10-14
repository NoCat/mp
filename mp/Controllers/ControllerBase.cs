using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mp.BLL;
using System.Web.Mvc;

namespace mp.Controllers
{
    public class ControllerBase : Controller
    {
        ManagermentService _service;
        public ManagermentService Service
        {
            get
            {
                if (_service == null)
                    _service = new ManagermentService();
                return _service;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_service != null)
                _service.Dispose();
            base.Dispose(disposing);
        }
    }
}