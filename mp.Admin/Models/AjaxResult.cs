using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mp.Admin.Models
{
    public class AjaxResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public AjaxResult()
        {
            Success = true;
            Message = "OK";
            Data = null;
        }
    }
}