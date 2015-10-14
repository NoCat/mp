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
        ManagementService _service;
        public ManagementService Service
        {
            get
            {
                if (_service == null)
                    _service = new ManagementService();
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